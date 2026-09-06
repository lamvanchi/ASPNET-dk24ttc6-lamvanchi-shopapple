using AppleStore.Data;
using AppleStore.Helpers;
using AppleStore.Models;
using AppleStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProductsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _env;

    public ProductsController(ApplicationDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public async Task<IActionResult> Index(string? q, int? categoryId)
    {
        var query = _db.Products.Include(p => p.Category).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(p => p.Name.Contains(q.Trim()));
        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId);

        ViewBag.Keyword = q;
        ViewBag.CategoryId = categoryId;
        ViewBag.Categories = new SelectList(await _db.Categories.OrderBy(c => c.Name).ToListAsync(), "Id", "Name", categoryId);
        return View(await query.OrderByDescending(p => p.CreatedAt).ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadCategoriesAsync();
        return View(new AdminProductViewModel { IsActive = true, Stock = 10 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AdminProductViewModel model)
    {
        await LoadCategoriesAsync(model.CategoryId);
        if (!ModelState.IsValid)
            return View(model);

        var product = MapToProduct(model, new Product { CreatedAt = DateTime.Now });
        product.MainImage = await SaveImageAsync(model.ImageFile) ?? "/images/products/placeholder.svg";
        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        await SaveGalleryAsync(product.Id, model.GalleryFiles, product.MainImage);
        TempData["Toast"] = "Đã thêm sản phẩm.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product is null)
            return NotFound();

        await LoadCategoriesAsync(product.CategoryId);
        return View(new AdminProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            CategoryId = product.CategoryId,
            Price = product.Price,
            SalePrice = product.SalePrice,
            Stock = product.Stock,
            ShortDescription = product.ShortDescription,
            Description = product.Description,
            Specifications = product.Specifications,
            ColorOptions = product.ColorOptions,
            StorageOptions = product.StorageOptions,
            IsFeatured = product.IsFeatured,
            IsActive = product.IsActive,
            CurrentImage = product.MainImage
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AdminProductViewModel model)
    {
        if (id != model.Id)
            return NotFound();

        await LoadCategoriesAsync(model.CategoryId);
        if (!ModelState.IsValid)
            return View(model);

        var product = await _db.Products.FindAsync(id);
        if (product is null)
            return NotFound();

        MapToProduct(model, product);
        var uploaded = await SaveImageAsync(model.ImageFile);
        if (!string.IsNullOrEmpty(uploaded))
            product.MainImage = uploaded;

        await _db.SaveChangesAsync();
        await SaveGalleryAsync(product.Id, model.GalleryFiles, product.MainImage);
        TempData["Toast"] = "Đã cập nhật sản phẩm.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _db.Products.Include(p => p.OrderDetails).FirstOrDefaultAsync(p => p.Id == id);
        if (product is null)
            return NotFound();

        if (product.OrderDetails.Any())
        {
            product.IsActive = false;
            await _db.SaveChangesAsync();
            TempData["Toast"] = "Sản phẩm đã có trong đơn hàng nên được ẩn thay vì xóa.";
            return RedirectToAction(nameof(Index));
        }

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        TempData["Toast"] = "Đã xóa sản phẩm.";
        return RedirectToAction(nameof(Index));
    }

    private static Product MapToProduct(AdminProductViewModel model, Product product)
    {
        product.Name = model.Name.Trim();
        product.Slug = SlugHelper.ToSlug(model.Name);
        product.CategoryId = model.CategoryId;
        product.Price = model.Price;
        product.SalePrice = model.SalePrice;
        product.Stock = model.Stock;
        product.ShortDescription = model.ShortDescription;
        product.Description = model.Description;
        product.Specifications = model.Specifications;
        product.ColorOptions = model.ColorOptions;
        product.StorageOptions = model.StorageOptions;
        product.IsFeatured = model.IsFeatured;
        product.IsActive = model.IsActive;
        return product;
    }

    private async Task LoadCategoriesAsync(int? selected = null)
    {
        ViewBag.Categories = new SelectList(await _db.Categories.OrderBy(c => c.Name).ToListAsync(), "Id", "Name", selected);
    }

    private async Task<string?> SaveImageAsync(IFormFile? file)
    {
        if (file is null || file.Length == 0)
            return null;

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif", ".svg" };
        if (!allowed.Contains(ext))
            return null;

        var folder = Path.Combine(_env.WebRootPath, "uploads", "products");
        Directory.CreateDirectory(folder);
        var fileName = $"{Guid.NewGuid():N}{ext}";
        var path = Path.Combine(folder, fileName);
        await using var stream = System.IO.File.Create(path);
        await file.CopyToAsync(stream);
        return $"/uploads/products/{fileName}";
    }

    private async Task SaveGalleryAsync(int productId, List<IFormFile>? files, string? mainImage)
    {
        if (files is { Count: > 0 })
        {
            var order = 1;
            foreach (var file in files)
            {
                var url = await SaveImageAsync(file);
                if (url is null) continue;
                _db.ProductImages.Add(new ProductImage { ProductId = productId, ImageUrl = url, DisplayOrder = order++ });
            }
            await _db.SaveChangesAsync();
            return;
        }

        if (!await _db.ProductImages.AnyAsync(x => x.ProductId == productId) && !string.IsNullOrEmpty(mainImage))
        {
            _db.ProductImages.Add(new ProductImage { ProductId = productId, ImageUrl = mainImage, DisplayOrder = 1 });
            await _db.SaveChangesAsync();
        }
    }
}
