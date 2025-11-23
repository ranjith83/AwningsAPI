// Services/AuditLogService.cs
using AwningsAPI.Database;
using AwningsAPI.Dto.Audit;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Audit;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Reflection;

namespace AwningsAPI.Services.AuditLogService
{
    public class AuditLogService : IAuditLogService
    {
        private readonly AppDbContext _context;

        public AuditLogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AuditLog> CreateAuditLogAsync(CreateAuditLogDto dto, string ipAddress, string userAgent)
        {
            var auditLog = new AuditLog
            {
                EntityType = dto.EntityType,
                EntityId = dto.EntityId,
                EntityName = dto.EntityName,
                Action = dto.Action,
                Changes = JsonSerializer.Serialize(dto.Changes),
                PerformedBy = dto.PerformedBy,
                PerformedByName = dto.PerformedByName ?? "Unknown",
                PerformedAt = DateTime.UtcNow,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Notes = dto.Notes
            };

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();

            return auditLog;
        }

        public async Task<AuditLogDto?> GetAuditLogByIdAsync(int auditId)
        {
            var auditLog = await _context.AuditLogs.FindAsync(auditId);

            if (auditLog == null)
                return null;

            return MapToDto(auditLog);
        }

        public async Task<List<AuditLogDto>> GetEntityAuditLogsAsync(string entityType, int entityId)
        {
            var auditLogs = await _context.AuditLogs
                .Where(a => a.EntityType == entityType && a.EntityId == entityId)
                .OrderByDescending(a => a.PerformedAt)
                .ToListAsync();

            return auditLogs.Select(MapToDto).ToList();
        }

        public async Task<AuditLogPagedResultDto> GetAuditLogsAsync(AuditLogFilterDto filter)
        {
            var query = _context.AuditLogs.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(filter.EntityType))
                query = query.Where(a => a.EntityType == filter.EntityType);

            if (filter.EntityId.HasValue)
                query = query.Where(a => a.EntityId == filter.EntityId.Value);

            if (!string.IsNullOrEmpty(filter.Action))
                query = query.Where(a => a.Action == filter.Action);

            if (filter.PerformedBy.HasValue)
                query = query.Where(a => a.PerformedBy == filter.PerformedBy.Value);

            if (filter.StartDate.HasValue)
                query = query.Where(a => a.PerformedAt >= filter.StartDate.Value);

            if (filter.EndDate.HasValue)
                query = query.Where(a => a.PerformedAt <= filter.EndDate.Value);

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(a =>
                    (a.EntityName != null && a.EntityName.Contains(filter.SearchTerm)) ||
                    a.PerformedByName.Contains(filter.SearchTerm) ||
                    (a.Notes != null && a.Notes.Contains(filter.SearchTerm)));
            }

            // Order by most recent first
            query = query.OrderByDescending(a => a.PerformedAt);

            // Get total count
            var totalCount = await query.CountAsync();

            // Apply pagination
            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new AuditLogPagedResultDto
            {
                Items = items.Select(MapToDto).ToList(),
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
            };
        }

        public async Task<AuditSummaryDto> GetAuditSummaryAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.AuditLogs.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(a => a.PerformedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(a => a.PerformedAt <= endDate.Value);

            var totalAudits = await query.CountAsync();

            var totalByAction = await query
                .GroupBy(a => a.Action)
                .Select(g => new { Action = g.Key, Count = g.Count() })
                .ToListAsync();

            var recentActivity = await query
                .OrderByDescending(a => a.PerformedAt)
                .Take(10)
                .ToListAsync();

            var topUsers = await query
                .GroupBy(a => new { a.PerformedBy, a.PerformedByName })
                .Select(g => new TopUserDto
                {
                    UserId = g.Key.PerformedBy,
                    UserName = g.Key.PerformedByName,
                    ActionCount = g.Count()
                })
                .OrderByDescending(x => x.ActionCount)
                .Take(10)
                .ToListAsync();

            return new AuditSummaryDto
            {
                TotalAudits = totalAudits,
                TotalByAction = new Dictionary<string, int>
                {
                    { "Creates", totalByAction.FirstOrDefault(x => x.Action == "CREATE")?.Count ?? 0 },
                    { "Updates", totalByAction.FirstOrDefault(x => x.Action == "UPDATE")?.Count ?? 0 },
                    { "Deletes", totalByAction.FirstOrDefault(x => x.Action == "DELETE")?.Count ?? 0 },
                    { "Views", totalByAction.FirstOrDefault(x => x.Action == "VIEW")?.Count ?? 0 }
                },
                RecentActivity = recentActivity.Select(MapToDto).ToList(),
                TopUsers = topUsers
            };
        }

        public List<FieldChange> CompareObjects<T>(T oldObj, T newObj, Dictionary<string, string> fieldLabels) where T : class
        {
            var changes = new List<FieldChange>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                // Skip ignored properties
                if (prop.Name.StartsWith("_") ||
                    prop.Name == "DateCreated" ||
                    prop.Name == "CreatedBy" ||
                    prop.Name == "DateUpdated" ||
                    prop.Name == "UpdatedBy" ||
                    prop.Name == "UpdatedDate")
                {
                    continue;
                }

                var oldValue = oldObj != null ? prop.GetValue(oldObj) : null;
                var newValue = newObj != null ? prop.GetValue(newObj) : null;

                // Compare values
                if (!AreValuesEqual(oldValue, newValue))
                {
                    changes.Add(new FieldChange
                    {
                        FieldName = prop.Name,
                        FieldLabel = fieldLabels.ContainsKey(prop.Name) ? fieldLabels[prop.Name] : FormatFieldName(prop.Name),
                        OldValue = oldValue,
                        NewValue = newValue,
                        DataType = GetDataType(newValue ?? oldValue)
                    });
                }
            }

            return changes;
        }

        private bool AreValuesEqual(object? value1, object? value2)
        {
            if (value1 == null && value2 == null) return true;
            if (value1 == null || value2 == null) return false;
            return value1.Equals(value2);
        }

        private string FormatFieldName(string fieldName)
        {
            // Convert PascalCase to Title Case
            return System.Text.RegularExpressions.Regex.Replace(fieldName, "([A-Z])", " $1").Trim();
        }

        private string GetDataType(object? value)
        {
            if (value == null) return "string";

            var type = value.GetType();

            if (type == typeof(bool)) return "boolean";
            if (type == typeof(int) || type == typeof(long) || type == typeof(decimal) || type == typeof(double)) return "number";
            if (type == typeof(DateTime)) return "date";
            if (type.IsClass && type != typeof(string)) return "object";

            return "string";
        }

        private AuditLogDto MapToDto(AuditLog auditLog)
        {
            var changes = new List<FieldChange>();

            if (!string.IsNullOrEmpty(auditLog.Changes))
            {
                try
                {
                    changes = JsonSerializer.Deserialize<List<FieldChange>>(auditLog.Changes) ?? new List<FieldChange>();
                }
                catch
                {
                    changes = new List<FieldChange>();
                }
            }

            return new AuditLogDto
            {
                AuditId = auditLog.AuditId,
                EntityType = auditLog.EntityType,
                EntityId = auditLog.EntityId,
                EntityName = auditLog.EntityName,
                Action = auditLog.Action,
                Changes = changes,
                PerformedBy = auditLog.PerformedBy,
                PerformedByName = auditLog.PerformedByName,
                PerformedAt = auditLog.PerformedAt,
                IpAddress = auditLog.IpAddress,
                UserAgent = auditLog.UserAgent,
                Notes = auditLog.Notes
            };
        }
    }
}