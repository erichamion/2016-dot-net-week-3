using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            Store.Store store = new Store.Store();

            UI.IMenuDisplayer displayer = new UI.BasicDisplayer();
            Menu.Menu menu = new Menu.UserMenu(store, displayer);

            do
            {
                menu = displayer.ShowMenuAndGetSelection(menu);
            } while (menu != null);
        }
    }
}
