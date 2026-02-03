using CompanyName.MyMeetings.Modules.Orders.Application.DTOs;
using CompanyName.MyMeetings.Modules.Orders.Domain;
using CompanyName.MyMeetings.Modules.Orders.Infrastructure.Persistence;
using CompanyName.MyMeetings.Modules.Orders.Infrastructure.Services;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.Orders.UnitTests;

[TestFixture]
public class OrderServiceTests
{
    private IOrderRepository _orderRepository = null!;
    private OrderService _orderService = null!;

    [SetUp]
    public void SetUp()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _orderService = new OrderService(_orderRepository);
    }

    [Test]
    public async Task CreateOrderAsync_WithValidRequest_ReturnsCreatedOrder()
    {
        var request = new CreateOrderRequest
        {
            CustomerId = Guid.NewGuid(),
            CustomerName = "John Doe",
            Items = new List<OrderItemRequest>
            {
                new()
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product 1",
                    Quantity = 2,
                    UnitPrice = 10.00m
                }
            },
            ShippingAddress = "123 Main St",
            Notes = "Test order"
        };

        _orderRepository.AddAsync(Arg.Any<Order>()).Returns(callInfo =>
        {
            var order = callInfo.Arg<Order>();
            return order;
        });

        var result = await _orderService.CreateOrderAsync(request);

        result.Should().NotBeNull();
        result.CustomerId.Should().Be(request.CustomerId);
        result.CustomerName.Should().Be(request.CustomerName);
        result.Items.Should().HaveCount(1);
        result.TotalAmount.Should().Be(20.00m);
        result.Status.Should().Be(OrderStatus.Pending);
        result.ShippingAddress.Should().Be(request.ShippingAddress);
        result.Notes.Should().Be(request.Notes);

        await _orderRepository.Received(1).AddAsync(Arg.Any<Order>());
    }

    [Test]
    public async Task GetOrderByIdAsync_WhenOrderExists_ReturnsOrder()
    {
        var orderId = Guid.NewGuid();
        var order = new Order
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            CustomerName = "John Doe",
            OrderDate = DateTime.UtcNow,
            Items = new List<OrderItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product 1",
                    Quantity = 1,
                    UnitPrice = 15.00m
                }
            },
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _orderRepository.GetByIdAsync(orderId).Returns(order);

        var result = await _orderService.GetOrderByIdAsync(orderId);

        result.Should().NotBeNull();
        result!.Id.Should().Be(orderId);
        result.CustomerName.Should().Be("John Doe");
    }

    [Test]
    public async Task GetOrderByIdAsync_WhenOrderDoesNotExist_ReturnsNull()
    {
        var orderId = Guid.NewGuid();
        _orderRepository.GetByIdAsync(orderId).Returns((Order?)null);

        var result = await _orderService.GetOrderByIdAsync(orderId);

        result.Should().BeNull();
    }

    [Test]
    public async Task GetAllOrdersAsync_ReturnsAllOrders()
    {
        var orders = new List<Order>
        {
            new()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                CustomerName = "Customer 1",
                OrderDate = DateTime.UtcNow,
                Items = new List<OrderItem>(),
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                CustomerName = "Customer 2",
                OrderDate = DateTime.UtcNow,
                Items = new List<OrderItem>(),
                Status = OrderStatus.Confirmed,
                CreatedAt = DateTime.UtcNow
            }
        };

        _orderRepository.GetAllAsync().Returns(orders);

        var result = await _orderService.GetAllOrdersAsync();

        result.Should().HaveCount(2);
    }

    [Test]
    public async Task GetOrdersByCustomerIdAsync_ReturnsCustomerOrders()
    {
        var customerId = Guid.NewGuid();
        var orders = new List<Order>
        {
            new()
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                CustomerName = "Customer 1",
                OrderDate = DateTime.UtcNow,
                Items = new List<OrderItem>(),
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            }
        };

        _orderRepository.GetByCustomerIdAsync(customerId).Returns(orders);

        var result = await _orderService.GetOrdersByCustomerIdAsync(customerId);

        result.Should().HaveCount(1);
        result.First().CustomerId.Should().Be(customerId);
    }

    [Test]
    public async Task UpdateOrderAsync_WhenOrderExists_ReturnsUpdatedOrder()
    {
        var orderId = Guid.NewGuid();
        var existingOrder = new Order
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            CustomerName = "John Doe",
            OrderDate = DateTime.UtcNow,
            Items = new List<OrderItem>(),
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var updateRequest = new UpdateOrderRequest
        {
            CustomerName = "Jane Doe",
            Status = OrderStatus.Confirmed
        };

        _orderRepository.GetByIdAsync(orderId).Returns(existingOrder);
        _orderRepository.UpdateAsync(Arg.Any<Order>()).Returns(callInfo =>
        {
            var order = callInfo.Arg<Order>();
            return order;
        });

        var result = await _orderService.UpdateOrderAsync(orderId, updateRequest);

        result.Should().NotBeNull();
        result!.CustomerName.Should().Be("Jane Doe");
        result.Status.Should().Be(OrderStatus.Confirmed);
        result.UpdatedAt.Should().NotBeNull();
    }

    [Test]
    public async Task UpdateOrderAsync_WhenOrderDoesNotExist_ReturnsNull()
    {
        var orderId = Guid.NewGuid();
        var updateRequest = new UpdateOrderRequest
        {
            CustomerName = "Jane Doe"
        };

        _orderRepository.GetByIdAsync(orderId).Returns((Order?)null);

        var result = await _orderService.UpdateOrderAsync(orderId, updateRequest);

        result.Should().BeNull();
    }

    [Test]
    public async Task DeleteOrderAsync_WhenOrderExists_ReturnsTrue()
    {
        var orderId = Guid.NewGuid();
        _orderRepository.DeleteAsync(orderId).Returns(true);

        var result = await _orderService.DeleteOrderAsync(orderId);

        result.Should().BeTrue();
    }

    [Test]
    public async Task DeleteOrderAsync_WhenOrderDoesNotExist_ReturnsFalse()
    {
        var orderId = Guid.NewGuid();
        _orderRepository.DeleteAsync(orderId).Returns(false);

        var result = await _orderService.DeleteOrderAsync(orderId);

        result.Should().BeFalse();
    }
}
