using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.DataAccess.Enums;

namespace Muzzuf.Service.DTO.AuthDTO
{
    public class RegisterDto
    {
        public string FullName { get; set; }
        //public string NationalId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Bio { get; set; }
        // Employee
        public List<string>? ProgrammingLanguages { get; set; }
        public UserLevel? Level { get; set; }

        //Employer

        public string? CompanyName { get; set; }
        public string? CompanyDescription { get; set; }


        public UserRole UserType { get; set; }

    }
}
