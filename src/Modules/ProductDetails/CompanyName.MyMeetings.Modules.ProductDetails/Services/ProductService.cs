using System.Text.Json;
using CompanyName.MyMeetings.Modules.ProductDetails.Models;

namespace CompanyName.MyMeetings.Modules.ProductDetails.Services;

public class ProductService : IProductService
{
    private readonly string _dataFilePath;

    private readonly JsonSerializerOptions _jsonOptions;

    private readonly SemaphoreSlim _fileLock = new(1, 1);

    public ProductService(string? dataFilePath = null)
    {
        _dataFilePath = dataFilePath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "products.json");
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        EnsureDataFileExists();
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await ReadProductsFromFileAsync();
    }

    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        var products = await ReadProductsFromFileAsync();
        return products.FirstOrDefault(p => p.Id == id);
    }

    public async Task<Product> CreateProductAsync(CreateProductRequest request)
    {
        var products = await ReadProductsFromFileAsync();

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Category = request.Category,
            StockQuantity = request.StockQuantity,
            SKU = request.SKU,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        products.Add(product);
        await WriteProductsToFileAsync(products);

        return product;
    }

    public async Task<Product?> UpdateProductAsync(Guid id, UpdateProductRequest request)
    {
        var products = await ReadProductsFromFileAsync();
        var existingProduct = products.FirstOrDefault(p => p.Id == id);

        if (existingProduct == null)
        {
            return null;
        }

        existingProduct.Name = request.Name;
        existingProduct.Description = request.Description;
        existingProduct.Price = request.Price;
        existingProduct.Category = request.Category;
        existingProduct.StockQuantity = request.StockQuantity;
        existingProduct.SKU = request.SKU;
        existingProduct.IsActive = request.IsActive;
        existingProduct.UpdatedAt = DateTime.UtcNow;

        await WriteProductsToFileAsync(products);

        return existingProduct;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var products = await ReadProductsFromFileAsync();
        var productToRemove = products.FirstOrDefault(p => p.Id == id);

        if (productToRemove == null)
        {
            return false;
        }

        products.Remove(productToRemove);
        await WriteProductsToFileAsync(products);

        return true;
    }

    private void EnsureDataFileExists()
    {
        var directory = Path.GetDirectoryName(_dataFilePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(_dataFilePath))
        {
            File.WriteAllText(_dataFilePath, "[]");
        }
    }

    private async Task<List<Product>> ReadProductsFromFileAsync()
    {
        await _fileLock.WaitAsync();
        try
        {
            var json = await File.ReadAllTextAsync(_dataFilePath);
            return JsonSerializer.Deserialize<List<Product>>(json, _jsonOptions) ?? new List<Product>();
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private async Task WriteProductsToFileAsync(List<Product> products)
    {
        await _fileLock.WaitAsync();
        try
        {
            var json = JsonSerializer.Serialize(products, _jsonOptions);
            await File.WriteAllTextAsync(_dataFilePath, json);
        }
        finally
        {
            _fileLock.Release();
        }
    }
}
