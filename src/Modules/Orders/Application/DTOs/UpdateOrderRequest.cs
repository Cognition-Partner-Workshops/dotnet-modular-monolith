using CompanyName.MyMeetings.Modules.Orders.Domain;

namespace CompanyName.MyMeetings.Modules.Orders.Application.DTOs;

public class UpdateOrderRequest
{
    public string? CustomerName { get; set; }

    public List<OrderItemRequest>? Items { get; set; }

    public OrderStatus? Status { get; set; }

    public string? ShippingAddress { get; set; }

    public string? Notes { get; set; }
}
