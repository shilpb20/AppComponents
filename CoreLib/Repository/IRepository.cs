using System.Linq.Expressions;

namespace AppComponents.CoreLib
{
    public interface IRepository<T> where T : class
    {
        Task<T?> AddAsync(T entity);
        Task<T?> DeleteAsync(T entity);
        public Task<IEnumerable<T>>? GetAllAsync(Expression<Func<T, bool>>? filter = null, bool asNoTracking = false);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool asNoTracking = false);
        Task SaveChangesAsync();
    }
}