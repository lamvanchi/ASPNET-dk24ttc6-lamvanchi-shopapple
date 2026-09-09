using AppleStore.Data;
using AppleStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _db.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        var model = new HomeViewModel
        {
            Categories = await _db.Categories.Where(c => c.IsActive).OrderBy(c => c.DisplayOrder).ToListAsync(),
            FeaturedProducts = products.Where(p => p.IsFeatured).Take(8).ToList(),
            IPhones = products.Where(p => p.Category.Slug == "iphone").Take(4).ToList(),
            MacBooks = products.Where(p => p.Category.Slug == "macbook").Take(4).ToList(),
            IPads = products.Where(p => p.Category.Slug == "ipad").Take(4).ToList(),
            Watches = products.Where(p => p.Category.Slug == "apple-watch").Take(3).ToList(),
            AirPods = products.Where(p => p.Category.Slug == "airpods").Take(3).ToList(),
            Accessories = products.Where(p => p.Category.Slug == "phu-kien").Take(4).ToList()
        };

        return View(model);
    }

    public IActionResult Privacy() => View();

    public IActionResult About() => View();

    public IActionResult Contact() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Contact(string? name, string? email, string? message)
    {
        TempData["Toast"] = "Cảm ơn bạn đã liên hệ. Chúng tôi sẽ phản hồi sớm nhất.";
        return RedirectToAction(nameof(Contact));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new Models.ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
    }
}
