
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs.Employees;
using TaskManagementApi.DTOs.Reports;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private const string LoginHeader = "X-Login-Id";

        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("employee")]
        public async Task<ActionResult<EmployeeReportDto>> GetEmployeeReport(
            [FromHeader(Name = LoginHeader)] string loginId)
        {
            EmployeeReportDto? report = await _reportService.GetEmployeeReportAsync(loginId);

            if (report == null)
            {
                return NotFound(new { message = "Employee not found." });
            }

            return Ok(report);
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<EmployeeReportDto>>> GetAllReports()
        {
            List<EmployeeReportDto> reports = await _reportService.GetAllEmployeeReportsAsync();

            return Ok(reports);
        }
    }
}
