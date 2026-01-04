using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Muzzuf.DataAccess.Entites;
using Muzzuf.Service.DTO.UserDTO;
using Muzzuf.Service.IService;

namespace Muzzuf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }
      

        [HttpPost("profile-image")]
        [Authorize]
        [RequestSizeLimit(5_242_880)] // 5MB limit
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {

            var user = await _userManager.GetUserAsync(User);
            await _userService.UploadProfileImageAsync(user.Id, file);

            return Ok(new { profileImage =  user.ProfileImageUrl });

        }

        [HttpPost("company-logo")]
        [Authorize(Roles ="Employer")]
        [RequestSizeLimit(5_242_880)] // 5MB limit
        public async Task<IActionResult> UploadCompanyLogo(IFormFile file)
        {
            var user = await _userManager.GetUserAsync(User);
            await _userService.UploadCompanyLogoImageAsync(user.Id, file);

            return Ok(new { logoUrl = user.CompanyLogoUrl });
        }


        [HttpPost("cv")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UploadCvFile(IFormFile file)
        {
            var user = await _userManager.GetUserAsync(User);

            await _userService.UploadCVAsync(user.Id, file);

            return Ok(new { CvUrl = user.CVUrl });
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            var userProfile = await _userService.GetProfileAsync(user.Id);

            return Ok(new {userProfile});
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfile(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userProfile = await _userService.GetUserProfileAsync(id, currentUser.Id);

            return Ok(new {userProfile});
        }



        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] int page = 1, [FromQuery] int limit = 5)
        {
            var result = await _userService.SearchAsync(query, page, limit);
            return Ok(result);
        }

        [HttpPut("update-user/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(string id, UpdateUserDto dto)
        {
             await _userService.UpdateProfileAsync(id, dto);
            return Ok("Updated Successfully");
        }

    }
}
