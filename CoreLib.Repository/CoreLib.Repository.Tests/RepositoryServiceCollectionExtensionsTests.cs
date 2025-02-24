using AppComponents.CoreLib.Repository;
using AppComponents.CoreLib.Repository.Abstraction;
using AppComponents.CoreLib.Repository.EFCore;
using CoreLib.Tests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class RepositoryServiceCollectionExtensionsTests
{
    [Fact]
    public void AddCoreLibRepository_RegistersIRepository()
    {
        // Arrange
        // Act
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddRepository<MockItem, TestDbContext>();

        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));
        var provider = services.BuildServiceProvider();


        // Assert
        var repository = provider.GetService<IRepository<MockItem, TestDbContext>>();
        Assert.NotNull(repository);
        Assert.IsType<Repository<MockItem, TestDbContext>>(repository);
    }
}
