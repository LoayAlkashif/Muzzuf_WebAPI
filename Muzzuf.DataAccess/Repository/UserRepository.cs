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
    public class UserRepository : IUserRepository
    {
        public MuzzufContext _Context { get; }

        public UserRepository(MuzzufContext MuzzufContext)
        {
            _Context = MuzzufContext;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _Context.Users.ToListAsync();
        }

        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
           return await _Context.Users
                .Include(u => u.JobsAdded)
                .Include(u => u.Applications)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            return await _Context.Users.FirstOrDefaultAsync(u => u.Email == email);

        }

        public async Task AddAsync(ApplicationUser entity)
        {
            await _Context.Users.AddAsync(entity);
        }

        public void Delete(ApplicationUser entity)
        {
            _Context.Users.Remove(entity);
        }

        public void Update(ApplicationUser entity)
        {
            _Context.Users.Update(entity);
        }


        public async Task SaveAsync()
        {
            await _Context.SaveChangesAsync();
        }



    }
}
