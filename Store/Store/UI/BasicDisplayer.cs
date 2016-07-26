using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Menu;

namespace Store.UI
{
    class BasicDisplayer : IMenuDisplayer
    {
        public Menu.Menu ShowMenuAndGetSelection(Menu.Menu menu)
        {
            Console.Clear();
            Console.WriteLine(menu.FullPrompt);
            char inp = Console.ReadKey(true).KeyChar;
            bool success = false;
            Menu.Menu result = null;
            do
            {
                try
                {
                    Menu.MenuItem chosenItem = menu[Char.IsDigit(inp) ? int.Parse(inp.ToString()) - 1 : inp];
                    Console.WriteLine(chosenItem.Description);
                    result = chosenItem.Execute();
                    success = true;
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine("Invalid selection. Press a key...\n\n");
                    Console.ReadKey();
                    // success remains false
                }
            } while (!success);

            return result;
        }
    }
}
