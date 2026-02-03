using CompanyName.MyMeetings.Modules.Orders.Domain;

namespace CompanyName.MyMeetings.Modules.Orders.Application.DTOs;

public class OrderResponse
{
    public Guid Id { get; set; }

    public DateTime OrderDate { get; set; }

    public Guid CustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public List<OrderItemResponse> Items { get; set; } = new();

    public decimal TotalAmount { get; set; }

    public OrderStatus Status { get; set; }

    public string StatusName => Status.ToString();

    public string? ShippingAddress { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public class OrderItemResponse
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}
