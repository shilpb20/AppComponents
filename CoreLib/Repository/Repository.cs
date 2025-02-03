using AppComponents.CoreLib.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppComponents.CoreLib
{
    /// <summary>
    /// A generic repository for performing basic CRUD operations on entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dataSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="DbContext"/> used for accessing the database.</param>
        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dataSet = _dbContext.Set<T>();
        }

        #region data update

        /// <summary>
        /// Adds the specified entity to the data store asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>
        /// The added entity if successful; otherwise, <c>null</c>.
        /// </returns>
        public virtual async Task<T?> AddAsync(T entity)
        {
            if (entity != null)
            {
                await _dataSet.AddAsync(entity);
                await SaveChangesAsync();
            }

            return entity;
        }

        /// <summary>
        /// Updates the specified entity in the data store asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>
        /// The updated entity if successful; otherwise, <c>null</c>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the provided <paramref name="entity"/> is <c>null</c>.
        /// </exception>
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

        /// <summary>
        /// Deletes the specified entity from the data store asynchronously.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>
        /// The deleted entity if successful; otherwise, <c>null</c>.
        /// </returns>
        public async Task<T?> DeleteAsync(T entity)
        {
            if (entity != null)
            {
                _dataSet.Remove(entity);
                await SaveChangesAsync();
            }

            return entity;
        }

        /// <summary>
        /// Persists all changes made in this context to the underlying database asynchronously.
        /// </summary>
        public virtual async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region data read

        /// <summary>
        /// Gets an <see cref="IQueryable{T}"/> representing all entities optionally filtered by the provided expression.
        /// This method allows for further query composition before execution.
        /// </summary>
        /// <param name="filter">
        /// An optional expression to filter the entities.
        /// If <c>null</c>, all entities are returned.
        /// </param>
        /// <param name="asNoTracking">
        /// A value indicating whether the returned entities should be tracked by the context.
        /// When set to <c>true</c>, no tracking is applied.
        /// </param>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> of entities.
        /// </returns>
        public virtual async Task<IQueryable<T>> GetAll(
            Expression<Func<T, bool>>? filter = null,
            bool asNoTracking = false,
            int? pageIndex = null,
            int? pageSize = null,
            Dictionary<string, bool>? orderByClause = null)
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

            if (pageIndex.HasValue && (pageIndex.Value < 0 || pageSize ==null) || 
                pageSize.HasValue &&  (pageIndex == null || pageSize.Value < 0))
            {

                return query.Skip(await query.CountAsync());
            }

            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int pointer = (pageIndex.Value - 1) * pageSize.Value;
                query = query.Skip(pointer).Take(pageSize.Value);
            }

            return query;
        }

        private static IQueryable<T> ApplyOrdering<T>(IQueryable<T> query, Dictionary<string, bool> orderByClause)
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



        /// <summary>
        /// Gets a materialized list of entities asynchronously.
        /// This method executes the query immediately, returning a list of results.
        /// </summary>
        /// <param name="filter">
        /// An optional expression to filter the entities.
        /// If <c>null</c>, all entities are returned.
        /// </param>
        /// <param name="asNoTracking">
        /// A value indicating whether the returned entities should be tracked by the context.
        /// When set to <c>true</c>, no tracking is applied.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of entities.
        /// </returns>
        public virtual async Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null, 
            bool asNoTracking = false, 
            int? pageIndex = null, 
            int? pageSize = null,
            Dictionary<string, bool>? orderByClause = null)
            {
                // Directly await the result of GetAll instead of double-awaiting
                IQueryable<T> query = await GetAll(filter, asNoTracking, pageIndex, pageSize, orderByClause);
                return await query.ToListAsync();
            }


        /// <summary>
        /// Gets a single entity matching the specified filter asynchronously.
        /// </summary>
        /// <param name="filter">An expression to filter the entities.</param>
        /// <param name="asNoTracking">
        /// A value indicating whether the returned entity should be tracked by the context.
        /// When set to <c>true</c>, no tracking is applied.
        /// </param>
        /// <returns>
        /// The first entity matching the filter; otherwise, <c>null</c> if no such entity exists.
        /// </returns>
        public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool asNoTracking = false)
        {
            IQueryable<T> query = GetQueryableDataset(asNoTracking);
            return await query.Where(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets a queryable dataset of entities with an option for disabling change tracking.
        /// </summary>
        /// <param name="asNoTracking">
        /// A value indicating whether the returned dataset should be tracked by the context.
        /// When set to <c>true</c>, no tracking is applied.
        /// </param>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> for the dataset.
        /// </returns>
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
