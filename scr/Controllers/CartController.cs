using System.Security.Claims;
using AppleStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppleStore.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        var cart = await _cartService.GetCartAsync(UserId());
        return View(cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, int quantity = 1, string? color = null, string? storage = null)
    {
        var result = await _cartService.AddAsync(UserId(), productId, quantity, color, storage);
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(new
            {
                success = result.Success,
                message = result.Message,
                cartCount = result.CartCount
            });
        }

        TempData[result.Success ? "Toast" : "ToastError"] = result.Message;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int itemId, int quantity)
    {
        var result = await _cartService.UpdateQuantityAsync(UserId(), itemId, quantity);
        TempData[result.Success ? "Toast" : "ToastError"] = result.Message;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int itemId)
    {
        await _cartService.RemoveAsync(UserId(), itemId);
        TempData["Toast"] = "Đã xóa sản phẩm khỏi giỏ hàng.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Count()
    {
        var count = await _cartService.CountAsync(UserId());
        return Json(new { count });
    }

    private string? UserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);
}
