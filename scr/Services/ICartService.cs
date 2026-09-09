using AppleStore.ViewModels;

namespace AppleStore.Services;

public interface ICartService
{
    Task<CartViewModel> GetCartAsync(string? userId, CancellationToken cancellationToken = default);
    Task<(bool Success, string Message, int CartCount)> AddAsync(string? userId, int productId, int quantity, string? color, string? storage, CancellationToken cancellationToken = default);
    Task<(bool Success, string Message)> UpdateQuantityAsync(string? userId, int itemId, int quantity, CancellationToken cancellationToken = default);
    Task RemoveAsync(string? userId, int itemId, CancellationToken cancellationToken = default);
    Task ClearAsync(string? userId, CancellationToken cancellationToken = default);
    Task MergeSessionCartAsync(string userId, CancellationToken cancellationToken = default);
    Task<int> CountAsync(string? userId, CancellationToken cancellationToken = default);
}
