using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;

//using TaskManagementApi.Data;
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
            var activeTaskCount = await _context.Tasks
                .CountAsync(t => t.assigned_to == dto.assignedTo &&
                                 t.status != "Done" &&
                                 t.status != "Dropped");

            if (activeTaskCount >= MaxActiveTasks)
                return null;

            var task = new TaskItem
            {
                title = dto.title,
                description = dto.description,
                create_date = DateTime.Today,
                due_date = dto.dueDate.Date,
                status = "Pending",
                assigned_to = dto.assignedTo,
                created_by = createdBy
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task<List<TaskDto>> GetAllTasksAsync(TaskFilterDto filter)
        {
            var query = _context.Tasks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.status))
                query = query.Where(t => t.status == filter.status);

            if (filter.fromDate.HasValue)
                query = query.Where(t => t.due_date >= filter.fromDate.Value.Date);

            if (filter.toDate.HasValue)
                query = query.Where(t => t.due_date <= filter.toDate.Value.Date);

            var tasks = await query.ToListAsync();
            return tasks.Select(MapToDto).ToList();
        }

        public async Task<List<TaskDto>> GetOverdueTasksAsync()
        {
            var today = DateTime.Today;

            var tasks = await _context.Tasks
                .Where(t => t.due_date < today &&
                            t.status != "Done" &&
                            t.status != "Dropped")
                .ToListAsync();

            return tasks.Select(MapToDto).ToList();
        }

        public async Task<TaskDto?> GetTaskByIdAsync(int taskId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.task_id == taskId);
            return task == null ? null : MapToDto(task);
        }

        public async Task<List<TaskDto>> GetMyTasksAsync(string loginId)
        {
            var tasks = await _context.Tasks
                .Where(t => t.assigned_to == loginId && t.status != "Dropped")
                .ToListAsync();

            return tasks.Select(MapToDto).ToList();
        }

        public async Task<TaskDto?> StartTaskAsync(int taskId, string loginId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.task_id == taskId);

            if (task == null || task.assigned_to != loginId || task.status != "Pending")
                return null;

            task.status = "InProgress";
            await _context.SaveChangesAsync();
            return MapToDto(task);
        }

        public async Task<TaskDto?> SubmitForReviewAsync(int taskId, string loginId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.task_id == taskId);

            if (task == null || task.assigned_to != loginId || task.status != "InProgress")
                return null;

            task.status = "Review";
            await _context.SaveChangesAsync();
            return MapToDto(task);
        }

        public async Task<TaskDto?> MarkDoneAsync(int taskId, string adminId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.task_id == taskId);

            if (task == null || task.status == "Done" || task.status == "Dropped")
                return null;

            task.status = "Done";
            task.actual_completed_date = DateTime.Today;
            await _context.SaveChangesAsync();
            return MapToDto(task);
        }

        public async Task<TaskDto?> DropTaskAsync(int taskId, string adminId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.task_id == taskId);

            if (task == null || task.status == "Done" || task.status == "Dropped")
                return null;

            task.status = "Dropped";
            await _context.SaveChangesAsync();
            return MapToDto(task);
        }

        private static TaskDto MapToDto(TaskItem task)
        {
            return new TaskDto
            {
                taskId = task.task_id,
                title = task.title,
                description = task.description,
                createDate = task.create_date,
                dueDate = task.due_date,
                actualCompletedDate = task.actual_completed_date,
                status = task.status,
                assignedTo = task.assigned_to,
                createdBy = task.created_by
            };
        }
    }
}