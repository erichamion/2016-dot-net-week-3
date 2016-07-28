using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Menu
{
    abstract class Menu
    {
        public abstract String Description { get; }
        public abstract int Length { get; }

        public abstract MenuItem this[int i] { get; }
        public abstract MenuItem this[char c] { get; }
        protected Stack<Menu> Breadcrumbs { get; }
        protected Store.Store Store { get; }
        protected UI.IMenuDisplayer Displayer { get; }


        public Menu(Store.Store store, UI.IMenuDisplayer displayer, Stack<Menu> breadcrumbs = null)
        {
            Store = store;
            Displayer = displayer;
            Breadcrumbs = breadcrumbs ?? new Stack<Menu>();
        }


        public String FullPrompt
        {
            get
            {
                String result = String.Format("{0}\n\n", Description);
                for (int i = 0; i < this.Length; i++)
                {
                    MenuItem item = this[i];
                    result = String.Format("{0}{1}: {2}\n", result, i + 1, item.Description);
                }
                return result;
            }
        }

        

        /**
         * The current Menu is assumed to have been pushed onto _breadcrumbs.
         */
        protected MenuItem GetBackMenuItem(String desc = "Back to previous menu", char? shortcut = 'b')
        {
            return new MenuItem(desc, shortcut, () =>
            {
                Breadcrumbs.Pop();
                return (Breadcrumbs.Count > 0) ? Breadcrumbs.Peek() : null;
            });
        }
    }
}
