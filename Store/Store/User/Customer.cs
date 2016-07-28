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
            Cart = new ShoppingCart();
            Wallet = new Wallet(checkoutCreator);

            checkoutCreator.OnPostCheckoutEvent += OnPostCheckout;
        }

        private void OnPostCheckout()
        {
            // Clear the cart after checkout.
            // This isn't done in the ShoppingCart class because we could 
            // instead decide to store replace this object's Cart property 
            // with a new (empty) cart. Perhaps there could be a feature 
            // to let the customer save and reuse carts for frequently 
            // used purchases, so we might want to create a new cart 
            // without destroying the contents of the old one.
            // This also lets ShoppingCart get by without any 
            // ICheckoutEventCreator parameter in the constructor. 
            // Customer needs an ICheckoutEventCreator anyway to pass
            // to Wallet's constructor.
            Cart.Clear();
        }
    }
}
