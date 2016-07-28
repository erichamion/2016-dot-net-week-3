using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.User
{
    class Wallet
    {
        public double Cash { get; private set; }

        public Wallet(Store.ICheckoutEventCreator checkoutCreator, double startingCash)
        {
            Cash = startingCash;

            checkoutCreator.OnPreCheckoutEvent += OnPreCheckout;
            checkoutCreator.OnCheckoutEvent += OnCheckout;
            // No need to register for OnPostCheckoutEvent
        }

        public Wallet(Store.ICheckoutEventCreator checkoutCreator, Random random) 
            : this(checkoutCreator, random.NextDouble() * 5000) { }

        public Wallet(Store.ICheckoutEventCreator checkoutCreator) : this(checkoutCreator, new Random()) { }


        public bool CanPay(double amount)
        {
            return Cash > amount;
        }

        public void Pay(double amount)
        {
            if (amount < 0 || !CanPay(amount))
            {
                throw new ArgumentOutOfRangeException("Cannot pay an amount that is either negative or greater than the current cash!");
            }
            Cash -= amount;
        }

        public void Fill(double amount)
        {
            Cash += amount;
        }

        private void OnPreCheckout(ShoppingCart cart, AddOnlyCollection<String> errors)
        {
            // Make sure we can cover the cost of the cart.
            double price = cart.GetTotalPrice();
            if (Cash < price)
            {
                errors.Add(String.Format("Not enough cash in wallet: Need {0:C}, have {1:C}", price, Cash));
            }
        }

        private void OnCheckout(ShoppingCart cart)
        {
            // Take money out of the wallet to cover the cart's total cost.
            Pay(cart.GetTotalPrice());
        }
    }
}
