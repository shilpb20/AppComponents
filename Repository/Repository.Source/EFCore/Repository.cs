using AppComponents.CoreLib.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AppComponents.CoreLib.Repository.EFCore
{
    public class Repository<T, TContext> : IRepository<T, TContext> 
        where T : class
        where TContext : DbContext
    {
        private readonly TContext _dbContext;
        private readonly DbSet<T> _dataSet;
        private readonly ILogger<Repository<T, TContext>> _logger;

        public Repository(TContext dbContext, ILogger<Repository<T, TContext>> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _dataSet = _dbContext.Set<T>();
        }


        #region data update

        public virtual async Task<T?> AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                _logger.LogInformation("Adding entity of type {EntityType} with ID {EntityId}",
                    typeof(T).Name, entity?.GetType().GetProperty("Id")?.GetValue(entity) ?? "Unknown");

                await _dataSet.AddAsync(entity);
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding entity");
                throw;
            }

            return entity;
        }

        public virtual async Task<T?> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                _logger.LogInformation($"Updating entity of type {typeof(T).Name}");
                _dataSet.Update(entity);
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entity");
                throw;
            }

            return entity;
        }

        public async Task<T?> DeleteAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                _logger.LogInformation($"Deleting entity of type {typeof(T).Name}");
                _dataSet.Remove(entity);
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity");
                throw;
            }

            return entity;
        }

        public virtual async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region data read

        public virtual async Task<IQueryable<T>> GetAll(
            Expression<Func<T, bool>>? filter = null,
            bool asNoTracking = false,
            Dictionary<string, bool>? orderByClause = null,
            Pagination? pagination = null)
        {
            IQueryable<T> query = GetQueryableDataset(asNoTracking);
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderByClause != null)
            {
                query = ApplyOrdering(query, orderByClause);
            }

            return pagination == null ? query : await pagination.GetPagedResult(query);
        }

        private static IQueryable<T> ApplyOrdering<T>(
            IQueryable<T> query,
            Dictionary<string, bool> orderByClause)
        {
            IOrderedQueryable<T>? orderedQuery = null;
            foreach (var order in orderByClause)
            {
                var entityType = typeof(T);
                var property = entityType.GetProperty(order.Key);

                if (property == null)
                {
                    throw new ArgumentException($"Invalid ordering property: {order.Key}");
                }

                var parameter = Expression.Parameter(entityType, "x");
                var propertyAccess = Expression.Property(parameter, property);
                var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(propertyAccess, typeof(object)), parameter);

                orderedQuery = orderedQuery == null
                    ? (order.Value ? query.OrderBy(lambda) : query.OrderByDescending(lambda))
                    : (order.Value ? orderedQuery.ThenBy(lambda) : orderedQuery.ThenByDescending(lambda));
            }

            return orderedQuery ?? query;
        }

        public virtual async Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            bool asNoTracking = false,
            Dictionary<string, bool>? orderByClause = null,
            Pagination? pagination = null)
        {
            IQueryable<T> query = await GetAll(filter, asNoTracking, orderByClause, pagination);
            return await query.ToListAsync();
        }

        public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool asNoTracking = false)
        {
            IQueryable<T> query = GetQueryableDataset(asNoTracking);
            return await query.Where(filter).FirstOrDefaultAsync();
        }

        private IQueryable<T> GetQueryableDataset(bool asNoTracking)
        {
            IQueryable<T> query = _dataSet.AsQueryable();
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        #endregion
    }
}