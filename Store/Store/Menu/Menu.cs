using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Menu
{
    abstract class Menu
    {
        public abstract String Description { get; }
        public abstract int Length { get; }

        public abstract MenuItem this[int i] { get; }
        public abstract MenuItem this[char c] { get; }

        protected readonly Stack<Menu> _breadcrumbs;

        public Menu(Stack<Menu> breadcrumbs = null)
        {
            _breadcrumbs = breadcrumbs ?? new Stack<Menu>();
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

        protected Menu PushBreadcrumbAndExecuteMenuItem(MenuItem menuItem)
        {
            _breadcrumbs.Push(this);
            return menuItem.Execute();
        }
    }
}
