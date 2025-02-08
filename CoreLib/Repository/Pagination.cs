using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppComponents.CoreLib.Repository
{
    public class Pagination
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public Pagination(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public async Task<IQueryable<T>> GetPagedResult<T>(IQueryable<T> query)
        {
            if (PageIndex < 0 || PageSize <= 0)
            {
                return query.Skip(await query.CountAsync());
            }

            if(PageIndex == 0)
            {
                return query.Take(PageSize);
            }

            int pointer = (PageIndex - 1) * PageSize;
            return query.Skip(pointer).Take(PageSize);
        }
    }
}
