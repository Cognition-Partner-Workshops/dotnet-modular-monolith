# Product Details Module

A standalone .NET Core module for managing product information with CRUD operations and file-based JSON storage.

## Module Structure

```
src/Modules/ProductDetails/
├── CompanyName.MyMeetings.Modules.ProductDetails/
│   ├── Controllers/
│   │   └── ProductController.cs       # REST API endpoints
│   ├── Models/
│   │   ├── Product.cs                 # Product entity
│   │   ├── CreateProductRequest.cs    # DTO for creating products
│   │   └── UpdateProductRequest.cs    # DTO for updating products
│   ├── Services/
│   │   ├── IProductService.cs         # Service interface
│   │   └── ProductService.cs          # Service implementation with JSON storage
│   ├── Data/
│   │   └── products.json              # JSON file storage (auto-created)
│   └── ProductDetailsModule.cs        # Module registration extension
└── Tests/
    └── CompanyName.MyMeetings.Modules.ProductDetails.Tests/
        └── ProductServiceTests.cs     # Unit tests for CRUD operations
```

## Product Entity

The Product model includes the following properties:

| Property | Type | Description |
|----------|------|-------------|
| Id | Guid | Unique identifier (auto-generated) |
| Name | string | Product name (required, max 200 chars) |
| Description | string | Product description (max 2000 chars) |
| Price | decimal | Product price (required, must be > 0) |
| Category | string | Product category (max 100 chars) |
| StockQuantity | int | Available stock quantity |
| SKU | string | Stock Keeping Unit (max 50 chars) |
| IsActive | bool | Whether the product is active |
| CreatedAt | DateTime | Creation timestamp (UTC) |
| UpdatedAt | DateTime? | Last update timestamp (UTC) |

## API Endpoints

### Base URL: `/api/Product`

| Method | Endpoint | Description | Request Body | Response |
|--------|----------|-------------|--------------|----------|
| GET | `/api/Product` | Get all products | - | `200 OK` with array of products |
| GET | `/api/Product/{id}` | Get product by ID | - | `200 OK` with product or `404 Not Found` |
| POST | `/api/Product` | Create a new product | CreateProductRequest | `201 Created` with product |
| PUT | `/api/Product/{id}` | Update an existing product | UpdateProductRequest | `200 OK` with product or `404 Not Found` |
| DELETE | `/api/Product/{id}` | Delete a product | - | `204 No Content` or `404 Not Found` |

### Request/Response Examples

#### Create Product (POST /api/Product)

Request:
```json
{
  "name": "Laptop",
  "description": "High-performance laptop",
  "price": 999.99,
  "category": "Electronics",
  "stockQuantity": 50,
  "sku": "LAPTOP-001",
  "isActive": true
}
```

Response (201 Created):
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Laptop",
  "description": "High-performance laptop",
  "price": 999.99,
  "category": "Electronics",
  "stockQuantity": 50,
  "sku": "LAPTOP-001",
  "isActive": true,
  "createdAt": "2026-02-03T12:00:00Z",
  "updatedAt": null
}
```

#### Update Product (PUT /api/Product/{id})

Request:
```json
{
  "name": "Updated Laptop",
  "description": "Updated description",
  "price": 1099.99,
  "category": "Electronics",
  "stockQuantity": 45,
  "sku": "LAPTOP-001",
  "isActive": true
}
```

## Module Registration

To register the Product Details module in your application, add the following to your `Program.cs` or `Startup.cs`:

```csharp
using CompanyName.MyMeetings.Modules.ProductDetails;

// Register the module with default data file path
services.AddProductDetailsModule();

// Or specify a custom data file path
services.AddProductDetailsModule("/path/to/products.json");
```

## Data Storage

The module uses file-based JSON storage for product data persistence. By default, the data file is created at:
`{AppDomain.CurrentDomain.BaseDirectory}/Data/products.json`

The storage implementation includes:
- Thread-safe file operations using SemaphoreSlim
- Automatic directory and file creation
- JSON serialization with camelCase naming

## Running Tests

To run the unit tests:

```bash
cd src/Modules/ProductDetails/Tests/CompanyName.MyMeetings.Modules.ProductDetails.Tests
dotnet test
```

All 12 unit tests cover:
- Create product functionality
- Read all products
- Read product by ID
- Update product
- Delete product
- Data persistence verification

## Integration Notes

This module is designed to be integrated with the Order Details module. The Product entity can be referenced by order items using the Product ID.
