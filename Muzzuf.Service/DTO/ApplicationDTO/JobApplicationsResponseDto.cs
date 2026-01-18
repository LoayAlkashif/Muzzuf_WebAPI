using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.Service.Helpers;

namespace Muzzuf.Service.DTO.ApplicationDTO
{
    public class JobApplicationsResponseDto
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public PagedResult<ApplicationListDto> Applications { get; set; }
    }
}
