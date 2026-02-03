using CompanyName.MyMeetings.Modules.ProductDetails.Models;
using CompanyName.MyMeetings.Modules.ProductDetails.Services;
using FluentAssertions;

namespace CompanyName.MyMeetings.Modules.ProductDetails.Tests;

public class ProductServiceTests : IDisposable
{
    private readonly string _testDataFilePath;

    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _testDataFilePath = Path.Combine(Path.GetTempPath(), $"products_test_{Guid.NewGuid()}.json");
        _productService = new ProductService(_testDataFilePath);
    }

    public void Dispose()
    {
        if (File.Exists(_testDataFilePath))
        {
            File.Delete(_testDataFilePath);
        }
    }

    [Fact]
    public async Task CreateProductAsync_WithValidRequest_ShouldCreateProduct()
    {
        var request = CreateValidProductRequest();

        var result = await _productService.CreateProductAsync(request);

        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Name.Should().Be(request.Name);
        result.Description.Should().Be(request.Description);
        result.Price.Should().Be(request.Price);
        result.Category.Should().Be(request.Category);
        result.StockQuantity.Should().Be(request.StockQuantity);
        result.SKU.Should().Be(request.SKU);
        result.IsActive.Should().Be(request.IsActive);
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        result.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public async Task GetAllProductsAsync_WhenNoProducts_ShouldReturnEmptyList()
    {
        var result = await _productService.GetAllProductsAsync();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllProductsAsync_WithProducts_ShouldReturnAllProducts()
    {
        await _productService.CreateProductAsync(CreateValidProductRequest("Product 1"));
        await _productService.CreateProductAsync(CreateValidProductRequest("Product 2"));
        await _productService.CreateProductAsync(CreateValidProductRequest("Product 3"));

        var result = await _productService.GetAllProductsAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetProductByIdAsync_WithExistingId_ShouldReturnProduct()
    {
        var createdProduct = await _productService.CreateProductAsync(CreateValidProductRequest());

        var result = await _productService.GetProductByIdAsync(createdProduct.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(createdProduct.Id);
        result.Name.Should().Be(createdProduct.Name);
    }

    [Fact]
    public async Task GetProductByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        var nonExistingId = Guid.NewGuid();

        var result = await _productService.GetProductByIdAsync(nonExistingId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateProductAsync_WithExistingId_ShouldUpdateProduct()
    {
        var createdProduct = await _productService.CreateProductAsync(CreateValidProductRequest());
        var updateRequest = new UpdateProductRequest
        {
            Name = "Updated Product Name",
            Description = "Updated Description",
            Price = 149.99m,
            Category = "Updated Category",
            StockQuantity = 200,
            SKU = "UPDATED-SKU",
            IsActive = false
        };

        var result = await _productService.UpdateProductAsync(createdProduct.Id, updateRequest);

        result.Should().NotBeNull();
        result!.Id.Should().Be(createdProduct.Id);
        result.Name.Should().Be(updateRequest.Name);
        result.Description.Should().Be(updateRequest.Description);
        result.Price.Should().Be(updateRequest.Price);
        result.Category.Should().Be(updateRequest.Category);
        result.StockQuantity.Should().Be(updateRequest.StockQuantity);
        result.SKU.Should().Be(updateRequest.SKU);
        result.IsActive.Should().Be(updateRequest.IsActive);
        result.UpdatedAt.Should().NotBeNull();
        result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task UpdateProductAsync_WithNonExistingId_ShouldReturnNull()
    {
        var nonExistingId = Guid.NewGuid();
        var updateRequest = new UpdateProductRequest
        {
            Name = "Updated Product Name",
            Description = "Updated Description",
            Price = 149.99m,
            Category = "Updated Category",
            StockQuantity = 200,
            SKU = "UPDATED-SKU",
            IsActive = false
        };

        var result = await _productService.UpdateProductAsync(nonExistingId, updateRequest);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteProductAsync_WithExistingId_ShouldDeleteProduct()
    {
        var createdProduct = await _productService.CreateProductAsync(CreateValidProductRequest());

        var deleteResult = await _productService.DeleteProductAsync(createdProduct.Id);
        var getResult = await _productService.GetProductByIdAsync(createdProduct.Id);

        deleteResult.Should().BeTrue();
        getResult.Should().BeNull();
    }

    [Fact]
    public async Task DeleteProductAsync_WithNonExistingId_ShouldReturnFalse()
    {
        var nonExistingId = Guid.NewGuid();

        var result = await _productService.DeleteProductAsync(nonExistingId);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task CreateProductAsync_ShouldPersistToFile()
    {
        var request = CreateValidProductRequest();

        var createdProduct = await _productService.CreateProductAsync(request);

        var newServiceInstance = new ProductService(_testDataFilePath);
        var retrievedProduct = await newServiceInstance.GetProductByIdAsync(createdProduct.Id);

        retrievedProduct.Should().NotBeNull();
        retrievedProduct!.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldPersistChangesToFile()
    {
        var createdProduct = await _productService.CreateProductAsync(CreateValidProductRequest());
        var updateRequest = new UpdateProductRequest
        {
            Name = "Persisted Update",
            Description = "Persisted Description",
            Price = 199.99m,
            Category = "Persisted Category",
            StockQuantity = 50,
            SKU = "PERSIST-SKU",
            IsActive = true
        };

        await _productService.UpdateProductAsync(createdProduct.Id, updateRequest);

        var newServiceInstance = new ProductService(_testDataFilePath);
        var retrievedProduct = await newServiceInstance.GetProductByIdAsync(createdProduct.Id);

        retrievedProduct.Should().NotBeNull();
        retrievedProduct!.Name.Should().Be(updateRequest.Name);
        retrievedProduct.Price.Should().Be(updateRequest.Price);
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldPersistDeletionToFile()
    {
        var createdProduct = await _productService.CreateProductAsync(CreateValidProductRequest());

        await _productService.DeleteProductAsync(createdProduct.Id);

        var newServiceInstance = new ProductService(_testDataFilePath);
        var retrievedProduct = await newServiceInstance.GetProductByIdAsync(createdProduct.Id);

        retrievedProduct.Should().BeNull();
    }

    private static CreateProductRequest CreateValidProductRequest(string name = "Test Product")
    {
        return new CreateProductRequest
        {
            Name = name,
            Description = "Test Description",
            Price = 99.99m,
            Category = "Electronics",
            StockQuantity = 100,
            SKU = "TEST-SKU-001",
            IsActive = true
        };
    }
}
