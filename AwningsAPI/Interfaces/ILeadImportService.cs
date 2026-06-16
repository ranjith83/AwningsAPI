using AwningsAPI.Dto.LeadImport;

namespace AwningsAPI.Interfaces
{
    public interface ILeadImportService
    {
        Task<LeadImportResultDto> ProcessLeadsFolderAsync(string folderName, string currentUser);
    }
}
