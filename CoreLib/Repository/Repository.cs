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

        public async Task<IEnumerable<T>>? GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            var query = _dataSet.AsQueryable();

            if(filter != null)
            {
                query =  query.Where(filter);
            }

            return await query.ToListAsync();
        }
    }
}
