using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram
{
    public class Program
    {
        public const String DEFAULT_INVENTORY_FILENAME = @"store.inv";

        static void Main(string[] args)
        {
            Store.Store store = new Store.Store(DEFAULT_INVENTORY_FILENAME);

            UI.IMenuDisplayer displayer = new UI.BasicDisplayer();
            Menu.Menu menu = new Menu.UserMenu(store, displayer);

            do
            {
                menu = displayer.ShowMenuAndGetSelection(menu);
            } while (menu != null);
        }
    }
}
