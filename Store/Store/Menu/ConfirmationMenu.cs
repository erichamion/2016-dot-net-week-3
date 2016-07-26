using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Menu
{
    class ConfirmationMenu : Menu
    {
        const String DESCRIPTION = "Are you sure?";
        const int YES = 0;
        const int NO = 1;

        private MenuItem[] _menuItems = new MenuItem[2];

        public ConfirmationMenu(Stack<Menu> breadcrumbs, MenuItem.Executable onYes) : base(breadcrumbs)
        {
            // Do NOT push this onto the breadcrumb stack. 

            _menuItems[YES] = new MenuItem("Yes", 'y', onYes);
            _menuItems[NO] = new MenuItem("no", 'n', () => 
            {
                return _breadcrumbs.Pop();
            });
        }

        public override MenuItem this[char c]
        {
            get
            {
                foreach (MenuItem item in _menuItems)
                {
                    if (c == item.Shortcut)
                    {
                        return item;
                    }
                }

                throw new IndexOutOfRangeException();
            }
        }

        public override MenuItem this[int i]
        {
            get
            {
                return _menuItems[i];
            }
        }

        public override string Description
        {
            get
            {
                return DESCRIPTION;
            }
        }

        public override int Length { get { return 2; } }
    }
}
