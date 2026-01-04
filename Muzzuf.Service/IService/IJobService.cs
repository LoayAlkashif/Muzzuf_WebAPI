using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.Service.DTO.JobDTO;
using Muzzuf.Service.Helpers;

namespace Muzzuf.Service.IService
{
    public interface IJobService
    {
        Task<PagedResult<JobDto>> GetActiveJobsAsync(string query, int page, int limit);

        Task<JobDto> GetJobByIdAsyc(int jobId, string currentUserId);

        Task<JobDto> CreateJobAsync(string employerId, CreateUpdateJobDto dto);

        Task<JobDto> UpdateJobAsync(int jobId, string employerId, CreateUpdateJobDto dto);

        Task DeleteJobAsync(int jobId, string employerId);

        Task<PagedResult<JobDto>> GetEmployerJobsAsync (string employerId,string query, int page, int limit);

        Task DeActiveJobAsync(int jobId, string employerId);



    }
}
