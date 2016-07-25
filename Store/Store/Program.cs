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
            Menu.IMenu menu = new Menu.UserMenu();
            UI.IMenuDisplayer displayer = new UI.BasicDisplayer();

            do
            {
                menu = displayer.ShowMenuAndGetSelection(menu);
            } while (menu != null);
        }
    }
}
