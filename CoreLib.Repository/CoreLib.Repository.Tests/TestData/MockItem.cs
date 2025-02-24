using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Tests.Data
{
    public class MockItem
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }  = string.Empty;
        public string? Value { get; set; }
    }
}
