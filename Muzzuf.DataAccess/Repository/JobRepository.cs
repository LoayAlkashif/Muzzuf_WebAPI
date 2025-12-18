using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Muzzuf.DataAccess.Context;
using Muzzuf.DataAccess.Entites;
using Muzzuf.DataAccess.IRepository;

namespace Muzzuf.DataAccess.Repository
{
    public class JobRepository : GenericRepository<Job> , IJobRepository
    {
        public JobRepository(MuzzufContext context) : base (context) {  }
        
        public async Task<IEnumerable<Job>> GetActiveJobsAsync()
        {
            return await _context.Jobs.Where(j => j.IsActive)
                .Include(j => j.AddedBy)
                .ToListAsync();
        }

        public async Task<IEnumerable<Job>> GetJobsByEmployerAsync(string employerId)
        {
            return await _context.Jobs.Where(j => j.AddedById == employerId)
                .Include(j => j.Questions)
                .ToListAsync();
        }
    }
}
