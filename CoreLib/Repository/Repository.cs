namespace AppComponents.CoreLib
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public async Task<IEnumerable<T>>? GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
