using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzzuf.Service.DTO.UserDTO
{
    public class UpdateUserDto
    {
        public string? FullName { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Bio { get; set; }

        public List<string>? ProgrammingLanguages { get; set; }
    }
}
