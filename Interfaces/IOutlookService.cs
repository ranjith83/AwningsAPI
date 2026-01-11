using AwningsAPI.Dto.Outlook;

namespace AwningsAPI.Interfaces
{
    public interface IOutlookService
    {
        Task<string> CreateShowroomInviteAsync(ShowroomInviteDto dto, string currentUser);
        Task<object> GetCalendarEventsAsync(DateTime startDate, DateTime endDate);
        Task UpdateCalendarEventAsync(string eventId, OutlookEventDto eventDto);
        Task DeleteCalendarEventAsync(string eventId);
        Task SendShowroomInviteEmailAsync(ShowroomInviteDto dto);
    }
}