using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Store
{
    class Store : ICheckoutEventCreator
    {
        public event CheckoutEvents.OnPreCheckout OnPreCheckoutEvent;
        public event CheckoutEvents.OnCheckout OnCheckoutEvent;
        public event CheckoutEvents.OnPostCheckout OnPostCheckoutEvent;

        public Inventory Inventory { get; }
        public User.Customer Customer { get; }

        public Store()
        {
            this.Inventory = new Inventory(this);
            this.Customer = new User.Customer(this);
        }

        public bool Checkout(out String errorMessage)
        {
            errorMessage = null;
            List<String> errors = new List<string>();
            OnPreCheckoutEvent?.Invoke(Customer.Cart, new AddOnlyCollection<string>(errors));
            if (errors.Count == 0)
            {
                //Everything went well
                OnCheckoutEvent?.Invoke(Customer.Cart);
                OnPostCheckoutEvent?.Invoke();
                return true;
            }
            else
            {
                //Somebody complained. Don't complete the checkout.
                errorMessage = String.Join("\n", errors);
                return false;
            }
        }
    }
}
