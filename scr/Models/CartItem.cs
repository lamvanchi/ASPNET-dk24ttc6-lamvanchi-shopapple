using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleStore.Models;

public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    [StringLength(50)]
    public string? Color { get; set; }

    [StringLength(50)]
    public string? Storage { get; set; }

    public Cart Cart { get; set; } = null!;
    public Product Product { get; set; } = null!;

    [NotMapped]
    public decimal LineTotal => Product.DisplayPrice * Quantity;
}
