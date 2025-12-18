using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.DataAccess.Entites;

namespace Muzzuf.Service.IService
{
    public interface IJwtService
    {
        Task<string> GenerateToken (ApplicationUser user);
    }
}
