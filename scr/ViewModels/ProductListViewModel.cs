using AppleStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppleStore.ViewModels;

public class ProductListViewModel
{
    public List<Product> Products { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public string? Keyword { get; set; }
    public int? CategoryId { get; set; }
    public string? CategorySlug { get; set; }
    public string? CategoryName { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string Sort { get; set; } = "newest";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 9;
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

    public IEnumerable<SelectListItem> SortOptions => new[]
    {
        new SelectListItem("Sản phẩm mới", "newest", Sort == "newest"),
        new SelectListItem("Giá thấp → cao", "price_asc", Sort == "price_asc"),
        new SelectListItem("Giá cao → thấp", "price_desc", Sort == "price_desc")
    };
}
