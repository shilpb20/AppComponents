using AppComponents.CoreLib;
using CoreLib.Tests.TestData;
using Moq;

namespace CoreLib.Tests
{
    public class RepositoryTests
    {

        [Fact]
        public async void GetAllAsync_ReturnsAllItems_WhenCalled()
        {
            //Arrange
            List<MockItem> mockItemList = new()
            {
                new MockItem { Id = 1, Name = "Item 1" },
                new MockItem { Id = 2, Name = "Item 2" },
                new MockItem { Id = 3, Name = "Item 3" }
            };

            var repository = new Mock<Repository<MockItem>>();

            //Act
            var allMockItems = await repository.Object.GetAllAsync();

            //Assert
            Assert.NotNull(allMockItems);
            Assert.Equal(3, mockItemList.Count);

            int i = 0;
           foreach(var item in allMockItems)
            {
                Assert.Equal(mockItemList[i].Id, item.Id);
                Assert.Equal(mockItemList[i].Name, item.Name);

                i++;
            }
        }
    }
}