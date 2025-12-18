using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Muzzuf.DataAccess.Enums;

namespace Muzzuf.DataAccess.Entites
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? NationalId { get; set; }

        public string? Region { get; set; }
        public string? City  { get; set; }

        public string? Bio {  get; set; }
        public UserLevel? Level { get; set; }

        public List<string>? ProgrammingLanguages { get; set; } = new List<string>();

        public bool Verified { get; set; } = false;
        public int Views { get; set; } = 0;

        //Employee Features
        public string? ProfileImageUrl { get; set; }
        public string? CVUrl { get; set; }

        //Employer Features
        public string? CompanyName { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CompanyLogoUrl { get; set; }

        public ICollection<Job>? JobsAdded { get; set; }
        public ICollection<Application>? Applications { get; set; }
    }
}
