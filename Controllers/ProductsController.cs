using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VersioningDemo.Data;
using VersioningDemo.Models;

[ApiController]
[ApiVersion(1.0)]
[ApiVersion(2.0)]
//[Route("api/v{version:apiVersion}/products")]
[Route("api/products")] // <-- هذا المسار يتيح تشغيل الرابط GET /api/products مباشرة بدون الحاجة لكتابة رقم الإصدار في الـ URL
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    // GET /api/products (مع AsNoTracking، Select للـ Projection، و Pagination)
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var products = await _context.Products
            .AsNoTracking() // 1. تقليل استهلاك الموارد لعمليات القراءة فقط[cite: 7]
            .OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize) // 3. Pagination (تجاوز الصفحات)
            .Take(pageSize) // 3. Pagination (أخذ عدد محدد من العناصر)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                CategoryName = p.Category.Name // 2. Projection لإحضار اسم الفئة في نفس الاستعلام وتجنب مشاكل N+1 Queries[cite: 10]
            })
            .ToListAsync();

        return Ok(products);
    }

    // PUT /api/products/{id} (لتعديل الحقول المسموح بها مع إدارة Change Tracking والـ Concurrency)
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        
        if (product == null)
            return NotFound(new { message = "المنتج غير موجود" });

        // تعديل الحقول المسموح بها فقط بعناية
        product.Name = dto.Name;
        product.Price = dto.Price;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(new { message = "تم التعديل بنجاح", product });
        }
        catch (DbUpdateConcurrencyException)
        {
            // التعامل مع تعارض البيانات عبر الـ RowVersion[cite: 17]
            return Conflict(new { message = "حدث تعارض في البيانات! تم تعديل هذا المنتج من قبل مستخدم آخر." });
        }
    }
}

public class ProductUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}