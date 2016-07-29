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
         * OnPreReserveEvent occurs just before reserving any number of
         * a single product.
         * Handlers for this event should ensure that the reservation
         * can validly take place (e.g., there are enough of the given
         * item), but MUST NOT actually reserve items. To cancel the 
         * reservation, add a String error message to the errors parameter.
         */
        event CheckoutEvents.OnPreReserve OnPreReserveEvent;

        /**
         * OnReserveEvent occurs when any number of a single product must 
         * be reserved for checkout.
         * This event does not occur if any handler for OnPreReserveEvent
         * indicated an error.
         */
        event CheckoutEvents.OnReserve OnReserveEvent;

        /**
         * OnPreReleaseEvent occurs just before any number of a single 
         * reserved product is released from reservation without checking 
         * out or ending the transaction. Handlers must NOT release the 
         * items at this time. If the specified number of units cannot be 
         * released (for example, if fewer units are already  reserved), 
         * then handlers should add to the maxRelease parameter  the 
         * maximum number of units that can be released. If the specified
         * number of units can be released, handlers can add to maxRelease 
         * any integer greater than or equal to count, but this is neither
         * required nor encouraged.
         */
        event CheckoutEvents.OnPreRelease OnPreReleaseEvent;

        /**
         * OnReleaseEvent occurs when any number of a single reserved 
         * product is to be released without checking out or ending the 
         * transaction. If any OnPreReleaseEvent handler indicated that
         * zero units can be released, then this event does not occur.
         */
        event CheckoutEvents.OnRelease OnReleaseEvent;


        /**
         * OnPreCheckoutEvent occurs just before a checkout.
         * Handlers for this event should ensure that the checkout can validly
         * take place, but MUST NOT actually complete the checkout. To cancel
         * the checkout, add a String error message to the errors parameter.
         * The cart must not be modified, because other event handlers would then
         * see conflicting data.
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
         * OnTransactionEnded occurs after the checkout is complete.
         * It is an opportunity to conduct any cleanup, such as
         * emptying the cart.
         */
        event CheckoutEvents.OnTransactionEnded OnTransactionEndedEvent;


    }
}
