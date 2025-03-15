using AppComponents.Repository.Abstraction;
using AppComponents.Repository.EFCore.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class TransactionManager<TContext> : ITransactionManager where TContext : DbContext
{
    private readonly TContext _dbContext;
    private readonly TransactionSettings _settings;

    public TransactionManager(TContext dbContext, IOptions<TransactionSettings> options)
    {
        _dbContext = dbContext;
        _settings = options.Value;
    }

    public async Task BeginTransactionAsync()
    {
        if (_settings.UseInMemoryDatabase)
        {
            return;
        }

        if (_dbContext.Database.CurrentTransaction == null)
        {
            await _dbContext.Database.BeginTransactionAsync();
        }
    }

    public async Task CommitTransactionAsync()
    {
        if (_settings.UseInMemoryDatabase)
        {
            return;
        }

        if (_dbContext.Database.CurrentTransaction != null)
        {
            await _dbContext.Database.CommitTransactionAsync();
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_settings.UseInMemoryDatabase)
        {
            return;
        }

        if (_dbContext.Database.CurrentTransaction != null)
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }
    }
}
