using System;
using System.Collections.Generic;

namespace AwningsAPI.Dto.Audit
{
    public class FieldChange
    {
        public string FieldName { get; set; } = string.Empty;
        public string FieldLabel { get; set; } = string.Empty;
        public object? OldValue { get; set; }
        public object? NewValue { get; set; }
        public string DataType { get; set; } = "string";
    }

    public class CreateAuditLogDto
    {
        public string EntityType { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public string? EntityName { get; set; }
        public string Action { get; set; } = string.Empty;
        public List<FieldChange> Changes { get; set; } = new();
        public int PerformedBy { get; set; }
        public string? PerformedByName { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Notes { get; set; }
    }

    public class AuditLogDto
    {
        public int AuditId { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public string? EntityName { get; set; }
        public string Action { get; set; } = string.Empty;
        public List<FieldChange> Changes { get; set; } = new();
        public int PerformedBy { get; set; }
        public string PerformedByName { get; set; } = string.Empty;
        public DateTime PerformedAt { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Notes { get; set; }
    }

    public class AuditLogFilterDto
    {
        public string? EntityType { get; set; }
        public int? EntityId { get; set; }
        public string? Action { get; set; }
        public int? PerformedBy { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class AuditLogPagedResultDto
    {
        public List<AuditLogDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    public class AuditSummaryDto
    {
        public int TotalAudits { get; set; }
        public Dictionary<string, int> TotalByAction { get; set; } = new();
        public List<AuditLogDto> RecentActivity { get; set; } = new();
        public List<TopUserDto> TopUsers { get; set; } = new();
    }

    public class TopUserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int ActionCount { get; set; }
    }
}