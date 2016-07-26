using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    class Program
    {
        static void Main(string[] args)
        {
            Store.Inventory inventory = new Store.Inventory(Store.Inventory.AccessLevels.USER);
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
            foreach (Store.Product product in products)
            {
                inventory.AddProduct(product);
            }
            Store.Store store = new Store.Store(inventory);

            Menu.Menu menu = new Menu.UserMenu(store);
            UI.IMenuDisplayer displayer = new UI.BasicDisplayer();

            do
            {
                menu = displayer.ShowMenuAndGetSelection(menu);
            } while (menu != null);
        }
    }
}
