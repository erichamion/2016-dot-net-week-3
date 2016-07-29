using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.User
{
    class Customer : Store.ITransactionIdProvider
    {
        private static int _nextTransactionId = 1;
        private int _transactionId;

        public ShoppingCart Cart { get; }
        public Wallet Wallet { get; }

        public int TransactionId { get { return _transactionId; } }

        public void StartNewTransaction()
        {
            _transactionId = _nextTransactionId++;
        }

        public Customer(Store.ICheckoutEventCreator checkoutCreator)
        {
            Cart = new ShoppingCart(checkoutCreator);
            Wallet = new Wallet(checkoutCreator);
            StartNewTransaction();
        }

        
    }
}
