# Order Details Module

This module provides a standalone Order Details service with basic CRUD functionality for managing orders. It uses file-based storage (JSON file) for order data persistence.

## Module Structure

The module follows the standard modular monolith architecture with the following layers:

```
Modules/Orders/
├── Domain/                    # Domain entities and enums
│   ├── Order.cs              # Main Order entity
│   ├── OrderItem.cs          # Order item entity
│   └── OrderStatus.cs        # Order status enumeration
├── Application/              # Application layer with DTOs and contracts
│   ├── Contracts/
│   │   └── IOrderService.cs  # Service interface
│   └── DTOs/
│       ├── CreateOrderRequest.cs
│       ├── UpdateOrderRequest.cs
│       └── OrderResponse.cs
├── Infrastructure/           # Infrastructure implementation
│   ├── Persistence/
│   │   ├── IOrderRepository.cs
│   │   └── JsonOrderRepository.cs  # JSON file-based storage
│   └── Services/
│       └── OrderService.cs   # Service implementation
└── Tests/
    └── UnitTests/            # Unit tests for CRUD operations
        ├── OrderServiceTests.cs
        └── JsonOrderRepositoryTests.cs
```

## Domain Models

### Order

The main entity representing an order with the following properties:

| Property | Type | Description |
|----------|------|-------------|
| Id | Guid | Unique identifier |
| OrderDate | DateTime | Date when the order was placed |
| CustomerId | Guid | Customer identifier |
| CustomerName | string | Customer name |
| Items | List<OrderItem> | List of order items |
| TotalAmount | decimal | Calculated total (sum of item totals) |
| Status | OrderStatus | Current order status |
| ShippingAddress | string? | Shipping address |
| Notes | string? | Additional notes |
| CreatedAt | DateTime | Creation timestamp |
| UpdatedAt | DateTime? | Last update timestamp |

### OrderItem

Represents an item within an order:

| Property | Type | Description |
|----------|------|-------------|
| Id | Guid | Unique identifier |
| ProductId | Guid | Product identifier |
| ProductName | string | Product name |
| Quantity | int | Quantity ordered |
| UnitPrice | decimal | Price per unit |
| TotalPrice | decimal | Calculated total (Quantity * UnitPrice) |

### OrderStatus

Enumeration of possible order statuses:

- Pending (0)
- Confirmed (1)
- Processing (2)
- Shipped (3)
- Delivered (4)
- Cancelled (5)

## API Endpoints

The OrdersController exposes the following REST API endpoints:

### Get All Orders
```
GET /api/orders
```
Returns a list of all orders.

**Response:** `200 OK` with `IEnumerable<OrderResponse>`

### Get Order by ID
```
GET /api/orders/{id}
```
Returns a specific order by its ID.

**Response:** 
- `200 OK` with `OrderResponse`
- `404 Not Found` if order doesn't exist

### Get Orders by Customer ID
```
GET /api/orders/customer/{customerId}
```
Returns all orders for a specific customer.

**Response:** `200 OK` with `IEnumerable<OrderResponse>`

### Create Order
```
POST /api/orders
```
Creates a new order.

**Request Body:**
```json
{
  "customerId": "guid",
  "customerName": "string",
  "items": [
    {
      "productId": "guid",
      "productName": "string",
      "quantity": 1,
      "unitPrice": 10.00
    }
  ],
  "shippingAddress": "string",
  "notes": "string"
}
```

**Response:** 
- `201 Created` with `OrderResponse`
- `400 Bad Request` if no items provided

### Update Order
```
PUT /api/orders/{id}
```
Updates an existing order.

**Request Body:**
```json
{
  "customerName": "string",
  "items": [...],
  "status": 1,
  "shippingAddress": "string",
  "notes": "string"
}
```
All fields are optional - only provided fields will be updated.

**Response:** 
- `200 OK` with `OrderResponse`
- `404 Not Found` if order doesn't exist

### Delete Order
```
DELETE /api/orders/{id}
```
Deletes an order.

**Response:** 
- `204 No Content` on success
- `404 Not Found` if order doesn't exist

## Data Storage

Orders are persisted to a JSON file located at `{AppDomain.CurrentDomain.BaseDirectory}/Data/orders.json`. The repository uses a semaphore for thread-safe file operations.

## Dependency Injection

The module uses Autofac for dependency injection. Register the module in your application startup:

```csharp
builder.RegisterModule<OrdersAutofacModule>();
```

This registers:
- `JsonOrderRepository` as `IOrderRepository` (Singleton)
- `OrderService` as `IOrderService` (InstancePerLifetimeScope)

## Unit Tests

The module includes comprehensive unit tests covering:

- OrderService CRUD operations (9 tests)
- JsonOrderRepository file operations (9 tests)

Run tests with:
```bash
dotnet test Modules/Orders/Tests/UnitTests/CompanyName.MyMeetings.Modules.Orders.UnitTests.csproj
```

## Integration with Product Details Module

This Order Details module is designed to be integrated with the Product Details module. The `OrderItem.ProductId` property can be used to reference products from the Product Details module.
