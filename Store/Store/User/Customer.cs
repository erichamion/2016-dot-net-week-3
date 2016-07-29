using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.User
{
    class Customer
    {
        public ShoppingCart Cart { get; }
        public Wallet Wallet { get; }

        public Customer(Store.ICheckoutEventCreator checkoutCreator)
        {
            Cart = new ShoppingCart(checkoutCreator);
            Wallet = new Wallet(checkoutCreator);

        }

        
    }
}
