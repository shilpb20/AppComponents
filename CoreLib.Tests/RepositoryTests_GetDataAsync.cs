using AppComponents.CoreLib;
using CoreLib.Tests.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var result = await repository.GetAsync(_queryItemWithId0);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAsync_ReturnsMatchingItem_WhenSingleMatchingDataFound()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            var result = await repository.GetAsync(_queryItemWithId1);

            //Assert
            AssertMockItem(TestData.MockItems.First(), result);
        }

        [Fact]
        public async Task GetAsync_ReturnsFirstMatchingItem_WhenMultipleMatchingDataFound()
        {
            //Arrange
            var expectedResult = new MockItem() { Id = 4, Name = TestData.DuplicateName };

            await InitializeAsync(TestData.DuplicateMockItems);
            Repository<MockItem> repository = GetRepository();

            //Act
            var result = await repository.GetAsync(_queryItemWitDuplicateName);

            //Assert
           AssertMockItem(expectedResult, result);
        }

        [Fact]
        public async Task GetAsync_ReturnsTrackingAllBehavior_WhenTrackingIsSet()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            var result = await repository.GetAsync(_queryItemWithId1, false);

            //Assert
            AssertTrackingBehavior(_trackAll);
            AssertMockItem(TestData.MockItems.First(), result);
        }

        [Fact]
        public async Task GetAsync_ReturnsNoTrackingBehavior_WhenNoTrackingIsSet()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            var result = await repository.GetAsync(_queryItemWithId1, true);

            //Assert
            AssertTrackingBehavior(_noTrack);
            AssertMockItem(TestData.MockItems.First(), result);
        }

        [Fact]
        public async Task GetAsync_ReturnsTrackingAllBehavior_WhenImplicitlyCalled()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            var result = await repository.GetAsync(_queryItemWithId1);

            //Assert
            AssertTrackingBehavior(_trackAll);
            AssertMockItem(TestData.MockItems.First(), result);
        }

        [Fact]
        public async Task GetAsync_TrackingBehaviorSwitches_WhenCalledWithAlternatingValues()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            //Assert
            await repository.GetAsync(_queryItemWithId1, false);
            AssertTrackingBehavior(_trackAll);

            await repository.GetAsync(_queryItemWithId1, true);
            AssertTrackingBehavior(_noTrack);

            await repository.GetAsync(_queryItemWithId1, false);
            AssertTrackingBehavior(_trackAll);
        }
    }
}
