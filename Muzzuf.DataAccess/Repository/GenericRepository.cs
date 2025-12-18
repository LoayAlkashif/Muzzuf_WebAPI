using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Muzzuf.DataAccess.Context;
using Muzzuf.DataAccess.IRepository;

namespace Muzzuf.DataAccess.Repository
{

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly MuzzufContext _context;
        protected readonly DbSet<T> _db;

        public GenericRepository(MuzzufContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }
      

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _db.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _db.FindAsync(id);
        }


        public async Task AddAsync(T entity)
        {
            await _db.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _db.Update(entity);
        }

        public void Delete (T entity)
        {
             _db.Remove(entity);
        }


        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync(); 
        }

       
    }
}
