using AppComponents.CoreLib;
using CoreLib.Tests.TestData;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CoreLib.Tests
{
    public class RepositoryTests
    {
        private readonly TestDbContext _dbContext;

        private async Task InitialiseData()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
           .UseInMemoryDatabase("TestDatabase")
           .Options;

            _dbContext = new TestDbContext(options);
            await _dbContext.MockItems.AddRangeAsync(DataList.MockItems);
            await _dbContext.SaveChangesAsync();
        }

        [Fact]
        public async void GetAllAsync_ReturnsAllItems_WhenCalled()
        {
            //Arrange
            await InitialiseData();

            var repository = new Repository<MockItem>(_dbContext);

            //Act
            var allMockItems = await repository.GetAllAsync();

            //Assert
            Assert.NotNull(allMockItems);
            Assert.Equal(DataList.MockItems.Count, DataList.MockItems.Count);

            int i = 0;
           foreach(var item in allMockItems)
            {
                Assert.Equal(DataList.MockItems[i].Id, item.Id);
                Assert.Equal(DataList.MockItems[i].Name, item.Name);

                i++;
            }
        }
    }
}