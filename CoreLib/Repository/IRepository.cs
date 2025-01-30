namespace AppComponents.CoreLib
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>>? GetAllAsync();
    }
}