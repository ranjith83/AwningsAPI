using AwningsAPI.Database;
using AwningsAPI.Dto.Tasks;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmailTaskController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITaskService _taskService;
        private readonly ILogger<EmailTaskController> _logger;

        public EmailTaskController(
            AppDbContext context,
            ITaskService taskService,
            ILogger<EmailTaskController> logger)
        {
            _context = context;
            _taskService = taskService;
            _logger = logger;
        }

        /// <summary>
        /// Get all tasks by status (Pending, Processed, Junk)
        /// </summary>
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<EmailTaskDto>>> GetTasksByStatus(string status)
        {
            try
            {
                var tasks = await _context.EmailTasks
                    .Include(t => t.TaskAttachments)
                    .Include(t => t.TaskComments)
                    .Where(t => t.Status == status)
                    .OrderByDescending(t => t.DateAdded)
                    .ToListAsync();

                var taskDtos = tasks.Select(t => MapToDto(t)).ToList();
                return Ok(taskDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving tasks with status: {status}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific task by ID
        /// </summary>
        [HttpGet("{taskId}")]
        public async Task<ActionResult<EmailTaskDto>> GetTaskById(int taskId)
        {
            try
            {
                var task = await _context.EmailTasks
                    .Include(t => t.TaskAttachments)
                    .Include(t => t.TaskComments)
                    .Include(t => t.TaskHistories)
                    .FirstOrDefaultAsync(t => t.TaskId == taskId);

                if (task == null)
                {
                    return NotFound(new { error = "Task not found" });
                }

                var taskDto = MapToDto(task);
                return Ok(taskDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving task {taskId}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get tasks assigned to a specific user
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<EmailTaskDto>>> GetTasksByUser(int userId)
        {
            try
            {
                var tasks = await _context.EmailTasks
                    .Include(t => t.TaskAttachments)
                    .Where(t => t.AssignedToUserId == userId)
                    .OrderByDescending(t => t.DateAdded)
                    .ToListAsync();

                var taskDtos = tasks.Select(t => MapToDto(t)).ToList();
                return Ok(taskDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving tasks for user {userId}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get tasks by category
        /// </summary>
        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<EmailTaskDto>>> GetTasksByCategory(string category)
        {
            try
            {
                var tasks = await _context.EmailTasks
                    .Include(t => t.TaskAttachments)
                    .Where(t => t.Category == category)
                    .OrderByDescending(t => t.DateAdded)
                    .ToListAsync();

                var taskDtos = tasks.Select(t => MapToDto(t)).ToList();
                return Ok(taskDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving tasks for category {category}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new task manually
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EmailTaskDto>> CreateTask([FromBody] CreateTaskDto createDto)
        {
            try
            {
                var currentUser = GetCurrentUserName();

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
                    DueDate = createDto.DueDate,
                    AssignedToUserId = createDto.AssignedToUserId,
                    CustomerId = createDto.CustomerId,
                    WorkflowId = createDto.WorkflowId,
                    Status = "Pending",
                    DateAdded = DateTime.UtcNow,
                    DateCreated = DateTime.UtcNow,
                    CreatedBy = currentUser
                };

                _context.EmailTasks.Add(task);
                await _context.SaveChangesAsync();

                // Add history entry
                await AddHistoryEntry(task.TaskId, "Created", null, null, "Task created manually", currentUser);

                var taskDto = MapToDto(task);
                return CreatedAtAction(nameof(GetTaskById), new { taskId = task.TaskId }, taskDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Update a task
        /// </summary>
        [HttpPut("{taskId}")]
        public async Task<ActionResult<EmailTaskDto>> UpdateTask(int taskId, [FromBody] UpdateTaskDto updateDto)
        {
            try
            {
                var task = await _context.EmailTasks.FindAsync(taskId);
                if (task == null)
                {
                    return NotFound(new { error = "Task not found" });
                }

                var currentUser = GetCurrentUserName();
                var changes = new List<string>();

                // Track changes for history
                if (updateDto.Status != null && task.Status != updateDto.Status)
                {
                    await AddHistoryEntry(taskId, "StatusChanged", task.Status, updateDto.Status,
                        $"Status changed from {task.Status} to {updateDto.Status}", currentUser);
                    task.Status = updateDto.Status;
                    changes.Add("Status");
                }

                if (updateDto.Priority != null && task.Priority != updateDto.Priority)
                {
                    await AddHistoryEntry(taskId, "Updated", task.Priority, updateDto.Priority,
                        $"Priority changed from {task.Priority} to {updateDto.Priority}", currentUser);
                    task.Priority = updateDto.Priority;
                    changes.Add("Priority");
                }

                if (updateDto.DueDate.HasValue && task.DueDate != updateDto.DueDate)
                {
                    task.DueDate = updateDto.DueDate;
                    changes.Add("DueDate");
                }

                if (updateDto.AssignedToUserId.HasValue && task.AssignedToUserId != updateDto.AssignedToUserId)
                {
                    var oldUser = task.AssignedToUserName;
                    task.AssignedToUserId = updateDto.AssignedToUserId;
                    task.AssignedByUserId = GetCurrentUserId();
                    task.AssignedByUserName = currentUser;

                    // Get new assignee name
                    var newUser = await _context.Users.FindAsync(updateDto.AssignedToUserId);
                    task.AssignedToUserName = newUser?.FirstName;

                    await AddHistoryEntry(taskId, "Assigned", oldUser, task.AssignedToUserName,
                        $"Task assigned to {task.AssignedToUserName}", currentUser);
                    changes.Add("Assignment");
                }

                if (updateDto.SelectedAction != null)
                {
                    task.SelectedAction = updateDto.SelectedAction;
                    changes.Add("SelectedAction");
                }

                if (updateDto.CustomerId.HasValue)
                {
                    task.CustomerId = updateDto.CustomerId;
                    changes.Add("CustomerId");
                }

                if (updateDto.WorkflowId.HasValue)
                {
                    task.WorkflowId = updateDto.WorkflowId;
                    changes.Add("WorkflowId");
                }

                if (!string.IsNullOrEmpty(updateDto.CompanyNumber))
                {
                    task.CompanyNumber = updateDto.CompanyNumber;
                    changes.Add("CompanyNumber");
                }

                task.DateUpdated = DateTime.UtcNow;
                task.UpdatedBy = currentUser;

                await _context.SaveChangesAsync();

                if (changes.Any())
                {
                    await AddHistoryEntry(taskId, "Updated", null, null,
                        $"Task updated: {string.Join(", ", changes)}", currentUser);
                }

                var taskDto = MapToDto(task);
                return Ok(taskDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating task {taskId}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Assign a task to a user
        /// </summary>
        [HttpPost("{taskId}/assign")]
        public async Task<IActionResult> AssignTask(int taskId, [FromBody] AssignTaskDto assignDto)
        {
            try
            {
                var task = await _context.EmailTasks.FindAsync(taskId);
                if (task == null)
                {
                    return NotFound(new { error = "Task not found" });
                }

                var currentUser = GetCurrentUserName();
                var currentUserId = GetCurrentUserId();

                // Get assignee details
                var assignee = await _context.Users.FindAsync(assignDto.AssignedToUserId);
                if (assignee == null)
                {
                    return BadRequest(new { error = "User not found" });
                }

                var oldAssignee = task.AssignedToUserName;

                task.AssignedToUserId = assignDto.AssignedToUserId;
                task.AssignedToUserName = assignee.FirstName;
                task.AssignedByUserId = currentUserId;
                task.AssignedByUserName = currentUser;
                task.DateUpdated = DateTime.UtcNow;
                task.UpdatedBy = currentUser;

                await _context.SaveChangesAsync();

                // Add history entry
                var details = string.IsNullOrEmpty(assignDto.Notes)
                    ? $"Task assigned to {assignee.FirstName}"
                    : $"Task assigned to {assignee.FirstName}. Notes: {assignDto.Notes}";

                await AddHistoryEntry(taskId, "Assigned", oldAssignee, assignee.FirstName, details, currentUser);

                return Ok(new { message = "Task assigned successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error assigning task {taskId}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Update task status
        /// </summary>
        [HttpPatch("{taskId}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int taskId, [FromBody] UpdateTaskStatusDto statusDto)
        {
            try
            {
                var task = await _context.EmailTasks.FindAsync(taskId);
                if (task == null)
                {
                    return NotFound(new { error = "Task not found" });
                }

                var currentUser = GetCurrentUserName();
                var oldStatus = task.Status;

                task.Status = statusDto.Status;
                task.DateUpdated = DateTime.UtcNow;
                task.UpdatedBy = currentUser;

                if (statusDto.Status == "Processed")
                {
                    task.DateProcessed = DateTime.UtcNow;
                    task.ProcessedBy = currentUser;
                    task.CompletedDate = DateTime.UtcNow;
                    task.CompletedBy = currentUser;
                    task.CompletionNotes = statusDto.CompletionNotes;
                }

                await _context.SaveChangesAsync();

                // Add history entry
                await AddHistoryEntry(taskId, "StatusChanged", oldStatus, statusDto.Status,
                    $"Status changed from {oldStatus} to {statusDto.Status}", currentUser);

                return Ok(new { message = "Task status updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating task status {taskId}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Execute an action on a task
        /// </summary>
        [HttpPost("{taskId}/action/{action}")]
        public async Task<IActionResult> ExecuteAction(int taskId, string action, [FromBody] object data)
        {
            try
            {
                var task = await _context.EmailTasks.FindAsync(taskId);
                if (task == null)
                {
                    return NotFound(new { error = "Task not found" });
                }

                var currentUser = GetCurrentUserName();

                // Store the selected action
                task.SelectedAction = action;
                task.DateUpdated = DateTime.UtcNow;
                task.UpdatedBy = currentUser;

                // Add history entry
                await AddHistoryEntry(taskId, "Updated", null, action,
                    $"Action '{action}' executed", currentUser);

                await _context.SaveChangesAsync();

                // Delegate to task service for actual action execution
                var result = await _taskService.ExecuteTaskActionAsync(taskId, action, data);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing action {action} on task {taskId}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Add a comment to a task
        /// </summary>
        [HttpPost("{taskId}/comments")]
        public async Task<ActionResult<TaskCommentDto>> AddComment(int taskId, [FromBody] AddCommentDto commentDto)
        {
            try
            {
                var task = await _context.EmailTasks.FindAsync(taskId);
                if (task == null)
                {
                    return NotFound(new { error = "Task not found" });
                }

                var currentUser = GetCurrentUserName();
                var currentUserId = GetCurrentUserId();

                var comment = new TaskComment
                {
                    TaskId = taskId,
                    CommentText = commentDto.CommentText,
                    UserId = currentUserId,
                    UserName = currentUser,
                    DateCreated = DateTime.UtcNow
                };

                _context.TaskComments.Add(comment);
                await _context.SaveChangesAsync();

                // Add history entry
                await AddHistoryEntry(taskId, "Commented", null, null,
                    $"Comment added by {currentUser}", currentUser);

                var commentDtoResult = new TaskCommentDto
                {
                    CommentId = comment.CommentId,
                    TaskId = comment.TaskId,
                    CommentText = comment.CommentText,
                    UserId = comment.UserId,
                    UserName = comment.UserName,
                    DateCreated = comment.DateCreated,
                    IsEdited = comment.IsEdited
                };

                return Ok(commentDtoResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding comment to task {taskId}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get task history
        /// </summary>
        [HttpGet("{taskId}/history")]
        public async Task<ActionResult<IEnumerable<TaskHistoryDto>>> GetTaskHistory(int taskId)
        {
            try
            {
                var history = await _context.TaskHistories
                    .Where(h => h.TaskId == taskId)
                    .OrderByDescending(h => h.DateCreated)
                    .Select(h => new TaskHistoryDto
                    {
                        HistoryId = h.HistoryId,
                        TaskId = h.TaskId,
                        Action = h.Action,
                        OldValue = h.OldValue,
                        NewValue = h.NewValue,
                        Details = h.Details,
                        DateCreated = h.DateCreated,
                        CreatedBy = h.CreatedBy
                    })
                    .ToListAsync();

                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving history for task {taskId}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            try
            {
                var task = await _context.EmailTasks
                    .Include(t => t.TaskComments)
                    .Include(t => t.TaskAttachments)
                    .Include(t => t.TaskHistories)
                    .FirstOrDefaultAsync(t => t.TaskId == taskId);

                if (task == null)
                {
                    return NotFound(new { error = "Task not found" });
                }

                _context.EmailTasks.Remove(task);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Task deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting task {taskId}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get task statistics
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<TaskStatistics>> GetStatistics()
        {
            try
            {
                var tasks = await _context.EmailTasks.ToListAsync();

                var stats = new TaskStatistics
                {
                    TotalTasks = tasks.Count,
                    PendingTasks = tasks.Count(t => t.Status == "Pending"),
                    InProgressTasks = tasks.Count(t => t.Status == "InProgress"),
                    CompletedTasks = tasks.Count(t => t.Status == "Processed"),
                    OverdueTasks = tasks.Count(t => t.DueDate.HasValue && t.DueDate < DateTime.UtcNow && t.Status != "Processed"),
                    HighPriorityTasks = tasks.Count(t => t.Priority == "High"),
                    UrgentTasks = tasks.Count(t => t.Priority == "Urgent"),
                    TasksByType = tasks
                        .Where(t => !string.IsNullOrEmpty(t.TaskType))
                        .GroupBy(t => t.TaskType)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    TasksByAssignee = tasks
                        .Where(t => !string.IsNullOrEmpty(t.AssignedToUserName))
                        .GroupBy(t => t.AssignedToUserName)
                        .ToDictionary(g => g.Key, g => g.Count())
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task statistics");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get statistics for a specific user
        /// </summary>
        [HttpGet("statistics/user/{userId}")]
        public async Task<ActionResult<TaskStatistics>> GetUserStatistics(int userId)
        {
            try
            {
                var tasks = await _context.EmailTasks
                    .Where(t => t.AssignedToUserId == userId)
                    .ToListAsync();

                var stats = new TaskStatistics
                {
                    TotalTasks = tasks.Count,
                    PendingTasks = tasks.Count(t => t.Status == "Pending"),
                    InProgressTasks = tasks.Count(t => t.Status == "InProgress"),
                    CompletedTasks = tasks.Count(t => t.Status == "Processed"),
                    OverdueTasks = tasks.Count(t => t.DueDate.HasValue && t.DueDate < DateTime.UtcNow && t.Status != "Processed"),
                    HighPriorityTasks = tasks.Count(t => t.Priority == "High"),
                    UrgentTasks = tasks.Count(t => t.Priority == "Urgent"),
                    TasksByType = tasks
                        .Where(t => !string.IsNullOrEmpty(t.TaskType))
                        .GroupBy(t => t.TaskType)
                        .ToDictionary(g => g.Key, g => g.Count())
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving statistics for user {userId}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Search and filter tasks
        /// </summary>
        [HttpPost("search")]
        public async Task<ActionResult<object>> SearchTasks([FromBody] TaskFilterDto filter)
        {
            try
            {
                var query = _context.EmailTasks
                    .Include(t => t.TaskAttachments)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(filter.Status))
                {
                    query = query.Where(t => t.Status == filter.Status);
                }

                if (!string.IsNullOrEmpty(filter.TaskType))
                {
                    query = query.Where(t => t.TaskType == filter.TaskType);
                }

                if (!string.IsNullOrEmpty(filter.Priority))
                {
                    query = query.Where(t => t.Priority == filter.Priority);
                }

                if (filter.AssignedToUserId.HasValue)
                {
                    query = query.Where(t => t.AssignedToUserId == filter.AssignedToUserId);
                }

                if (filter.CustomerId.HasValue)
                {
                    query = query.Where(t => t.CustomerId == filter.CustomerId);
                }

                if (filter.DueDateFrom.HasValue)
                {
                    query = query.Where(t => t.DueDate >= filter.DueDateFrom);
                }

                if (filter.DueDateTo.HasValue)
                {
                    query = query.Where(t => t.DueDate <= filter.DueDateTo);
                }

                if (filter.CreatedDateFrom.HasValue)
                {
                    query = query.Where(t => t.DateCreated >= filter.CreatedDateFrom);
                }

                if (filter.CreatedDateTo.HasValue)
                {
                    query = query.Where(t => t.DateCreated <= filter.CreatedDateTo);
                }

                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(t =>
                        t.Subject.Contains(filter.SearchTerm) ||
                        t.FromName.Contains(filter.SearchTerm) ||
                        t.FromEmail.Contains(filter.SearchTerm) ||
                        t.EmailBody.Contains(filter.SearchTerm)
                    );
                }

                // Get total count
                var totalCount = await query.CountAsync();

                // Apply sorting
                query = filter.SortBy?.ToLower() switch
                {
                    "dateadded" => filter.SortDirection?.ToUpper() == "ASC"
                        ? query.OrderBy(t => t.DateAdded)
                        : query.OrderByDescending(t => t.DateAdded),
                    "subject" => filter.SortDirection?.ToUpper() == "ASC"
                        ? query.OrderBy(t => t.Subject)
                        : query.OrderByDescending(t => t.Subject),
                    "priority" => filter.SortDirection?.ToUpper() == "ASC"
                        ? query.OrderBy(t => t.Priority)
                        : query.OrderByDescending(t => t.Priority),
                    _ => query.OrderByDescending(t => t.DateAdded)
                };

                // Apply pagination
                var tasks = await query
                    .Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync();

                var taskDtos = tasks.Select(t => MapToDto(t)).ToList();

                return Ok(new
                {
                    page = filter.Page,
                    pageSize = filter.PageSize,
                    totalCount = totalCount,
                    totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize),
                    tasks = taskDtos
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching tasks");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        #region Helper Methods

        private EmailTaskDto MapToDto(EmailTask task)
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
                History = task.TaskHistories?.Select(h => new TaskHistoryDto
                {
                    HistoryId = h.HistoryId,
                    TaskId = h.TaskId,
                    Action = h.Action,
                    OldValue = h.OldValue,
                    NewValue = h.NewValue,
                    Details = h.Details,
                    DateCreated = h.DateCreated,
                    CreatedBy = h.CreatedBy
                }).ToList() ?? new List<TaskHistoryDto>()
            };
        }

        private async Task AddHistoryEntry(int taskId, string action, string oldValue, string newValue, string details, string createdBy)
        {
            var history = new TaskHistory
            {
                TaskId = taskId,
                Action = action,
                OldValue = oldValue,
                NewValue = newValue,
                Details = details,
                DateCreated = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            _context.TaskHistories.Add(history);
            await _context.SaveChangesAsync();
        }

        private string GetCurrentUserName()
        {
            // Get from claims or authentication context
            return User?.Identity?.Name ?? "System";
        }

        private int GetCurrentUserId()
        {
            // Get from claims or authentication context
            var userIdClaim = User?.FindFirst("userId")?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        #endregion
    }
}