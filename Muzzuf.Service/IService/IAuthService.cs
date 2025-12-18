using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.Service.DTO.AuthDTO;

namespace Muzzuf.Service.IService
{
    public interface IAuthService
    {
        Task Register(RegisterDto dto);
        Task<string> Login(LoginDto dto);
        Task ConfirmEmailAsync(string userId, string token);

    }
}
