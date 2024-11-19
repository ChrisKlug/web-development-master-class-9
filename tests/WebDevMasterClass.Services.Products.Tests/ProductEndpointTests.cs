using Newtonsoft.Json.Linq;
using System.Net;
using WebDevMasterClass.Services.Products.Data;
using WebDevMasterClass.Services.Products.Tests.Data;
using WebDevMasterClass.Testing;

namespace WebDevMasterClass.Services.Products.Tests;

public class ProductEndpointTests
{
    [Fact]
    public Task GET_Returns_HTTP_200_and_the_requested_product()
    {
        int productId = default;
        return TestHelper.ExecuteTest<Program, ProductsContext>(
                    dbSetup: async cmd =>
                    {
                        productId = await cmd.AddProduct("Product 1", "Description 1", 100m, true, "product1");
                    },
                    test: async client =>
                    {
                        var response = await client.GetAsync($"/api/products/{productId}");

                        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                        dynamic json = JObject.Parse(await response.Content.ReadAsStringAsync());

                        Assert.Equal(productId, (int)json.id);
                        Assert.Equal("Product 1", (string)json.name);
                        Assert.Equal("Description 1", (string)json.description);
                        Assert.Equal(100m, (decimal)json.price);
                        Assert.True((bool)json.isFeatured);
                        Assert.Equal("product1_thumbnail.jpg", (string)json.thumbnailUrl);
                        Assert.Equal("product1.jpg", (string)json.imageUrl);
                    }
                );
    }
}
