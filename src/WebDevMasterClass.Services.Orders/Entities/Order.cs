namespace WebDevMasterClass.Services.Orders.Entities;

public class Order
{
    private int id;
    private List<OrderItem> _items = new();
    private List<Address> _addresses = new();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Order() { } // For EF Core
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private Order(string orderId, DeliveryAddress invoiceAddress, BillingAddress billingAddress, DateTimeOffset orderDate)
    {
        OrderId = orderId;
        _addresses.Add(invoiceAddress);
        _addresses.Add(billingAddress);
        OrderDate = orderDate;
    }

    public static Order Create(DeliveryAddress deliveryAddress, BillingAddress billingAddress)
        => new Order(GenerateOrderId(), deliveryAddress, billingAddress, DateTimeOffset.Now);

    public OrderItem AddItem(string name, int quantity, decimal price)
    {
        var item = OrderItem.Create(_items.Count + 1, name, quantity, price);
        _items.Add(item);
        Total += item.Price * item.Quantity;
        return item;
    }

    private static string GenerateOrderId()
    {
        return Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper();
    }

    public string OrderId { get; private set; }
    public DateTimeOffset OrderDate { get; private set; }
    public decimal Total { get; private set; }
    public DeliveryAddress DeliveryAddress => _addresses.OfType<DeliveryAddress>().First();
    public BillingAddress BillingAddress => _addresses.OfType<BillingAddress>().First();
    public OrderItem[] Items => _items.ToArray();
}