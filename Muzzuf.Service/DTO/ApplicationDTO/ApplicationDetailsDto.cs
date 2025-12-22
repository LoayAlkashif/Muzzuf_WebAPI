using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.DataAccess.Enums;

namespace Muzzuf.Service.DTO.ApplicationDTO
{
    public class ApplicationDetailsDto
    {

        public int ApplicationId { get; set; }

        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string JobTitle { get; set; }

        public ApplicationStatus Status { get; set; }

        public List<ApplicationAnswerDetailsDto> Answers { get; set; }
    }
}
