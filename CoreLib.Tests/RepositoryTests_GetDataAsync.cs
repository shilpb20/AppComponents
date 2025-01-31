using AppComponents.CoreLib;
using CoreLib.Tests.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Tests
{
    public class RepositoryTests_GetDataAsync  : RepositoryTestsBase, IAsyncLifetime
    {
        [Fact]
        public async Task GetAsync_ReturnsNull_WhenMatchingDataNotFound()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            var result = await repository.GetAsync(x => x.Id == 0);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAsync_ReturnsMatchingItem_WhenSingleMatchingDataFound()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            var result = await repository.GetAsync(x => x.Id == 1);

            //Assert
            var expectedData = DataList.MockItems.Find(x => x.Id == 1);
            Assert.Equal(expectedData.Id, result.Id);
            Assert.Equal(expectedData.Name, result.Name);
        }

        [Fact]
        public async Task GetAsync_ReturnsFirstMatchingItem_WhenMultipleMatchingDataFound()
        {
            //Arrange
            int id = 4;
            string name = "Item";

            await InitializeAsync(DataList.DuplicateMockItems);
            Repository<MockItem> repository = GetRepository();

            //Act
            var result = await repository.GetAsync(x => x.Name == "Item");

            //Assert
            Assert.Equal(id, result.Id);
            Assert.Equal(name, result.Name);
        }
    }
}
