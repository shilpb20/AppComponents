using AppComponents.CoreLib;
using CoreLib.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Tests
{
    public class RepositoryTests_UpdateAsync  : RepositoryTestsBase, IAsyncLifetime
    {
        [Fact]
        public async Task UpdateAsync_ReturnsUpdateValue_WhenValidValueIsUpdatedOnExistingObject()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act  
          

            //Assert
          
        }
    }
}
