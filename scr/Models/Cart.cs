namespace AppleStore.Models;

public class Cart
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public ApplicationUser User { get; set; } = null!;
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
