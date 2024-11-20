namespace WebDevMasterClass.Web.ShoppingCart;

public interface IShoppingCart : IGrainWithStringKey
{
    Task AddItem(ShoppingCartItem item);
    Task<ShoppingCartItem[]> GetItems();
    Task Clear();
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
    private readonly IPersistentState<State> state;

    public ShoppingCartGrain(
        [PersistentState("ShoppingCartState")]
        IPersistentState<State> state
    )
    {
        this.state = state;
    }

    public Task AddItem(ShoppingCartItem item)
    {
        var existingItem = state.State.Items.FirstOrDefault(x => x.ProductId == item.ProductId);
        if (existingItem is null)
        {
            state.State.Items.Add(item);
        }
        else
        {
            existingItem.Count += item.Count;
        }
        return state.WriteStateAsync();
    }

    public Task<ShoppingCartItem[]> GetItems()
        => Task.FromResult(state.State.Items.ToArray());

    public Task Clear()
    {
        DeactivateOnIdle();
        return state.ClearStateAsync();
    }

    public class State
    {
        public List<ShoppingCartItem> Items { get; set; } = new();
    }
}
