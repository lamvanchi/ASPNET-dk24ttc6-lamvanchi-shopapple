using AppleStore.Models;
using AppleStore.ViewModels;

namespace AppleStore.Services;

public interface IOrderService
{
    Task<(bool Success, string Message, Order? Order)> PlaceOrderAsync(string userId, CheckoutViewModel input, CancellationToken cancellationToken = default);
}
