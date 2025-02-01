using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AppComponents.CoreLib.Repository
{
    /// <summary>
    /// Represents a generic repository for performing basic CRUD operations on entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Adds the specified entity to the data store asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added entity if successful; otherwise, <c>null</c>.</returns>
        Task<T?> AddAsync(T entity);

        /// <summary>
        /// Updates the specified entity in the data store asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated entity if successful; otherwise, <c>null</c>.</returns>
        Task<T?> UpdateAsync(T entity);

        /// <summary>
        /// Deletes the specified entity from the data store asynchronously.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the deleted entity if successful; otherwise, <c>null</c>.</returns>
        Task<T?> DeleteAsync(T entity);

        /// <summary>
        /// Persists all changes made in the context to the underlying database asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task SaveChangesAsync();

        /// <summary>
        /// Gets an <see cref="IQueryable{T}"/> representing all entities optionally filtered by the provided expression.
        /// This method allows for further query composition before execution.
        /// </summary>
        /// <param name="filter">
        /// An optional expression to filter the entities.
        /// If <c>null</c>, all entities are returned.
        /// </param>
        /// <param name="asNoTracking">
        /// If <c>true</c>, the returned entities are not tracked by the context.
        /// </param>
        /// <returns>A task representing the asynchronous operation. The task result contains an <see cref="IQueryable{T}"/> of entities.</returns>
        Task<IQueryable<T>> GetAll(Expression<Func<T, bool>>? filter = null, 
            bool asNoTracking = false, 
            int? pageIndex = null,
            int? pageSize = null);

        /// <summary>
        /// Gets a materialized list of entities asynchronously.
        /// This method executes the query immediately, returning a list of results.
        /// </summary>
        /// <param name="filter">
        /// An optional expression to filter the entities.
        /// If <c>null</c>, all entities are returned.
        /// </param>
        /// <param name="asNoTracking">
        /// If <c>true</c>, the returned entities are not tracked by the context.
        /// </param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of entities.</returns>
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, 
            bool asNoTracking = false, 
            int? pageIndex = null, 
            int? pageSize = null);

        /// <summary>
        /// Gets a single entity matching the specified filter asynchronously.
        /// </summary>
        /// <param name="filter">An expression to filter the entities.</param>
        /// <param name="asNoTracking">
        /// If <c>true</c>, the returned entity is not tracked by the context.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains the first entity matching the filter; 
        /// if no such entity exists, the result is <c>null</c>.
        /// </returns>
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, 
            bool asNoTracking = false);
    }
}
