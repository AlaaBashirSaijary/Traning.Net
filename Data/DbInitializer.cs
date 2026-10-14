using VersioningDemo.Models;

namespace VersioningDemo.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        // التأكد من إنشائها (مفيدة لقاعدة البيانات الـ InMemory)
        context.Database.EnsureCreated();

        // إذا كانت المنتجات موجودة مسبقاً، لا تقم بإعادة الإدخال
        if (context.Products.Any())
        {
            return; 
        }

        // 1. إنشاء فئات (Categories) وهمية
        var categories = new Category[]
        {
            new Category { Name = "Electronics" },
            new Category { Name = "Clothing" },
            new Category { Name = "Books" }
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();

        // 2. إنشاء منتجات (Products) وهمية مربوطة بالفئات
        var products = new Product[]
        {
            new Product { Name = "Laptop", Price = 25000, CategoryId = categories[0].Id },
            new Product { Name = "Smartphone", Price = 18000, CategoryId = categories[0].Id },
            new Product { Name = "Wireless Mouse", Price = 450, CategoryId = categories[0].Id },
            new Product { Name = "T-Shirt", Price = 350, CategoryId = categories[1].Id },
            new Product { Name = "C# Programming Guide", Price = 250, CategoryId = categories[2].Id }
        };

        context.Products.AddRange(products);
        context.SaveChanges();
    }
}