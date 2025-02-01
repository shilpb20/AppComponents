using AppComponents.CoreLib.Repository;
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

        public virtual async Task<T?> AddAsync(T entity)
        {
            if (entity != null)
            {
                await _dataSet.AddAsync(entity);
                await SaveChangesAsync();
            }

            return entity;
        }

        public virtual async Task<T?> UpdateAsync(T entity)
        {
            if (entity != null)
            {
                 _dataSet.Update(entity);
                await SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException(RepositoryConstants.NullUpdate);
            }


            return entity;
        }

        public async Task<T?> DeleteAsync(T entity)
        {
            if(entity != null)
            {
                _dataSet.Remove(entity);
                await SaveChangesAsync();
            }

            return entity;
        }

        public virtual async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, bool asNoTracking = false)
        {
            IQueryable<T>? query = GetQueryableDataset(asNoTracking);
            if (filter != null)
            {
                query = query?.Where(filter);
            }

            return query;
        }

        public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool asNoTracking = false)
        {
            IQueryable<T>? query = GetQueryableDataset(asNoTracking);
            return await query.Where(filter).FirstOrDefaultAsync();
        }

        private IQueryable<T>? GetQueryableDataset(bool asNoTracking)
        {
            IQueryable<T>? query = _dataSet.AsQueryable();

            if (asNoTracking)
            {
                query = query?.AsNoTracking();
            }

            return query;
        }
    }
}
