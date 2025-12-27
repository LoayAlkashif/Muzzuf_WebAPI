using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
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
            if (dto.Password != dto.ConfirmPassword)
                throw new BadRequestException("Passwords not match");

            if (await _userRepository.GetByEmailAsync(dto.Email) != null)
                throw new ConflictException("Email already Exist");

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email,
                Region = dto.Region,
                City = dto.City,
                Bio = dto.Bio,
                Level = dto.Level,
                ProgrammingLanguages = dto.ProgrammingLanguages,
                CompanyName = dto.CompanyName,
                CompanyDescription = dto.CompanyDescription,
                Verified = false
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.First().Description);

            await _userManager.AddToRoleAsync(user, dto.UserType.ToString());

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode( Encoding.UTF8.GetBytes(token));

            var verificationLink =
                $"{_config["JWT:AudienceIp"]}/auth/confirm-email?userId={user.Id}&token={encodedToken}";

            await _emailService.SendAsync(
                user.Email,
                "Verify your Email",
                $@"
                <h2>Welcome {user.FullName}</h2>
                <p>Please verify your email by clicking the link below:</p>
                <a href='{verificationLink}'>Verify Email</a>
                ");

            Console.WriteLine("REGISTER TOKEN:");
            Console.WriteLine(token);
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
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("User not found");

            if (user.EmailConfirmed)
                throw new BadRequestException("Email already verified");

            var decodedToken = Encoding.UTF8.GetString(
                                WebEncoders.Base64UrlDecode(token)
                            );

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
                throw new BadRequestException("Invalid or Expired verification token");

            user.Verified = true;
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
        }

    }
}
