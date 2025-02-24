﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppComponents.CoreLib.Repository.Abstraction
{
    public class Pagination
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public Pagination(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex < 1 ? 1 : pageIndex;
            PageSize = pageSize <= 0 ? 10 : pageSize;
        }

        public async Task<IQueryable<T>> GetPagedResult<T>(IQueryable<T> query)
        {
            return query.Skip((PageIndex - 1) * PageSize).Take(PageSize);
        }
    }
}