using System.Net.Http.Json;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using WebDevMasterClass.Services.Products.Client;
using WebDevMasterClass.Testing;
using WebDevMasterClass.Web.ShoppingCart;

namespace WebDevMasterClass.Web.Tests;

public class ShoppingCartTests
{
    public class GetShoppingCart
    {
        [Fact]
        public Task Gets_empty_shopping_cart_by_default()
            => ExecuteTest(
                serviceConfig: (grainFactory, _) =>
                {
                    var grainFake = A.Fake<IShoppingCart>();
                    A.CallTo(() => grainFake.GetItems()).Returns([ new ShoppingCartItem {
                        ProductId = 1,
                        ProductName = "Test Product",
                        Count = 1,
                        Price = 1.23m
                    }]);

                    A.CallTo(() => grainFactory.GetGrain<IShoppingCart>("TestCart", null)).Returns(grainFake);
                },
                test: async client =>
                {
                    client.DefaultRequestHeaders.Add("Cookie", "ShoppingCartId=TestCart");
                    var response = await client.GetAsync("/api/shopping-cart");

                    response.EnsureSuccessStatusCode();

                    Assert.Equal("application/json", response.Content.Headers.ContentType!.MediaType);

                    var shoppingCart = JArray.Parse(await response.Content.ReadAsStringAsync());

                    var item = (JObject)Assert.Single(shoppingCart);

                    Assert.Equal(1, item["productId"]);
                    Assert.Equal("Test Product", item["productName"]);
                    Assert.Equal(1, item["count"]);
                    Assert.Equal(1.23m, item["price"]);
                }
            );
        
        [Fact]
        public Task Gets_shopping_cart_from_grain_if_ShoppingCartId_cookie_exists()
        {
            var grainFake = A.Fake<IShoppingCart>();

            return ExecuteTest(
                serviceConfig: (grainFactory, productsClient) =>
                {
                    A.CallTo(() => grainFactory.GetGrain<IShoppingCart>("TestCart", null)).Returns(grainFake);

                    A.CallTo(() => productsClient.GetProduct(1)).Returns(
                        new Product(1, "Test Product", "A Test Product", 1.23m, false, "thumbnail.jpg", "image.jpg")
                    );
                },
                test: async client =>
                {
                    client.DefaultRequestHeaders.Add("Cookie", "ShoppingCartId=TestCart");
                    var response = await client.PostAsJsonAsync("/api/shopping-cart", new { ProductId = 1, Count = 1 });

                    response.EnsureSuccessStatusCode();

                    A.CallTo(() => grainFake.AddItem(A<ShoppingCartItem>
                            .That.Matches(x => x.ProductId == 1 && x.Price == 1.23m && x.Count == 1)))
                        .MustHaveHappenedOnceExactly();
                }
            );
        }
    }
    
    public class AddShoppingCartItem
    {
        [Fact]
        public Task Adds_item_to_grain_with_id_from_ShoppingCartId_cookie()
        {
            var grainFake = A.Fake<IShoppingCart>();

            var grainFactoryFake = A.Fake<IGrainFactory>();
            A.CallTo(() => grainFactoryFake.GetGrain<IShoppingCart>("TestCart", null)).Returns(grainFake);

            var productsClientFake = A.Fake<IProductsClient>();
            A.CallTo(() => productsClientFake.GetProduct(1)).Returns(
                new Product(1, "Test Product", "A Test Product", 1.23m, false, "thumbnail.jpg", "image.jpg")
            );

            return TestHelper.ExecuteTest<Program>(
                serviceConfig: services =>
                {
                    services.AddSingleton(grainFactoryFake);
                    services.AddSingleton(productsClientFake);
                },
                test: async client => {

                    client.DefaultRequestHeaders.Add("Cookie", "ShoppingCartId=TestCart");
                    var response = await client.PostAsJsonAsync("/api/shopping-cart", new { ProductId = 1, Count = 1 });

                    response.EnsureSuccessStatusCode();

                    A.CallTo(() => grainFake.AddItem(A<ShoppingCartItem>
                            .That.Matches(x => x.ProductId == 1 && x.Price == 1.23m && x.Count == 1)))
                        .MustHaveHappenedOnceExactly();
                });
        }
    }
    
    protected static Task ExecuteTest(
        Func<HttpClient, Task> test,
        Action<IGrainFactory, IProductsClient> serviceConfig
    )
    {
        var grainFactoryFake = A.Fake<IGrainFactory>();
        var productsClientFake = A.Fake<IProductsClient>();

        serviceConfig(grainFactoryFake, productsClientFake);

        return TestHelper.ExecuteTest<Program>(
            serviceConfig: services =>
            {
                services.AddSingleton(grainFactoryFake);
                services.AddSingleton(productsClientFake);
            },
            test: test);
    }
}