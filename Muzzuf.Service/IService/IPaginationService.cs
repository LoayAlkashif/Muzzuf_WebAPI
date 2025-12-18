using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.Service.Helpers;

namespace Muzzuf.Service.IService
{
    public interface IPaginationService
    {
        Task<PagedResult<T>> PaginateAsync<T>(
        IQueryable<T> query,
        int page,
        int limit = 10);
    }
}
