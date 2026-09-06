using AppleStore.Data;
using AppleStore.Helpers;
using AppleStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CategoriesController : Controller
{
    private readonly ApplicationDbContext _db;

    public CategoriesController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _db.Categories
            .Include(c => c.Products)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();
        return View(categories);
    }

    [HttpGet]
    public IActionResult Create() => View(new Category { IsActive = true });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category model)
    {
        ModelState.Remove(nameof(Category.Products));
        if (string.IsNullOrWhiteSpace(model.Slug))
        {
            model.Slug = SlugHelper.ToSlug(model.Name);
            ModelState.Remove(nameof(model.Slug));
        }

        if (await _db.Categories.AnyAsync(c => c.Slug == model.Slug))
            ModelState.AddModelError(nameof(model.Slug), "Slug đã tồn tại.");

        if (!ModelState.IsValid)
            return View(model);

        _db.Categories.Add(model);
        await _db.SaveChangesAsync();
        TempData["Toast"] = "Đã thêm danh mục.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        return category is null ? NotFound() : View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category model)
    {
        if (id != model.Id)
            return NotFound();

        ModelState.Remove(nameof(Category.Products));
        if (string.IsNullOrWhiteSpace(model.Slug))
        {
            model.Slug = SlugHelper.ToSlug(model.Name);
            ModelState.Remove(nameof(model.Slug));
        }

        if (await _db.Categories.AnyAsync(c => c.Slug == model.Slug && c.Id != id))
            ModelState.AddModelError(nameof(model.Slug), "Slug đã tồn tại.");

        if (!ModelState.IsValid)
            return View(model);

        _db.Update(model);
        await _db.SaveChangesAsync();
        TempData["Toast"] = "Đã cập nhật danh mục.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _db.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
        if (category is null)
            return NotFound();

        if (category.Products.Any())
        {
            TempData["ToastError"] = "Không thể xóa danh mục vì còn sản phẩm liên quan.";
            return RedirectToAction(nameof(Index));
        }

        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();
        TempData["Toast"] = "Đã xóa danh mục.";
        return RedirectToAction(nameof(Index));
    }
}
