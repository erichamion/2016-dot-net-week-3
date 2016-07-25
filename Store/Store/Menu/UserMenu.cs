using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Menu
{
    class UserMenu : IMenu
    {
        const String DESCRIPTION = "What would you like to do?";

        public String Description { get { return DESCRIPTION; } }

        private MenuItem[] _menuItems; 

        public UserMenu()
        {
            _menuItems = new MenuItem[]
            {
                new MenuItemListProducts(),
                new MenuItemProductCategories(),
                new MenuItemViewCart(),
                new MenuItemViewWallet(),
                new MenuItemExitPrompt(this)
            };
        }

        public MenuItem this[int i] { get { return _menuItems[i]; } }

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

        public int Length { get { return _menuItems.Length; } }


    }
}
