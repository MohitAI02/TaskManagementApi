using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs.Employees;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(
            CreateEmployeeDto dto)
        {
            EmployeeDto? employee = await _employeeService
                .CreateEmployeeAsync(dto);

            if (employee == null)
            {
                return BadRequest(new
                {
                    message = "Employee already exists or EMPLOYEE role was not found."
                });
            }

            return CreatedAtAction(
                nameof(GetEmployee),
                new { loginId = employee.LoginId },
                employee);
        }

        [HttpGet]
        public async Task<ActionResult<List<EmployeeDto>>> GetAllEmployees()
        {
            List<EmployeeDto> employees = await _employeeService
                .GetAllEmployeesAsync();

            return Ok(employees);
        }

        [HttpGet("{loginId}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(
            string loginId)
        {
            EmployeeDto? employee = await _employeeService
                .GetEmployeeAsync(loginId);

            if (employee == null)
            {
                return NotFound(new
                {
                    message = "Employee not found."
                });
            }

            return Ok(employee);
        }
    }
}