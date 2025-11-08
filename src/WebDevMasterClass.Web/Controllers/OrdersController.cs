using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebDevMasterClass.Services.Orders.gRPC;
using WebDevMasterClass.Services.Products.Client;
using WebDevMasterClass.Web.ShoppingCart;

namespace WebDevMasterClass.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController(IProductsClient products,
    OrdersService.OrdersServiceClient ordersService,
    IGrainFactory grainFactory,
    ILogger<OrdersController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddOrder(AddOrderModel order)
    {
        var request = new AddOrderRequest
        {
            DeliveryAddress = new Address
            {
                Name = order.DeliveryAddress.Name,
                Street1 = order.DeliveryAddress.Street1,
                Street2 = order.DeliveryAddress.Street2 ?? "",
                PostalCode = order.DeliveryAddress.PostalCode,
                City = order.DeliveryAddress.City,
                Country = order.DeliveryAddress.Country,
            },
            BillingAddress = new Address
            {
                Name = order.BillingAddress.Name,
                Street1 = order.BillingAddress.Street1,
                Street2 = order.BillingAddress.Street2 ?? "",
                PostalCode = order.BillingAddress.PostalCode,
                City = order.BillingAddress.City,
                Country = order.BillingAddress.Country,
            }
        };

        var retrievalTasks = order.Items.Select(x =>
            products.GetProduct(x.ItemId)
        ).ToArray();
        
        try
        {
            await Task.WhenAll(retrievalTasks);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve product information");
            return StatusCode(500, "Something went wrong...");
        }
     
        var retrievedProducts = retrievalTasks.Select(x => x.Result!).ToArray();
        
        foreach (var item in order.Items)
        {
            var product = retrievedProducts.First(x => x.Id == item.ItemId);
            request.Items.Add(new OrderItem
            {
                Name = product.Name,
                Price = (float)product.Price,
                Quantity = item.Quantity
            });
        }

        AddOrderResponse response;
        try
        {
            response = await ordersService.AddOrderAsync(request);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add order");
            return StatusCode(500, "Something went wrong...");
        }

        if (!response.Success)
        {
            logger.LogError("Failed to add order: {error}", response.Error);
            return StatusCode(500, "Something went wrong...");
        }
        
        var shoppingCart = grainFactory.GetGrain<IShoppingCart>(Request.Cookies["ShoppingCartId"]!);
        await shoppingCart.Clear();

        return Ok(new { response.Success, response.OrderId });
    }
}