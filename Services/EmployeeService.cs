using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;

//using TaskManagementApi.Data;
using TaskManagementApi.DTOs.Employees;
using TaskManagementApi.Models;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeDto?> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            var existingEmployee = await _context.Users
                .FirstOrDefaultAsync(u => u.login_id == dto.email);

            if (existingEmployee != null)
            {
                return null;
            }

            var employeeRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.role_name == "EMPLOYEE");

            if (employeeRole == null)
            {
                return null;
            }

            var employee = new User
            {
                login_id = dto.email,
                password = "Pass@1234",
                role_id = employeeRole.role_id,
                name = dto.name,
                designation = dto.designation
            };

            _context.Users.Add(employee);
            await _context.SaveChangesAsync();

            return new EmployeeDto
            {
                userId = employee.user_id,
                loginId = employee.login_id,
                name = employee.name,
                designation = employee.designation
            };
        }

        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role != null && u.Role.role_name == "EMPLOYEE")
                .ToListAsync();

            return employees
                .Select(employee => new EmployeeDto
                {
                    userId = employee.user_id,
                    loginId = employee.login_id,
                    name = employee.name,
                    designation = employee.designation
                })
                .ToList();
        }

        public async Task<EmployeeDto?> GetEmployeeAsync(string loginId)
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

            return new EmployeeDto
            {
                userId = employee.user_id,
                loginId = employee.login_id,
                name = employee.name,
                designation = employee.designation
            };
        }
    }
}