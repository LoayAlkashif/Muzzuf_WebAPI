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
    public class ApplicationRepository : GenericRepository<Application> , IApplicationRepository
    {
        public ApplicationRepository(MuzzufContext context) : base(context) { }

        public async Task<Application?> GetApplicationWithAnswersAsync(int applicationId)
        {
            return await _context.Applications.Include(a => a.Answers)
                .ThenInclude(ans => ans.Question)
                .FirstOrDefaultAsync(a => a.Id == applicationId);
        }

        public async Task<bool> HasUserAppliedAsync(string employeeId, int jobId)
        {
            return await _context.Applications.AnyAsync(a => a.EmployeeId == employeeId && a.JobId == jobId);
        }
    }
}
