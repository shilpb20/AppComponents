using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Tests.Repository.TestContext
{
    public static class TestData
    {
        public static readonly string DuplicateName = "Item";
        public static readonly string UpdatedName = "Item 5";

        public static readonly string Column1 = "Id";
        public static readonly string Column2 = "Name";
        public static readonly string Column3 = "Value";

        public static readonly MockItem NewItem = new MockItem() { Id = 6, Name = "New Item", Value = "New Value" };
        public static readonly MockItem FirstDuplicateItem = new MockItem() { Id = 4, Name = DuplicateName, Value = "Value 4" };
        public static readonly MockItem LastDuplicateItem = new MockItem() { Id = 5, Name = DuplicateName, Value = "Value 4" };

        public static readonly MockItem UpdateItem = new MockItem() { Id = 5, Name = "Item 5", Value = "Value 5" };
        public static readonly MockItem InvalidUpdateItem = new MockItem() { Id = 5, Name = "It", Value = "Value 5" };

        public static readonly List<MockItem> MockItems = new()
        {
            new MockItem { Id = 1, Name = "Item 1", Value = "Value 1" },
            new MockItem { Id = 2, Name = "Item 2", Value = "Value 2" },
            new MockItem { Id = 3, Name = "Item 3", Value = "Value 3" },
            new MockItem { Id = 4, Name = "Item 4", Value = "Value 4" },
            new MockItem { Id = 5, Name = "Item 5", Value = "Value 5" },
        };

        public static readonly List<MockItem> DuplicateMockItems = new()
        {
            new MockItem { Id = 1, Name = "Item 1", Value = "Value 1" },
            new MockItem { Id = 2, Name = "Item 2", Value = "Value 2" },
            new MockItem { Id = 3, Name = "Item 3", Value = "Value 3" },
            new MockItem { Id = 4, Name = DuplicateName, Value = "Value 4" },
            new MockItem { Id = 5, Name = DuplicateName, Value = "Value 5" },
        };


        public static readonly List<MockItem> MockItemsWithOddIds = new()
        {
            new MockItem { Id = 1, Name = "Item 1", Value = "Value 1" },
            new MockItem { Id = 3, Name = "Item 3", Value = "Value 3" },
            new MockItem { Id = 5, Name = "Item 5", Value = "Value 5" },
        };

        public static readonly List<MockItem> MockItemsWithEvenIds = new()
        {
            new MockItem { Id = 2, Name = "Item 2", Value = "Value 2" },
            new MockItem { Id = 4, Name = "Item 4", Value = "Value 4" },
        };

        public static readonly List<MockItem> MockItemsForPagination = new()
        {
            new MockItem { Id = 1, Name = "Item 1",  Value = "Value 1" },
            new MockItem { Id = 2, Name = "Item 2", Value = "Value 2" },
            new MockItem { Id = 3, Name = "Item 3", Value = "Value 3" },
            new MockItem { Id = 4, Name = "Item 4", Value = "Value 4" },
            new MockItem { Id = 5, Name = "Item 5", Value = "Value 5" },
            new MockItem { Id = 6, Name = "Item 6", Value = "Value 6" },
            new MockItem { Id = 7, Name = "Item 7", Value = "Value 7" },
            new MockItem { Id = 8, Name = "Item 8", Value = "Value 8" },
            new MockItem { Id = 9, Name = "Item 9", Value = "Value 9" },
            new MockItem { Id = 10, Name = "Item 10", Value = "Value 10" },
            new MockItem { Id = 11, Name = "Item 11", Value = "Value 11" },
            new MockItem { Id = 12, Name = "Item 12", Value = "Value 12" },
            new MockItem { Id = 13, Name = "Item 13", Value = "Value 13" },
            new MockItem { Id = 14, Name = "Item 14", Value = "Value 14" },
            new MockItem { Id = 15, Name = "Item 15", Value = "Value 15" },
            new MockItem { Id = 16, Name = "Item 16", Value =   "Value 16" },
            new MockItem { Id = 17, Name = "Item 17", Value = "Value 17" },
            new MockItem { Id = 18, Name = "Item 18", Value = "Value 18" },
            new MockItem { Id = 19, Name = "Item 19", Value = "Value 19" },
            new MockItem { Id = 20, Name = "Item 20", Value = "Value 20" },
        };

        public static readonly List<MockItem> MockItemsForOrderBy = new()
        {
            new MockItem { Id = 2, Name = "Item 2", Value = "Value 2" },
            new MockItem { Id = 1, Name = "Item 2",  Value = "Value 1" },
            new MockItem { Id = 4, Name = "Item 3", Value = "Value 4" },
            new MockItem { Id = 3, Name = "Item 3", Value = "Value 3" },
            new MockItem { Id = 6, Name = "Item 6", Value = "Value 6" },
            new MockItem { Id = 5, Name = "Item 5", Value = "Value 5" },
            new MockItem { Id = 8, Name = "Item 8", Value = "Value 8" },
            new MockItem { Id = 7, Name = "Item 7", Value = "Value 7" },
            new MockItem { Id = 10, Name = "Item 10", Value = "Value 10" },
            new MockItem { Id = 9, Name = "Item 9", Value = "Value 9" },
        };
    }
}
