using AwningsAPI.Dto.Workflow;

namespace AwningsAPI.Interfaces
{
    public interface IFollowUpService
    {
        Task<int> GeneratePendingFollowUpsAsync();
        Task<List<FollowUpDto>> GetActiveFollowUpsAsync();
        Task<List<FollowUpDto>> GetAllFollowUpsAsync();

        Task<List<FollowUpDto>> GetFollowUpsByCustomerAsync(int customerId);
        Task<bool> DismissFollowUpAsync(int followUpId, string notes, string currentUser);

    }
}