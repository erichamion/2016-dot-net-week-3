using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Store
{
    class Store : ICheckoutEventCreator
    {
        public event CheckoutEvents.OnPreReserve OnPreReserveEvent;
        public event CheckoutEvents.OnReserve OnReserveEvent;
        public event CheckoutEvents.OnPreRelease OnPreReleaseEvent;
        public event CheckoutEvents.OnRelease OnReleaseEvent;
        public event CheckoutEvents.OnPreCheckout OnPreCheckoutEvent;
        public event CheckoutEvents.OnCheckout OnCheckoutEvent;
        public event CheckoutEvents.OnTransactionEnded OnTransactionEndedEvent;

        private int _transactionId = 1;


        public Inventory Inventory { get; }
        public User.Customer Customer { get; }

        public Store()
        {
            this.Inventory = new Inventory(this);
            this.Customer = new User.Customer(this);
        }

        public bool ReserveProducts(Product product, int count, out String errorMessage)
        {
            if (count < 0) throw new ArgumentException("Cannot reserve negative items!");

            errorMessage = null;
            List<String> errors = new List<string>();
            OnPreReserveEvent?.Invoke(product, count, _transactionId, new AddOnlyCollection<String>(errors));
            if (errors.Count == 0)
            {
                // Everything is good
                OnReserveEvent?.Invoke(product, count, _transactionId);
                return true;
            }
            else
            {
                errorMessage = String.Join("\n", errors);
                return false;
            }
        }

        /**
         * Returns the number of units actually released, which may be fewer
         * than requested.
         */
        public int ReleaseProducts(Product product, int count)
        {
            if (count < 0) throw new ArgumentException("Cannot release negative reserved items!");

            // Put the current count into the maxCount list so it's
            // easier to compare afterward (just need to look at the
            // list's minimum, rather than get the list's minimum 
            // and compare it to count).
            List<int> maxCount = new List<int> { count };
            OnPreReleaseEvent(product, count, _transactionId, new AddOnlyCollection<int>(maxCount));

            int realCount = maxCount.Min();
            if (realCount > 0)
            {
                OnReleaseEvent(product, count, _transactionId);
            }

            return Math.Max(realCount, 0);
        }

        public bool Checkout(out String errorMessage)
        {
            errorMessage = null;
            List<String> errors = new List<string>();
            OnPreCheckoutEvent?.Invoke(Customer.Cart, _transactionId, new AddOnlyCollection<string>(errors));
            if (errors.Count == 0)
            {
                //Everything went well
                OnCheckoutEvent?.Invoke(Customer.Cart, _transactionId);
                OnTransactionEndedEvent?.Invoke(_transactionId);

                // The next reserve will be a new transaction.
                _transactionId++;
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
