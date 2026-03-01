using AwningsAPI.Dto.Tasks;
using AwningsAPI.Services.WorkflowService;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AwningsAPI.Controllers
{
    /// <summary>
    /// Controller for managing email-derived tasks
    /// All business logic and database access is delegated to ITaskService
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmailTaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IEmailReaderService _emailReaderService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailTaskController> _logger;
        private readonly FollowUpService _followUpService;

        public EmailTaskController(
            ITaskService taskService,
            IEmailReaderService emailReaderService,
            IConfiguration configuration,
            ILogger<EmailTaskController> logger,
            FollowUpService followUpService)
        {
            _taskService = taskService;
            _emailReaderService = emailReaderService;
            _configuration = configuration;
            _logger = logger;
            _followUpService = followUpService;
        }

        #region GET Endpoints

        /// <summary>
        /// Get all tasks by status (Pending, Processed, Junk)
        /// </summary>
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<EmailTaskDto>>> GetTasksByStatus(string status)
        {
            try
            {
                var isAdmin = User.IsInRole("Admin");
                var currentUserId = GetCurrentUserId();

                var filter = new TaskFilterDto
                {
                    Status = status,
                    SortBy = "DateAdded",
                    SortDirection = "DESC",
                    Page = 1,
                    PageSize = 1000,
                    CurrentUserId = currentUserId > 0 ? currentUserId : null,
                    IsAdmin = isAdmin
                };

                var (tasks, totalCount) = await _taskService.GetTasksWithFiltersAsync(filter);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving tasks with status: {status}");
                return StatusCode(500, new { error = "An error occurred while retrieving tasks", details = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific task by ID with all related data
        /// </summary>
        [HttpGet("{taskId}")]
        public async Task<ActionResult<EmailTaskDto>> GetTaskById(int taskId)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(taskId);

                if (task == null)
                {
                    return NotFound(new { error = "Task not found" });
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving task {taskId}");
                return StatusCode(500, new { error = "An error occurred while retrieving the task", details = ex.Message });
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
                var tasks = await _taskService.GetTasksByUserAsync(userId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving tasks for user {userId}");
                return StatusCode(500, new { error = "An error occurred while retrieving user tasks", details = ex.Message });
            }
        }

        /// <summary>
        /// Get tasks assigned to the current authenticated user
        /// </summary>
        [HttpGet("my-tasks")]
        public async Task<ActionResult<IEnumerable<EmailTaskDto>>> GetMyTasks()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return BadRequest(new { error = "User ID not found in authentication context" });
                }

                var tasks = await _taskService.GetTasksByUserAsync(userId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving current user's tasks");
                return StatusCode(500, new { error = "An error occurred while retrieving your tasks", details = ex.Message });
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
                var filter = new TaskFilterDto
                {
                    SearchTerm = category, // Use search term for category filtering
                    SortBy = "DateAdded",
                    SortDirection = "DESC",
                    Page = 1,
                    PageSize = 1000
                };

                var (tasks, totalCount) = await _taskService.GetTasksWithFiltersAsync(filter);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving tasks for category {category}");
                return StatusCode(500, new { error = "An error occurred while retrieving category tasks", details = ex.Message });
            }
        }

        /// <summary>
        /// Get tasks by task type
        /// </summary>
        [HttpGet("type/{taskType}")]
        public async Task<ActionResult<IEnumerable<EmailTaskDto>>> GetTasksByType(string taskType)
        {
            try
            {
                var tasks = await _taskService.GetTasksByTypeAsync(taskType);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving tasks for type {taskType}");
                return StatusCode(500, new { error = "An error occurred while retrieving tasks by type", details = ex.Message });
            }
        }

        /// <summary>
        /// Get tasks for a specific customer
        /// </summary>
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<EmailTaskDto>>> GetTasksByCustomer(int customerId)
        {
            try
            {
                var tasks = await _taskService.GetTasksByCustomerAsync(customerId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving tasks for customer {customerId}");
                return StatusCode(500, new { error = "An error occurred while retrieving customer tasks", details = ex.Message });
            }
        }

        /// <summary>
        /// Get overdue tasks
        /// </summary>
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<EmailTaskDto>>> GetOverdueTasks()
        {
            try
            {
                var tasks = await _taskService.GetOverdueTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving overdue tasks");
                return StatusCode(500, new { error = "An error occurred while retrieving overdue tasks", details = ex.Message });
            }
        }

        /// <summary>
        /// Get tasks due today
        /// </summary>
        [HttpGet("due-today")]
        public async Task<ActionResult<IEnumerable<EmailTaskDto>>> GetTasksDueToday()
        {
            try
            {
                var tasks = await _taskService.GetTasksDueTodayAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tasks due today");
                return StatusCode(500, new { error = "An error occurred while retrieving tasks due today", details = ex.Message });
            }
        }

        /// <summary>
        /// Search and filter tasks with pagination
        /// </summary>
        [HttpPost("search")]
        public async Task<ActionResult> SearchTasks([FromBody] TaskFilterDto filter)
        {
            try
            {
                // Enforce role-based visibility server-side
                var isAdmin = User.IsInRole("Admin");
                var currentUserId = GetCurrentUserId();
                filter.IsAdmin = isAdmin;
                if (!isAdmin && currentUserId > 0)
                    filter.CurrentUserId = currentUserId;

                var (tasks, totalCount) = await _taskService.GetTasksWithFiltersAsync(filter);

                return Ok(new
                {
                    page = filter.Page,
                    pageSize = filter.PageSize,
                    totalCount = totalCount,
                    totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize),
                    tasks = tasks
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching tasks");
                return StatusCode(500, new { error = "An error occurred while searching tasks", details = ex.Message });
            }
        }

        #endregion

        #region POST Endpoints

        /// <summary>
        /// Create a new task manually
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EmailTaskDto>> CreateTask([FromBody] CreateTaskDto createDto)
        {
            try
            {
                var currentUser = GetCurrentUserName();
                var task = await _taskService.CreateTaskAsync(createDto, currentUser);

                return CreatedAtAction(nameof(GetTaskById), new { taskId = task.TaskId }, task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task");
                return StatusCode(500, new { error = "An error occurred while creating the task", details = ex.Message });
            }
        }

        /// <summary>
        /// Create a task from an existing email
        /// </summary>
        [HttpPost("from-email/{incomingEmailId}")]
        public async Task<ActionResult<EmailTaskDto>> CreateTaskFromEmail(int incomingEmailId)
        {
            try
            {
                var currentUser = GetCurrentUserName();
                var task = await _taskService.CreateTaskFromEmailAsync(incomingEmailId, currentUser);

                if (task == null)
                {
                    return NotFound(new { error = "Email not found or task could not be created" });
                }

                return CreatedAtAction(nameof(GetTaskById), new { taskId = task.TaskId }, task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating task from email {incomingEmailId}");
                return StatusCode(500, new { error = "An error occurred while creating task from email", details = ex.Message });
            }
        }

        #endregion

        #region PUT Endpoints

        /// <summary>
        /// Update a task
        /// </summary>
        [HttpPut("{taskId}")]
        public async Task<ActionResult<EmailTaskDto>> UpdateTask(int taskId, [FromBody] UpdateTaskDto updateDto)
        {
            try
            {
                var currentUser = GetCurrentUserName();
                var task = await _taskService.UpdateTaskAsync(taskId, updateDto, currentUser);

                if (task == null)
                {
                    return NotFound(new { error = "Task not found" });
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating task {taskId}");
                return StatusCode(500, new { error = "An error occurred while updating the task", details = ex.Message });
            }
        }

        /// <summary>
        /// Update task status
        /// </summary>
        [HttpPut("{taskId}/status")]
        public async Task<ActionResult<EmailTaskDto>> UpdateTaskStatus(int taskId, [FromBody] UpdateTaskStatusDto statusDto)
        {
            try
            {
                var currentUser = GetCurrentUserName();
                var task = await _taskService.UpdateTaskStatusAsync(taskId, statusDto, currentUser);

                if (task == null)
                {
                    return NotFound(new { error = "Task not found" });
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating task status {taskId}");
                return StatusCode(500, new { error = "An error occurred while updating task status", details = ex.Message });
            }
        }

        /// <summary>
        /// Complete a task
        /// </summary>
        [HttpPut("{taskId}/complete")]
        public async Task<ActionResult<EmailTaskDto>> CompleteTask(int taskId, [FromBody] string completionNotes = null)
        {
            try
            {
                var currentUser = GetCurrentUserName();
                var task = await _taskService.CompleteTaskAsync(taskId, completionNotes, currentUser);

                if (task == null)
                {
                    return NotFound(new { error = "Task not found" });
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error completing task {taskId}");
                return StatusCode(500, new { error = "An error occurred while completing the task", details = ex.Message });
            }
        }

        /// <summary>
        /// Assign a task to a user
        /// </summary>
        [HttpPut("{taskId}/assign")]
        public async Task<ActionResult<EmailTaskDto>> AssignTask(int taskId, [FromBody] AssignTaskDto assignDto)
        {
            try
            {
                var currentUser = GetCurrentUserName();
                var task = await _taskService.AssignTaskAsync(taskId, assignDto, currentUser);

                if (task == null)
                {
                    return NotFound(new { error = "Task not found" });
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error assigning task {taskId}");
                return StatusCode(500, new { error = "An error occurred while assigning the task", details = ex.Message });
            }
        }

        /// <summary>
        /// Unassign a task
        /// </summary>
        [HttpPut("{taskId}/unassign")]
        public async Task<ActionResult<EmailTaskDto>> UnassignTask(int taskId)
        {
            try
            {
                var currentUser = GetCurrentUserName();
                var task = await _taskService.UnassignTaskAsync(taskId, currentUser);

                if (task == null)
                {
                    return NotFound(new { error = "Task not found" });
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error unassigning task {taskId}");
                return StatusCode(500, new { error = "An error occurred while unassigning the task", details = ex.Message });
            }
        }

        /// <summary>
        /// Paginated task audit log — shown in the front-end "Task Audit" tab.
        /// Returns TaskHistory rows filtered to: Created | Assigned | Unassigned
        ///
        /// GET /api/EmailTask/audit
        /// GET /api/EmailTask/audit?page=1&pageSize=20
        /// GET /api/EmailTask/audit?action=Assigned
        /// </summary>
        [HttpGet("audit")]
        public async Task<ActionResult<TaskHistoryPagedDto>> GetTaskAuditHistory(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? action = null)
        {
            try
            {
                var result = await _taskService.GetTaskAuditHistoryAsync(page, pageSize, action);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task audit history");
                return StatusCode(500, new { error = "Failed to retrieve audit history", details = ex.Message });
            }
        }
        #endregion

        #region DELETE Endpoints

        /// <summary>
        /// Delete a task
        /// </summary>
        [HttpDelete("{taskId}")]
        public async Task<ActionResult> DeleteTask(int taskId)
        {
            try
            {
                var result = await _taskService.DeleteTaskAsync(taskId);

                if (!result)
                {
                    return NotFound(new { error = "Task not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting task {taskId}");
                return StatusCode(500, new { error = "An error occurred while deleting the task", details = ex.Message });
            }
        }

        #endregion

        #region Comments

        /// <summary>
        /// Get comments for a task
        /// </summary>
        [HttpGet("{taskId}/comments")]
        public async Task<ActionResult<IEnumerable<TaskCommentDto>>> GetTaskComments(int taskId)
        {
            try
            {
                var comments = await _taskService.GetTaskCommentsAsync(taskId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving comments for task {taskId}");
                return StatusCode(500, new { error = "An error occurred while retrieving comments", details = ex.Message });
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
                var currentUser = GetCurrentUserName();
                var comment = await _taskService.AddCommentAsync(taskId, commentDto, currentUser);

                if (comment == null)
                {
                    return NotFound(new { error = "Task not found" });
                }

                return CreatedAtAction(nameof(GetTaskComments), new { taskId = taskId }, comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding comment to task {taskId}");
                return StatusCode(500, new { error = "An error occurred while adding the comment", details = ex.Message });
            }
        }

        /// <summary>
        /// Delete a comment
        /// </summary>
        [HttpDelete("comments/{commentId}")]
        public async Task<ActionResult> DeleteComment(int commentId)
        {
            try
            {
                var result = await _taskService.DeleteCommentAsync(commentId);

                if (!result)
                {
                    return NotFound(new { error = "Comment not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting comment {commentId}");
                return StatusCode(500, new { error = "An error occurred while deleting the comment", details = ex.Message });
            }
        }

        #endregion

        #region History

        /// <summary>
        /// Get history for a task
        /// </summary>
        [HttpGet("{taskId}/history")]
        public async Task<ActionResult<IEnumerable<TaskHistoryDto>>> GetTaskHistory(int taskId)
        {
            try
            {
                var history = await _taskService.GetTaskHistoryAsync(taskId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving history for task {taskId}");
                return StatusCode(500, new { error = "An error occurred while retrieving task history", details = ex.Message });
            }
        }

        #endregion

        #region Statistics

        /// <summary>
        /// Get overall task statistics
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<TaskStatistics>> GetTaskStatistics()
        {
            try
            {
                var stats = await _taskService.GetTaskStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task statistics");
                return StatusCode(500, new { error = "An error occurred while retrieving statistics", details = ex.Message });
            }
        }

        /// <summary>
        /// Get task statistics for a specific user
        /// </summary>
        [HttpGet("statistics/user/{userId}")]
        public async Task<ActionResult<TaskStatistics>> GetUserTaskStatistics(int userId)
        {
            try
            {
                var stats = await _taskService.GetUserTaskStatisticsAsync(userId);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving statistics for user {userId}");
                return StatusCode(500, new { error = "An error occurred while retrieving user statistics", details = ex.Message });
            }
        }

        /// <summary>
        /// Get task statistics for the current user
        /// </summary>
        [HttpGet("statistics/my-stats")]
        public async Task<ActionResult<TaskStatistics>> GetMyTaskStatistics()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return BadRequest(new { error = "User ID not found in authentication context" });
                }

                var stats = await _taskService.GetUserTaskStatisticsAsync(userId);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving current user's statistics");
                return StatusCode(500, new { error = "An error occurred while retrieving your statistics", details = ex.Message });
            }
        }

        #endregion

        #region Task Actions

        /// <summary>
        /// Execute a specific action on a task (add_company, generate_quote, etc.)
        /// </summary>
        [HttpPost("{taskId}/action/{action}")]
        public async Task<ActionResult<object>> ExecuteTaskAction(
            int taskId, string action,
            [FromBody] TaskActionRequest request)
        {
            try
            {
                var result = await _taskService.ExecuteTaskActionAsync(taskId, action, request.Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing action on task {taskId}");
                return StatusCode(500, new { error = "An error occurred while executing the action", details = ex.Message });
            }
        }


        // ==================== NEW: CUSTOMER CREATION ENDPOINTS ====================

        /// <summary>
        /// Returns AI-extracted customer data from an email task,
        /// used to pre-fill the customer creation form in the UI.
        /// Called by Angular: getExtractedCustomerData(taskId)
        /// GET /api/EmailTask/{taskId}/extracted-customer-data
        /// </summary>
        [HttpGet("{taskId}/extracted-customer-data")]
        public async Task<IActionResult> GetExtractedCustomerData(int taskId)
        {
            try
            {
                var data = await _taskService.GetExtractedCustomerDataAsync(taskId);

                if (data == null)
                    return NotFound(new { error = "Task not found" });

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting extracted customer data for task {TaskId}", taskId);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Checks whether a customer already exists by email or company number,
        /// preventing duplicate creation before the user submits the form.
        /// Called by Angular: checkCustomerExists({ email, companyNumber })
        /// POST /api/EmailTask/check-customer-exists
        /// </summary>
        [HttpPost("check-customer-exists")]
        public async Task<IActionResult> CheckCustomerExists([FromBody] CheckCustomerRequestDto request)
        {
            try
            {
                var result = await _taskService.CheckCustomerExistsAsync(request.Email, request.CompanyNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking customer exists");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Links an existing customer record to a task after the user
        /// either creates a new customer or selects an existing one.
        /// Called by Angular: linkCustomerToTask(taskId, customerId)
        /// POST /api/EmailTask/{taskId}/link-customer
        /// </summary>
        [HttpPost("{taskId}/link-customer")]
        public async Task<IActionResult> LinkCustomerToTask(int taskId, [FromBody] LinkCustomerRequestDto request)
        {
            try
            {
                var currentUser = GetCurrentUserName();
                var task = await _taskService.LinkCustomerToTaskAsync(taskId, request.CustomerId, currentUser);

                if (task == null)
                    return NotFound(new { error = "Task not found" });

                return Ok(task);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error linking customer {CustomerId} to task {TaskId}",
                    request.CustomerId, taskId);
                return StatusCode(500, new { error = ex.Message });
            }
        }


        /// <summary>
        /// Links a newly created workflow to a task after the user
        /// creates a workflow from the email-task screen.
        /// POST /api/EmailTask/{taskId}/link-workflow
        /// </summary>
        [HttpPost("{taskId}/link-workflow")]
        public async Task<IActionResult> LinkWorkflowToTask(int taskId, [FromBody] LinkWorkflowRequestDto request)
        {
            try
            {
                var currentUser = GetCurrentUserName();
                var task = await _taskService.LinkWorkflowToTaskAsync(taskId, request.WorkflowId, currentUser);

                if (task == null)
                    return NotFound(new { error = "Task not found" });

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error linking workflow {WorkflowId} to task {TaskId}",
                    request.WorkflowId, taskId);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Send an email reply directly from the task viewer.
        /// Uses the original email's Graph messageId to reply in-thread when available,
        /// otherwise sends a fresh email to the task's fromEmail address.
        ///
        /// POST /api/EmailTask/{taskId}/send-email
        /// Body: SendTaskEmailRequest
        /// </summary>
        [HttpPost("{taskId}/send-email")]
        public async Task<IActionResult> SendTaskEmail(int taskId, [FromBody] SendTaskEmailRequest request)
        {
            try
            {
#if DEBUG 
                request.ToEmail = "gistyf30@hotmail.com"; // "mrk.ranjithkumar@gmail.com";
#endif
                var mailboxEmail = _configuration["AzureAd:MonitoredMailbox"]
                    ?? _configuration["AzureAd:OrganizerEmail"]
                    ?? throw new InvalidOperationException("Monitored mailbox not configured");

                // Resolve recipient details from the task if not supplied in the request
                if (string.IsNullOrWhiteSpace(request.ToEmail))
                {
                    var task = await _taskService.GetTaskByIdAsync(taskId);
                    if (task == null)
                        return NotFound(new { error = "Task not found" });

                    request.ToEmail = task.FromEmail;
                    request.ToName = task.FromName ?? task.FromEmail;
                }

                // Map attachment DTOs to the tuple list SendEmailAsync expects
                var attachments = request.Attachments?
                    .Where(a => !string.IsNullOrWhiteSpace(a.Base64Content))
                    .Select(a => (a.FileName, a.Base64Content, a.ContentType))
                    .ToList();

                await _emailReaderService.SendEmailAsync(
                    mailboxEmail: mailboxEmail,
                    toEmail: request.ToEmail,
                    toName: request.ToName ?? request.ToEmail,
                    subject: request.Subject,
                    bodyHtml: WrapPlainTextAsHtml(request.Body),
                    replyToEmailId: request.OriginalEmailGraphId,  // null = fresh email
                    attachments: attachments?.Count > 0 ? attachments : null
                );

                _logger.LogInformation(
                    "Email sent from task {TaskId} to {ToEmail} by {User} ({AttachCount} attachments)",
                    taskId, request.ToEmail, GetCurrentUserName(), attachments?.Count ?? 0);

                // Auto-dismiss any active follow-up for this task's workflow.
                // Sending a reply email constitutes "following up" — the 3-day timer resets.
                var sentTask = await _taskService.GetTaskByIdAsync(taskId);
                if (sentTask?.WorkflowId.HasValue == true)
                {
                    await _followUpService.DismissActiveForWorkflowAsync(
                        sentTask.WorkflowId.Value, GetCurrentUserName());
                    _logger.LogInformation(
                        "Auto-dismissed follow-up for workflow {WorkflowId} after email reply",
                        sentTask.WorkflowId.Value);
                }

                return Ok(new { message = "Email sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email for task {TaskId}", taskId);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Send a fresh outbound email without requiring a task context.
        /// Used when an enquiry is added manually (no inbound email task exists yet).
        /// Always sends as a new email — never threaded.
        ///
        /// POST /api/EmailTask/send-direct
        /// Body: SendDirectEmailRequest
        /// </summary>
        [HttpPost("send-direct")]
        public async Task<IActionResult> SendDirectEmail([FromBody] SendDirectEmailRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.ToEmail))
                    return BadRequest(new { error = "ToEmail is required" });

                if (string.IsNullOrWhiteSpace(request.Subject))
                    return BadRequest(new { error = "Subject is required" });

#if DEBUG
                request.ToEmail = "gistyf30@hotmail.com"; // "mrk.ranjithkumar@gmail.com";
#endif

                var mailboxEmail = _configuration["AzureAd:MonitoredMailbox"]
                    ?? _configuration["AzureAd:OrganizerEmail"]
                    ?? throw new InvalidOperationException("Monitored mailbox not configured");

                // Map attachment DTOs to the tuple list SendEmailAsync expects
                var attachments = request.Attachments?
                    .Where(a => !string.IsNullOrWhiteSpace(a.Base64Content))
                    .Select(a => (a.FileName, a.Base64Content, a.ContentType))
                    .ToList();

                await _emailReaderService.SendEmailAsync(
                    mailboxEmail: mailboxEmail,
                    toEmail: request.ToEmail,
                    toName: request.ToName ?? request.ToEmail,
                    subject: request.Subject,
                    bodyHtml: WrapPlainTextAsHtml(request.Body),
                    replyToEmailId: null,   // always a fresh email — no thread
                    attachments: attachments?.Count > 0 ? attachments : null
                );

                _logger.LogInformation(
                    "Direct email sent to {ToEmail} by {User} ({AttachCount} attachments, no task context)",
                    request.ToEmail, GetCurrentUserName(), attachments?.Count ?? 0);

                return Ok(new { message = "Email sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending direct email to {ToEmail}", request.ToEmail);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Converts a plain-text body (with line breaks) to minimal HTML so Graph
        /// renders it correctly. If the caller already supplies HTML it is passed through.
        /// </summary>
        private static string WrapPlainTextAsHtml(string body)
        {
            if (string.IsNullOrWhiteSpace(body)) return "<p></p>";

            // Already HTML — don't double-wrap
            if (body.TrimStart().StartsWith("<", StringComparison.Ordinal))
                return body;

            var escaped = System.Net.WebUtility.HtmlEncode(body)
                                               .Replace("\r\n", "<br>")
                                               .Replace("\n", "<br>");
            return $"<div style=\"font-family:Aptos,Calibri,Arial,sans-serif;font-size:12pt;\">{escaped}</div>";
        }

        // ==================== REQUEST DTOs ====================

        public class CheckCustomerRequestDto
        {
            public string? Email { get; set; }
            public string? CompanyNumber { get; set; }
        }

        public class LinkWorkflowRequestDto
        {
            public int WorkflowId { get; set; }
        }

        public class AssignTaskRequestDto
        {
            public int AssignedToUserId { get; set; }
            public string? Notes { get; set; }
        }

        public class ExecuteActionRequestDto
        {
            public object? Data { get; set; }
        }

        public class EmailAttachmentDto
        {
            /// <summary>File name shown in email client, e.g. "Quote-001.pdf".</summary>
            public string FileName { get; set; } = string.Empty;

            /// <summary>Base64-encoded file content.</summary>
            public string Base64Content { get; set; } = string.Empty;

            /// <summary>MIME type, e.g. "application/pdf".</summary>
            public string ContentType { get; set; } = "application/octet-stream";
        }

        public class SendDirectEmailRequest
        {
            /// <summary>Recipient email address. Required.</summary>
            public string ToEmail { get; set; } = string.Empty;
            public string? ToName { get; set; }

            /// <summary>Email subject line. Required.</summary>
            public string Subject { get; set; } = string.Empty;

            /// <summary>Plain-text or HTML body.</summary>
            public string Body { get; set; } = string.Empty;

            /// <summary>Optional file attachments encoded as base64.</summary>
            public List<EmailAttachmentDto>? Attachments { get; set; }
        }

        public class SendTaskEmailRequest
        {
            /// <summary>Recipient — resolved from task.FromEmail when omitted.</summary>
            public string? ToEmail { get; set; }
            public string? ToName { get; set; }

            /// <summary>Email subject line.</summary>
            public string Subject { get; set; } = string.Empty;

            /// <summary>Plain-text or HTML body.</summary>
            public string Body { get; set; } = string.Empty;

            /// <summary>
            /// The Graph message ID of the original inbound email.
            /// When supplied the reply is threaded; when null a fresh email is sent.
            /// Maps to IncomingEmail.EmailId stored on the task.
            /// </summary>
            public string? OriginalEmailGraphId { get; set; }

            /// <summary>Optional file attachments encoded as base64.</summary>
            public List<EmailAttachmentDto>? Attachments { get; set; }
        }

        #endregion

        #region Helper Methods

        private string GetCurrentUserName()
        {
            return User?.Identity?.Name ?? "System";
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User?.FindFirst("userId")?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        #endregion
    }

    /// <summary>
    /// Request model for executing task actions
    /// </summary>
    public class TaskActionRequest
    {
        public string Action { get; set; }
        public object Data { get; set; }
    }
}