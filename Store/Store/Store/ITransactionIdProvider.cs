using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Store
{
    interface ITransactionIdProvider
    {
        int TransactionId { get; }
        void StartNewTransaction();
    }
}
