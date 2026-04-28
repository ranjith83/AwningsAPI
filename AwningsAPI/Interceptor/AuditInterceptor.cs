// Interceptors/AuditInterceptor.cs
using AwningsAPI.Model.Audit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace AwningsAPI.Interceptors
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Dictionary to map entity types to their display names
        private readonly Dictionary<string, string> _entityTypeMap = new()
        {
            { "Customer", AuditEntityType.CUSTOMER },
            { "CustomerContact", AuditEntityType.CONTACT },
            { "WorkflowStart", AuditEntityType.WORKFLOW },
            { "Quote", AuditEntityType.QUOTE },
            { "Invoice", AuditEntityType.INVOICE },
            { "SiteVisit", AuditEntityType.SITE_VISIT }
        };

        // Field labels for better readability
        private readonly Dictionary<string, string> _fieldLabels = new()
        {
            // Customer fields
            { "Name", "Company Name" },
            { "CompanyNumber", "Company Number" },
            { "Residential", "Residential Customer" },
            { "RegistrationNumber", "Registration Number" },
            { "VATNumber", "VAT Number" },
            { "Address1", "Address Line 1" },
            { "Address2", "Address Line 2" },
            { "Address3", "Address Line 3" },
            { "County", "County" },
            { "Phone", "Phone" },
            { "Mobile", "Mobile" },
            { "Email", "Email" },
            { "TaxNumber", "Tax Number" },
            { "Eircode", "Eircode" },
            
            // Contact fields
            { "FirstName", "First Name" },
            { "LastName", "Last Name" },
            { "DateOfBirth", "Date of Birth" },
            
            // Workflow fields
            { "Description", "Description" },
            { "WorkflowName", "Workflow Name" },
            { "ProductId", "Product" },
            { "SupplierId", "Supplier" },
            
            // Quote/Invoice fields
            { "QuoteNumber", "Quote Number" },
            { "InvoiceNumber", "Invoice Number" },
            { "TotalAmount", "Total Amount" },
            { "Status", "Status" },
            
            // Site Visit fields
            { "ProductModelType", "Product Model Type" },
            { "SiteLayout", "Site Layout" },
            { "Structure", "Structure" }
        };

        public AuditInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            if (eventData.Context != null)
            {
                CaptureAuditLogs(eventData.Context);
            }

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context != null)
            {
                CaptureAuditLogs(eventData.Context);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void CaptureAuditLogs(DbContext context)
        {
            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                           e.State == EntityState.Modified ||
                           e.State == EntityState.Deleted)
                .Where(e => e.Entity.GetType().Name != "AuditLog") // Don't audit the audit log itself
                .ToList();

            var auditLogs = new List<AuditLog>();
            var currentUser = GetCurrentUser();
            var ipAddress = GetIpAddress();
            var userAgent = GetUserAgent();

            foreach (var entry in entries)
            {
                var auditLog = CreateAuditLog(entry, currentUser, ipAddress, userAgent);
                if (auditLog != null)
                {
                    auditLogs.Add(auditLog);
                }
            }

            // Add audit logs to context
            if (auditLogs.Any())
            {
                context.Set<AuditLog>().AddRange(auditLogs);
            }
        }

        private AuditLog? CreateAuditLog(
            EntityEntry entry,
            (int userId, string userName) currentUser,
            string ipAddress,
            string userAgent)
        {
            var entityType = entry.Entity.GetType().Name;

            // Map to audit entity type
            if (!_entityTypeMap.TryGetValue(entityType, out var mappedEntityType))
            {
                // Skip entities we don't want to audit
                return null;
            }

            var entityId = GetEntityId(entry);
            if (entityId == 0)
            {
                // For new entities, we'll need to get the ID after insert
                // Store a placeholder for now
                entityId = -1;
            }

            var entityName = GetEntityName(entry);
            var action = GetAction(entry.State);
            var changes = GetChanges(entry);

            // Skip if no changes (shouldn't happen, but just in case)
            if (!changes.Any() && entry.State == EntityState.Modified)
            {
                return null;
            }

            return new AuditLog
            {
                EntityType = mappedEntityType,
                EntityId = entityId,
                EntityName = entityName,
                Action = action,
                Changes = JsonSerializer.Serialize(changes),
                PerformedBy = currentUser.userId,
                PerformedByName = currentUser.userName,
                PerformedAt = DateTime.UtcNow,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Notes = GenerateNotes(entry.State, changes.Count)
            };
        }

        private int GetEntityId(EntityEntry entry)
        {
            // Try common ID property names
            var idProperties = new[] { "CustomerId", "ContactId", "WorkflowId", "QuoteId", "InvoiceId", "SiteVisitId", "Id" };

            foreach (var propName in idProperties)
            {
                var property = entry.Properties.FirstOrDefault(p => p.Metadata.Name == propName);
                if (property != null && property.CurrentValue != null)
                {
                    if (int.TryParse(property.CurrentValue.ToString(), out int id))
                    {
                        return id;
                    }
                }
            }

            return 0;
        }

        private string GetEntityName(EntityEntry entry)
        {
            // Try to get a meaningful name for the entity
            var nameProperties = new[] { "Name", "CompanyName", "WorkflowName", "Description", "QuoteNumber", "InvoiceNumber" };

            foreach (var propName in nameProperties)
            {
                var property = entry.Properties.FirstOrDefault(p => p.Metadata.Name == propName);
                if (property?.CurrentValue != null)
                {
                    return property.CurrentValue.ToString() ?? "Unknown";
                }
            }

            // For contacts, combine first and last name
            var firstName = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "FirstName")?.CurrentValue?.ToString();
            var lastName = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "LastName")?.CurrentValue?.ToString();
            if (!string.IsNullOrEmpty(firstName) || !string.IsNullOrEmpty(lastName))
            {
                return $"{firstName} {lastName}".Trim();
            }

            return entry.Entity.GetType().Name;
        }

        private string GetAction(EntityState state)
        {
            return state switch
            {
                EntityState.Added => AuditAction.CREATE,
                EntityState.Modified => AuditAction.UPDATE,
                EntityState.Deleted => AuditAction.DELETE,
                _ => "UNKNOWN"
            };
        }

        private List<FieldChange> GetChanges(EntityEntry entry)
        {
            var changes = new List<FieldChange>();

            // Properties to ignore
            var ignoredProperties = new HashSet<string>
            {
                "DateCreated", "CreatedBy", "DateUpdated", "UpdatedBy",
                "UpdatedDate", "CreatedDate", "PasswordHash", "RefreshTokens"
            };

            foreach (var property in entry.Properties)
            {
                var propertyName = property.Metadata.Name;

                // Skip ignored properties
                if (ignoredProperties.Contains(propertyName) || propertyName.StartsWith("_"))
                {
                    continue;
                }

                object? oldValue = null;
                object? newValue = null;

                if (entry.State == EntityState.Added)
                {
                    // For new entities, old value is null
                    newValue = property.CurrentValue;

                    // Only log if new value is not null or empty
                    if (newValue == null || (newValue is string str && string.IsNullOrEmpty(str)))
                    {
                        continue;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    oldValue = property.OriginalValue;
                    newValue = property.CurrentValue;

                    // Skip if values are the same
                    if (AreValuesEqual(oldValue, newValue))
                    {
                        continue;
                    }
                }
                else if (entry.State == EntityState.Deleted)
                {
                    // For deleted entities, capture the original values
                    oldValue = property.OriginalValue;
                    newValue = null;
                }

                changes.Add(new FieldChange
                {
                    FieldName = propertyName,
                    FieldLabel = _fieldLabels.ContainsKey(propertyName) ? _fieldLabels[propertyName] : FormatFieldName(propertyName),
                    OldValue = oldValue,
                    NewValue = newValue,
                    DataType = GetDataType(newValue ?? oldValue)
                });
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

        private string GenerateNotes(EntityState state, int changeCount)
        {
            return state switch
            {
                EntityState.Added => "Record created",
                EntityState.Modified => $"Updated {changeCount} field(s)",
                EntityState.Deleted => "Record deleted",
                _ => "Unknown action"
            };
        }

        private (int userId, string userName) GetCurrentUser()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext?.User?.Identity?.IsAuthenticated == true)
                {
                    var userIdClaim = httpContext.User.FindFirst("UserId")?.Value
                                   ?? httpContext.User.FindFirst("sub")?.Value;
                    var userName = httpContext.User.Identity.Name
                                ?? httpContext.User.FindFirst("name")?.Value
                                ?? "Unknown User";

                    if (int.TryParse(userIdClaim, out int userId))
                    {
                        return (userId, userName);
                    }

                    return (1, userName); // Default to user ID 1 if parsing fails
                }
            }
            catch
            {
                // If anything fails, return default
            }

            return (1, "System");
        }

        private string GetIpAddress()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    // Check for forwarded IP first (in case behind proxy/load balancer)
                    var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(forwardedFor))
                    {
                        return forwardedFor.Split(',')[0].Trim();
                    }

                    return httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                }
            }
            catch
            {
                // If anything fails, return unknown
            }

            return "Unknown";
        }

        private string GetUserAgent()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    return httpContext.Request.Headers["User-Agent"].ToString();
                }
            }
            catch
            {
                // If anything fails, return unknown
            }

            return "Unknown";
        }
    }

    // Helper class for field changes
    public class FieldChange
    {
        public string FieldName { get; set; } = string.Empty;
        public string FieldLabel { get; set; } = string.Empty;
        public object? OldValue { get; set; }
        public object? NewValue { get; set; }
        public string DataType { get; set; } = "string";
    }
}