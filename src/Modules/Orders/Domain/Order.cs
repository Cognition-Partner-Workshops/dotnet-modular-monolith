namespace CompanyName.MyMeetings.Modules.Orders.Domain;

public class Order
{
    public Guid Id { get; set; }

    public DateTime OrderDate { get; set; }

    public Guid CustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public List<OrderItem> Items { get; set; } = new();

    public decimal TotalAmount => Items.Sum(item => item.TotalPrice);

    public OrderStatus Status { get; set; }

    public string? ShippingAddress { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
