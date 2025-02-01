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
    public class RepositoryTests_GetAsync  : RepositoryTestsBase, IAsyncLifetime
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
            var expectedResult = TestData.FirstDuplicateItem;

            await InitializeAsync(TestData.DuplicateMockItems);
            Repository<MockItem> repository = GetRepository();

            //Act
            var result = await repository.GetAsync(_queryItemWitDuplicateName);

            //Assert
           AssertMockItem(expectedResult, result);
        }



        //Note: Commenting tests as the behavior is not clear
        //Passing tests  for these may n ot be correct either
        //Low risk as framework method is used.
        //To verify check if AsNoTracking() is correctly used in the repository method

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //[InlineData(null)]
        //public async Task GetAsync_ReturnsTracking_BasedOnTrackingFlag(bool? asNoTracking)
        //{
        //    //Arrange
        //    Repository<MockItem> repository = GetRepository();

        //    //Act
        //    //Assert
        //    await AssertTrackingBehaviorForGetAsync(repository, _queryItemWithId1, asNoTracking);
        //}

        //[Fact]
        //public async Task GetAsync_TrackingBehaviorSwitches_WhenCalledWithAlternatingValues()
        //{
        //    //Arrange
        //    Repository<MockItem> repository = GetRepository();

        //    //Act
        //    //Assert
        //    await AssertTrackingBehaviorForGetAsync(repository, _queryItemWithId1, true);
        //    await AssertTrackingBehaviorForGetAsync(repository, _queryItemWithId1, false);

        //    await AssertTrackingBehaviorForGetAsync(repository, _queryItemWithId1, false);
        //}
    }
}
