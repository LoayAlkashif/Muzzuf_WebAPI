using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzzuf.Service.DTO.UserDTO
{
    public class UserDTO
    {
        public string Id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }

        public string? Region { get; set; }
        public string? City { get; set; }
        public string? ProfileImageUrl { get; set; }
        public List<string>? ProgrammingLanguages { get; set; }

        public int Views { get; set; }

        public string? CvUrl { get; set; }


        public string? ComapnyName { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CompanyLogoUrl { get; set; }
    }
}
