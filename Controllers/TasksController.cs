using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs.Tasks;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private const string LoginHeader = "X-Login-Id";

        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> CreateTask(
            CreateTaskDto dto,
            [FromHeader(Name = LoginHeader)] string loginId)
        {
            TaskDto? task = await _taskService.CreateTaskAsync(dto, loginId);

            if (task == null)
            {
                return BadRequest(new { message = "Task could not be created. Check admin, employee, or active task limit." });
            }

            return CreatedAtAction(nameof(GetTaskById), new { taskId = task.taskId }, task);
        }


        [HttpGet]
        public async Task<ActionResult<List<TaskDto>>> GetAllTasks([FromQuery] TaskFilterDto filter)
        {
            return Ok(await _taskService.GetAllTasksAsync(filter));
        }

        [HttpGet("overdue")]
        public async Task<ActionResult<List<TaskDto>>> GetOverdueTasks()
        {
            return Ok(await _taskService.GetOverdueTasksAsync());
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<TaskDto>> GetTaskById(int taskId)
        {
            TaskDto? task = await _taskService.GetTaskByIdAsync(taskId);

            if (task == null)
            {
                return NotFound(new { message = "Task not found." });
            }

            return Ok(task);
        }

        [HttpGet("my")]
        public async Task<ActionResult<List<TaskDto>>> GetMyTasks(
            [FromHeader(Name = LoginHeader)] string loginId)
        {
            return Ok(await _taskService.GetMyTasksAsync(loginId));
        }


        [HttpPut("{taskId}/start")]
        public async Task<ActionResult<TaskDto>> StartTask(
            int taskId,
            [FromHeader(Name = LoginHeader)] string loginId)
        {
            TaskDto? task = await _taskService.StartTaskAsync(taskId, loginId);

            if (task == null)
            {  
                return BadRequest(new { message = "Task cannot be started." });
            }

            return Ok(task);
        }

        [HttpPut("{taskId}/review")]
        public async Task<ActionResult<TaskDto>> SubmitForReview(
            int taskId,
            [FromHeader(Name = LoginHeader)] string loginId)
        {
            TaskDto? task = await _taskService.SubmitForReviewAsync(taskId, loginId);

            if (task == null)
            {
                return BadRequest(new { message = "Task cannot be submitted for review." });
            }

            return Ok(task);
        }


        [HttpPut("{taskId}/done")]
        public async Task<ActionResult<TaskDto>> MarkDone(
            int taskId,
            [FromHeader(Name = LoginHeader)] string loginId)
        {
            TaskDto? task = await _taskService.MarkDoneAsync(taskId, loginId);

            if (task == null)
            {
                return BadRequest(new { message = "Task cannot be marked as Done." });
            }

            return Ok(task);
        }

        [HttpPut("{taskId}/drop")]
        public async Task<ActionResult<TaskDto>> DropTask(
            int taskId,
            [FromHeader(Name = LoginHeader)] string loginId)
        {
            TaskDto? task = await _taskService.DropTaskAsync(taskId, loginId);

            if (task == null)
            {
                return BadRequest(new { message = "Task cannot be dropped." });
            }

            return Ok(task);
        }
    }
}