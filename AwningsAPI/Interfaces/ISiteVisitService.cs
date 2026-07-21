using AwningsAPI.Dto.SiteVisit;
using AwningsAPI.Model.SiteVisit;

namespace AwningsAPI.Interfaces
{
    public interface ISiteVisitService
    {
        Task<IEnumerable<Model.SiteVisit.SiteVisit>> GetSiteVisitsByWorkflowIdAsync(int workflowId);
        Task<IEnumerable<SiteVisitDto>> GetSiteVisitDtosByWorkflowIdAsync(int workflowId);
        Task<Model.SiteVisit.SiteVisit?> GetSiteVisitByIdAsync(int id);
        Task<SiteVisitDto?> GetSiteVisitDtoByIdAsync(int id);
        Task<Model.SiteVisit.SiteVisit> CreateSiteVisitAsync(CreateSiteVisitDto dto, string currentUser);
        Task<Model.SiteVisit.SiteVisit> UpdateSiteVisitAsync(int id, SiteVisitDto dto, string currentUser);
        Task<bool> DeleteSiteVisitAsync(int id);
        Task<IEnumerable<SiteVisitValues>> GetAllValuesAsync();
        Task<IEnumerable<SiteVisitValues>> GetValuesByCategoryAsync(string category);
        Task<Dictionary<string, List<string>>> GetAllValuesDictionaryAsync();
        Task SaveImageUrlsAsync(int siteVisitId, List<string> imageUrls, string currentUser);
        Task<bool> DeleteImagesAsync(int siteVisitId, List<string> imageUrls);
        Task<(IEnumerable<ScheduledShowroomInviteDto> Items, int TotalCount)> GetUpcomingShowroomInvitesAsync(int page, int pageSize);
        Task<int> GetUpcomingShowroomInviteCountAsync();
        Task<bool> CompletePendingShowroomInviteAsync(int workflowId, string currentUser);
    }
}