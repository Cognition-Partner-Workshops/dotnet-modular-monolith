using System.ComponentModel.DataAnnotations;

namespace CompanyName.MyMeetings.Modules.ProductDetails.Models;

public class UpdateProductRequest
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [StringLength(100)]
    public string Category { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [StringLength(50)]
    public string SKU { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
