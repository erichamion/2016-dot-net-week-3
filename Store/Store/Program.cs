using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreProgram.UI;

namespace StoreProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            Store.Inventory inventory = new Store.Inventory();
            Store.Product[] products = new Store.Product[]
            {
                new Store.Product("Self-sealing stem bolt (144 pack)", "Unknown", 35.99),
                new Store.Product("Reverse-ratcheting routing planer", "Unknown", 12.97),
                new Store.Product("Yamok sauce (10 wrappages)", "Food", 6.99),
                new Store.Product("Gagh", "Food", 0.03),
                new Store.Product("Klingon bloodwine", "Food", 10.00),
                new Store.Product("Romulan ale", "Food", 3599.00),
                new Store.Product("Alphanumeric sequencer", "Engineering Tools", 531.75),
                new Store.Product("Quantum flux regulator", "Engineering Tools", 997.97),
                new Store.Product("Thermal regulator", "Engineering Tools", 222.22)
            };
            int[] productCounts = new int[]
            {
                12000,
                3000,
                1000,
                1000,
                300,
                0,
                74,
                27,
                42
            };
            for (int i = 0; i < products.Length; i++)
            {
                Store.Product product = products[i];
                int count = productCounts[i];
            
                inventory.AddProduct(product, count);
            }
            Store.Store store = new Store.Store(inventory);

            UI.IMenuDisplayer displayer = new BasicDisplayer();
            Menu.Menu menu = new Menu.UserMenu(store, displayer);

            do
            {
                menu = displayer.ShowMenuAndGetSelection(menu);
            } while (menu != null);
        }
    }
}
