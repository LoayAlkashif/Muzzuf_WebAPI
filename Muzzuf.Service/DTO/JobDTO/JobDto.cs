using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.DataAccess.Enums;

namespace Muzzuf.Service.DTO.JobDTO
{
    public class JobDto
    {
        public string Id { get; set; }
        public string EmployerName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Region { get; set; }

        public JobLevel Level { get; set; }

        public List<string> RequiredLanguage { get; set; }

        public bool IsActive { get; set; }

        public List<JobQuestionDto>? Questions { get; set; }
    }
}
