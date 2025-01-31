using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppComponents.CoreLib
{
    public class Repository<T> : IRepository<T> where T : class
    {                                                             
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dataSet;

        public Repository(DbContext dbContext) 
        { 
            _dbContext = dbContext;
            _dataSet = _dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>>? GetAllAsync(Expression<Func<T, bool>>? filter = null, bool asNoTracking = false)
        {
            IQueryable<T>? query = _dataSet;

            _dbContext.ChangeTracker.QueryTrackingBehavior = asNoTracking
                  ? QueryTrackingBehavior.NoTracking
                  : QueryTrackingBehavior.TrackAll;

            if (filter != null)
            {
                query =  query?.Where(filter);
            }

           return await query?.ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _dataSet.Where(filter).FirstOrDefaultAsync();
        }
    }
}
