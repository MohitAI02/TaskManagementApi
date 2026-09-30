using TaskManagementApi.DTOs.Reports;

namespace TaskManagementApi.Services.Interfaces
{
    public interface IReportService
    {
        Task<EmployeeReportDto?> GetEmployeeReportAsync(string loginId);

        Task<List<EmployeeReportDto>> GetAllEmployeeReportsAsync();
    }
}