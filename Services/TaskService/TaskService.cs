using AwningsAPI.Database;
using AwningsAPI.Dto.Tasks;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Customers;
using AwningsAPI.Model.Email;
using AwningsAPI.Model.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AwningsAPI.Services.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly Microsoft.Extensions.Logging.ILogger<TaskService> _logger;

        public TaskService(AppDbContext context, Microsoft.Extensions.Logging.ILogger<TaskService> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Basic CRUD Operations

        public async Task<EmailTaskDto> GetTaskByIdAsync(int taskId)
        {
            var task = await _context.EmailTasks
                .Include(t => t.TaskComments)
                .Include(t => t.TaskAttachments)
                .Include(t => t.TaskHistories)
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null)
                return null;

            return await MapToDto(task);
        }

        public async Task<IEnumerable<EmailTaskDto>> GetAllTasksAsync()
        {
            var tasks = await _context.EmailTasks
                .Include(t => t.TaskComments)
                .Include(t => t.TaskAttachments)
                .OrderByDescending(t => t.DateAdded)
                .ToListAsync();

            var taskDtos = new List<EmailTaskDto>();
            foreach (var task in tasks)
            {
                taskDtos.Add(await MapToDto(task));
            }

            return taskDtos;
        }

        public async Task<EmailTaskDto> CreateTaskAsync(CreateTaskDto createDto, string currentUser)
        {
            var task = new EmailTask
            {
                IncomingEmailId = createDto.IncomingEmailId,
                FromName = createDto.FromName,
                FromEmail = createDto.FromEmail,
                Subject = createDto.Subject,
                Category = createDto.Category,
                EmailBody = createDto.EmailBody,
                CompanyNumber = createDto.CompanyNumber,
                Priority = createDto.Priority ?? "Normal",
                Status = "Pending",
                DueDate = createDto.DueDate,
                AssignedToUserId = createDto.AssignedToUserId,
                CustomerId = createDto.CustomerId,
                WorkflowId = createDto.WorkflowId,
                DateAdded = DateTime.UtcNow,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };

            // Get assignee name if assigned
            if (task.AssignedToUserId.HasValue)
            {
                var user = await _context.Users.FindAsync(task.AssignedToUserId.Value);
                if (user != null)
                {
                    task.AssignedToUserName = user.Username;
                }
            }

            _context.EmailTasks.Add(task);
            await _context.SaveChangesAsync();

            // Add history entry
            await AddHistoryEntry(
                taskId: task.TaskId,
                action: "Created",
                oldValue: null,
                newValue: null,
                details: "Task created from email",
                createdBy: currentUser,
                customerName: task.CustomerName,
                subject: task.Subject,
                category: task.Category);

            return await GetTaskByIdAsync(task.TaskId);
        }

        public async Task<EmailTaskDto> UpdateTaskAsync(int taskId, UpdateTaskDto updateDto, string currentUser)
        {
            var task = await _context.EmailTasks.FindAsync(taskId);
            if (task == null)
                return null;

            var changes = new List<string>();

            if (!string.IsNullOrEmpty(updateDto.Status) && task.Status != updateDto.Status)
            {
                await AddHistoryEntry(
                    taskId: taskId,
                    action: "StatusChanged",
                    oldValue: task.Status,
                    newValue: updateDto.Status,
                    details: null,
                    createdBy: currentUser);
                task.Status = updateDto.Status;

                if (updateDto.Status == "Processed")
                {
                    task.DateProcessed = DateTime.UtcNow;
                    task.ProcessedBy = currentUser;
                }
            }

            if (!string.IsNullOrEmpty(updateDto.Priority) && task.Priority != updateDto.Priority)
            {
                changes.Add($"Priority changed from {task.Priority} to {updateDto.Priority}");
                task.Priority = updateDto.Priority;
            }

            if (updateDto.DueDate.HasValue && task.DueDate != updateDto.DueDate)
            {
                changes.Add($"Due date changed");
                task.DueDate = updateDto.DueDate;
            }

            if (updateDto.AssignedToUserId.HasValue && task.AssignedToUserId != updateDto.AssignedToUserId)
            {
                var user = await _context.Users.FindAsync(updateDto.AssignedToUserId.Value);
                if (user != null)
                {
                    changes.Add($"Assigned to {user.Username}");
                    task.AssignedToUserId = updateDto.AssignedToUserId;
                    task.AssignedToUserName = user.Username;
                }
            }

            if (!string.IsNullOrEmpty(updateDto.SelectedAction))
            {
                task.SelectedAction = updateDto.SelectedAction;
                changes.Add($"Action set to: {updateDto.SelectedAction}");
            }

            if (updateDto.CustomerId.HasValue)
                task.CustomerId = updateDto.CustomerId;

            if (updateDto.WorkflowId.HasValue)
                task.WorkflowId = updateDto.WorkflowId;

            if (!string.IsNullOrEmpty(updateDto.CompanyNumber))
                task.CompanyNumber = updateDto.CompanyNumber;

            task.DateUpdated = DateTime.UtcNow;
            task.UpdatedBy = currentUser;

            await _context.SaveChangesAsync();

            if (changes.Any())
            {
                await AddHistoryEntry(
                    taskId: taskId,
                    action: "Updated",
                    oldValue: null,
                    newValue: null,
                    details: string.Join("; ", changes),
                    createdBy: currentUser);
            }

            return await GetTaskByIdAsync(taskId);
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var task = await _context.EmailTasks.FindAsync(taskId);
            if (task == null)
                return false;

            _context.EmailTasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Status Management

        public async Task<EmailTaskDto> UpdateTaskStatusAsync(int taskId, UpdateTaskStatusDto statusDto, string currentUser)
        {
            var task = await _context.EmailTasks.FindAsync(taskId);
            if (task == null)
                return null;

            var oldStatus = task.Status;
            task.Status = statusDto.Status;
            task.DateUpdated = DateTime.UtcNow;
            task.UpdatedBy = currentUser;

            if (statusDto.Status == "Processed")
            {
                task.DateProcessed = DateTime.UtcNow;
                task.ProcessedBy = currentUser;
                task.CompletionNotes = statusDto.CompletionNotes;
            }
            else if (statusDto.Status == "Completed")
            {
                task.CompletedDate = DateTime.UtcNow;
                task.CompletedBy = currentUser;
                task.CompletionNotes = statusDto.CompletionNotes;
            }

            await _context.SaveChangesAsync();

            await AddHistoryEntry(
                taskId: taskId,
                action: "StatusChanged",
                oldValue: oldStatus,
                newValue: statusDto.Status,
                details: statusDto.CompletionNotes,
                createdBy: currentUser);

            return await GetTaskByIdAsync(taskId);
        }

        public async Task<EmailTaskDto> CompleteTaskAsync(int taskId, string completionNotes, string currentUser)
        {
            var statusDto = new UpdateTaskStatusDto
            {
                Status = "Completed",
                CompletionNotes = completionNotes
            };

            return await UpdateTaskStatusAsync(taskId, statusDto, currentUser);
        }

        #endregion

        #region Assignment

        public async Task<EmailTaskDto> AssignTaskAsync(int taskId, AssignTaskDto assignDto, string currentUser)
        {
            try
            {
                var task = await _context.EmailTasks.FindAsync(taskId);
                if (task == null)
                    return null;

                var user = await _context.Users.FindAsync(assignDto.AssignedToUserId);
                if (user == null)
                    return null;

                var oldAssignee = task.AssignedToUserName ?? "Unassigned";

                // Assign the user
                task.AssignedToUserId = assignDto.AssignedToUserId;
                task.AssignedToUserName = user.Username;
                task.DateUpdated = DateTime.UtcNow;
                task.UpdatedBy = currentUser;

                // Assigning a task marks it as Processed and moves it to the Processed tab
                var oldStatus = task.Status;
                task.Status = "Processed";
                task.DateProcessed = DateTime.UtcNow;
                task.ProcessedBy = currentUser;

                await _context.SaveChangesAsync();

                await AddHistoryEntry(
                    taskId: taskId,
                    action: "Assigned",
                    oldValue: oldAssignee,
                    newValue: user.Username,
                    details: assignDto.Notes,
                    createdBy: currentUser,
                    customerName: task.CustomerName,
                    subject: task.Subject,
                    category: task.Category);

                if (oldStatus != "Processed")
                    await AddHistoryEntry(
                        taskId: taskId,
                        action: "StatusChanged",
                        oldValue: oldStatus,
                        newValue: "Processed",
                        details: $"Status set to Processed when assigned to {user.Username}",
                        createdBy: currentUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return await GetTaskByIdAsync(taskId);
        }

        public async Task<EmailTaskDto> UnassignTaskAsync(int taskId, string currentUser)
        {
            var task = await _context.EmailTasks.FindAsync(taskId);
            if (task == null)
                return null;

            var oldAssignee = task.AssignedToUserName ?? "Unassigned";
            task.AssignedToUserId = null;
            task.AssignedToUserName = null;
            task.DateUpdated = DateTime.UtcNow;
            task.UpdatedBy = currentUser;

            await _context.SaveChangesAsync();

            await AddHistoryEntry(
                taskId: taskId,
                action: "Unassigned",
                oldValue: oldAssignee,
                newValue: "Unassigned",
                details: null,
                createdBy: currentUser,
                customerName: task.CustomerName,
                subject: task.Subject,
                category: task.Category);

            return await GetTaskByIdAsync(taskId);
        }

        #endregion

        #region Comments

        public async Task<TaskCommentDto> AddCommentAsync(int taskId, AddCommentDto commentDto, string currentUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUser);

            var comment = new TaskComment
            {
                TaskId = taskId,
                CommentText = commentDto.CommentText,
                UserId = user?.UserId ?? 0,
                UserName = currentUser,
                DateCreated = DateTime.UtcNow
            };

            _context.TaskComments.Add(comment);
            await _context.SaveChangesAsync();

            await AddHistoryEntry(
                taskId: taskId,
                action: "Commented",
                oldValue: null,
                newValue: null,
                details: "Comment added",
                createdBy: currentUser);

            return new TaskCommentDto
            {
                CommentId = comment.CommentId,
                TaskId = comment.TaskId,
                CommentText = comment.CommentText,
                UserId = comment.UserId,
                UserName = comment.UserName,
                DateCreated = comment.DateCreated,
                IsEdited = comment.IsEdited
            };
        }

        public async Task<IEnumerable<TaskCommentDto>> GetTaskCommentsAsync(int taskId)
        {
            var comments = await _context.TaskComments
                .Where(c => c.TaskId == taskId)
                .OrderByDescending(c => c.DateCreated)
                .ToListAsync();

            return comments.Select(c => new TaskCommentDto
            {
                CommentId = c.CommentId,
                TaskId = c.TaskId,
                CommentText = c.CommentText,
                UserId = c.UserId,
                UserName = c.UserName,
                DateCreated = c.DateCreated,
                DateUpdated = c.DateUpdated,
                IsEdited = c.IsEdited
            });
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            var comment = await _context.TaskComments.FindAsync(commentId);
            if (comment == null)
                return false;

            _context.TaskComments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Filtering & Queries

        public async Task<(IEnumerable<EmailTaskDto> Tasks, int TotalCount)> GetTasksWithFiltersAsync(TaskFilterDto filterDto)
        {
            var query = _context.EmailTasks
                .Include(t => t.TaskComments)
                .Include(t => t.TaskAttachments)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(filterDto.Status))
                query = query.Where(t => t.Status == filterDto.Status);

            if (!string.IsNullOrEmpty(filterDto.TaskType))
                query = query.Where(t => t.TaskType == filterDto.TaskType);

            if (!string.IsNullOrEmpty(filterDto.Priority))
                query = query.Where(t => t.Priority == filterDto.Priority);

            if (filterDto.AssignedToUserId.HasValue)
                query = query.Where(t => t.AssignedToUserId == filterDto.AssignedToUserId);

            if (filterDto.CustomerId.HasValue)
                query = query.Where(t => t.CustomerId == filterDto.CustomerId);

            if (filterDto.DueDateFrom.HasValue)
                query = query.Where(t => t.DueDate >= filterDto.DueDateFrom);

            if (filterDto.DueDateTo.HasValue)
                query = query.Where(t => t.DueDate <= filterDto.DueDateTo);

            if (filterDto.CreatedDateFrom.HasValue)
                query = query.Where(t => t.DateAdded >= filterDto.CreatedDateFrom);

            if (filterDto.CreatedDateTo.HasValue)
                query = query.Where(t => t.DateAdded <= filterDto.CreatedDateTo);

            if (!string.IsNullOrEmpty(filterDto.SearchTerm))
            {
                query = query.Where(t =>
                    t.Subject.Contains(filterDto.SearchTerm) ||
                    t.FromName.Contains(filterDto.SearchTerm) ||
                    t.FromEmail.Contains(filterDto.SearchTerm) ||
                    t.CustomerName.Contains(filterDto.SearchTerm));
            }

            var totalCount = await query.CountAsync();

            // Sorting
            query = filterDto.SortBy?.ToLower() switch
            {
                "subject" => filterDto.SortDirection == "ASC" ? query.OrderBy(t => t.Subject) : query.OrderByDescending(t => t.Subject),
                "status" => filterDto.SortDirection == "ASC" ? query.OrderBy(t => t.Status) : query.OrderByDescending(t => t.Status),
                "priority" => filterDto.SortDirection == "ASC" ? query.OrderBy(t => t.Priority) : query.OrderByDescending(t => t.Priority),
                "duedate" => filterDto.SortDirection == "ASC" ? query.OrderBy(t => t.DueDate) : query.OrderByDescending(t => t.DueDate),
                "category" => filterDto.SortDirection == "ASC" ? query.OrderBy(t => t.Category) : query.OrderByDescending(t => t.Category),
                _ => filterDto.SortDirection == "ASC" ? query.OrderBy(t => t.DateAdded) : query.OrderByDescending(t => t.DateAdded)
            };

            // Pagination
            var tasks = await query
                .Skip((filterDto.Page - 1) * filterDto.PageSize)
                .Take(filterDto.PageSize)
                .ToListAsync();

            var taskDtos = new List<EmailTaskDto>();
            foreach (var task in tasks)
            {
                taskDtos.Add(await MapToDto(task));
            }

            return (taskDtos, totalCount);
        }

        public async Task<IEnumerable<EmailTaskDto>> GetTasksByUserAsync(int userId)
        {
            var tasks = await _context.EmailTasks
                .Include(t => t.TaskComments)
                .Include(t => t.TaskAttachments)
                .Where(t => t.AssignedToUserId == userId)
                .OrderByDescending(t => t.DateAdded)
                .ToListAsync();

            var taskDtos = new List<EmailTaskDto>();
            foreach (var task in tasks)
            {
                taskDtos.Add(await MapToDto(task));
            }

            return taskDtos;
        }

        public async Task<IEnumerable<EmailTaskDto>> GetTasksByCustomerAsync(int customerId)
        {
            var tasks = await _context.EmailTasks
                .Include(t => t.TaskComments)
                .Include(t => t.TaskAttachments)
                .Where(t => t.CustomerId == customerId)
                .OrderByDescending(t => t.DateAdded)
                .ToListAsync();

            var taskDtos = new List<EmailTaskDto>();
            foreach (var task in tasks)
            {
                taskDtos.Add(await MapToDto(task));
            }

            return taskDtos;
        }

        public async Task<IEnumerable<EmailTaskDto>> GetTasksByTypeAsync(string taskType)
        {
            var tasks = await _context.EmailTasks
                .Include(t => t.TaskComments)
                .Include(t => t.TaskAttachments)
                .Where(t => t.TaskType == taskType || t.Category == taskType)
                .OrderByDescending(t => t.DateAdded)
                .ToListAsync();

            var taskDtos = new List<EmailTaskDto>();
            foreach (var task in tasks)
            {
                taskDtos.Add(await MapToDto(task));
            }

            return taskDtos;
        }

        public async Task<IEnumerable<EmailTaskDto>> GetOverdueTasksAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tasks = await _context.EmailTasks
                .Include(t => t.TaskComments)
                .Include(t => t.TaskAttachments)
                .Where(t => t.DueDate.HasValue && t.DueDate.Value.Date < today && t.Status != "Completed" && t.Status != "Processed")
                .OrderBy(t => t.DueDate)
                .ToListAsync();

            var taskDtos = new List<EmailTaskDto>();
            foreach (var task in tasks)
            {
                taskDtos.Add(await MapToDto(task));
            }

            return taskDtos;
        }

        public async Task<IEnumerable<EmailTaskDto>> GetTasksDueTodayAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tasks = await _context.EmailTasks
                .Include(t => t.TaskComments)
                .Include(t => t.TaskAttachments)
                .Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == today && t.Status != "Completed" && t.Status != "Processed")
                .OrderBy(t => t.Priority)
                .ToListAsync();

            var taskDtos = new List<EmailTaskDto>();
            foreach (var task in tasks)
            {
                taskDtos.Add(await MapToDto(task));
            }

            return taskDtos;
        }

        #endregion

        #region Statistics

        public async Task<TaskStatistics> GetTaskStatisticsAsync()
        {
            var today = DateTime.UtcNow.Date;

            var stats = new TaskStatistics
            {
                TotalTasks = await _context.EmailTasks.CountAsync(),
                PendingTasks = await _context.EmailTasks.CountAsync(t => t.Status == "Pending"),
                InProgressTasks = await _context.EmailTasks.CountAsync(t => t.Status == "In Progress"),
                CompletedTasks = await _context.EmailTasks.CountAsync(t => t.Status == "Completed" || t.Status == "Processed"),
                OverdueTasks = await _context.EmailTasks.CountAsync(t => t.DueDate.HasValue && t.DueDate.Value.Date < today && t.Status != "Completed" && t.Status != "Processed"),
                HighPriorityTasks = await _context.EmailTasks.CountAsync(t => t.Priority == "High" && t.Status != "Completed"),
                UrgentTasks = await _context.EmailTasks.CountAsync(t => t.Priority == "Urgent" && t.Status != "Completed")
            };

            // Tasks by type
            stats.TasksByType = await _context.EmailTasks
                .GroupBy(t => t.Category)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Type ?? "Unknown", x => x.Count);

            // Tasks by assignee
            stats.TasksByAssignee = await _context.EmailTasks
                .Where(t => t.AssignedToUserName != null)
                .GroupBy(t => t.AssignedToUserName)
                .Select(g => new { Assignee = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Assignee, x => x.Count);

            return stats;
        }

        public async Task<TaskStatistics> GetUserTaskStatisticsAsync(int userId)
        {
            var today = DateTime.UtcNow.Date;

            var stats = new TaskStatistics
            {
                TotalTasks = await _context.EmailTasks.CountAsync(t => t.AssignedToUserId == userId),
                PendingTasks = await _context.EmailTasks.CountAsync(t => t.AssignedToUserId == userId && t.Status == "Pending"),
                InProgressTasks = await _context.EmailTasks.CountAsync(t => t.AssignedToUserId == userId && t.Status == "In Progress"),
                CompletedTasks = await _context.EmailTasks.CountAsync(t => t.AssignedToUserId == userId && (t.Status == "Completed" || t.Status == "Processed")),
                OverdueTasks = await _context.EmailTasks.CountAsync(t => t.AssignedToUserId == userId && t.DueDate.HasValue && t.DueDate.Value.Date < today && t.Status != "Completed" && t.Status != "Processed"),
                HighPriorityTasks = await _context.EmailTasks.CountAsync(t => t.AssignedToUserId == userId && t.Priority == "High" && t.Status != "Completed"),
                UrgentTasks = await _context.EmailTasks.CountAsync(t => t.AssignedToUserId == userId && t.Priority == "Urgent" && t.Status != "Completed")
            };

            // Tasks by type for this user
            stats.TasksByType = await _context.EmailTasks
                .Where(t => t.AssignedToUserId == userId)
                .GroupBy(t => t.Category)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Type ?? "Unknown", x => x.Count);

            return stats;
        }

        #endregion

        #region History

        public async Task<IEnumerable<TaskHistoryDto>> GetTaskHistoryAsync(int taskId)
        {
            var history = await _context.TaskHistories
                .Where(h => h.TaskId == taskId)
                .OrderByDescending(h => h.DateCreated)
                .ToListAsync();

            return history.Select(h => new TaskHistoryDto
            {
                HistoryId = h.HistoryId,
                TaskId = h.TaskId,
                Action = h.Action,
                OldValue = h.OldValue,
                NewValue = h.NewValue,
                Details = h.Details,
                DateCreated = h.DateCreated,
                CreatedBy = h.CreatedBy
            });
        }

        #endregion

        #region Create Task From Email

        public async Task<EmailTaskDto> CreateTaskFromEmailAsync(int incomingEmailId, string currentUser)
        {
            var email = await _context.IncomingEmails
                .Include(e => e.Attachments)
                .FirstOrDefaultAsync(e => e.Id == incomingEmailId);

            if (email == null)
                return null;

            _logger.LogInformation($"Creating task from email ID: {incomingEmailId}");

            // Extract data from AI analysis
            var extractedData = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(email.ExtractedData))
            {
                try
                {
                    extractedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(email.ExtractedData);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Failed to parse extracted data: {ex.Message}");
                }
            }

            // Map category
            var category = MapCategoryToDisplay(email.Category);
            var taskType = email.Category;

            // Determine priority
            var priority = DeterminePriority(email.Importance, email.Category);

            // Set due date based on priority
            var dueDate = CalculateDueDate(priority);

            // Extract company number if available
            var companyNumber = GetValue(extractedData, "companyNumber", null);

            var createDto = new CreateTaskDto
            {
                IncomingEmailId = incomingEmailId,
                FromName = email.FromName,
                FromEmail = email.FromEmail,
                Subject = email.Subject,
                Category = category,
                EmailBody = email.BodyContent,
                CompanyNumber = companyNumber,
                Priority = priority,
                DueDate = dueDate
            };

            var task = await CreateTaskAsync(createDto, currentUser);

            // Copy attachments from email to task
            if (email.HasAttachments && email.Attachments.Any())
            {
                foreach (var emailAttachment in email.Attachments)
                {
                    var taskAttachment = new TaskAttachment
                    {
                        TaskId = task.TaskId,
                        EmailAttachmentId = emailAttachment.Id,
                        FileName = emailAttachment.FileName,
                        FileType = emailAttachment.ContentType,
                        FileSize = emailAttachment.Size,
                        BlobUrl = emailAttachment.BlobStorageUrl,
                        ExtractedText = emailAttachment.ExtractedText,
                        DateUploaded = DateTime.UtcNow,
                        UploadedBy = "System"
                    };

                    _context.TaskAttachments.Add(taskAttachment);
                }

                await _context.SaveChangesAsync();
            }

            // Update the task with AI data
            var dbTask = await _context.EmailTasks.FindAsync(task.TaskId);
            if (dbTask != null)
            {
                dbTask.TaskType = taskType;
                dbTask.ExtractedData = email.ExtractedData;
                dbTask.AIConfidence = email.CategoryConfidence;
                dbTask.HasAttachments = email.HasAttachments;

                await _context.SaveChangesAsync();
            }

            _logger.LogInformation($"Task created successfully: TaskId={task.TaskId}, Category={category}");

            return await GetTaskByIdAsync(task.TaskId);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Writes one row to TaskHistories.
        /// The optional customerName / subject / category params are denormalised
        /// at write time so the Audit tab grid never needs a join back to EmailTasks.
        /// Only pass them for the three audit-visible actions:
        ///   Created | Assigned | Unassigned
        /// </summary>
        private async Task AddHistoryEntry(
            int taskId,
            string action,
            string? oldValue,
            string? newValue,
            string? details,
            string createdBy,
            string? customerName = null,
            string? subject = null,
            string? category = null)
        {
            var history = new TaskHistory
            {
                TaskId = taskId,
                Action = action,
                OldValue = oldValue,
                NewValue = newValue,
                Details = details,
                DateCreated = DateTime.UtcNow,
                CreatedBy = createdBy,
                CustomerName = customerName,
                Subject = subject,
                Category = category
            };

            _context.TaskHistories.Add(history);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Returns paginated TaskHistory rows filtered to the three audit actions:
        ///   Created | Assigned | Unassigned
        /// Optionally narrow to one action via the action parameter.
        /// Called by: GET /api/EmailTask/audit
        /// </summary>
        public async Task<TaskHistoryPagedDto> GetTaskAuditHistoryAsync(
            int page = 1,
            int pageSize = 20,
            string? action = null)
        {
            var auditActions = new[] { "Created", "Assigned", "Unassigned" };

            var query = _context.TaskHistories
                .Where(h => auditActions.Contains(h.Action));

            if (!string.IsNullOrEmpty(action) && auditActions.Contains(action))
                query = query.Where(h => h.Action == action);

            query = query.OrderByDescending(h => h.DateCreated);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new TaskHistoryPagedDto
            {
                Items = items.Select(h => new TaskHistoryDto
                {
                    HistoryId = h.HistoryId,
                    TaskId = h.TaskId,
                    Action = h.Action,
                    OldValue = h.OldValue,
                    NewValue = h.NewValue,
                    Details = h.Details,
                    DateCreated = h.DateCreated,
                    CreatedBy = h.CreatedBy,
                    CustomerName = h.CustomerName,
                    Subject = h.Subject,
                    Category = h.Category
                }).ToList(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        private async Task<EmailTaskDto> MapToDto(EmailTask task)
        {
            return new EmailTaskDto
            {
                TaskId = task.TaskId,
                IncomingEmailId = task.IncomingEmailId,
                FromName = task.FromName,
                FromEmail = task.FromEmail,
                Subject = task.Subject,
                Category = task.Category,
                DateAdded = task.DateAdded,
                Status = task.Status,
                TaskType = task.TaskType,
                Priority = task.Priority,
                AssignedToUserId = task.AssignedToUserId,
                AssignedToUserName = task.AssignedToUserName,
                AssignedByUserId = task.AssignedByUserId,
                AssignedByUserName = task.AssignedByUserName,
                CompanyNumber = task.CompanyNumber,
                EmailBody = task.EmailBody,
                HasAttachments = task.HasAttachments,
                SelectedAction = task.SelectedAction,
                CustomerId = task.CustomerId,
                CustomerName = task.CustomerName,
                CustomerEmail = task.CustomerEmail,
                WorkflowId = task.WorkflowId,
                DueDate = task.DueDate,
                DateProcessed = task.DateProcessed,
                ProcessedBy = task.ProcessedBy,
                CompletedDate = task.CompletedDate,
                CompletedBy = task.CompletedBy,
                CompletionNotes = task.CompletionNotes,
                ExtractedData = task.ExtractedData,
                AIConfidence = task.AIConfidence,
                AIReasoning = task.AIReasoning,
                DateCreated = task.DateCreated,
                DateUpdated = task.DateUpdated,
                CreatedBy = task.CreatedBy,
                UpdatedBy = task.UpdatedBy,
                Comments = task.TaskComments?.Select(c => new TaskCommentDto
                {
                    CommentId = c.CommentId,
                    TaskId = c.TaskId,
                    CommentText = c.CommentText,
                    UserId = c.UserId,
                    UserName = c.UserName,
                    DateCreated = c.DateCreated,
                    DateUpdated = c.DateUpdated,
                    IsEdited = c.IsEdited
                }).ToList() ?? new List<TaskCommentDto>(),
                Attachments = task.TaskAttachments?.Select(a => new TaskAttachmentDto
                {
                    AttachmentId = a.AttachmentId,
                    TaskId = a.TaskId,
                    FileName = a.FileName,
                    FileType = a.FileType,
                    FileSize = a.FileSize,
                    FilePath = a.FilePath,
                    BlobUrl = a.BlobUrl,
                    DateUploaded = a.DateUploaded,
                    UploadedBy = a.UploadedBy
                }).ToList() ?? new List<TaskAttachmentDto>(),
                History = task.TaskHistories?.Select(h => new TaskHistoryDto
                {
                    HistoryId = h.HistoryId,
                    TaskId = h.TaskId,
                    Action = h.Action,
                    OldValue = h.OldValue,
                    NewValue = h.NewValue,
                    Details = h.Details,
                    DateCreated = h.DateCreated,
                    CreatedBy = h.CreatedBy,
                    CustomerName = h.CustomerName,
                    Subject = h.Subject,
                    Category = h.Category
                }).OrderByDescending(h => h.DateCreated).ToList() ?? new List<TaskHistoryDto>()
            };
        }

        private string MapCategoryToDisplay(string category)
        {
            return category switch
            {
                "invoice_creation" => "New Invoice",
                "quote_creation" => "New Quote",
                "customer_creation" => "New Customer",
                "showroom_booking" => "Site Visit",
                "product_inquiry" => "Inquiry",
                "complaint" => "Complaint",
                "general_inquiry" => "Inquiry",
                "payment" => "Payment",
                _ => category ?? "Inquiry"
            };
        }

        private string DeterminePriority(string emailImportance, string category)
        {
            if (category == "complaint") return "Urgent";
            if (emailImportance == "High") return "High";
            if (category == "invoice_creation" || category == "quote_creation") return "High";
            if (category == "showroom_booking") return "High";
            return "Normal";
        }

        private DateTime CalculateDueDate(string priority)
        {
            return priority switch
            {
                "Urgent" => DateTime.UtcNow.AddDays(1),
                "High" => DateTime.UtcNow.AddDays(3),
                "Normal" => DateTime.UtcNow.AddDays(7),
                _ => DateTime.UtcNow.AddDays(14)
            };
        }

        private string GetValue(Dictionary<string, object> dict, string key, string defaultValue)
        {
            if (dict != null && dict.TryGetValue(key, out var value))
                return value?.ToString() ?? defaultValue;
            return defaultValue;
        }

        #endregion


        /// <summary>
        /// Execute a specific action on a task
        /// </summary>
        public async Task<object> ExecuteTaskActionAsync(int taskId, string action, object data)
        {
            var task = await _context.EmailTasks
                .Include(t => t.TaskAttachments)
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null)
            {
                throw new Exception("Task not found");
            }

            _logger.LogInformation($"Executing action '{action}' on task {taskId}");

            return action.ToLower() switch
            {
                "add_company" => await ExecuteAddCompanyAction(task, data),
                "generate_quote" => await ExecuteGenerateQuoteAction(task, data),
                "generate_invoice" => await ExecuteGenerateInvoiceAction(task, data),
                "add_site_visit" => await ExecuteAddSiteVisitAction(task, data),
                "move_to_junk" => await ExecuteMoveToJunkAction(task, data),
                _ => throw new Exception($"Unknown action: {action}")
            };
        }

        private async Task<object> ExecuteAddCompanyAction(EmailTask task, object data)
        {
            // Extract company data from the email
            var extractedData = string.IsNullOrEmpty(task.ExtractedData)
                ? new Dictionary<string, object>()
                : JsonConvert.DeserializeObject<Dictionary<string, object>>(task.ExtractedData);

            var companyData = new
            {
                CompanyName = GetValue(extractedData, "companyName", task.FromName),
                ContactEmail = task.FromEmail,
                ContactName = task.FromName,
                CompanyNumber = task.CompanyNumber,
                Source = "Email",
                Notes = $"Created from email task: {task.Subject}",
                ExtractedData = task.ExtractedData
            };

            _logger.LogInformation($"Add company action data prepared for task {task.TaskId}");

            // Return data that can be used to create the company
            // The actual company creation would be handled by a CompanyService
            return new
            {
                success = true,
                action = "add_company",
                taskId = task.TaskId,
                data = companyData,
                message = "Company data extracted successfully. Ready for company creation."
            };
        }

        #region Action Execution
        private async Task<object> ExecuteGenerateQuoteAction(EmailTask task, object data)
        {
            // Extract quote-related data from the email
            var extractedData = string.IsNullOrEmpty(task.ExtractedData)
                ? new Dictionary<string, object>()
                : JsonConvert.DeserializeObject<Dictionary<string, object>>(task.ExtractedData);

            var quoteData = new
            {
                CustomerName = task.FromName,
                CustomerEmail = task.FromEmail,
                CustomerId = task.CustomerId,
                CompanyNumber = task.CompanyNumber,
                Subject = task.Subject,
                Items = GetValue(extractedData, "items", ""),
                Quantities = GetValue(extractedData, "quantities", ""),
                Notes = task.EmailBody,
                Attachments = task.TaskAttachments?.Select(a => new
                {
                    a.FileName,
                    a.BlobUrl,
                    a.FileType
                }).ToList(),
                ExtractedData = task.ExtractedData
            };

            _logger.LogInformation($"Generate quote action data prepared for task {task.TaskId}");

            return new
            {
                success = true,
                action = "generate_quote",
                taskId = task.TaskId,
                data = quoteData,
                message = "Quote data extracted successfully. Ready for quote generation."
            };
        }

        private async Task<object> ExecuteGenerateInvoiceAction(EmailTask task, object data)
        {
            // Extract invoice-related data from the email
            var extractedData = string.IsNullOrEmpty(task.ExtractedData)
                ? new Dictionary<string, object>()
                : JsonConvert.DeserializeObject<Dictionary<string, object>>(task.ExtractedData);

            var invoiceData = new
            {
                CustomerName = task.FromName,
                CustomerEmail = task.FromEmail,
                CustomerId = task.CustomerId,
                CompanyNumber = task.CompanyNumber,
                InvoiceNumber = GetValue(extractedData, "invoiceNumber", ""),
                Amount = GetValue(extractedData, "amount", ""),
                DueDate = GetValue(extractedData, "dueDate", ""),
                Items = GetValue(extractedData, "items", ""),
                Notes = task.EmailBody,
                Attachments = task.TaskAttachments?.Select(a => new
                {
                    a.FileName,
                    a.BlobUrl,
                    a.FileType
                }).ToList(),
                ExtractedData = task.ExtractedData
            };

            _logger.LogInformation($"Generate invoice action data prepared for task {task.TaskId}");

            return new
            {
                success = true,
                action = "generate_invoice",
                taskId = task.TaskId,
                data = invoiceData,
                message = "Invoice data extracted successfully. Ready for invoice generation."
            };
        }

        private async Task<object> ExecuteAddSiteVisitAction(EmailTask task, object data)
        {
            // Extract site visit data from the email
            var extractedData = string.IsNullOrEmpty(task.ExtractedData)
                ? new Dictionary<string, object>()
                : JsonConvert.DeserializeObject<Dictionary<string, object>>(task.ExtractedData);

            var siteVisitData = new
            {
                CustomerName = task.FromName,
                CustomerEmail = task.FromEmail,
                CustomerId = task.CustomerId,
                CompanyNumber = task.CompanyNumber,
                RequestedDate = GetValue(extractedData, "requestedDate", ""),
                Location = GetValue(extractedData, "location", ""),
                Address = GetValue(extractedData, "address", ""),
                ContactPhone = GetValue(extractedData, "phone", ""),
                Purpose = GetValue(extractedData, "purpose", task.Subject),
                Notes = task.EmailBody,
                Priority = task.Priority,
                ExtractedData = task.ExtractedData
            };

            _logger.LogInformation($"Add site visit action data prepared for task {task.TaskId}");

            return new
            {
                success = true,
                action = "add_site_visit",
                taskId = task.TaskId,
                data = siteVisitData,
                message = "Site visit data extracted successfully. Ready for scheduling."
            };
        }

        private async Task<object> ExecuteMoveToJunkAction(EmailTask task, object data)
        {
            task.Status = "Junk";
            task.DateUpdated = DateTime.UtcNow;
            task.UpdatedBy = "System";

            await _context.SaveChangesAsync();

            await AddHistoryEntry(task.TaskId, "StatusChanged", "Pending", "Junk",
                "Task moved to junk", "System");

            _logger.LogInformation($"Task {task.TaskId} moved to junk");

            return new
            {
                success = true,
                action = "move_to_junk",
                taskId = task.TaskId,
                message = "Task moved to junk successfully."
            };
        }

        #endregion

        #region Customer Linking

        /// <summary>
        /// GET /api/EmailTask/{taskId}/extracted-customer-data
        /// Returns customer data extracted by AI from the email, for pre-filling the customer creation form.
        /// </summary>
        public async Task<ExtractedCustomerDataDto> GetExtractedCustomerDataAsync(int taskId)
        {
            var task = await _context.EmailTasks
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null)
                return null;

            var result = new ExtractedCustomerDataDto
            {
                TaskId = taskId,
                Email = task.FromEmail,
                FromName = task.FromName,
                CompanyNumber = task.CompanyNumber,
                Subject = task.Subject,
                CustomerName = task.CustomerName
            };

            // Split FromName into first / last for the customer form
            if (!string.IsNullOrWhiteSpace(task.FromName))
            {
                var parts = task.FromName.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                result.ContactFirstName = parts[0];
                result.ContactLastName = parts.Length > 1 ? parts[1] : string.Empty;
            }

            // Overlay any richer fields that the AI already extracted
            if (!string.IsNullOrWhiteSpace(task.ExtractedData))
            {
                try
                {
                    var extracted = JsonConvert.DeserializeObject<Dictionary<string, object>>(task.ExtractedData);
                    if (extracted != null)
                    {
                        result.Phone = GetValue(extracted, "phone", null);
                        result.Mobile = GetValue(extracted, "mobile", null);
                        result.Address = GetValue(extracted, "address", null);

                        var firstName = GetValue(extracted, "firstName", null);
                        var lastName = GetValue(extracted, "lastName", null);
                        if (!string.IsNullOrWhiteSpace(firstName)) result.ContactFirstName = firstName;
                        if (!string.IsNullOrWhiteSpace(lastName)) result.ContactLastName = lastName;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not parse ExtractedData for task {TaskId}", taskId);
                }
            }

            return result;
        }

        /// <summary>
        /// POST /api/EmailTask/check-customer-exists
        /// Checks whether a customer already exists by email or company number.
        /// </summary>
        public async Task<CustomerExistsResponseDto> CheckCustomerExistsAsync(string? email, string? companyNumber)
        {
            Customer customer = null;

            if (!string.IsNullOrWhiteSpace(email))
                customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());

            if (customer == null && !string.IsNullOrWhiteSpace(companyNumber))
                customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CompanyNumber == companyNumber);

            return new CustomerExistsResponseDto
            {
                Exists = customer != null,
                CustomerId = customer?.CustomerId,
                CustomerName = customer?.Name,
                Email = customer?.Email,
                CompanyNumber = customer?.CompanyNumber
            };
        }

        /// <summary>
        /// POST /api/EmailTask/{taskId}/link-customer
        /// Links an existing customer to a task and records the change in history.
        /// </summary>
        public async Task<EmailTaskDto> LinkCustomerToTaskAsync(int taskId, int customerId, string currentUser)
        {
            var task = await _context.EmailTasks
                .Include(t => t.TaskAttachments)
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null)
                return null;

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == customerId)
                ?? throw new KeyNotFoundException($"Customer {customerId} not found");

            var previousCustomerId = task.CustomerId;
            var previousCustomerName = task.CustomerName;

            task.CustomerId = customerId;
            task.CustomerName = customer.Name;
            task.CompanyNumber = customer.CompanyNumber;
            task.DateUpdated = DateTime.UtcNow;
            task.UpdatedBy = currentUser;

            await _context.SaveChangesAsync();

            var note = previousCustomerId.HasValue
                ? $"Customer changed from '{previousCustomerName}' to '{customer.Name}'"
                : $"Customer linked: '{customer.Name}'";

            await AddHistoryEntry(
                taskId: taskId,
                action: "CustomerLinked",
                oldValue: previousCustomerId?.ToString(),
                newValue: customerId.ToString(),
                details: note,
                createdBy: currentUser);

            _logger.LogInformation("Customer {CustomerId} linked to task {TaskId} by {User}",
                customerId, taskId, currentUser);

            return await MapToDto(task);
        }

        #endregion


    }
}

// ==================== NEW DTOs (add to your Dto/Tasks folder) ====================
// ExtractedCustomerDataDto.cs
public class ExtractedCustomerDataDto
{
    public int TaskId { get; set; }
    public string? Email { get; set; }
    public string? FromName { get; set; }
    public string? CompanyNumber { get; set; }
    public string? Subject { get; set; }
    public string? CustomerName { get; set; }
    public string? ContactFirstName { get; set; }
    public string? ContactLastName { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Address { get; set; }
}

// CustomerExistsResponseDto.cs
public class CustomerExistsResponseDto
{
    public bool Exists { get; set; }
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? Email { get; set; }
    public string? CompanyNumber { get; set; }
}

// LinkCustomerRequestDto.cs
public class LinkCustomerRequestDto
{
    public int CustomerId { get; set; }
}