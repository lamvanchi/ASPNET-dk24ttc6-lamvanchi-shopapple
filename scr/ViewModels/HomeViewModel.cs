using AppleStore.Models;

namespace AppleStore.ViewModels;

public class HomeViewModel
{
    public List<Category> Categories { get; set; } = new();
    public List<Product> FeaturedProducts { get; set; } = new();
    public List<Product> IPhones { get; set; } = new();
    public List<Product> MacBooks { get; set; } = new();
    public List<Product> IPads { get; set; } = new();
    public List<Product> Watches { get; set; } = new();
    public List<Product> AirPods { get; set; } = new();
    public List<Product> Accessories { get; set; } = new();
}
