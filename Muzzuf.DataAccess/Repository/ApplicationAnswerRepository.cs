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
    public class ApplicationAnswerRepository : GenericRepository<ApplicationAnswer>, IApplicationAnswerRepository
    {

        public ApplicationAnswerRepository(MuzzufContext context) : base(context) { }

        public async Task<IEnumerable<ApplicationAnswer>> GetByApplicationIdAsync(int applicationId)
        {
            return await _context.ApplicationAnswers.Where(a =>  a.ApplicationId == applicationId)
                .Include(q => q.Question)
                .ToListAsync();
        }


    }
}
