using CompanyName.MyMeetings.Modules.Orders.Domain;
using CompanyName.MyMeetings.Modules.Orders.Infrastructure.Persistence;
using FluentAssertions;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.Orders.UnitTests;

[TestFixture]
public class JsonOrderRepositoryTests
{
    private string _testFilePath = null!;
    private JsonOrderRepository _repository = null!;

    [SetUp]
    public void SetUp()
    {
        _testFilePath = Path.Combine(Path.GetTempPath(), $"orders_test_{Guid.NewGuid()}.json");
        _repository = new JsonOrderRepository(_testFilePath);
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    [Test]
    public async Task AddAsync_AddsOrderToFile()
    {
        var order = CreateTestOrder();

        var result = await _repository.AddAsync(order);

        result.Should().NotBeNull();
        result.Id.Should().Be(order.Id);

        var retrievedOrder = await _repository.GetByIdAsync(order.Id);
        retrievedOrder.Should().NotBeNull();
        retrievedOrder!.CustomerName.Should().Be(order.CustomerName);
    }

    [Test]
    public async Task GetByIdAsync_WhenOrderExists_ReturnsOrder()
    {
        var order = CreateTestOrder();
        await _repository.AddAsync(order);

        var result = await _repository.GetByIdAsync(order.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(order.Id);
    }

    [Test]
    public async Task GetByIdAsync_WhenOrderDoesNotExist_ReturnsNull()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllOrders()
    {
        var order1 = CreateTestOrder();
        var order2 = CreateTestOrder();
        await _repository.AddAsync(order1);
        await _repository.AddAsync(order2);

        var result = await _repository.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Test]
    public async Task GetByCustomerIdAsync_ReturnsOrdersForCustomer()
    {
        var customerId = Guid.NewGuid();
        var order1 = CreateTestOrder(customerId);
        var order2 = CreateTestOrder(customerId);
        var order3 = CreateTestOrder();
        await _repository.AddAsync(order1);
        await _repository.AddAsync(order2);
        await _repository.AddAsync(order3);

        var result = await _repository.GetByCustomerIdAsync(customerId);

        result.Should().HaveCount(2);
        result.All(o => o.CustomerId == customerId).Should().BeTrue();
    }

    [Test]
    public async Task UpdateAsync_WhenOrderExists_UpdatesOrder()
    {
        var order = CreateTestOrder();
        await _repository.AddAsync(order);

        order.CustomerName = "Updated Name";
        order.Status = OrderStatus.Confirmed;

        var result = await _repository.UpdateAsync(order);

        result.Should().NotBeNull();
        result!.CustomerName.Should().Be("Updated Name");
        result.Status.Should().Be(OrderStatus.Confirmed);

        var retrievedOrder = await _repository.GetByIdAsync(order.Id);
        retrievedOrder!.CustomerName.Should().Be("Updated Name");
    }

    [Test]
    public async Task UpdateAsync_WhenOrderDoesNotExist_ReturnsNull()
    {
        var order = CreateTestOrder();

        var result = await _repository.UpdateAsync(order);

        result.Should().BeNull();
    }

    [Test]
    public async Task DeleteAsync_WhenOrderExists_RemovesOrderAndReturnsTrue()
    {
        var order = CreateTestOrder();
        await _repository.AddAsync(order);

        var result = await _repository.DeleteAsync(order.Id);

        result.Should().BeTrue();

        var retrievedOrder = await _repository.GetByIdAsync(order.Id);
        retrievedOrder.Should().BeNull();
    }

    [Test]
    public async Task DeleteAsync_WhenOrderDoesNotExist_ReturnsFalse()
    {
        var result = await _repository.DeleteAsync(Guid.NewGuid());

        result.Should().BeFalse();
    }

    private static Order CreateTestOrder(Guid? customerId = null)
    {
        return new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId ?? Guid.NewGuid(),
            CustomerName = "Test Customer",
            OrderDate = DateTime.UtcNow,
            Items = new List<OrderItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),
                    ProductName = "Test Product",
                    Quantity = 1,
                    UnitPrice = 10.00m
                }
            },
            Status = OrderStatus.Pending,
            ShippingAddress = "123 Test St",
            Notes = "Test notes",
            CreatedAt = DateTime.UtcNow
        };
    }
}
