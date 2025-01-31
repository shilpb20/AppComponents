using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Tests
{
    public class RepositoryTests_GetDataAsync  : RepositoryTestsBase
    {
        public async Task GetAsync_ReturnsNull_WhenMatchingDataNotFound()
        {
            //Arrange
            var repository = GetRepository();

            //Act
            var result = await repository.GetAsync(x => x.Id == 0);

            //Assert
            Assert.Null(result);
        }
    }
}
