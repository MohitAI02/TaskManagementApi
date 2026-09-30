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
            User? employee = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u =>
                    u.LoginId == loginId &&
                    u.Role != null &&
                    u.Role.RoleName == "EMPLOYEE");

            if (employee == null)
            {
                return null;
            }

            List<TaskItem> tasks = await _context.Tasks
                .Where(t => t.AssignedTo == loginId)
                .ToListAsync();

            return BuildReport(employee, tasks);
        }

        public async Task<List<EmployeeReportDto>> GetAllEmployeeReportsAsync()
        {
            List<User> employees = await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role != null && u.Role.RoleName == "EMPLOYEE")
                .ToListAsync();

            List<TaskItem> allTasks = await _context.Tasks.ToListAsync();

            List<EmployeeReportDto> reports = new List<EmployeeReportDto>();

            foreach (User employee in employees)
            {
                List<TaskItem> employeeTasks = allTasks
                    .Where(t => t.AssignedTo == employee.LoginId)
                    .ToList();

                reports.Add(BuildReport(employee, employeeTasks));
            }

            return reports;
        }

        private static EmployeeReportDto BuildReport(User employee, List<TaskItem> tasks)
        {
            DateTime today = DateTime.Today;

            int totalAssigned = tasks.Count;

            int completedInTime = tasks.Count(t =>
                t.Status == "Done" &&
                t.ActualCompleted_date.HasValue &&
                t.ActualCompleted_date.Value <= t.DueDate);

            int overdueCount = tasks.Count(t =>
                t.DueDate < today &&
                t.Status != "Done" &&
                t.Status != "Dropped");

            return new EmployeeReportDto
            {
                LoginId = employee.LoginId,
                Name = employee.Name,
                TotalAssigned = totalAssigned,
                CompletedInTime = completedInTime,
                OverdueCount = overdueCount,
                RatioCompletedToOverdue = $"{completedInTime}:{overdueCount}"
            };
        }
    }
}