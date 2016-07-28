using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Store
{
    static class CheckoutEvents
    {
        public delegate void OnPreCheckout(User.ShoppingCart cart, AddOnlyCollection<String> errors);
        public delegate void OnCheckout(User.ShoppingCart cart);
        public delegate void OnPostCheckout();
    }
}
