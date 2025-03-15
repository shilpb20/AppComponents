using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppComponents.Repository.EFCore.Transaction
{
    public class TransactionSettings
    {
        public bool UseInMemoryDatabase { get; set; } = false;
    }
}
