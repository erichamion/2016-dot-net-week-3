using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Store
{
    interface ICheckoutEventCreator
    {
        /**
         * OnPreCheckoutEvent occurs just before a checkout.
         * Handlers for this event should ensure that the checkout can validly
         * take place, but MUST NOT actually complete the checkout. To cancel
         * the checkout, add a String error message to the errors parameter.
         * The cart must not be modified, because other event handlers would then
         * see conflicting data.
         * 
         * TODO: Consider allowing handlers to "reserve" the items they need for
         * checkout (for example, the Wallet would reserve a portion of its cash).
         * This would require an additional OnCheckoutCancelledEvent to free 
         * reserved items, and it would likely require some form of ID to associate
         * the OnPreCheckoutEvent/OnCheckoutEvent/OnCheckoutCancelledEvent
         * together.
         */
        event CheckoutEvents.OnPreCheckout OnPreCheckoutEvent;

        /**
         * OnCheckoutEvent occurs for the actual checkout. This event does not
         * occur if any OnPreCheckout handler indicated an error.
         * The cart must not be modified, or else other event handlers
         * will see conflicting data.
         */
        event CheckoutEvents.OnCheckout OnCheckoutEvent;

        /**
         * OnPostCheckoutEvent occurs after the checkout is complete.
         * It is an opportunity to conduct any cleanup, such as
         * emptying the cart.
         */
        event CheckoutEvents.OnPostCheckout OnPostCheckoutEvent;


    }
}
