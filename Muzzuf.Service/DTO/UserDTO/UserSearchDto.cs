using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzzuf.Service.DTO.UserDTO
{
    public class UserSearchDto
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string? ProfileImageUrl { get; set; }
        public string? Bio { get; set; }
    }
}
