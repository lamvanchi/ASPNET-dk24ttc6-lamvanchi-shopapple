using AppleStore.Data;
using AppleStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _db;

    public ProductController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? q, string? category, int? categoryId, decimal? minPrice, decimal? maxPrice, string sort = "newest", int page = 1)
    {
        page = Math.Max(1, page);
        const int pageSize = 9;

        var query = _db.Products.Include(p => p.Category).Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(q))
        {
            var keyword = q.Trim();
            query = query.Where(p =>
                p.Name.Contains(keyword) ||
                (p.ShortDescription != null && p.ShortDescription.Contains(keyword)) ||
                p.Category.Name.Contains(keyword));
        }

        string? categoryName = null;
        if (!string.IsNullOrWhiteSpace(category))
        {
            var cat = await _db.Categories.FirstOrDefaultAsync(c => c.Slug == category && c.IsActive);
            if (cat is not null)
            {
                categoryId = cat.Id;
                categoryName = cat.Name;
            }
        }
        else if (categoryId.HasValue)
        {
            var cat = await _db.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            categoryName = cat?.Name;
        }

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId);

        if (minPrice.HasValue)
            query = query.Where(p => (p.SalePrice != null && p.SalePrice > 0 ? p.SalePrice : p.Price) >= minPrice);
        if (maxPrice.HasValue)
            query = query.Where(p => (p.SalePrice != null && p.SalePrice > 0 ? p.SalePrice : p.Price) <= maxPrice);

        query = sort switch
        {
            "price_asc" => query.OrderBy(p => p.SalePrice != null && p.SalePrice > 0 && p.SalePrice < p.Price ? p.SalePrice : p.Price),
            "price_desc" => query.OrderByDescending(p => p.SalePrice != null && p.SalePrice > 0 && p.SalePrice < p.Price ? p.SalePrice : p.Price),
            _ => query.OrderByDescending(p => p.CreatedAt)
        };

        var total = await query.CountAsync();
        var products = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var model = new ProductListViewModel
        {
            Products = products,
            Categories = await _db.Categories.Where(c => c.IsActive).OrderBy(c => c.DisplayOrder).ToListAsync(),
            Keyword = q,
            CategoryId = categoryId,
            CategorySlug = category,
            CategoryName = categoryName,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            Sort = sort,
            Page = page,
            PageSize = pageSize,
            TotalItems = total
        };

        return View(model);
    }

    public async Task<IActionResult> Details(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return NotFound();

        var product = await _db.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Slug == slug && p.IsActive);

        if (product is null)
            return NotFound();

        var related = await _db.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive && p.CategoryId == product.CategoryId && p.Id != product.Id)
            .OrderByDescending(p => p.IsFeatured)
            .Take(4)
            .ToListAsync();

        var model = new ProductDetailViewModel
        {
            Product = product,
            RelatedProducts = related,
            Colors = SplitOptions(product.ColorOptions),
            Storages = SplitOptions(product.StorageOptions)
        };

        return View(model);
    }

    private static List<string> SplitOptions(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new List<string>();

        return value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
    }
}
