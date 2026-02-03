using System.Text.Json;
using CompanyName.MyMeetings.Modules.Orders.Domain;

namespace CompanyName.MyMeetings.Modules.Orders.Infrastructure.Persistence;

public class JsonOrderRepository : IOrderRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public JsonOrderRepository(string? filePath = null)
    {
        _filePath = filePath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "orders.json");
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        EnsureDataDirectoryExists();
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        var orders = await LoadOrdersAsync();
        return orders.FirstOrDefault(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await LoadOrdersAsync();
    }

    public async Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId)
    {
        var orders = await LoadOrdersAsync();
        return orders.Where(o => o.CustomerId == customerId);
    }

    public async Task<Order> AddAsync(Order order)
    {
        await _lock.WaitAsync();
        try
        {
            var orders = await LoadOrdersAsync();
            var ordersList = orders.ToList();
            ordersList.Add(order);
            await SaveOrdersAsync(ordersList);
            return order;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<Order?> UpdateAsync(Order order)
    {
        await _lock.WaitAsync();
        try
        {
            var orders = await LoadOrdersAsync();
            var ordersList = orders.ToList();
            var existingIndex = ordersList.FindIndex(o => o.Id == order.Id);

            if (existingIndex == -1)
            {
                return null;
            }

            ordersList[existingIndex] = order;
            await SaveOrdersAsync(ordersList);
            return order;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        await _lock.WaitAsync();
        try
        {
            var orders = await LoadOrdersAsync();
            var ordersList = orders.ToList();
            var order = ordersList.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return false;
            }

            ordersList.Remove(order);
            await SaveOrdersAsync(ordersList);
            return true;
        }
        finally
        {
            _lock.Release();
        }
    }

    private void EnsureDataDirectoryExists()
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    private async Task<IEnumerable<Order>> LoadOrdersAsync()
    {
        if (!File.Exists(_filePath))
        {
            return Enumerable.Empty<Order>();
        }

        var json = await File.ReadAllTextAsync(_filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return Enumerable.Empty<Order>();
        }

        return JsonSerializer.Deserialize<List<Order>>(json, _jsonOptions) ?? new List<Order>();
    }

    private async Task SaveOrdersAsync(IEnumerable<Order> orders)
    {
        EnsureDataDirectoryExists();
        var json = JsonSerializer.Serialize(orders, _jsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }
}
