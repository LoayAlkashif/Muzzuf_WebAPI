using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzzuf.Service.DTO.ApplicationDTO
{
    public class ApplyJobDto
    {
        public int JobId { get; set; }

        public List<ApplicationAnswerDto> Answers { get; set; } = new();
    }
}
