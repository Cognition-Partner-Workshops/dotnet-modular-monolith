namespace CompanyName.MyMeetings.Modules.Orders.Application.DTOs;

public class CreateOrderRequest
{
    public Guid CustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public List<OrderItemRequest> Items { get; set; } = new();

    public string? ShippingAddress { get; set; }

    public string? Notes { get; set; }
}

public class OrderItemRequest
{
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}
