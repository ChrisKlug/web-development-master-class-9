namespace WebDevMasterClass.Web.ShoppingCart;

public interface IShoppingCart : IGrainWithStringKey
{
    Task AddItem(ShoppingCartItem item);
    Task<ShoppingCartItem[]> GetItems();
}

[GenerateSerializer]
public class ShoppingCartItem
{
    [Id(0)]
    public int ProductId { get; set; }
    [Id(1)]
    public string ProductName { get; set; } = string.Empty;
    [Id(2)]
    public decimal Price { get; set; }
    [Id(3)]
    public int Count { get; set; }
}

public class ShoppingCartGrain : Grain, IShoppingCart
{
    private readonly List<ShoppingCartItem> items = new();

    public Task AddItem(ShoppingCartItem item)
    {
        var existingItem = items.FirstOrDefault(x => x.ProductId == item.ProductId);
        if (existingItem is null)
        {
            items.Add(item);
        }
        else
        {
            existingItem.Count += item.Count;
        }
        return Task.CompletedTask;
    }

    public Task<ShoppingCartItem[]> GetItems()
        => Task.FromResult(items.ToArray());
}
