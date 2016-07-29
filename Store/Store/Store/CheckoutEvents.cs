using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Store
{
    static class CheckoutEvents
    {
        public delegate void OnPreReserve(Product product, int count, int transactionId, AddOnlyCollection<String> errors);
        public delegate void OnReserve(Product product, int count, int transactionId);

        public delegate void OnPreRelease(Product product, int count, int transactionId, AddOnlyCollection<int> maxReleased);
        public delegate void OnRelease(Product product, int count, int transactionId);

        public delegate void OnPreCheckout(User.ShoppingCart cart, int transactionId, AddOnlyCollection<String> errors);
        public delegate void OnCheckout(User.ShoppingCart cart, int transactionId);

        public delegate void OnTransactionEnded(int transactionId);
    }
}
