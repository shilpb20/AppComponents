using AppComponents.Repository.Abstraction;
using AppComponents.Repository.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AppComponents.Repository.Tests
{
    public class DependencyRegistrationTests
    {
        [Fact]
        public void AddRepository_RegistersIRepository()
        {
            // Arrange
            // Act
            var services = new ServiceCollection();
            services.AddLogging();

            services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));


            //Act
            services.AddRepository<MockItem, TestDbContext>();
            var provider = services.BuildServiceProvider();

            // Assert
            var repository = provider.GetService<IRepository<MockItem, TestDbContext>>();
            Assert.NotNull(repository);
            Assert.IsType<Repository<MockItem, TestDbContext>>(repository);
        }

        [Fact]
        public void AddTimeStampedRepository_RegistersIRepository()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));


            // Act
            services.AddTimeStampedRepository<TimeStampedMockItem, TestDbContext>();

            var provider = services.BuildServiceProvider();
            var repository = provider.GetService<IRepository<TimeStampedMockItem, TestDbContext>>();

            // Assert
            Assert.NotNull(repository);
            Assert.IsType<TimeStampedRepository<TimeStampedMockItem, TestDbContext>>(repository);
        }
    }
}


