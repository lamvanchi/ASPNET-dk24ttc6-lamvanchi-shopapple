using AppleStore.Data;
using AppleStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CustomersController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;

    public CustomersController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
    {
        _userManager = userManager;
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.OrderByDescending(u => u.CreatedAt).ToListAsync();
        var counts = await _db.Orders.GroupBy(o => o.UserId).Select(g => new { g.Key, Count = g.Count() }).ToListAsync();
        ViewBag.OrderCounts = counts.ToDictionary(x => x.Key, x => x.Count);
        return View(users);
    }

    public async Task<IActionResult> Details(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            return NotFound();

        ViewBag.Orders = await _db.Orders.Where(o => o.UserId == id).OrderByDescending(o => o.CreatedAt).ToListAsync();
        return View(user);
    }
}
