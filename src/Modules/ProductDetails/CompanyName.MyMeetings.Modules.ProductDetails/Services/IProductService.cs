using CompanyName.MyMeetings.Modules.ProductDetails.Models;

namespace CompanyName.MyMeetings.Modules.ProductDetails.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();

    Task<Product?> GetProductByIdAsync(Guid id);

    Task<Product> CreateProductAsync(CreateProductRequest request);

    Task<Product?> UpdateProductAsync(Guid id, UpdateProductRequest request);

    Task<bool> DeleteProductAsync(Guid id);
}
