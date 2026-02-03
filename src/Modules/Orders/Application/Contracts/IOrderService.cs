using CompanyName.MyMeetings.Modules.Orders.Application.DTOs;

namespace CompanyName.MyMeetings.Modules.Orders.Application.Contracts;

public interface IOrderService
{
    Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request);

    Task<OrderResponse?> GetOrderByIdAsync(Guid id);

    Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();

    Task<IEnumerable<OrderResponse>> GetOrdersByCustomerIdAsync(Guid customerId);

    Task<OrderResponse?> UpdateOrderAsync(Guid id, UpdateOrderRequest request);

    Task<bool> DeleteOrderAsync(Guid id);
}
