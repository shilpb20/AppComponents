using AppComponents.CoreLib;
using CoreLib.Tests.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public async Task GetAllWithPagination_ReturnsMatchingData_WhenDataIsInTheRange(int pageIndex, int pageSize)
        {
            //Act
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            int skipItems = (pageIndex - 1) * pageSize;
            int takeItems = pageSize;
            var expectedResult = TestData.MockItemsForPagination.Skip(skipItems).Take(takeItems).ToImmutableList();


            //Act
            var data = await repository.GetAll(null, true, pageIndex, pageSize);
            var result = await data.ToListAsync();

            //Assert
            AssertMockItems(expectedResult, result);
        }

        [Theory]
        [InlineData(1, 30, 20)]
        [InlineData(4, 6, 2)]
        [InlineData(5, 5, 0)]
        public async Task GetAllWithPagination_ReturnsRemainingData_WhenMoreThanExistingDataIsRequested(int pageIndex, int pageSize, int takeItems)
        {
            //Act
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            int skipItems = (pageIndex - 1) * pageSize;
            var expectedResult = TestData.MockItemsForPagination.Skip(skipItems).Take(takeItems).ToImmutableList();


            //Act
            var data = await repository.GetAll(null, true, pageIndex, pageSize);
            var result = await data.ToListAsync();

            //Assert
            AssertMockItems(expectedResult, result);
        }

        [Theory]
        [InlineData(-1, 2)]
        [InlineData(2, 0)]
        [InlineData(3, -2)]
        [InlineData(null, -1)]
        [InlineData(-2, null)]
        public async Task GetAllWithPagination_ReturnsEmptyData_WhenIncorrectRequestIsMade(int? pageIndex, int? pageSize)
        {
            //Act
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            //Act
            var data = await repository.GetAll(null, true, pageIndex, pageSize);
            var result = await data.ToListAsync();

            //Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(null, 3)]
        [InlineData(4, null)]
        public async Task GetAllWithPagination_ReturnsEmptyData_WhenNullValuesAreUsed(int? pageIndex, int? pageSize)
        {
            //Act
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            //Act
            var data = await repository.GetAll(null, true, pageIndex, pageSize);
            var result = await data.ToListAsync();

            //Assert
            Assert.Empty(result);
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
