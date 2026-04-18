using AwningsAPI.Dto.Tasks;
using AwningsAPI.Model.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwningsAPI.Interfaces
{
    public interface ITaskService
    {
        // Basic CRUD
        Task<AppTaskDto> GetTaskByIdAsync(int taskId);
        Task<IEnumerable<AppTaskDto>> GetAllTasksAsync();
        Task<AppTaskDto> CreateTaskAsync(CreateTaskDto createDto, string currentUser);
        Task<AppTaskDto> UpdateTaskAsync(int taskId, UpdateTaskDto updateDto, string currentUser);
        Task<bool> DeleteTaskAsync(int taskId);

        Task<TaskHistoryPagedDto> GetTaskAuditHistoryAsync(int page = 1, int pageSize = 20, string? action = null);

        // Status Management
        Task<AppTaskDto> UpdateTaskStatusAsync(int taskId, UpdateTaskStatusDto statusDto, string currentUser);
        Task<AppTaskDto> CompleteTaskAsync(int taskId, string completionNotes, string currentUser);

        // Assignment
        Task<AppTaskDto> AssignTaskAsync(int taskId, AssignTaskDto assignDto, string currentUser);
        Task<AppTaskDto> UnassignTaskAsync(int taskId, string currentUser);

        // Comments
        Task<TaskCommentDto> AddCommentAsync(int taskId, AddCommentDto commentDto, string currentUser);
        Task<IEnumerable<TaskCommentDto>> GetTaskCommentsAsync(int taskId);
        Task<bool> DeleteCommentAsync(int commentId);

        // Filtering & Search
        Task<(IEnumerable<AppTaskDto> Tasks, int TotalCount)> GetTasksWithFiltersAsync(TaskFilterDto filterDto);
        Task<IEnumerable<AppTaskDto>> GetTasksByUserAsync(int userId);
        Task<IEnumerable<AppTaskDto>> GetTasksByCustomerAsync(int customerId);
        Task<IEnumerable<AppTaskDto>> GetTasksByTypeAsync(string taskType);
        Task<IEnumerable<AppTaskDto>> GetOverdueTasksAsync();
        Task<IEnumerable<AppTaskDto>> GetTasksDueTodayAsync();

        // Statistics
        Task<TaskStatistics> GetTaskStatisticsAsync();
        Task<TaskStatistics> GetUserTaskStatisticsAsync(int userId);

        // History
        Task<IEnumerable<TaskHistoryDto>> GetTaskHistoryAsync(int taskId);

        // Automatic task creation from email
        Task<AppTaskDto> CreateTaskFromEmailAsync(int incomingEmailId, string currentUser);

        /// <summary>
        /// Execute a specific action on a task
        /// </summary>
        /// <param name="taskId">The task ID</param>
        /// <param name="action">Action to execute (add_company, generate_quote, etc.)</param>
        /// <param name="data">Additional data for the action</param>
        /// <returns>Result of the action execution</returns>
        Task<object> ExecuteTaskActionAsync(int taskId, string action, object data);

        Task<ExtractedCustomerDataDto> GetExtractedCustomerDataAsync(int taskId);
        Task<CustomerExistsResponseDto> CheckCustomerExistsAsync(string? email, string? companyNumber);
        Task<AppTaskDto> LinkCustomerToTaskAsync(int taskId, int customerId, string currentUser);

        Task<AppTaskDto> LinkWorkflowToTaskAsync(int taskId, int workflowId, string currentUser);

        Task<(IEnumerable<AppTaskDto> Items, int TotalCount)> GetFilteredTasksAsync(TaskFilterDto filter);

        Task StoreSiteVisitLinkAsync(int taskId, int siteVisitId, string currentUser);

    }
}