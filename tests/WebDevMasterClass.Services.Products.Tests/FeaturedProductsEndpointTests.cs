using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using WebDevMasterClass.Services.Products.Data;
using WebDevMasterClass.Services.Products.Tests.Data;
using WebDevMasterClass.Testing;

namespace WebDevMasterClass.Services.Products.Tests;

public class FeaturedProductsEndpointTests
{
    [Fact]
    public Task GET_Returns_HTTP_200_and_all_products_marked_as_featured()
        => TestHelper.ExecuteTest<Program, ProductsContext>(
                dbSetup: async cmd =>
                {
                    await cmd.AddProduct("Product 1", "Description 1", 100m, true, "product1");
                    await cmd.AddProduct("Product 2", "Description 2", 200m, true, "product2");
                    await cmd.AddProduct("Product 3", "Description 3", 300m, true, "product3");
                    await cmd.AddProduct("Product 4", "Description 4", 50m, false, "product4");
                },
                test: async client =>
                {
                    var response = await client.GetAsync("/api/products/featured");
                    
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    
                    var products = JArray.Parse(await response.Content.ReadAsStringAsync());
                    
                    Assert.Equal(3, products.Count);
                    
                    Assert.Contains(products, x => x.Value<string>("name") == "Product 1");
                    Assert.Contains(products, x => x.Value<string>("name") == "Product 2");
                    Assert.Contains(products, x => x.Value<string>("name") == "Product 3");
                }
            );
}