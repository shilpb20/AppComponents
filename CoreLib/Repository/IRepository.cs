using System.Linq.Expressions;

namespace AppComponents.CoreLib
{
    public interface IRepository<T> where T : class
    {
       public Task<IEnumerable<T>>? GetAllAsync(Expression<Func<T, bool>>? filter = null, bool asNoTracking = false);
    }
}