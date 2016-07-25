using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Menu
{
    class ConfirmationMenu : IMenu
    {
        const String DESCRIPTION = "Are you sure?";
        const int YES = 0;
        const int NO = 1;

        private MenuItem[] _menuItems = new MenuItem[2];

        public ConfirmationMenu(IMenu yesResult, IMenu noResult)
        {
            _menuItems[YES] = new MenuItemConfirm(yesResult, "yes", 0);
            _menuItems[NO] = new MenuItemConfirm(noResult, "no", 0);
        }

        public MenuItem this[char c]
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

        public MenuItem this[int i]
        {
            get
            {
                return _menuItems[i];
            }
        }

        public string Description
        {
            get
            {
                return DESCRIPTION;
            }
        }

        public string FullPrompt
        {
            get
            {
                String result = String.Format("{0}\n\n", Description);
                for (int i = 0; i < _menuItems.Length; i++)
                {
                    MenuItem item = _menuItems[i];
                    result = String.Format("{0}{1}: {2}\n", result, i + 1, item.Description);
                }
                return result;
            }
        }

        public int Length { get { return 2; } }
    }
}
