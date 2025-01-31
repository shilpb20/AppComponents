using AppComponents.CoreLib;
using CoreLib.Tests.TestData;
using Microsoft.EntityFrameworkCore;
using Moq;

using System.Linq.Expressions;

namespace CoreLib.Tests
{
    public class RepositoryTests
    {
        private TestDbContext _dbContext;

        private async Task InitializeData()
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
            try
            {
                //Arrange
                await InitializeData();

                var repository = new Repository<MockItem>(_dbContext);

                //Act
                var allMockItems = await repository.GetAllAsync();

                //Assert
                Assert.NotNull(allMockItems);
                Assert.Equal(DataList.MockItems.Count, allMockItems.Count());

                int i = 0;
                foreach (var item in allMockItems)
                {
                    Assert.Equal(DataList.MockItems[i].Id, item.Id);
                    Assert.Equal(DataList.MockItems[i].Name, item.Name);

                    i++;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                Assert.Fail(ex.ToString());
            }           
        }

        [Fact]
        public async void GetAllAsync_ReturnsAllItemsWithMatchingCondition_WhenCalledWithAMatchCondition_EvenIds()
        {
            try
            {
                //Arrange
                await InitializeData();

                var repository = new Repository<MockItem>(_dbContext);

                //Act
                var mockItemsWithEvenIds = await repository.GetAllAsync(x => x.Id % 2 == 0);

                //Assert
                Assert.NotNull(mockItemsWithEvenIds);
                Assert.Equal(DataList.MockItemsWithEvenIds.Count, mockItemsWithEvenIds.Count());

                int i = 0;
                foreach (var item in mockItemsWithEvenIds)
                {
                    Assert.Equal(DataList.MockItems[i].Id, item.Id);
                    Assert.Equal(DataList.MockItems[i].Name, item.Name);

                    i++;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                Assert.Fail(ex.ToString());
            }
        }

        [Fact]
        public async void GetAllAsync_ReturnsAllItemsWithMatchingCondition_WhenCalledWithAMatchCondition_OddIds()
        {
            try
            {
                //Arrange
                await InitializeData();

                var repository = new Repository<MockItem>(_dbContext);

                //Act
                var mockItemsWithOddIds = await repository.GetAllAsync(X => X.Id % 2 != 0);

                //Assert
                Assert.NotNull(mockItemsWithOddIds);
                Assert.Equal(DataList.MockItemsWithOddIds.Count, mockItemsWithOddIds.Count());

                int i = 0;
                foreach (var item in mockItemsWithOddIds)
                {
                    Assert.Equal(DataList.MockItems[i].Id, item.Id);
                    Assert.Equal(DataList.MockItems[i].Name, item.Name);

                    i++;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                Assert.Fail(ex.ToString());
            } 
        }
    }
}