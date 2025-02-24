using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AppComponents.CoreLib.Repository.Abstraction
{
    public interface IRepository<T, TContext>
        where T : class
        where TContext : DbContext
    {
        Task<T?> AddAsync(T entity);

        Task<T?> UpdateAsync(T entity);

        Task<T?> DeleteAsync(T entity);

        Task SaveChangesAsync();

        Task<IQueryable<T>> GetAll(
            Expression<Func<T, bool>>? filter = null,
            bool asNoTracking = false,
            Dictionary<string, bool>? orderByClause = null,
            Pagination? paginationSpec = null);

        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            bool asNoTracking = false,
            Dictionary<string, bool>? orderByClause = null,
            Pagination? paginationSpec = null);

        Task<T?> GetAsync(
            Expression<Func<T, bool>> filter,
            bool asNoTracking = false);
    }
}
