using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.DTOs.Reports;
using TaskManagementApi.Models;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeReportDto?> GetEmployeeReportAsync(string loginId)
        {
            var employee = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u =>
                    u.login_id == loginId &&
                    u.Role != null &&
                    u.Role.role_name == "EMPLOYEE");

            if (employee == null)
            {
                return null;
            }

            var tasks = await _context.Tasks
                .Where(t => t.assigned_to == loginId)
                .ToListAsync();

            return BuildReport(employee, tasks);
        }

        public async Task<List<EmployeeReportDto>> GetAllEmployeeReportsAsync()
        {
            var employees = await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role != null && u.Role.role_name == "EMPLOYEE")
                .ToListAsync();

            var allTasks = await _context.Tasks.ToListAsync();

            var reports = new List<EmployeeReportDto>();

            foreach (var employee in employees)
            {
                var employeeTasks = allTasks
                    .Where(t => t.assigned_to == employee.login_id)
                    .ToList();

                reports.Add(BuildReport(employee, employeeTasks));
            }

            return reports;
        }

        private static EmployeeReportDto BuildReport(User employee, List<TaskItem> tasks)
        {
            var today = DateTime.Today;

            var totalAssigned = tasks.Count;

            var completedInTime = tasks.Count(t =>
                t.status == "Done" &&
                t.actual_completed_date.HasValue &&
                t.actual_completed_date.Value <= t.due_date);

            var overdueCount = tasks.Count(t =>
                t.due_date < today &&
                t.status != "Done" &&
                t.status != "Dropped");

            return new EmployeeReportDto
            {
                loginId = employee.login_id,
                name = employee.name,
                totalAssigned = totalAssigned,
                completedInTime = completedInTime,
                overdueCount = overdueCount,
                ratioCompletedToOverdue = $"{completedInTime}:{overdueCount}"
            };
        }
    }
}