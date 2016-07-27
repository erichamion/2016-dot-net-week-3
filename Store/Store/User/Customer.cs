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

        public Customer()
        {
            Cart = new ShoppingCart();
        }
    }
}
