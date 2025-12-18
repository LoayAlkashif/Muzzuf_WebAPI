using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.DataAccess.Entites;

namespace Muzzuf.DataAccess.IRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser?> GetByIdAsync(string id);
        Task<ApplicationUser?> GetByEmailAsync(string email);

        Task AddAsync(ApplicationUser entity);

        void Update(ApplicationUser entity);

        void Delete(ApplicationUser entity);

        Task SaveAsync();
    }
}
