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

        // ============================================================
        // CREATE EMPLOYEE
        // POST: api/Employees
        // ============================================================

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(
            CreateEmployeeDto dto)
        {
            var employee = await _employeeService
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
                new { loginId = employee.loginId },
                employee);
        }


        // ============================================================
        // GET ALL EMPLOYEES
        // GET: api/Employees
        // ============================================================

        [HttpGet]
        public async Task<ActionResult<List<EmployeeDto>>> GetAllEmployees()
        {
            var employees = await _employeeService
                .GetAllEmployeesAsync();

            return Ok(employees);
        }


        // ============================================================
        // GET EMPLOYEE BY LOGIN ID
        // GET: api/Employees/{loginId}
        // ============================================================

        [HttpGet("{loginId}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(
            string loginId)
        {
            var employee = await _employeeService
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