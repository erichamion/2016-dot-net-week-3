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
        private Store.Store _store;

        public UserMenu(Store.Store store, Stack<Menu> breadcrumbs = null) : base(breadcrumbs)
        {
            _breadcrumbs.Push(this);

            _menuItems = new MenuItem[]
            {
                new MenuItem("List all products", 'l', () => 
                {
                    return new InventoryMenu(_store, _breadcrumbs);
                }),
                new MenuItem("Product categories", 'p', () =>
                {
                    throw new NotImplementedException();
                }),
                new MenuItem("view Cart", 'c', () => 
                {
                    throw new NotImplementedException();
                }),
                new MenuItem("view Wallet", 'w', () =>
                {
                    throw new NotImplementedException();
                }),
                new MenuItem("Exit", 'e', () => 
                {
                    return new ConfirmationMenu(_breadcrumbs, () => { return null; });
                })
            };

            _store = store;
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
