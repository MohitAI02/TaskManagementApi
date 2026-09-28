using TaskManagementApi.DTOs.Employees;

namespace TaskManagementApi.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDto?> CreateEmployeeAsync(CreateEmployeeDto dto);
        Task<List<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto?> GetEmployeeAsync(string loginId);
    }
}