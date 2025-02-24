using AppComponents.CoreLib.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppComponents.CoreLib.Repository.EFCore
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

        #region data update

        public virtual async Task<T?> AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dataSet.AddAsync(entity);
            await SaveChangesAsync();

            return entity;
        }

        public virtual async Task<T?> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dataSet.Update(entity);
            await SaveChangesAsync();

            return entity;
        }

        public async Task<T?> DeleteAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dataSet.Remove(entity);
            await SaveChangesAsync();

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
                if (property == null) throw new ArgumentException($"Property {order.Key} not found on type {entityType.Name}");

                var parameter = Expression.Parameter(entityType, "x");
                var propertyAccess = Expression.Property(parameter, property);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);

                string methodName;

                if (orderedQuery == null)
                {
                    methodName = order.Value ? "OrderBy" : "OrderByDescending";
                    orderedQuery = (IOrderedQueryable<T>)typeof(Queryable).GetMethods()
                        .First(method => method.Name == methodName && method.GetParameters().Length == 2)
                        .MakeGenericMethod(entityType, property.PropertyType)
                        .Invoke(null, new object[] { query, orderByExpression })!;
                }
                else
                {
                    methodName = order.Value ? "ThenBy" : "ThenByDescending";
                    orderedQuery = (IOrderedQueryable<T>)typeof(Queryable).GetMethods()
                        .First(method => method.Name == methodName && method.GetParameters().Length == 2)
                        .MakeGenericMethod(entityType, property.PropertyType)
                        .Invoke(null, new object[] { orderedQuery, orderByExpression })!;
                }
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