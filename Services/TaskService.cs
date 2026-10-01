using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;


using TaskManagementApi.DTOs.Tasks;
using TaskManagementApi.Models;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private const int MaxActiveTasks = 4;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TaskDto?> CreateTaskAsync(CreateTaskDto dto, string createdBy)
        {
            int activeTaskCount = await _context.Tasks
                .CountAsync(t => t.AssignedTo == dto.AssignedTo &&
                                 t.Status != "Done" &&
                                 t.Status != "Dropped");

            if (activeTaskCount >= MaxActiveTasks)
                return null;

            TaskItem task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                CreateDate = DateTime.Today,
                DueDate = dto.DueDate.Date,
                Status = "Pending",
                AssignedTo = dto.AssignedTo,
                CreatedDy = createdBy
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task<List<TaskDto>> GetAllTasksAsync(TaskFilterDto filter)
        {
            List<TaskItem> tasks = await _context.Tasks.ToListAsync();

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                tasks = tasks
                    .Where(t => t.Status == filter.Status)
                    .ToList();
            }

            if (filter.FromDate.HasValue)
            {
                tasks = tasks
                    .Where(t => t.DueDate >= filter.FromDate.Value.Date)
                    .ToList();
            }

            if (filter.ToDate.HasValue)
            {
                tasks = tasks
                    .Where(t => t.DueDate <= filter.ToDate.Value.Date)
                    .ToList();
            }

            return tasks.Select(MapToDto).ToList();
        }

        public async Task<List<TaskDto>> GetOverdueTasksAsync()
        {
            DateTime today = DateTime.Today;

            List<TaskItem> tasks = await _context.Tasks
                .Where(t => t.DueDate < today &&
                            t.Status != "Done" &&
                            t.Status != "Dropped")
                .ToListAsync();

            return tasks.Select(MapToDto).ToList();
        }

        public async Task<TaskDto?> GetTaskByIdAsync(int taskId)
        {
            TaskItem? task = await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);
            return task == null ? null : MapToDto(task);
        }

        public async Task<List<TaskDto>> GetMyTasksAsync(string loginId)
        {
            List<TaskItem> tasks = await _context.Tasks
                .Where(t => t.AssignedTo == loginId && t.Status != "Dropped")
                .ToListAsync();

            return tasks.Select(MapToDto).ToList();
        }

        public async Task<TaskDto?> StartTaskAsync(int taskId, string loginId)
        {
            TaskItem? task = await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null || task.AssignedTo != loginId || task.Status != "Pending")
                return null;

            task.Status = "InProgress";
            await _context.SaveChangesAsync();
            return MapToDto(task);
        }

        public async Task<TaskDto?> SubmitForReviewAsync(int taskId, string loginId)
        {
            TaskItem? task = await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null || task.AssignedTo != loginId || task.Status != "InProgress")
                return null;

            task.Status = "Review";
            await _context.SaveChangesAsync();
            return MapToDto(task);
        }

        public async Task<TaskDto?> MarkDoneAsync(int taskId, string adminId)
        {
            TaskItem? task = await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null || task.Status == "Done" || task.Status == "Dropped")
                return null;

            task.Status = "Done";
            task.ActualCompleted_date = DateTime.Today;
            await _context.SaveChangesAsync();
            return MapToDto(task);
        }

        public async Task<TaskDto?> DropTaskAsync(int taskId, string adminId)
        {
            TaskItem? task = await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null || task.Status == "Done" || task.Status == "Dropped")
                return null;

            task.Status = "Dropped";
            await _context.SaveChangesAsync();
            return MapToDto(task);
        }

        private static TaskDto MapToDto(TaskItem task)
        {
            return new TaskDto
            {
                taskId = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                CreateDate = task.CreateDate,
                DueDate = task.DueDate,
                ActualCompletedDate = task.ActualCompleted_date,
                Status = task.Status,
                AssignedTo = task.AssignedTo,
                CreatedBy = task.CreatedDy
            };
        }
    }
}