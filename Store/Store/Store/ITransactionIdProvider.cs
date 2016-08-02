using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Store
{
    public interface ITransactionIdProvider
    {
        int TransactionId { get; }
        void StartNewTransaction();
    }
}
