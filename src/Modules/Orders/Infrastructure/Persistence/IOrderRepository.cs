using CompanyName.MyMeetings.Modules.Orders.Domain;

namespace CompanyName.MyMeetings.Modules.Orders.Infrastructure.Persistence;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);

    Task<IEnumerable<Order>> GetAllAsync();

    Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId);

    Task<Order> AddAsync(Order order);

    Task<Order?> UpdateAsync(Order order);

    Task<bool> DeleteAsync(Guid id);
}
