using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Muzzuf.DataAccess.Entites;
using Muzzuf.DataAccess.IRepository;
using Muzzuf.DataAccess.Repository;
using Muzzuf.Service.CustomError;
using Muzzuf.Service.DTO.AuthDTO;
using Muzzuf.Service.IService;

namespace Muzzuf.Service.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _config;

        public AuthService(UserManager<ApplicationUser> userManager, IUserRepository userRepository
            , IEmailService emailService
            ,IJwtService jwtService
            ,IConfiguration config)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _emailService = emailService;
            _jwtService = jwtService;
            _config = config;
        }

        public async Task Register(RegisterDto dto)
        {
            if (dto.Password != dto.ConfirmPassword) throw new BadRequestException("Passwords not match");

            if (await _userRepository.GetByEmailAsync(dto.Email) != null)
                throw new ConflictException("Email already Exist");

            if(!string.IsNullOrEmpty(dto.NationalId))
            {
                var nationalIdExist = _userManager.Users.Any(u => u.NationalId == dto.NationalId);
                if (nationalIdExist)
                    throw new ConflictException("National ID is already Exist");
            }

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email,
                NationalId = dto.NationalId,
                Region = dto.Region,
                City = dto.City,
                Bio = dto.Bio,
                Level = dto.Level ?? null,
                ProgrammingLanguages = dto.ProgrammingLanguages,
                CompanyName = dto.CompanyName ?? null,
                CompanyDescription = dto.CompanyDescription ?? null,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.First().Description);


            await _userManager.AddToRoleAsync(user, dto.UserType.ToString());

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = Uri.EscapeDataString(token);


            var verificationLink = $"{_config["BASEURL"]}/api/auth/confirm-email?userId={user.Id}&token={token}";

            await _emailService.SendAsync(user.Email,
                "Verifiy your Email",
                $@"
                    <h2> Welcome {user.FullName} </h2>
                    <p> Please Verifiy your email by clicking the link below: </p>
                    <a href='{verificationLink}'> Verify Email </a>
                "
                );

        }



        public async Task<string> Login(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
                throw new NotAuthorizeException("Invalid Email or Password");

            bool UserFound = await _userManager.CheckPasswordAsync(user, dto.Password);

            if(!UserFound)
                throw new NotAuthorizeException("Invalid Email or password");

            if (!user.Verified)
                throw new NotAuthorizeException("Email is not Verified please verified your Email First");

            var token = await _jwtService.GenerateToken(user);

            return token;
        }


        public async Task ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("User not found");

            token = Uri.UnescapeDataString(token);

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                throw new BadRequestException("Invaild or Expired verification toke");

            user.Verified = true;
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
        }
        
    }
}
