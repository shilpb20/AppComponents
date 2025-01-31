using AppComponents.CoreLib;
using AppComponents.CoreLib.Repository;
using CoreLib.Tests.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CoreLib.Tests
{
    public class RepositoryTests_UpdateAsync  : RepositoryTestsBase, IAsyncLifetime
    {
        [Fact]
        public async Task UpdateAsync_ReturnsUpdatedValue_OnValidUpdate()
        {
            //Arrange
            await InitializeAsync(TestData.DuplicateMockItems);
            Repository<MockItem> repository = GetRepository();

            //Act
            //Assert
            var updateObject = await repository.GetAsync(_queryItemForUpdateData);
            AssertMockItem(TestData.DuplicateMockItems.Last(), updateObject);
            updateObject.Name = TestData.UpdateItem.Name;
            updateObject.Value = TestData.UpdateItem.Value;

            var result = await repository.UpdateAsync(updateObject);
            AssertMockItem(TestData.UpdateItem, result);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsInvalidOperationException_OnUpdateNull()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();
            MockItem mockItem = null;

            //Act  
            //Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await repository.UpdateAsync(null);
            });

            Assert.Equal(RepositoryConstants.NullUpdate, exception.Message);
        }
    }
}
