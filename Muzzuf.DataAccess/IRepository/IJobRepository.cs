using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.DataAccess.Entites;

namespace Muzzuf.DataAccess.IRepository
{
    public interface IJobRepository : IGenericRepository<Job>
    {
        public IQueryable<Job> GetJobsByEmployerAsync(string employerId);

        IQueryable<Job> GetActiveJobsAsync();

        Task<Job?> GetByIdWithQuestionsAsync(int id);
    }
}
