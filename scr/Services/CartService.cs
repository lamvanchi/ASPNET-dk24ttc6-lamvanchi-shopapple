using AppleStore.Data;
using AppleStore.Models;
using AppleStore.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AppleStore.Services;

public class CartService : ICartService
{
    private const string SessionKey = "GUEST_CART";
    private readonly ApplicationDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CartViewModel> GetCartAsync(string? userId, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            var cart = await GetOrCreateDbCartAsync(userId, cancellationToken);
            return MapDbCart(cart);
        }

        return MapSessionCart(GetSessionItems());
    }

    public async Task<(bool Success, string Message, int CartCount)> AddAsync(
        string? userId, int productId, int quantity, string? color, string? storage, CancellationToken cancellationToken = default)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId && p.IsActive, cancellationToken);
        if (product is null)
            return (false, "Sản phẩm không tồn tại.", 0);

        if (!product.InStock)
            return (false, "Sản phẩm hiện đang hết hàng.", 0);

        quantity = Math.Max(1, quantity);

        if (!string.IsNullOrEmpty(userId))
        {
            var cart = await GetOrCreateDbCartAsync(userId, cancellationToken);
            var item = cart.Items.FirstOrDefault(i =>
                i.ProductId == productId &&
                (i.Color ?? "") == (color ?? "") &&
                (i.Storage ?? "") == (storage ?? ""));

            var newQty = (item?.Quantity ?? 0) + quantity;
            if (newQty > product.Stock)
                return (false, $"Chỉ còn {product.Stock} sản phẩm trong kho.", cart.Items.Sum(x => x.Quantity));

            if (item is null)
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Color = color,
                    Storage = storage
                });
            }
            else
            {
                item.Quantity = newQty;
            }

            cart.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync(cancellationToken);
            var count = cart.Items.Sum(x => x.Quantity);
            return (true, "Đã thêm vào giỏ hàng.", count);
        }

        var items = GetSessionItems();
        var sessionItem = items.FirstOrDefault(i =>
            i.ProductId == productId &&
            (i.Color ?? "") == (color ?? "") &&
            (i.Storage ?? "") == (storage ?? ""));

        var sessionQty = (sessionItem?.Quantity ?? 0) + quantity;
        if (sessionQty > product.Stock)
            return (false, $"Chỉ còn {product.Stock} sản phẩm trong kho.", items.Sum(x => x.Quantity));

        if (sessionItem is null)
        {
            items.Add(new SessionCartItem
            {
                Id = items.Count == 0 ? 1 : items.Max(x => x.Id) + 1,
                ProductId = productId,
                Quantity = quantity,
                Color = color,
                Storage = storage
            });
        }
        else
        {
            sessionItem.Quantity = sessionQty;
        }

        SaveSessionItems(items);
        return (true, "Đã thêm vào giỏ hàng.", items.Sum(x => x.Quantity));
    }

    public async Task<(bool Success, string Message)> UpdateQuantityAsync(
        string? userId, int itemId, int quantity, CancellationToken cancellationToken = default)
    {
        quantity = Math.Max(1, quantity);

        if (!string.IsNullOrEmpty(userId))
        {
            var item = await _db.CartItems
                .Include(x => x.Cart)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == itemId && x.Cart.UserId == userId, cancellationToken);

            if (item is null)
                return (false, "Không tìm thấy sản phẩm trong giỏ.");

            if (quantity > item.Product.Stock)
                return (false, $"Chỉ còn {item.Product.Stock} sản phẩm trong kho.");

            item.Quantity = quantity;
            item.Cart.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync(cancellationToken);
            return (true, "Đã cập nhật giỏ hàng.");
        }

        var items = GetSessionItems();
        var sessionItem = items.FirstOrDefault(x => x.Id == itemId);
        if (sessionItem is null)
            return (false, "Không tìm thấy sản phẩm trong giỏ.");

        var product = await _db.Products.FindAsync([sessionItem.ProductId], cancellationToken);
        if (product is null)
            return (false, "Sản phẩm không tồn tại.");

        if (quantity > product.Stock)
            return (false, $"Chỉ còn {product.Stock} sản phẩm trong kho.");

        sessionItem.Quantity = quantity;
        SaveSessionItems(items);
        return (true, "Đã cập nhật giỏ hàng.");
    }

    public async Task RemoveAsync(string? userId, int itemId, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            var item = await _db.CartItems
                .Include(x => x.Cart)
                .FirstOrDefaultAsync(x => x.Id == itemId && x.Cart.UserId == userId, cancellationToken);
            if (item is not null)
            {
                _db.CartItems.Remove(item);
                await _db.SaveChangesAsync(cancellationToken);
            }
            return;
        }

        var items = GetSessionItems();
        items.RemoveAll(x => x.Id == itemId);
        SaveSessionItems(items);
    }

    public async Task ClearAsync(string? userId, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            var cart = await _db.Carts.Include(x => x.Items).FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            if (cart is not null)
            {
                _db.CartItems.RemoveRange(cart.Items);
                await _db.SaveChangesAsync(cancellationToken);
            }
            return;
        }

        SaveSessionItems(new List<SessionCartItem>());
    }

    public async Task MergeSessionCartAsync(string userId, CancellationToken cancellationToken = default)
    {
        var sessionItems = GetSessionItems();
        if (sessionItems.Count == 0)
            return;

        foreach (var item in sessionItems)
        {
            await AddAsync(userId, item.ProductId, item.Quantity, item.Color, item.Storage, cancellationToken);
        }

        SaveSessionItems(new List<SessionCartItem>());
    }

    public async Task<int> CountAsync(string? userId, CancellationToken cancellationToken = default)
    {
        var cart = await GetCartAsync(userId, cancellationToken);
        return cart.Count;
    }

    private async Task<Cart> GetOrCreateDbCartAsync(string userId, CancellationToken cancellationToken)
    {
        var cart = await _db.Carts
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (cart is null)
        {
            cart = new Cart { UserId = userId, UpdatedAt = DateTime.Now };
            _db.Carts.Add(cart);
            await _db.SaveChangesAsync(cancellationToken);
        }

        return cart;
    }

    private static CartViewModel MapDbCart(Cart cart)
    {
        return new CartViewModel
        {
            Items = cart.Items.Select(i => new CartItemViewModel
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Slug = i.Product.Slug,
                Image = i.Product.MainImage,
                Price = i.Product.DisplayPrice,
                Quantity = i.Quantity,
                Stock = i.Product.Stock,
                Color = i.Color,
                Storage = i.Storage
            }).ToList()
        };
    }

    private CartViewModel MapSessionCart(List<SessionCartItem> items)
    {
        var productIds = items.Select(x => x.ProductId).Distinct().ToList();
        var products = _db.Products.Where(p => productIds.Contains(p.Id)).ToList();

        return new CartViewModel
        {
            Items = items.Select(i =>
            {
                var product = products.FirstOrDefault(p => p.Id == i.ProductId);
                return new CartItemViewModel
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = product?.Name ?? "Sản phẩm",
                    Slug = product?.Slug,
                    Image = product?.MainImage,
                    Price = product?.DisplayPrice ?? 0,
                    Quantity = i.Quantity,
                    Stock = product?.Stock ?? 0,
                    Color = i.Color,
                    Storage = i.Storage
                };
            }).Where(x => x.Stock >= 0 && !string.IsNullOrEmpty(x.ProductName)).ToList()
        };
    }

    private List<SessionCartItem> GetSessionItems()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session is null)
            return new List<SessionCartItem>();

        var json = session.GetString(SessionKey);
        if (string.IsNullOrEmpty(json))
            return new List<SessionCartItem>();

        return JsonSerializer.Deserialize<List<SessionCartItem>>(json) ?? new List<SessionCartItem>();
    }

    private void SaveSessionItems(List<SessionCartItem> items)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        session?.SetString(SessionKey, JsonSerializer.Serialize(items));
    }

    private class SessionCartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? Color { get; set; }
        public string? Storage { get; set; }
    }
}
