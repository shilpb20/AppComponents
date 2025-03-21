namespace AppComponents.Repository.Abstraction
{
    public interface ITransactionManager
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }

}
