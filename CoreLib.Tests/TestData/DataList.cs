using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Tests.TestData
{
    public static class DataList
    {

        public static readonly List<MockItem> MockItems = new()
        {
            new MockItem { Id = 1, Name = "Item 1" },
            new MockItem { Id = 2, Name = "Item 2" },
            new MockItem { Id = 3, Name = "Item 3" }
        };
    }
}
