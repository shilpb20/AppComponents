using AppComponents.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using Repository.Tests.Repository.TestContext;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Tests.Repository
{
    public class RepositoryPaginationTests : RepositoryTestsBase, IAsyncLifetime
    {
        #region get-all


        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public async Task GetAllWithPagination_ReturnsMatchingData_WhenDataIsInTheRange(int pageIndex, int pageSize)
        {
            //Act
            var paginationSpec = new Pagination(pageIndex, pageSize);
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            int skipItems = (pageIndex - 1) * pageSize;
            int takeItems = pageSize;
            var expectedResult = TestData.MockItemsForPagination.Skip(skipItems).Take(takeItems).ToImmutableList();


            //Act
            var data = await repository.GetAll(null, true, null, paginationSpec);
            var result = await data.ToListAsync();

            //Assert
            AssertMockItems(expectedResult, result);
        }


        [Theory]
        [InlineData(2, 0)]
        [InlineData(3, -2)]
        [InlineData(4, null)]
        [InlineData(null, -1)]
        [InlineData(-1, 2)]
        [InlineData(-2, null)]
        [InlineData(null, 3)]
        public async Task GetAllWithPagination_UsesDefaultValues_WhenInvalidValuesAreUsed(int pageIndex, int pageSize)
        {
            //Act
            var paginationSpec = new Pagination(pageIndex, pageSize);
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            int expectedPageIndex = pageIndex <= 0 ? 1 : pageIndex;
            int expectedPageSize = pageSize <= 0 ? 10 : pageSize;

            int skipItems = (pageIndex - 1) * expectedPageSize;
            int takeItems = expectedPageSize;
            var expectedResult = TestData.MockItemsForPagination.Skip(skipItems).Take(takeItems).ToImmutableList();


            //Act
            var data = await repository.GetAll(null, true, null, paginationSpec);
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
            var pageSpec = new Pagination(pageIndex, pageSize);
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            int skipItems = (pageIndex - 1) * pageSize;
            var expectedResult = TestData.MockItemsForPagination.Skip(skipItems).Take(takeItems).ToImmutableList();


            //Act
            var data = await repository.GetAll(null, true, null, pageSpec);
            var result = await data.ToListAsync();

            //Assert
            AssertMockItems(expectedResult, result);
        }

        #endregion

        #region get-all-async

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public async Task GetAllAsyncWithPagination_ReturnsMatchingData_WhenDataIsInTheRange(int pageIndex, int pageSize)
        {
            //Act
            var paginationSpec = new Pagination(pageIndex, pageSize);
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            int skipItems = (pageIndex - 1) * pageSize;
            int takeItems = pageSize;
            var expectedResult = TestData.MockItemsForPagination.Skip(skipItems).Take(takeItems).ToImmutableList();


            //Act
            var data = await repository.GetAllAsync(null, true, null, paginationSpec);

            //Assert
            AssertMockItems(expectedResult, data);
        }


        [Theory]
        [InlineData(2, 0)]
        [InlineData(3, -2)]
        [InlineData(4, null)]
        [InlineData(null, -1)]
        [InlineData(-1, 2)]
        [InlineData(-2, null)]
        [InlineData(null, 3)]
        public async Task GetAllAsyncWithPagination_UsesDefaultValues_WhenInvalidValuesAreUsed(int pageIndex, int pageSize)
        {
            //Act
            var paginationSpec = new Pagination(pageIndex, pageSize);
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            int expectedPageIndex = pageIndex <= 0 ? 1 : pageIndex;
            int expectedPageSize = pageSize <= 0 ? 10 : pageSize;

            int skipItems = (pageIndex - 1) * expectedPageSize;
            int takeItems = expectedPageSize;
            var expectedResult = TestData.MockItemsForPagination.Skip(skipItems).Take(takeItems).ToImmutableList();


            //Act
            var data = await repository.GetAllAsync(null, true, null, paginationSpec);

            //Assert
            AssertMockItems(expectedResult, data);
        }



        [Theory]
        [InlineData(1, 30, 20)]
        [InlineData(4, 6, 2)]
        [InlineData(5, 5, 0)]
        public async Task GetAllAsyncWithPagination_ReturnsRemainingData_WhenMoreThanExistingDataIsRequested(int pageIndex, int pageSize, int takeItems)
        {
            //Act
            var pageSpec = new Pagination(pageIndex, pageSize);
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            int skipItems = (pageIndex - 1) * pageSize;
            var expectedResult = TestData.MockItemsForPagination.Skip(skipItems).Take(takeItems).ToImmutableList();


            //Act
            var data = await repository.GetAllAsync(null, true, null, pageSpec);

            //Assert
            AssertMockItems(expectedResult, data);
        }

        #endregion
    }
}
