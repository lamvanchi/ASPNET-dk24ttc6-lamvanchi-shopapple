using AppleStore.Data;
using AppleStore.Models;
using AppleStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class StatisticsController : Controller
{
    private readonly ApplicationDbContext _db;

    public StatisticsController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var completed = OrderStatus.Completed;
        var now = DateTime.Now;

        var monthly = await _db.Orders
            .Where(o => o.Status != OrderStatus.Cancelled && o.CreatedAt >= now.AddMonths(-11))
            .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Total = g.Sum(x => x.TotalAmount) })
            .ToListAsync();

        var labels = new List<string>();
        var values = new List<decimal>();
        for (var i = 11; i >= 0; i--)
        {
            var date = now.AddMonths(-i);
            labels.Add($"Thg {date.Month}/{date.Year}");
            values.Add(monthly.FirstOrDefault(x => x.Year == date.Year && x.Month == date.Month)?.Total ?? 0);
        }

        var bestSellers = await _db.OrderDetails
            .Include(d => d.Order)
            .Where(d => d.Order.Status != OrderStatus.Cancelled)
            .GroupBy(d => new { d.ProductId, d.ProductName, d.ProductImage })
            .Select(g => new BestSellerItem
            {
                ProductName = g.Key.ProductName,
                Image = g.Key.ProductImage,
                QuantitySold = g.Sum(x => x.Quantity),
                Revenue = g.Sum(x => x.UnitPrice * x.Quantity)
            })
            .OrderByDescending(x => x.QuantitySold)
            .Take(8)
            .ToListAsync();

        var model = new StatisticsViewModel
        {
            TotalRevenue = await _db.Orders.Where(o => o.Status == completed).SumAsync(o => (decimal?)o.TotalAmount) ?? 0,
            TotalOrders = await _db.Orders.CountAsync(),
            ProductsSold = await _db.OrderDetails.Where(d => d.Order.Status != OrderStatus.Cancelled).SumAsync(d => (int?)d.Quantity) ?? 0,
            BestSellers = bestSellers,
            ChartLabels = labels,
            ChartValues = values
        };

        return View(model);
    }
}
