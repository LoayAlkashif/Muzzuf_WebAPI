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
        
        public IQueryable<Job> GetActiveJobsAsync()
        {
            return _context.Jobs.Where(j => j.IsActive == true)
                .Include(j => j.AddedBy).Include(q => q.Questions);
        }

        public Task<Job?> GetByIdWithQuestionsAsync(int id)
        {
            return _context.Jobs.Include(j => j.AddedBy)
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        public IQueryable<Job> GetJobsByEmployerAsync(string employerId)
        {
            return _context.Jobs.Where(j => j.AddedById == employerId)
                .Include(q => q.Questions);
        }
    }
}
