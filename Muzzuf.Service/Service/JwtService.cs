using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Muzzuf.DataAccess.Entites;
using Muzzuf.Service.IService;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.Xml;

namespace Muzzuf.Service.Service
{
    public class JwtService : IJwtService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public JwtService(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        public async Task <string> GenerateToken(ApplicationUser user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var SigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecurityKey"]));

            var Signature = new SigningCredentials(SigninKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["BASEURL"],
                        audience: _config["JWT:AudienceIp"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddHours(5),
                        signingCredentials: Signature
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
