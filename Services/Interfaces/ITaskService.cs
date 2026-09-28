using TaskManagementApi.DTOs.Tasks;

namespace TaskManagementApi.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskDto?> CreateTaskAsync(CreateTaskDto dto, string createdBy);
        Task<List<TaskDto>> GetAllTasksAsync(TaskFilterDto filter);
        Task<List<TaskDto>> GetOverdueTasksAsync();
        Task<TaskDto?> GetTaskByIdAsync(int taskId);
        Task<List<TaskDto>> GetMyTasksAsync(string loginId);
        Task<TaskDto?> StartTaskAsync(int taskId, string loginId);
        Task<TaskDto?> SubmitForReviewAsync(int taskId, string loginId);
        Task<TaskDto?> MarkDoneAsync(int taskId, string adminId);
        Task<TaskDto?> DropTaskAsync(int taskId, string adminId);
    }
}