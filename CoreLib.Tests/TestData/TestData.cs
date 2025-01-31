using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Tests.Data
{
    public static class TestData
    {
        public static readonly string DuplicateName = "Item";

        public static readonly MockItem NewItem = new MockItem() { Id = 6,  Name = "New Item" };

        public static readonly List<MockItem> MockItems = new()
        {
            new MockItem { Id = 1, Name = "Item 1" },
            new MockItem { Id = 2, Name = "Item 2" },
            new MockItem { Id = 3, Name = "Item 3" },
            new MockItem { Id = 4, Name = "Item 4" },
            new MockItem { Id = 5, Name = "Item 5" }
        };

        public static readonly List<MockItem> DuplicateMockItems = new()
        {
            new MockItem { Id = 1, Name = "Item 1" },
            new MockItem { Id = 2, Name = "Item 2" },
            new MockItem { Id = 3, Name = "Item 3" },
            new MockItem { Id = 4, Name = DuplicateName },
            new MockItem { Id = 5, Name = DuplicateName }
        };


        public static readonly List<MockItem> MockItemsWithOddIds = new()
        {
            new MockItem { Id = 1, Name = "Item 1" },
            new MockItem { Id = 3, Name = "Item 3" },
            new MockItem { Id = 5, Name = "Item 5" }
        };

        public static readonly List<MockItem> MockItemsWithEvenIds = new()
        {
            new MockItem { Id = 2, Name = "Item 2" },
            new MockItem { Id = 4, Name = "Item 4" },
        };
    }
}
