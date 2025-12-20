using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.DataAccess.Enums;

namespace Muzzuf.Service.DTO.JobDTO
{
    public class CreateUpdateJobDto
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public JobLevel Level { get; set; }
        public List<string> RequiredLanguage {  get; set; }
        public List<JobQuestionDto> Questions { get; set; } = new();
    }
}
