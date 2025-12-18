using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.DataAccess.Entites;

namespace Muzzuf.DataAccess.IRepository
{
    public interface IApplicationAnswerRepository : IGenericRepository<ApplicationAnswer>
    {

        Task<IEnumerable<ApplicationAnswer>> GetByApplicationIdAsync(int applicationId);
    }
}
