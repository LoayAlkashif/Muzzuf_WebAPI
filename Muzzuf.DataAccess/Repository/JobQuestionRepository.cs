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
    public class JobQuestionRepository : GenericRepository<JobQuestion>, IJobQuestionRepository
    {
        public JobQuestionRepository(MuzzufContext context) : base(context) { }

        public async Task<IEnumerable<JobQuestion>> GetByJobIdAsync(int jobId)
        {
            return await _context.JobQuestions.Where(q => q.JobId == jobId)
                .ToListAsync();
        }
    }
}
