using CompanyName.MyMeetings.Modules.Orders.Application.Contracts;
using CompanyName.MyMeetings.Modules.Orders.Application.DTOs;
using CompanyName.MyMeetings.Modules.Orders.Domain;
using CompanyName.MyMeetings.Modules.Orders.Infrastructure.Persistence;

namespace CompanyName.MyMeetings.Modules.Orders.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            OrderDate = DateTime.UtcNow,
            CustomerId = request.CustomerId,
            CustomerName = request.CustomerName,
            Items = request.Items.Select(item => new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList(),
            Status = OrderStatus.Pending,
            ShippingAddress = request.ShippingAddress,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        var createdOrder = await _orderRepository.AddAsync(order);
        return MapToResponse(createdOrder);
    }

    public async Task<OrderResponse?> GetOrderByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        return order == null ? null : MapToResponse(order);
    }

    public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Select(MapToResponse);
    }

    public async Task<IEnumerable<OrderResponse>> GetOrdersByCustomerIdAsync(Guid customerId)
    {
        var orders = await _orderRepository.GetByCustomerIdAsync(customerId);
        return orders.Select(MapToResponse);
    }

    public async Task<OrderResponse?> UpdateOrderAsync(Guid id, UpdateOrderRequest request)
    {
        var existingOrder = await _orderRepository.GetByIdAsync(id);
        if (existingOrder == null)
        {
            return null;
        }

        if (request.CustomerName != null)
        {
            existingOrder.CustomerName = request.CustomerName;
        }

        if (request.Items != null)
        {
            existingOrder.Items = request.Items.Select(item => new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList();
        }

        if (request.Status.HasValue)
        {
            existingOrder.Status = request.Status.Value;
        }

        if (request.ShippingAddress != null)
        {
            existingOrder.ShippingAddress = request.ShippingAddress;
        }

        if (request.Notes != null)
        {
            existingOrder.Notes = request.Notes;
        }

        existingOrder.UpdatedAt = DateTime.UtcNow;

        var updatedOrder = await _orderRepository.UpdateAsync(existingOrder);
        return updatedOrder == null ? null : MapToResponse(updatedOrder);
    }

    public async Task<bool> DeleteOrderAsync(Guid id)
    {
        return await _orderRepository.DeleteAsync(id);
    }

    private static OrderResponse MapToResponse(Order order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            CustomerId = order.CustomerId,
            CustomerName = order.CustomerName,
            Items = order.Items.Select(item => new OrderItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.TotalPrice
            }).ToList(),
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            ShippingAddress = order.ShippingAddress,
            Notes = order.Notes,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };
    }
}
