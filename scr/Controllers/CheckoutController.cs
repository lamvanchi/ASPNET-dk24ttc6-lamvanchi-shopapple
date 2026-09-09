using System.Security.Claims;
using AppleStore.Models;
using AppleStore.Services;
using AppleStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppleStore.Controllers;

[Authorize]
public class CheckoutController : Controller
{
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CheckoutController(ICartService cartService, IOrderService orderService, UserManager<ApplicationUser> userManager)
    {
        _cartService = cartService;
        _orderService = orderService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var cart = await _cartService.GetCartAsync(UserId());
        if (cart.IsEmpty)
        {
            TempData["ToastError"] = "Giỏ hàng đang trống.";
            return RedirectToAction("Index", "Cart");
        }

        var user = await _userManager.GetUserAsync(User);
        var model = new CheckoutViewModel
        {
            ReceiverName = user?.FullName ?? string.Empty,
            Phone = user?.PhoneNumber ?? string.Empty,
            Address = user?.Address ?? string.Empty,
            Cart = cart
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CheckoutViewModel model)
    {
        model.Cart = await _cartService.GetCartAsync(UserId());
        if (model.Cart.IsEmpty)
        {
            TempData["ToastError"] = "Giỏ hàng đang trống.";
            return RedirectToAction("Index", "Cart");
        }

        if (!ModelState.IsValid)
            return View(model);

        var result = await _orderService.PlaceOrderAsync(UserId()!, model);
        if (!result.Success || result.Order is null)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        return RedirectToAction(nameof(Success), new { code = result.Order.OrderCode });
    }

    public IActionResult Success(string code)
    {
        ViewBag.OrderCode = code;
        return View();
    }

    private string? UserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);
}
