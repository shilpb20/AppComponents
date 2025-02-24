using AppComponents.Repository.Abstraction;
using AppComponents.Repository.EFCore;
using Repository.Tests.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Repository.Tests
{
    public class RepositoryTests_UpdateAsync : RepositoryTestsBase, IAsyncLifetime
    {
        [Fact]
        public async Task UpdateAsync_ReturnsUpdatedValue_OnValidUpdate()
        {
            //Arrange
            await InitializeAsync(TestData.DuplicateMockItems);
            Repository<MockItem, TestDbContext> repository = GetRepository();

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
            Repository<MockItem, TestDbContext> repository = GetRepository();

            //Act  
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await repository.UpdateAsync(null);
            });
        }
    }
}
