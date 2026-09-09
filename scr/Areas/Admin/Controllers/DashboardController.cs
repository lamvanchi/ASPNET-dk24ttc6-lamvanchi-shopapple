using AppleStore.Data;
using AppleStore.Models;
using AppleStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var completed = OrderStatus.Completed;
        var cancelled = OrderStatus.Cancelled;
        var now = DateTime.Now;

        var monthly = await _db.Orders
            .Where(o => o.Status != cancelled && o.CreatedAt >= now.AddMonths(-5))
            .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Total = g.Sum(x => x.TotalAmount) })
            .ToListAsync();

        var labels = new List<string>();
        var values = new List<decimal>();
        for (var i = 5; i >= 0; i--)
        {
            var date = now.AddMonths(-i);
            labels.Add($"Thg {date.Month}");
            values.Add(monthly.FirstOrDefault(x => x.Year == date.Year && x.Month == date.Month)?.Total ?? 0);
        }

        var model = new DashboardViewModel
        {
            TotalProducts = await _db.Products.CountAsync(),
            TotalCustomers = _userManager.Users.Count(),
            TotalOrders = await _db.Orders.CountAsync(),
            TotalRevenue = await _db.Orders.Where(o => o.Status == completed).SumAsync(o => (decimal?)o.TotalAmount) ?? 0,
            PendingOrders = await _db.Orders.CountAsync(o => o.Status == OrderStatus.Pending),
            LowStockProducts = await _db.Products.CountAsync(p => p.Stock <= 10 && p.IsActive),
            ChartLabels = labels,
            ChartValues = values,
            RecentOrders = await _db.Orders.Include(o => o.User).OrderByDescending(o => o.CreatedAt).Take(6).ToListAsync(),
            LowStockList = await _db.Products.Include(p => p.Category).Where(p => p.Stock <= 10 && p.IsActive).OrderBy(p => p.Stock).Take(6).ToListAsync()
        };

        return View(model);
    }
}
