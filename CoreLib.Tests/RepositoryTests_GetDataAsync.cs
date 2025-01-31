using AppComponents.CoreLib;
using CoreLib.Tests.TestData;
using Microsoft.EntityFrameworkCore;
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

        [Fact]
        public async Task GetAsync_ReturnsTrackingAllBehavior_WhenTrackingIsSet()
        {
            //Arrange
            await InitializeAsync(DataList.DuplicateMockItems);
            Repository<MockItem> repository = GetRepository();

            //Act
            var result = await repository.GetAsync(x => x.Id == 1, false);
            var tracking = _dbContext.ChangeTracker.QueryTrackingBehavior;

            //Assert
            Assert.Equal(QueryTrackingBehavior.TrackAll, tracking);

            var expectedData = DataList.MockItems.Find(x => x.Id == 1);
            Assert.Equal(expectedData.Id, result.Id);
            Assert.Equal(expectedData.Name, result.Name);
        }

        [Fact]
        public async Task GetAsync_ReturnsNoTrackingBehavior_WhenNoTrackingIsSet()
        {
            //Arrange
            await InitializeAsync(DataList.DuplicateMockItems);
            Repository<MockItem> repository = GetRepository();

            //Act
            var result = await repository.GetAsync(x => x.Id == 1, false);
            var tracking = _dbContext.ChangeTracker.QueryTrackingBehavior;

            //Assert
            Assert.Equal(QueryTrackingBehavior.NoTracking, tracking);

            var expectedData = DataList.MockItems.Find(x => x.Id == 1);
            Assert.Equal(expectedData.Id, result.Id);
            Assert.Equal(expectedData.Name, result.Name);
        }

        [Fact]
        public async Task GetAsync_ReturnsTrackingAllBehavior_WhenImplicitlyCalled()
        {
            //Arrange
            await InitializeAsync(DataList.DuplicateMockItems);
            Repository<MockItem> repository = GetRepository();

            //Act
            var result = await repository.GetAsync(x => x.Id == 1);
            var tracking = _dbContext.ChangeTracker.QueryTrackingBehavior;

            //Assert
            Assert.Equal(QueryTrackingBehavior.TrackAll, tracking);

            var expectedData = DataList.MockItems.Find(x => x.Id == 1);
            Assert.Equal(expectedData.Id, result.Id);
            Assert.Equal(expectedData.Name, result.Name);
        }

        [Fact]
        public async Task GetAsync_TrackingBehaviorSwitches_WhenCalledWithAlternatingValues()
        {
            //Arrange
            await InitializeAsync(DataList.DuplicateMockItems);
            Repository<MockItem> repository = GetRepository();

            //Act
            //Assert
            await repository.GetAsync(x => x.Id == 1, false);
            var tracking = _dbContext.ChangeTracker.QueryTrackingBehavior;
            Assert.Equal(QueryTrackingBehavior.TrackAll, tracking);

            await repository.GetAsync(x => x.Id == 1, true);
            var trackingOnFirstSwitch = _dbContext.ChangeTracker.QueryTrackingBehavior;
            Assert.Equal(QueryTrackingBehavior.NoTracking, trackingOnFirstSwitch);

            await repository.GetAsync(x => x.Id == 1, true);
            var tracking = _dbContext.ChangeTracker.QueryTrackingBehavior;
            Assert.Equal(QueryTrackingBehavior.TrackAll, tracking);
        }
    }
}
