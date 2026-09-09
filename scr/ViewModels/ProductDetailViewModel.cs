using AppleStore.Models;

namespace AppleStore.ViewModels;

public class ProductDetailViewModel
{
    public Product Product { get; set; } = null!;
    public List<Product> RelatedProducts { get; set; } = new();
    public List<string> Colors { get; set; } = new();
    public List<string> Storages { get; set; } = new();
    public int Quantity { get; set; } = 1;
}
