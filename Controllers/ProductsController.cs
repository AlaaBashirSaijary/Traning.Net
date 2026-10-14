using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VersioningDemo.Data;
using VersioningDemo.Models;

namespace VersioningDemo.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [ApiVersion(2.0)]
    [Route("api/products")] // المسار الأساسي الموحد والمباشر
    [Route("api/v{version:apiVersion}/products")] // دعم مسار الإصدارات
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. GET V1: جلب البيانات الأساسية
        // ==========================================
        [HttpGet]
        [MapToApiVersion(1.0)]
        public async Task<IActionResult> GetProductsV1([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var products = await _context.Products
                .AsNoTracking()
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price
                })
                .ToListAsync();

            return Ok(products);
        }

        // ==========================================
        // 2. GET V2: جلب البيانات مع اسم الفئة (Projection)
        // ==========================================
        [HttpGet]
        [MapToApiVersion(2.0)]
        public async Task<IActionResult> GetProductsV2([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var products = await _context.Products
                .AsNoTracking()
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    CategoryName = p.Category != null ? p.Category.Name : "Without Category"
                })
                .ToListAsync();

            return Ok(products);
        }

        // ==========================================
        // 3. PUT: التعديل المحمي والتعامل مع Concurrency
        // ==========================================
      // ==========================================
        // 3. PUT: التعديل المحمي والتعامل مع Concurrency
        // ==========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            
            if (product == null)
                return NotFound(new { message = "المنتج غير موجود" });

            product.Name = dto.Name;
            product.Price = dto.Price;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "تم التعديل بنجاح", product });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "حدث تعارض في البيانات! تم تعديل هذا المنتج من قبل مستخدم آخر." });
            }
        }

        // ==========================================
        // 4. DELETE: الحذف الناعم (Soft Delete)
        // ==========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound(new { message = "المنتج غير موجود" });

            // تطبيق الحذف الناعم (Soft Delete)
            product.IsDeleted = true;

            await _context.SaveChangesAsync();

            return Ok(new { message = "تم تنفيذ الحذف الناعم (Soft Delete) بنجاح وإخفاء المنتج." });
        }
    }

    public class ProductUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}