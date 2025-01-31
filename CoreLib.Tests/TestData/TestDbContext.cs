using AppComponents.CoreLib;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Tests.TestData
{
    public class TestDbContext : DbContext
    {
        public DbSet<MockItem> MockItems { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

    }
}
