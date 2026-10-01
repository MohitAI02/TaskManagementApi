using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
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
            User? existingEmployee = await _context.Users
                .FirstOrDefaultAsync(u => u.LoginId == dto.Email);

            if (existingEmployee != null)
            {
                return null;
            }

            Role? employeeRole = await _context.Roles
                .FirstOrDefaultAsync(Role => Role.RoleName == "EMPLOYEE");

            if (employeeRole == null)
            {
                return null;
            }

            User employee = new User
                {
                    LoginId = dto.Email,
                    Password = "Pass@1234",
                    RoleId = employeeRole.RoleId,
                    Name = dto.Name,
                    Designation = dto.Designation
                };

            _context.Users.Add(employee);
            await _context.SaveChangesAsync();

            return new EmployeeDto
            {
                UserId = employee.UserId,
                LoginId = employee.LoginId,
                Name = employee.Name,
                Designation = employee.Designation
            };
        }

        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            List<User> employees = await _context.Users
                .Where(u => u.Role != null && u.Role.RoleName == "EMPLOYEE")
                .ToListAsync();

            return employees
                .Select(employee => new EmployeeDto
                {
                    UserId = employee.UserId,
                    LoginId = employee.LoginId,
                    Name = employee.Name,
                    Designation = employee.Designation
                })
                .ToList();
        }

        public async Task<EmployeeDto?> GetEmployeeAsync(string loginId)
        {
            User? employee = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.LoginId == loginId &&
                    u.Role != null &&
                    u.Role.RoleName == "EMPLOYEE");

            if (employee == null)
            {
                return null;
            }

            return new EmployeeDto
            {
                UserId = employee.UserId,
                LoginId = employee.LoginId,
                Name = employee.Name,
                Designation = employee.Designation
            };
        }
    }
}