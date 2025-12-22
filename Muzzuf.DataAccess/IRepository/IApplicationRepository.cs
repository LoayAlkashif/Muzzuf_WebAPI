using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.DataAccess.Entites;

namespace Muzzuf.DataAccess.IRepository
{
    public interface IApplicationRepository : IGenericRepository<Application>
    {
        Task<Application?> GetApplicationWithAnswersAsync(int applicationId);

        Task<bool> HasUserAppliedAsync(string employeeId, int jobId);

        IQueryable<Application> GetJobApplicationsQueryable(int jobId);
    }
}
