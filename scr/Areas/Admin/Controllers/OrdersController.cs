using AppleStore.Data;
using AppleStore.Helpers;
using AppleStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class OrdersController : Controller
{
    private readonly ApplicationDbContext _db;

    public OrdersController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? q, OrderStatus? status)
    {
        var query = _db.Orders.Include(o => o.User).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            var keyword = q.Trim();
            query = query.Where(o =>
                o.OrderCode.Contains(keyword) ||
                o.ReceiverName.Contains(keyword) ||
                o.User.FullName.Contains(keyword) ||
                (o.User.Email != null && o.User.Email.Contains(keyword)));
        }

        if (status.HasValue)
            query = query.Where(o => o.Status == status);

        ViewBag.Keyword = q;
        ViewBag.Status = status;
        ViewBag.StatusOptions = FormatHelper.StatusOptions(status);
        return View(await query.OrderByDescending(o => o.CreatedAt).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var order = await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Details)
            .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        return order is null ? NotFound() : View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order is null)
            return NotFound();

        if (order.Status == OrderStatus.Cancelled && status != OrderStatus.Cancelled)
        {
            TempData["ToastError"] = "Không thể đổi trạng thái đơn đã hủy.";
            return RedirectToAction(nameof(Details), new { id });
        }

        if (order.Status == OrderStatus.Completed && status != OrderStatus.Completed)
        {
            TempData["ToastError"] = "Đơn đã hoàn thành không thể đổi trạng thái.";
            return RedirectToAction(nameof(Details), new { id });
        }

        order.Status = status;
        order.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();
        TempData["Toast"] = "Đã cập nhật trạng thái đơn hàng.";
        return RedirectToAction(nameof(Details), new { id });
    }
}
