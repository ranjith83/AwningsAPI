using AwningsAPI.Database;
using AwningsAPI.Dto.Tasks;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Customers;
using AwningsAPI.Model.Email;
using AwningsAPI.Model.Tasks;
using AwningsAPI.Model.Workflow;
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

        public async Task<AppTaskDto> GetTaskByIdAsync(int taskId)
        {
            var task = await _context.Tasks
                .Include(t => t.TaskComments)
                .Include(t => t.TaskAttachments)
                .Include(t => t.TaskHistories)
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null)
                return null;

            return await MapToDto(task);
        }

        public async Task<IEnumerable<AppTaskDto>> GetAllTasksAsync()
        {
            var tasks = await _context.Tasks
                .Include(t => t.TaskComments)
                .Include(t => t.TaskAttachments)
                .OrderByDescending(t => t.DateAdded)
                .ToListAsync();

            var taskDtos = new List<AppTaskDto>();
            foreach (var task in tasks)
            {
                taskDtos.Add(await MapToDto(task));
            }

            return taskDtos;
        }


        public async Task<AppTaskDto> CreateTaskAsync(CreateTaskDto createDto, string currentUser)
        {
            // Derive the display title
            var title = !string.IsNullOrWhiteSpace(createDto.Title)
                ? createDto.Title
                : createDto.Subject ?? "(No title)";

            var task = new AppTask
            {
                // Discriminator — ALWAYS set explicitly
                SourceType = createDto.SourceType,

                Title = title,
                IncomingEmailId = createDto.IncomingEmailId,   // null for non-email
                FromName = createDto.FromName,
                FromEmail = createDto.FromEmail,
                Subject = createDto.Subject,
                Category = createDto.Category,
                EmailBody = createDto.EmailBody,
                //CompanyNumber = createDto.CompanyNumber,
                Priority = createDto.Priority ?? "Normal",
                Status = TaskStatusValue.New,         // all tasks start as New
                AssignedToUserId = null,                       // no assignee on creation
                DueDate = createDto.DueDate,
                CustomerId = createDto.CustomerId,
                WorkflowId = createDto.WorkflowId,
                SiteVisitId = createDto.SiteVisitId,       // set by SiteVisitController
                DateAdded = DateTime.UtcNow,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            // History entry — subject/category context used in audit grid
            await AddHistoryEntry(
                taskId: task.TaskId,
                action: "Created",
                oldValue: null,
                newValue: null,
                details: createDto.SourceType == TaskSourceType.Email
                                 ? "Task created from email"
                                 : $"Task created manually (source: {createDto.SourceType})",
                createdBy: currentUser,
                customerName: task.CustomerName,
                subject: task.Subject ?? task.Title,
                category: task.Category);

            return await GetTaskByIdAsync(task.TaskId);
        }

        public async Task<AppTaskDto> UpdateTaskAsync(
             int taskId, UpdateTaskDto updateDto, string currentUser)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return null;

            var changes = new List<string>();

            if (!string.IsNullOrEmpty(updateDto.Status) && task.Status != updateDto.Status)
            {
                await AddHistoryEntry(taskId, "StatusChanged",
                    task.Status, updateDto.Status, null, currentUser);
                task.Status = updateDto.Status;

                if (updateDto.Status == TaskStatusValue.InProgress)
                {
                    task.DateProcessed = DateTime.UtcNow;
                    task.ProcessedBy = currentUser;
                }
                else if (updateDto.Status == TaskStatusValue.Completed)
                {
                    task.CompletedDate = DateTime.UtcNow;
                    task.CompletedBy = currentUser;
                }
            }

            if (!string.IsNullOrEmpty(updateDto.Title) && task.Title != updateDto.Title)
            {
                changes.Add($"Title changed to '{updateDto.Title}'");
                task.Title = updateDto.Title;
            }

            if (!string.IsNullOrEmpty(updateDto.Priority) && task.Priority != updateDto.Priority)
            {
                changes.Add($"Priority: {task.Priority} → {updateDto.Priority}");
                task.Priority = updateDto.Priority;
            }

            if (updateDto.DueDate.HasValue && task.DueDate != updateDto.DueDate)
            {
                changes.Add("Due date changed");
                task.DueDate = updateDto.DueDate;
            }

            if (updateDto.AssignedToUserId.HasValue &&
                task.AssignedToUserId != updateDto.AssignedToUserId)
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
                changes.Add($"Action: {updateDto.SelectedAction}");
            }

            if (updateDto.CustomerId.HasValue) task.CustomerId = updateDto.CustomerId;
            if (updateDto.WorkflowId.HasValue) task.WorkflowId = updateDto.WorkflowId;
            if (!string.IsNullOrEmpty(updateDto.CompanyNumber))
                task.CompanyNumber = updateDto.CompanyNumber;

            task.DateUpdated = DateTime.UtcNow;
            task.UpdatedBy = currentUser;

            await _context.SaveChangesAsync();

            if (changes.Any())
                await AddHistoryEntry(taskId, "Updated", null, null,
                    string.Join("; ", changes), currentUser);

            return await GetTaskByIdAsync(taskId);
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Status Management

        public async Task<AppTaskDto> UpdateTaskStatusAsync(int taskId, UpdateTaskStatusDto statusDto, string currentUser)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                return null;

            var oldStatus = task.Status;
            task.Status = statusDto.Status;
            task.DateUpdated = DateTime.UtcNow;
            task.UpdatedBy = currentUser;

            if (statusDto.Status == "In Progress")
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

            if (statusDto.Status == "Completed")
            {
                // For Completed transitions write only ONE rich "Completed" audit entry
                // (includes customer/subject/category context for the Audit tab).
                // We do NOT also write a generic "StatusChanged" entry to avoid duplicates.
                await AddHistoryEntry(
                    taskId: taskId,
                    action: "Completed",
                    oldValue: oldStatus,
                    newValue: "Completed",
                    details: string.IsNullOrWhiteSpace(statusDto.CompletionNotes)
                        ? $"Task completed by {currentUser}"
                        : statusDto.CompletionNotes,
                    createdBy: currentUser,
                    customerName: task.CustomerName,
                    subject: task.Subject,
                    category: task.Category);
            }
            else
            {
                // For all other status changes write a generic StatusChanged entry
                await AddHistoryEntry(
                    taskId: taskId,
                    action: "StatusChanged",
                    oldValue: oldStatus,
                    newValue: statusDto.Status,
                    details: statusDto.CompletionNotes,
                    createdBy: currentUser);
            }

            return await GetTaskByIdAsync(taskId);
        }

        public async Task<AppTaskDto> CompleteTaskAsync(int taskId, string completionNotes, string currentUser)
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

        public async Task<AppTaskDto> AssignTaskAsync(int taskId, AssignTaskDto assignDto, string currentUser)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(taskId);
                if (task == null)
                    return null;

                var user = await _context.Users.FindAsync(assignDto.AssignedToUserId);
                if (user == null)
                    return null;

                var oldAssignee = task.AssignedToUserName ?? "Unassigned";
                var isReassignment = task.AssignedToUserId.HasValue && task.AssignedToUserId != assignDto.AssignedToUserId;

                // Track previous assignee when re-assigning
                if (isReassignment)
                {
                    task.PreviousAssignedToUserId = task.AssignedToUserId;
                    task.PreviousAssignedToUserName = task.AssignedToUserName;
                }

                // Assign the user
                task.AssignedToUserId = assignDto.AssignedToUserId;
                task.AssignedToUserName = user.Username;
                task.DateUpdated = DateTime.UtcNow;
                task.UpdatedBy = currentUser;

                // First assignment → In Progress; Re-assignment → More Info (In Progress tab)
                var oldStatus = task.Status;
                if (isReassignment)
                {
                    task.Status = "More Info";
                }
                else
                {
                    task.Status = "In Progress";
                }
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

                if (oldStatus != "In Progress" && oldStatus != "More Info")
                    await AddHistoryEntry(
                        taskId: taskId,
                        action: "StatusChanged",
                        oldValue: oldStatus,
                        newValue: task.Status,
                        details: isReassignment
                            ? $"Status set to More Info when re-assigned from {oldAssignee} to {user.Username}"
                            : $"Status set to In Progress when assigned to {user.Username}",
                        createdBy: currentUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return await GetTaskByIdAsync(taskId);
        }

        public async Task<AppTaskDto> UnassignTaskAsync(int taskId, string currentUser)
        {
            var task = await _context.Tasks.FindAsync(taskId);
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

        public async Task<(IEnumerable<AppTaskDto> Tasks, int TotalCount)> GetTasksWithFiltersAsync(TaskFilterDto filterDto)
        {
            var query = _context.Tasks
                .Include(t => t.TaskComments)
                .Include(t => t.TaskAttachments)
                .AsQueryable();

            // Role-based visibility: non-admin users only see their own tasks
            if (!filterDto.IsAdmin && filterDto.CurrentUserId.HasValue)
                query = query.Where(t => t.AssignedToUserId == filterDto.CurrentUserId.Value);

            // Apply filters
            if (filterDto.Statuses != null && filterDto.Statuses.Count > 0)
                query = query.Where(t => filterDto.Statuses.Contains(t.Status));
            else if (!string.IsNullOrEmpty(filterDto.Status))
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
                "sourcetype" => filterDto.SortDirection == "ASC" ? query.OrderBy(t => t.SourceType) : query.OrderByDescending(t => t.SourceType),
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

            var taskDtos = new List<AppTaskDto>();
            foreach (var task in tasks)
            {
                taskDtos.Add(await MapToDto(task));
            }

            return (taskDtos, totalCount);
        }

        public async Task<IEnumerable<AppTaskDto>> GetTasksByUserAsync(int userId)
        {
            var tasks = await _context.Tasks
                .Include(t => t.TaskComments)
                .Include(t => t.TaskAttachments)
                .Where(t => t.AssignedToUserId == userId)
                .OrderByDescending(t => t.DateAdded)
                .ToListAsync();

            var taskDtos = new List<AppTaskDto>();
            foreach (var task in tasks)
            {
                taskDtos.Add(await MapToDto(task));
            }

            return taskDtos;
        }

        public async Task<IEnumerable<AppTaskDto>> GetTasksByCustomerAsync(int customerId)
        {
            var tasks = await _context.Tasks
                .Include(t => t.TaskComments)
                .Include(t => t.TaskAttachments)
                .Where(t => t.CustomerId == customerId)
                .OrderByDescending(t => t.DateAdded)
                .ToListAsync();

            var taskDtos = new List<AppTaskDto>();
            foreach (var task in tasks)
            {
                taskDtos.Add(await MapToDto(task));
            }

            return taskDtos;
        }

        public async Task<IEnumerable<AppTaskDto>> GetTasksByTypeAsync(string taskType)
        {
            var tasks = await _context.Tasks
                .Include(t => t.TaskComments)
                .Include(t => t.TaskAttachments)
                .Where(t => t.TaskType == taskType || t.Category == taskType)
                .OrderByDescending(t => t.DateAdded)
                .ToListAsync();

            var taskDtos = new List<AppTaskDto>();
            foreach (var task in tasks)
            {
                taskDtos.Add(await MapToDto(task));
            }

            return taskDtos;
        }

        public async Task<IEnumerable<AppTaskDto>> GetOverdueTasksAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tasks = await _context.Tasks
                .Include(t => t.TaskComments)
                .Include(t => t.TaskAttachments)
                .Where(t => t.DueDate.HasValue && t.DueDate.Value.Date < today && t.Status != "Completed" && t.Status != "Processed")
                .OrderBy(t => t.DueDate)
                .ToListAsync();

            var taskDtos = new List<AppTaskDto>();
            foreach (var task in tasks)
            {
                taskDtos.Add(await MapToDto(task));
            }

            return taskDtos;
        }

        public async Task<IEnumerable<AppTaskDto>> GetTasksDueTodayAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tasks = await _context.Tasks
                .Include(t => t.TaskComments)
                .Include(t => t.TaskAttachments)
                .Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == today && t.Status != "Completed" && t.Status != "Processed")
                .OrderBy(t => t.Priority)
                .ToListAsync();

            var taskDtos = new List<AppTaskDto>();
            foreach (var task in tasks)
            {
                taskDtos.Add(await MapToDto(task));
            }

            return taskDtos;
        }


        // ────────────────────────────────────────────────────────────────────
        // GetFilteredTasksAsync — applies SourceType filter for tab routing
        // (replaces / supplements whatever paging method you already have)
        // ────────────────────────────────────────────────────────────────────
        public async Task<(IEnumerable<AppTaskDto> Items, int TotalCount)>  GetFilteredTasksAsync(TaskFilterDto filter)
        {
            var query = _context.Tasks.AsQueryable();

            // ── Source type filter (tabs: All | Email | Site Visits | Manual) ─
            var sourceTypes = filter.SourceTypes ?? (
                filter.SourceType != null ? new List<string> { filter.SourceType } : null);

            if (sourceTypes != null && sourceTypes.Count > 0)
                query = query.Where(t => sourceTypes.Contains(t.SourceType));

            // ── Status filter ─────────────────────────────────────────────────
            var statuses = filter.Statuses ?? (
                filter.Status != null ? new List<string> { filter.Status } : null);

            if (statuses != null && statuses.Count > 0)
                query = query.Where(t => statuses.Contains(t.Status));

            // ── Other filters ─────────────────────────────────────────────────
            if (!string.IsNullOrEmpty(filter.TaskType))
                query = query.Where(t => t.TaskType == filter.TaskType);

            if (!string.IsNullOrEmpty(filter.Priority))
                query = query.Where(t => t.Priority == filter.Priority);

            if (filter.AssignedToUserId.HasValue)
                query = query.Where(t => t.AssignedToUserId == filter.AssignedToUserId);

            if (filter.CustomerId.HasValue)
                query = query.Where(t => t.CustomerId == filter.CustomerId);

            if (filter.DueDateFrom.HasValue)
                query = query.Where(t => t.DueDate >= filter.DueDateFrom);

            if (filter.DueDateTo.HasValue)
                query = query.Where(t => t.DueDate <= filter.DueDateTo);

            if (filter.CreatedDateFrom.HasValue)
                query = query.Where(t => t.DateCreated >= filter.CreatedDateFrom);

            if (filter.CreatedDateTo.HasValue)
                query = query.Where(t => t.DateCreated <= filter.CreatedDateTo);

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var term = filter.SearchTerm.ToLower();
                query = query.Where(t =>
                    (t.Title != null && t.Title.ToLower().Contains(term)) ||
                    (t.Subject != null && t.Subject.ToLower().Contains(term)) ||
                    (t.FromName != null && t.FromName.ToLower().Contains(term)) ||
                    (t.CustomerName != null && t.CustomerName.ToLower().Contains(term)));
            }

            // ── Visibility: non-admins see only their own tasks ───────────────
            if (!filter.IsAdmin && filter.CurrentUserId.HasValue)
                query = query.Where(t => t.AssignedToUserId == filter.CurrentUserId);

            // ── Sort ──────────────────────────────────────────────────────────
            query = (filter.SortBy, filter.SortDirection.ToUpper()) switch
            {
                ("DateAdded", "ASC") => query.OrderBy(t => t.DateAdded),
                ("DateAdded", _) => query.OrderByDescending(t => t.DateAdded),
                ("DueDate", "ASC") => query.OrderBy(t => t.DueDate),
                ("DueDate", _) => query.OrderByDescending(t => t.DueDate),
                ("Priority", "ASC") => query.OrderBy(t => t.Priority),
                ("Priority", _) => query.OrderByDescending(t => t.Priority),
                _ => query.OrderByDescending(t => t.DateAdded)
            };

            var totalCount = await query.CountAsync();

            var tasks = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Include(t => t.TaskComments)
                .Include(t => t.TaskAttachments)
                .ToListAsync();

            var dtos = new List<AppTaskDto>();
            foreach (var t in tasks)
                dtos.Add(await MapToDto(t));

            return (dtos, totalCount);
        }


        public async Task StoreSiteVisitLinkAsync(
          int taskId, int siteVisitId, string currentUser)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return;

            task.SiteVisitId = siteVisitId;
            task.DateUpdated = DateTime.UtcNow;
            task.UpdatedBy = currentUser;

            await _context.SaveChangesAsync();

            await AddHistoryEntry(
                taskId: taskId,
                action: "SiteVisitLinked",
                oldValue: null,
                newValue: siteVisitId.ToString(),
                details: $"Site visit {siteVisitId} linked to task",
                createdBy: currentUser,
                customerName: task.CustomerName,
                subject: task.Subject ?? task.Title,
                category: task.Category);

            _logger.LogInformation(
                "SiteVisit {SiteVisitId} linked to task {TaskId} by {User}",
                siteVisitId, taskId, currentUser);
        }

        #endregion

        #region Statistics

        public async Task<TaskStatistics> GetTaskStatisticsAsync()
        {
            var today = DateTime.UtcNow.Date;

            var stats = new TaskStatistics
            {
                TotalTasks = await _context.Tasks.CountAsync(),
                PendingTasks = await _context.Tasks.CountAsync(t => t.Status == "Pending"),
                InProgressTasks = await _context.Tasks.CountAsync(t => t.Status == "In Progress"),
                CompletedTasks = await _context.Tasks.CountAsync(t => t.Status == "Completed" || t.Status == "Processed"),
                OverdueTasks = await _context.Tasks.CountAsync(t => t.DueDate.HasValue && t.DueDate.Value.Date < today && t.Status != "Completed" && t.Status != "Processed"),
                HighPriorityTasks = await _context.Tasks.CountAsync(t => t.Priority == "High" && t.Status != "Completed"),
                UrgentTasks = await _context.Tasks.CountAsync(t => t.Priority == "Urgent" && t.Status != "Completed")
            };

            // Tasks by type
            stats.TasksByType = await _context.Tasks
                .GroupBy(t => t.Category)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Type ?? "Unknown", x => x.Count);

            // Tasks by assignee
            stats.TasksByAssignee = await _context.Tasks
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
                TotalTasks = await _context.Tasks.CountAsync(t => t.AssignedToUserId == userId),
                PendingTasks = await _context.Tasks.CountAsync(t => t.AssignedToUserId == userId && t.Status == "Pending"),
                InProgressTasks = await _context.Tasks.CountAsync(t => t.AssignedToUserId == userId && t.Status == "In Progress"),
                CompletedTasks = await _context.Tasks.CountAsync(t => t.AssignedToUserId == userId && (t.Status == "Completed" || t.Status == "Processed")),
                OverdueTasks = await _context.Tasks.CountAsync(t => t.AssignedToUserId == userId && t.DueDate.HasValue && t.DueDate.Value.Date < today && t.Status != "Completed" && t.Status != "Processed"),
                HighPriorityTasks = await _context.Tasks.CountAsync(t => t.AssignedToUserId == userId && t.Priority == "High" && t.Status != "Completed"),
                UrgentTasks = await _context.Tasks.CountAsync(t => t.AssignedToUserId == userId && t.Priority == "Urgent" && t.Status != "Completed")
            };

            // Tasks by type for this user
            stats.TasksByType = await _context.Tasks
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

        public async Task<AppTaskDto> CreateTaskFromEmailAsync(int incomingEmailId, string currentUser)
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
                //CustomerId = email.cus
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
            var dbTask = await _context.Tasks.FindAsync(task.TaskId);
            if (dbTask != null)
            {
                dbTask.TaskType = taskType;
                dbTask.ExtractedData = email.ExtractedData;
                dbTask.AIConfidence = email.CategoryConfidence;
                dbTask.HasAttachments = email.HasAttachments;

                await _context.SaveChangesAsync();
            }

            _logger.LogInformation($"Task created successfully: TaskId={task.TaskId}, Category={category}");

            // ── Auto-link customer and workflow when sender email matches a known customer ──
            // If the sender's email matches an existing Customer.Email or any CustomerContact.Email,
            // automatically set CustomerId/CustomerName on the task so it appears linked on creation.
            // If that customer has exactly ONE workflow, also set WorkflowId so action buttons are
            // immediately enabled without the user needing to manually link anything.
            await TryAutoLinkCustomerAndWorkflowAsync(task.TaskId, email.FromEmail, currentUser);

            // ── Auto-create InitialEnquiry record for initial_enquiry emails ──────────
            // This links the incoming email directly to the InitialEnquiry table so
            // the Initial Enquiry screen can show it, and so follow-up generation
            // can use it as the timer anchor.
            //
            // We only create the record if the task already has a WorkflowId
            // (i.e. the customer was previously linked to a workflow). For new
            // customers the record is created later when the user creates the workflow.
            if (string.Equals(email.Category, "initial_enquiry", StringComparison.OrdinalIgnoreCase))
            {
                await CreateInitialEnquiryFromEmailAsync(task.TaskId, email, currentUser);
            }

            return await GetTaskByIdAsync(task.TaskId);
        }

        /// <summary>
        /// Called immediately after a task is created from an inbound email.
        ///
        /// Looks up the sender email against Customer.Email and CustomerContact.Email.
        /// If a customer match is found:
        ///   1. Sets CustomerId + CustomerName + CompanyNumber on the task.
        ///   2. If that customer has exactly ONE workflow, also sets WorkflowId.
        ///   3. If both a customer AND a single workflow were matched the task is
        ///      automatically marked as Completed — no manual action required.
        ///      A single "Completed" audit entry is written (customer/subject/category
        ///      all populated) so it appears correctly in the Audit tab.
        ///
        /// Non-fatal: errors are logged and the task is always saved even if the
        /// lookup or auto-complete step fails.
        /// </summary>
        private async Task TryAutoLinkCustomerAndWorkflowAsync(
            int taskId, string? fromEmail, string currentUser)
        {
            if (string.IsNullOrWhiteSpace(fromEmail))
                return;

            try
            {
                // ── 1. Match customer by sender email ────────────────────────────────
                var customer = await _context.Customers
                    .Include(c => c.CustomerContacts)
                    .FirstOrDefaultAsync(c =>
                        (c.Email != null && c.Email.ToLower() == fromEmail.ToLower()) ||
                        c.CustomerContacts.Any(cc => cc.Email != null && cc.Email.ToLower() == fromEmail.ToLower()));

                if (customer == null)
                {
                    _logger.LogInformation(
                        "TryAutoLinkCustomer: no customer found for email {FromEmail}", fromEmail);
                    return;
                }

                _logger.LogInformation(
                    "TryAutoLinkCustomer: matched customer {CustomerId} ({CustomerName}) for {FromEmail}",
                    customer.CustomerId, customer.Name, fromEmail);

                // ── 2. Apply customer (+ optional single workflow) to the task ───────
                var task = await _context.Tasks.FindAsync(taskId);
                if (task == null) return;

                task.CustomerId = customer.CustomerId;
                task.CustomerName = customer.Name;
                task.CompanyNumber ??= customer.CompanyNumber;
                task.DateUpdated = DateTime.UtcNow;
                task.UpdatedBy = currentUser;

                var workflows = await _context.WorkflowStarts
                    .Where(w => w.CustomerId == customer.CustomerId)
                    .ToListAsync();

                var singleWorkflowMatched = workflows.Count == 1;

                if (singleWorkflowMatched)
                {
                    task.WorkflowId = workflows[0].WorkflowId;
                    _logger.LogInformation(
                        "TryAutoLinkCustomer: auto-assigned single workflow {WorkflowId} to task {TaskId}",
                        workflows[0].WorkflowId, taskId);
                }

                // ── 3. Auto-complete when customer + single workflow both found ───────
                // Both conditions met → the task can be fully resolved automatically.
                // We set status fields here (before SaveChanges) so a single DB round-trip
                // covers both the link and the completion.
                if (singleWorkflowMatched)
                {
                    task.Status = "Completed";
                    task.CompletedDate = DateTime.UtcNow;
                    task.CompletedBy = currentUser;
                    task.CompletionNotes =
                        $"Auto-completed: customer '{customer.Name}' and workflow {task.WorkflowId} " +
                        $"matched from sender email '{fromEmail}'.";

                    _logger.LogInformation(
                        "TryAutoLinkCustomer: auto-completing task {TaskId} — customer {CustomerId} + workflow {WorkflowId} matched",
                        taskId, customer.CustomerId, task.WorkflowId);
                }

                await _context.SaveChangesAsync();

                // ── 4. Audit history ─────────────────────────────────────────────────

                // CustomerLinked
                var customerDetails = singleWorkflowMatched
                    ? $"Auto-linked customer '{customer.Name}' and workflow {task.WorkflowId} from sender email"
                    : $"Auto-linked customer '{customer.Name}' from sender email";

                await AddHistoryEntry(
                    taskId: taskId,
                    action: "CustomerLinked",
                    oldValue: null,
                    newValue: customer.CustomerId.ToString(),
                    details: customerDetails,
                    createdBy: currentUser,
                    customerName: customer.Name,
                    subject: task.Subject,
                    category: task.Category);

                // WorkflowLinked
                if (singleWorkflowMatched)
                {
                    await AddHistoryEntry(
                        taskId: taskId,
                        action: "WorkflowLinked",
                        oldValue: null,
                        newValue: task.WorkflowId.ToString(),
                        details: $"Workflow {task.WorkflowId} auto-linked (only workflow for customer)",
                        createdBy: currentUser,
                        customerName: customer.Name,
                        subject: task.Subject,
                        category: task.Category);

                    // Completed — single rich audit entry (no duplicate StatusChanged)
                    await AddHistoryEntry(
                        taskId: taskId,
                        action: "Completed",
                        oldValue: "New",
                        newValue: "Completed",
                        details: task.CompletionNotes,
                        createdBy: currentUser,
                        customerName: customer.Name,
                        subject: task.Subject,
                        category: task.Category);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "TryAutoLinkCustomerAndWorkflow failed for task {TaskId}, email {FromEmail}",
                    taskId, fromEmail);
                // Non-fatal: task is already saved
            }
        }

        /// <summary>
        /// Called when an inbound email is categorised as initial_enquiry.
        /// Creates an InitialEnquiry record so the workflow screen can immediately
        /// display it and so the follow-up timer starts from this email's date.
        ///
        /// If the task does not yet have a WorkflowId the record is created later
        /// (when the user creates a workflow) via AddInitialEnquiryFromTask().
        /// </summary>
        private async Task CreateInitialEnquiryFromEmailAsync(
            int taskId,
            IncomingEmail email,
            string currentUser)
        {
            try
            {
                // Find the task's WorkflowId (may be null if no workflow yet)
                var dbTask = await _context.Tasks
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.TaskId == taskId);

                if (dbTask?.WorkflowId == null)
                {
                    // No workflow yet — record will be created when workflow is linked
                    _logger.LogInformation(
                        "CreateInitialEnquiryFromEmail: task {TaskId} has no workflow yet — skipping auto-create",
                        taskId);
                    return;
                }

                // Guard: don't create duplicates if the processor runs twice
                var existing = await _context.InitialEnquiries
                    .AnyAsync(e => e.TaskId == taskId);
                if (existing)
                {
                    _logger.LogInformation(
                        "CreateInitialEnquiryFromEmail: InitialEnquiry already exists for task {TaskId}",
                        taskId);
                    return;
                }

                // Build a comments summary from the email subject + body preview
                var bodyPreview = email.BodyPreview ?? string.Empty;
                var commentsText = $"Email subject: {email.Subject}";
                if (!string.IsNullOrWhiteSpace(bodyPreview))
                    commentsText += $"{bodyPreview[..Math.Min(bodyPreview.Length, 500)]}";

                var enquiry = new InitialEnquiry
                {
                    WorkflowId = dbTask.WorkflowId.Value,
                    Comments = commentsText,
                    Email = email.FromEmail ?? string.Empty,
                    TaskId = taskId,
                    IncomingEmailId = email.Id,
                    DateCreated = email?.ReceivedDateTime ?? DateTime.UtcNow,
                    CreatedBy = "System (email processor)"
                };

                _context.InitialEnquiries.Add(enquiry);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "CreateInitialEnquiryFromEmail: created EnquiryId={EnquiryId} for task {TaskId} on workflow {WorkflowId}",
                    enquiry.EnquiryId, taskId, dbTask.WorkflowId);
            }
            catch (Exception ex)
            {
                // Non-fatal — log and continue. The task was created successfully.
                _logger.LogError(ex,
                    "CreateInitialEnquiryFromEmail: failed for task {TaskId}", taskId);
            }
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
            var auditActions = new[] { "Created", "Assigned", "Unassigned", "Completed" };

            //var query = _context.TaskHistories
            //    .Where(h => auditActions.Contains(h.Action));

            var query = _context.TaskHistories
              .Where(h =>
                  h.Action == "Created" ||
                  h.Action == "Assigned" ||
                  h.Action == "Unassigned" ||
                  h.Action == "Completed");

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
                    Category = h.Category,
                    // AssignedTo: for Assigned actions → NewValue is the assignee name;
                    //             for Unassigned actions → OldValue is the former assignee.
                    AssignedTo = h.Action == "Assigned" ? h.NewValue :
                                 h.Action == "Unassigned" ? h.OldValue : null,
                    // AssignedBy: the user who performed the assignment (always CreatedBy).
                    AssignedBy = (h.Action == "Assigned" || h.Action == "Unassigned" || h.Action == "Completed")
                                 ? h.CreatedBy : null,
                    // CompletedBy: the user who completed the task (always CreatedBy for Completed entries).
                    //CompletedBy = h.Action == "Completed" ? h.CreatedBy : null
                }).ToList(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        private async Task<AppTaskDto> MapToDto(AppTask task)
        {
            // Resolve the display title (Title takes priority; fall back to Subject)
            var displayTitle = !string.IsNullOrWhiteSpace(task.Title)
                ? task.Title
                : task.Subject ?? "(No title)";

            var dto = new AppTaskDto
            {
                TaskId = task.TaskId,
                SourceType = task.SourceType,
                DisplayTitle = displayTitle,
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
                PreviousAssignedToUserId = task.PreviousAssignedToUserId,
                PreviousAssignedToUserName = task.PreviousAssignedToUserName,
                CompanyNumber = task.CompanyNumber,
                EmailBody = task.EmailBody,
                HasAttachments = task.HasAttachments,
                SelectedAction = task.SelectedAction,
                CustomerId = task.CustomerId,
                CustomerName = task.CustomerName,
                CustomerEmail = task.CustomerEmail,
                WorkflowId = task.WorkflowId,
                SiteVisitId = task.SiteVisitId,      // ← NEW
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
                }).ToList() ?? new(),

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
                }).ToList() ?? new(),
            };

            // History is fetched separately to keep the query lean
            var histories = await _context.TaskHistories
                .Where(h => h.TaskId == task.TaskId)
                .OrderByDescending(h => h.DateCreated)
                .ToListAsync();

            dto.History = histories.Select(h => new TaskHistoryDto
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
                Category = h.Category,
                AssignedTo = h.Action is "Assigned" or "Unassigned" ? h.NewValue : null,
                AssignedBy = h.Action is "Assigned" or "Unassigned" ? h.CreatedBy : null
            }).ToList();

            return dto;
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
            var task = await _context.Tasks
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

        private async Task<object> ExecuteAddCompanyAction(AppTask task, object data)
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
        private async Task<object> ExecuteGenerateQuoteAction(AppTask task, object data)
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

        private async Task<object> ExecuteGenerateInvoiceAction(AppTask task, object data)
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

        private async Task<object> ExecuteAddSiteVisitAction(AppTask task, object data)
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

        private async Task<object> ExecuteMoveToJunkAction(AppTask task, object data)
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
            var task = await _context.Tasks
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
        public async Task<AppTaskDto> LinkCustomerToTaskAsync(int taskId, int customerId, string currentUser)
        {
            var task = await _context.Tasks
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

        /// <summary>
        /// Links a workflow to an email task after the user creates a workflow
        /// from the email-task screen.  Updates EmailTask.WorkflowId and writes
        /// a "WorkflowLinked" entry into task history.
        /// </summary>
        public async Task<AppTaskDto> LinkWorkflowToTaskAsync(
            int taskId, int workflowId, string currentUser)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return null;

            // Guard: don't overwrite if the same workflow is already set
            if (task.WorkflowId == workflowId)
                return await GetTaskByIdAsync(taskId);

            var oldWorkflowId = task.WorkflowId;
            task.WorkflowId = workflowId;
            task.DateUpdated = DateTime.UtcNow;
            task.UpdatedBy = currentUser;

            await _context.SaveChangesAsync();

            await AddHistoryEntry(
                taskId: taskId,
                action: "WorkflowLinked",
                oldValue: oldWorkflowId?.ToString(),
                newValue: workflowId.ToString(),
                details: $"Workflow {workflowId} linked to task from email-task screen",
                createdBy: currentUser,
                customerName: task.CustomerName,
                subject: task.Subject,
                category: task.Category);

            return await GetTaskByIdAsync(taskId);
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