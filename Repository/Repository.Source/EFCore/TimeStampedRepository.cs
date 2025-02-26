using AppComponents.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppComponents.Repository.EFCore
{
    public class TimeStampedRepository<T, TContext> : Repository<T, TContext>
        where T : TimeStampedBaseEntity
        where TContext : DbContext
    {

        public TimeStampedRepository(TContext dbContext, ILogger<Repository<T, TContext>> logger)
            : base(dbContext, logger)
        {

        }

        public override Task<T?> UpdateAsync(T entity)
        {
            entity.MarkModified();
            return base.UpdateAsync(entity);
        }
    }
}
