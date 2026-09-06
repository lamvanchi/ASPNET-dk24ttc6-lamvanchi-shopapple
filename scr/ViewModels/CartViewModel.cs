namespace AppleStore.ViewModels;

public class CartItemViewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? Image { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int Stock { get; set; }
    public string? Color { get; set; }
    public string? Storage { get; set; }
    public decimal LineTotal => Price * Quantity;
}

public class CartViewModel
{
    public List<CartItemViewModel> Items { get; set; } = new();
    public decimal Total => Items.Sum(x => x.LineTotal);
    public int Count => Items.Sum(x => x.Quantity);
    public bool IsEmpty => Items.Count == 0;
}
