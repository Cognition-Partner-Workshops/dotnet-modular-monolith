using CompanyName.MyMeetings.Modules.ProductDetails.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.MyMeetings.Modules.ProductDetails;

public static class ProductDetailsModule
{
    public static IServiceCollection AddProductDetailsModule(this IServiceCollection services, string? dataFilePath = null)
    {
        if (string.IsNullOrEmpty(dataFilePath))
        {
            services.AddSingleton<IProductService, ProductService>();
        }
        else
        {
            services.AddSingleton<IProductService>(provider => new ProductService(dataFilePath));
        }

        return services;
    }
}
