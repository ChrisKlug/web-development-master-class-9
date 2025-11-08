using System.Diagnostics;
using Grpc.Core;
using WebDevMasterClass.Services.Orders.Data;
using WebDevMasterClass.Services.Orders.Entities;
using WebDevMasterClass.Services.Orders.gRPC;
using WebDevMasterClass.Services.Orders.Observability;

namespace WebDevMasterClass.Services.Orders.Services;

public class OrdersService(
    OrdersContext ctx, 
    OrdersMetrics metrics, 
    ILogger<OrdersService> logger
) : gRPC.OrdersService.OrdersServiceBase
{
    public const string ActivitySourceName = "OrdersService";
    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    public override async Task<AddOrderResponse> AddOrder(AddOrderRequest request, ServerCallContext context)
    {
        Activity.Current?.SetTag("ServiceName", nameof(OrdersService));
        
        var deliveryAddress = DeliveryAddress.Create(request.DeliveryAddress.Name, 
            request.DeliveryAddress.Street1, 
            request.DeliveryAddress.Street2, 
            request.DeliveryAddress.PostalCode, 
            request.DeliveryAddress.City, 
            request.DeliveryAddress.Country);

        var billingAddress = BillingAddress.Create(request.BillingAddress.Name,
            request.BillingAddress.Street1,
            request.BillingAddress.Street2,
            request.BillingAddress.PostalCode,
            request.BillingAddress.City,
            request.BillingAddress.Country);

        var order = Order.Create(deliveryAddress, billingAddress);
        
        Activity.Current?.AddEvent(new ActivityEvent("Adding Order"));
        
        using var activity = ActivitySource.StartActivity("OrderService.AddOrder");
        
        activity?.SetTag("ServiceName", nameof(OrdersService));
        
        foreach (var item in request.Items)
        {
            order.AddItem(item.Name, item.Quantity, (decimal)item.Price);
        }

        try
        {
            activity?.AddEvent(new ActivityEvent("Adding Order", tags: new ActivityTagsCollection([
                new KeyValuePair<string, object?>("OrderId", order.OrderId),
                new KeyValuePair<string, object?>("OrderDate", order.OrderDate)
            ])));

            ctx.Add(order);
            await ctx.SaveChangesAsync();
            metrics.AddOrder();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding order {orderId}", order.OrderId);
            return new AddOrderResponse
            {
                Success = false,
                Error = ex.GetBaseException().Message
            };
        }
        
        return new AddOrderResponse
        {
            Success = true,
            OrderId = order.OrderId
        };
    }
}