using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Muzzuf.DataAccess.Entites;
using Muzzuf.Service.CustomError;
using Muzzuf.Service.DTO.UserDTO;
using Muzzuf.Service.Helpers;
using Muzzuf.Service.IService;

namespace Muzzuf.Service.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileService _fileService;
        private readonly IPaginationService _paginationService;

        public UserService(UserManager<ApplicationUser> userManager, IFileService fileService,
            IPaginationService paginationService
            )
        {
            _userManager = userManager;
            _fileService = fileService;
            _paginationService = paginationService;
        }


        private bool IsVaildImageFile(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }
        private bool IsVaildPdfFile(IFormFile file)
        {
            var allowedExtensions = new[] { ".pdf" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }


        public async Task<UserDTO> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync( userId ) ?? 
                throw new NotFoundException("User not Found");
            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Bio = user.Bio,
                Region = user.Region,
                City = user.City,
                ProfileImageUrl = user.ProfileImageUrl ?? null,
                ProgrammingLanguages = user.ProgrammingLanguages ?? null,
                Views = user.Views,
                CvUrl = user.CVUrl ?? null,

                CompanyName = user.CompanyName ?? null,
                CompanyDescription = user.CompanyDescription ?? null,
                CompanyLogoUrl = user.CompanyLogoUrl ?? null

            };
        }

        public async Task<UserDTO> GetUserProfileAsync(string profileUserId, string currentUserId)
        {
            var user = await _userManager.FindByIdAsync(profileUserId) ??
                throw new NotFoundException("User not Found");

            if(profileUserId != currentUserId)
            {
                user.Views++;
                await _userManager.UpdateAsync(user);
            }

            return new UserDTO
            {
                FullName = user.FullName,
                Email = user.Email,
                Bio = user.Bio,
                City = user.City,
                Region = user.Region,
                ProfileImageUrl = user.ProfileImageUrl ?? null,
                ProgrammingLanguages = user.ProgrammingLanguages ?? null,
                CvUrl = user.CVUrl ?? null,

                CompanyName = user.CompanyName ?? null,
                CompanyDescription = user.CompanyDescription ?? null,
                CompanyLogoUrl = user.CompanyLogoUrl ?? null

            };

        }


        public async Task<PagedResult<UserSearchDto>> SearchAsync(string query, int page, int limit)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new PagedResult<UserSearchDto>
                {
                    Data = new List<UserSearchDto>(),
                    CurrentPage = page,
                    TotalPages = 0,
                    TotalCount = 0
                };
            }

            var usersQuery = _userManager.Users
                .Where(u =>
                    u.EmailConfirmed &&
                    u.FullName.Contains(query))
                .OrderBy(u => u.FullName)
                .Select(u => new UserSearchDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    ProfileImageUrl = u.ProfileImageUrl,
                    Bio = u.Bio
                });

            return await _paginationService.PaginateAsync(usersQuery, page, limit);
        }

        public async Task UpdateProfileAsync(string userId, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId) ??
                throw new NotFoundException("User not Found");

            user.FullName = dto.FullName?.Trim() ?? user.FullName;
            user.Region = dto.Region ?? user.Region;
            user.City = dto.City ?? user.City;
            user.Bio = dto.Bio ?? user.Bio;

            user.CompanyName = dto.CompanyName?.Trim() ?? user.CompanyName;
            user.CompanyDescription = dto.CompanyDescription ?? user.CompanyDescription;

            if (dto.ProgrammingLanguages != null)
                user.ProgrammingLanguages = dto.ProgrammingLanguages;

            await _userManager.UpdateAsync(user);
        }

        public async Task UploadCompanyLogoImageAsync(string userId, IFormFile file)
        {
            if (!IsVaildImageFile(file))
                throw new BadRequestException("Invalid image format");

            var user = await _userManager.FindByIdAsync(userId) ??
               throw new NotFoundException("User not Found");

            if (file == null)
                throw new BadRequestException("Image is Required");

            if (user.CompanyLogoUrl is not null)
                _fileService.Delete(user.CompanyLogoUrl);

            user.CompanyLogoUrl = await _fileService.UploadAsync(file, "Company-Logo");
            await _userManager.UpdateAsync(user);
        }

        public async Task UploadCVAsync(string userId, IFormFile file)
        {

            if (!IsVaildPdfFile(file))
                throw new BadRequestException("Invlaid File Format extension must be .pdf");

            var user = await _userManager.FindByIdAsync(userId) ??
               throw new NotFoundException("User not Found");

            if (file == null)
                throw new BadRequestException("File is Required");

            if (user.CVUrl is not null)
                _fileService.Delete(user.CVUrl);

            user.CVUrl = await _fileService.UploadAsync(file, "CVs");
            await _userManager.UpdateAsync(user);
        }

        public async Task UploadProfileImageAsync(string userId, IFormFile file)
        {
            
            if (!IsVaildImageFile(file))
                throw new BadRequestException("Invalid image format");
            var user = await _userManager.FindByIdAsync(userId) ?? 
                throw new NotFoundException("User not Found");

            if (file == null)
                throw new BadRequestException("Image is Required");

            if (user.ProfileImageUrl is not null)
                _fileService.Delete(user.ProfileImageUrl);


            user.ProfileImageUrl = await _fileService.UploadAsync(file, "Profiles-Images");

            await _userManager.UpdateAsync(user);

        }
    }
}
