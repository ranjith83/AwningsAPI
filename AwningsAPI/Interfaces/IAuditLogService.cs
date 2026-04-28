// Interfaces/IAuditLogService.cs
using AwningsAPI.Dto.Audit;
using AwningsAPI.Model.Audit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwningsAPI.Interfaces
{
    public interface IAuditLogService
    {
        Task<AuditLog> CreateAuditLogAsync(CreateAuditLogDto dto, string ipAddress, string userAgent);
        Task<AuditLogDto?> GetAuditLogByIdAsync(int auditId);
        Task<List<AuditLogDto>> GetEntityAuditLogsAsync(string entityType, int entityId);
        Task<AuditLogPagedResultDto> GetAuditLogsAsync(AuditLogFilterDto filter);
        Task<AuditSummaryDto> GetAuditSummaryAsync(DateTime? startDate, DateTime? endDate);
        List<FieldChange> CompareObjects<T>(T oldObj, T newObj, Dictionary<string, string> fieldLabels) where T : class;
    }
}