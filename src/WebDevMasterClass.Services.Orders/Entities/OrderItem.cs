namespace WebDevMasterClass.Services.Orders.Entities;

public class OrderItem
{
    private int id;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private OrderItem() { } // For EF Core
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private OrderItem(int id, string name, int quantity, decimal price)
    {
        this.id = id;
        Name = name;
        Quantity = quantity;
        Price = price;
    }

    internal static OrderItem Create(int id, string name, int quantity, decimal price) => new OrderItem(id, name, quantity, price);

    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
