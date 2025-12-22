using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Muzzuf.DataAccess.Entites;
using Muzzuf.Service.DTO.JobDTO;
using Muzzuf.Service.IService;

namespace Muzzuf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobController(IJobService jobService, UserManager<ApplicationUser> userManager)
        {
            _jobService = jobService;
            _userManager = userManager;
        }


        [HttpPost("create-job")]
        [Authorize(Roles ="Employer")]
        public async Task<IActionResult> CreateJob([FromBody] CreateUpdateJobDto dto)
        {
            var employer = await _userManager.GetUserAsync(User);
           var job = await _jobService.CreateJobAsync(employer.Id, dto);

            return Ok(job);

        }

        [HttpGet("active-jobs")]
        public async Task<IActionResult> ActiveJobs([FromQuery] string? query, [FromQuery] int page =1,  [FromQuery] int limit = 10)
        {
            var result = await _jobService.GetActiveJobsAsync(query, page, limit);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetJobById(int id)
        {
            var job = await _jobService.GetJobByIdAsyc(id);

            return Ok(job);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] CreateUpdateJobDto dto)
        {
            var employer = await _userManager.GetUserAsync(User);
            var job = await _jobService.UpdateJobAsync(id, employer.Id, dto);

            return Ok(job);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var employer = await _userManager.GetUserAsync(User);
            await _jobService.DeleteJobAsync(id, employer.Id);

            return Ok(new { message = "Job Deleted Successfully" });
        }

        [HttpPatch("{id:int}/deactive")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> DeActiveJob(int id)
        {
            var employer = await _userManager.GetUserAsync(User);
            _jobService.DeActiveJobAsync(id, employer.Id);

            return Ok(new { message = "Job Deactivated Successfully" });
        }

        [HttpGet("employer-jobs")]
        [Authorize(Roles ="Employer")]
        public async Task<IActionResult> GetEmployerJobs([FromQuery] string? query, [FromQuery] int page = 1,  [FromQuery] int limit = 10)
        {
            var employer = await _userManager.GetUserAsync(User);
            var EmployerJobs = await _jobService.GetEmployerJobsAsync(employer.Id, query, page, limit);

            return Ok(EmployerJobs);
        }


    }
}
