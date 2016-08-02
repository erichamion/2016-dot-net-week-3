using System;
using StoreProgram.Menu;

namespace StoreProgram.UI
{
    public class BasicDisplayer : IMenuDisplayer
    {
        public Menu.Menu ShowMenuAndGetSelection(Menu.Menu menu)
        {
            bool success = false;
            Menu.Menu result = null;
            do
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine(menu.FullPrompt);
                    char inp = Console.ReadKey(true).KeyChar;

                    MenuItem chosenItem;
                    if (Char.IsDigit(inp))
                    {
                        chosenItem = menu[int.Parse(inp.ToString()) - 1];
                    }
                    else
                    {
                        chosenItem = menu[inp];
                    }
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

        public string PromptUserAndGetResponse(string prompt)
        {
            Console.WriteLine(prompt);
            Console.Write(" > ");
            return Console.ReadLine();
        }

        public void ShowMessage(String message)
        {
            Console.WriteLine(message);
        }
    }
}
