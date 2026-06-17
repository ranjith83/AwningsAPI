using AwningsAPI.Dto.ImportLeads;

namespace AwningsAPI.Interfaces
{
    public interface IImportLeadsService
    {
        Task<ImportLeadsResultDto> ProcessLeadsFolderAsync(string folderName, string currentUser);
    }
}
