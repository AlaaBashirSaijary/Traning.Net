using Microsoft.EntityFrameworkCore;
using VersioningDemo.Models;

namespace VersioningDemo.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // تعريف الجداول في قاعدة البيانات
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 3. إنشاء Index مناسب على حقل Name لأنه يُستخدم كثيراً في البحث لتسريع الاستعلامات[cite: 19]
        modelBuilder.Entity<Product>().HasIndex(p => p.Name);

        // 4. تطبيق Soft Delete باستخدام Global Query Filter لاستبعاد المحذوفين تلقائياً[cite: 20]
        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
    }
}
//سبب الاختيار: تم اختيار حقل الاسم (Name) لأنه الحقل الأساسي والأكثر استخداماً في عمليات البحث، التصفية (Filtering)، والفلترة عن المنتجات من قبل المستخدمين.