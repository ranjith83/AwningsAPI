namespace AwningsEmailFunction.Interfaces;

public interface IGraphSubscriptionService
{
    Task EnsureSubscriptionAsync();
    string? GetActiveSubscriptionId();
    Task DeleteSubscriptionAsync();
}
