using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Muzzuf.Service.Helpers;
using Muzzuf.Service.IService;

namespace Muzzuf.Service.Service
{
    public class PaginationService : IPaginationService
    {
        public async Task<PagedResult<T>> PaginateAsync<T>(
        IQueryable<T> query,
        int page,
        int limit = 10)
        {
            if (page <= 0) page = 1;

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)limit);

            var data = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return new PagedResult<T>
            {
                Data = data,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalCount = totalCount
            };
        }
    }
}
