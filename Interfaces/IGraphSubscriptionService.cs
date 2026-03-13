using System.Threading.Tasks;

namespace AwningsAPI.Interfaces
{
    public interface IGraphSubscriptionService
    {
        /// <summary>Creates the subscription if it doesn't exist, or renews it when near expiry.</summary>
        Task EnsureSubscriptionAsync();

        /// <summary>Returns the active Graph subscription ID, or null if none is registered.</summary>
        string? GetActiveSubscriptionId();

        /// <summary>Deletes the active subscription (called on graceful shutdown).</summary>
        Task DeleteSubscriptionAsync();
    }
}