using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Menu
{
    class UserMenu : Menu
    {
        const String DESCRIPTION = "What would you like to do?";


        public override String Description { get { return DESCRIPTION; } }

        private MenuItem[] _menuItems;
        
        public UserMenu(Store.Store store, UI.IMenuDisplayer displayer, Stack<Menu> breadcrumbs = null) : 
            base(store, displayer, breadcrumbs)
        {
            Breadcrumbs.Push(this);

            _menuItems = new MenuItem[]
            {
                new MenuItem("List all products", 'l', () => 
                {
                    return new InventoryMenu(Store, Displayer, Breadcrumbs);
                }),
                new MenuItem("Product categories", 'p', () =>
                {
                    return new CategoryMenu(Store, Displayer, Breadcrumbs);
                }),
                new MenuItem("view Cart", 'c', () => 
                {
                    return new CartMenu(Store, Displayer, Breadcrumbs);
                }),
                new MenuItem("view Wallet", 'w', () =>
                {
                    return new WalletMenu(Store, Displayer, Breadcrumbs);
                }),
                new MenuItem("Exit", 'e', () => 
                {
                    return new ConfirmationMenu(Store, Displayer, Breadcrumbs, () => { return null; });
                })
            };
        }

        public override MenuItem this[int i] { get { return _menuItems[i]; } }

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

        

        public override int Length { get { return _menuItems.Length; } }


    }
}
