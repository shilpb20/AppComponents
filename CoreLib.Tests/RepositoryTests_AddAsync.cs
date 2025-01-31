using AppComponents.CoreLib;
using CoreLib.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Tests
{
    public class RepositoryTests_AddAsync  : RepositoryTestsBase, IAsyncLifetime
    {
        [Fact]
        public async Task AddAsync_ReturnsNewlyAddedObject_WhenValidObjectIsAdded()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act  
            var newItem = TestData.NewItem;
            var result = await repository.AddAsync(newItem);

            var checkResult = await repository.GetAsync(_queryNewItemByName);

            //Assert
            AssertMockItem(result, checkResult);
            AssertMockItem(result, newItem);
        }
    }
}
