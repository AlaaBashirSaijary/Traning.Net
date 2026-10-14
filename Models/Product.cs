using System.ComponentModel.DataAnnotations;

namespace VersioningDemo.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    
    // خاصية الـ Soft Delete[cite: 20]
    public bool IsDeleted { get; set; }
    
    // العلاقة مع الفئة (Category)
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    // خاصية الـ Concurrency Token لمنع التعارض[cite: 17]
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
}