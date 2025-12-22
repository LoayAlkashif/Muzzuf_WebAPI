using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Muzzuf.Service.DTO.ApplicationDTO;
using Muzzuf.Service.Helpers;

namespace Muzzuf.Service.IService
{
    public interface IApplicationService
    {
        Task ApplyJobAsync(string employeeId, ApplyJobDto dto);
        Task AcceptApplicationAsync (int applicationId, string employerId);
        Task RejectApplicationAsync (int applicationId, string employerId);

        Task<PagedResult<ApplicationListDto>> GetJobApplicationsAsync(int jobId, string employerId, string query, int page, int limit);

        Task<ApplicationDetailsDto> GetApplicationDetailsAsync(int appId, string employerId);

    }
}
