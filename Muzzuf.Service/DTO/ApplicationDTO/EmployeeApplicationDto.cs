using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.DataAccess.Enums;

namespace Muzzuf.Service.DTO.ApplicationDTO
{
    public class EmployeeApplicationDto
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string JobCity { get; set; }
        public string JobRegion { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime AppliedAt { get; set; }
        public List<ApplicationAnswerDetailsDto> Answers { get; set; } = new();

    }
}
