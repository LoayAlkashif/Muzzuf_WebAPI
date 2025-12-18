using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Muzzuf.Service.DTO.UserDTO;
using Muzzuf.Service.Helpers;

namespace Muzzuf.Service.IService
{
    public interface IUserService
    {

        Task<UserDTO> GetProfileAsync(string userId);
        Task UpdateProfileAsync(string userId, UpdateUserDto dto);

        Task UploadProfileImageAsync(string userId, IFormFile file);
        Task UploadCompanyLogoImageAsync(string userId, IFormFile file);
        Task UploadCVAsync (string userId, IFormFile file);

        Task<UserDTO> GetUserProfileAsync(string profileUserId, string currentUserId);

        Task<PagedResult<UserSearchDto>> SearchAsync(string query, int page, int limit);
    }
}
