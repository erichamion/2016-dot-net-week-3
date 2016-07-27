﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Store
{
    class Store
    {
        public Inventory Inventory { get; }
        public User.Customer Customer { get; }

        public Store(Inventory inventory)
        {
            this.Inventory = inventory;
            this.Customer = new User.Customer();
        }
    }
}
