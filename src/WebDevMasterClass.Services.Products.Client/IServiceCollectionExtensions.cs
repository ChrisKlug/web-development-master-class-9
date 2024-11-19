using WebDevMasterClass.Services.Products.Client;

namespace Microsoft.Extensions.DependencyInjection;
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddProductsClient(this IServiceCollection services, Action<ProductsClientOptions> config)
    {
        var options = new ProductsClientOptions();
        config(options);

        ArgumentNullException.ThrowIfNull(options.BaseAddress, nameof(options.BaseAddress));

        services.AddHttpClient<IProductsClient, HttpProductsClient>(client =>
        {
            client.BaseAddress = new Uri(options.BaseAddress);
        });

        return services;
    }
}

public class ProductsClientOptions
{
    public string? BaseAddress { get; set; }
}