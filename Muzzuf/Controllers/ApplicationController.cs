using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Muzzuf.Service.DTO.ApplicationDTO;
using Muzzuf.Service.IService;

namespace Muzzuf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        
        [HttpPost("apply-job")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ApplyJob([FromForm] ApplyJobDto dto)
        {
            await _applicationService.ApplyJobAsync(GetUserId(), dto);
            return Ok(new { message = "Applied Successfuly" });
        }


        [HttpGet("job/{jobId}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetJobApplications(int jobId, [FromQuery] string? query, int page = 1, int limit = 10)
        {
            var result = await _applicationService.GetJobApplicationsAsync(jobId, GetUserId(), query, page, limit);

            return Ok(result);
        }


        [HttpGet("application-detials/{applicationId}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetApplicationDetails(int applicationId)
        {
            var result = await _applicationService.GetApplicationDetailsAsync(applicationId, GetUserId());
            return Ok(result);
        }


        [HttpPut("{applicationId}/accept")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> AcceptApplicatio(int applicationId)
        {
            await _applicationService.AcceptApplicationAsync(applicationId, GetUserId());
            return Ok(new {message = "Application Accepted"});
        }


        [HttpPut("{applicationId}/reject")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> RejectApplicatio(int applicationId)
        {
            await _applicationService.RejectApplicationAsync(applicationId, GetUserId());
            return Ok(new { message = "Application Rejected" });
        }

        [HttpGet("my-applications")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetMyApplications(int page =1, int limit = 8)
        {
            var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (employeeId == null) return Unauthorized();

            var applications = await _applicationService
                .GetEmployeeApplicationsAsync(employeeId,page, limit);

            return Ok(applications);
        }
    }
}
