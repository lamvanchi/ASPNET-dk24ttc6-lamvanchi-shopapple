using System.ComponentModel.DataAnnotations;

namespace AppleStore.Models;

public class ProductImage
{
    public int Id { get; set; }

    [Required, StringLength(300)]
    public string ImageUrl { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
