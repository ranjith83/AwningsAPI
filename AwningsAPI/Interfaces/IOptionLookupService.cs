using AwningsAPI.Dto.Common;

namespace AwningsAPI.Interfaces
{
    public interface IOptionLookupService
    {
        Task<IEnumerable<OptionLookupDto>> GetByCategoryAsync(string category);
    }
}
