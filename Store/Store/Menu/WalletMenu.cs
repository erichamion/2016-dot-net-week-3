using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreProgram.Store;
using StoreProgram.UI;

namespace StoreProgram.Menu
{
    class WalletMenu : Menu
    {
        private const String DESCRIPTION_FMT_STRING = "Wallet contains {0:C}";
        private readonly MenuItem[] _menuItems;

        public WalletMenu(Store.Store store, IMenuDisplayer displayer, Stack<Menu> breadcrumbs = null) 
            : base(store, displayer, breadcrumbs)
        {
            Breadcrumbs.Push(this);

            _menuItems = new MenuItem[]
            {
                new MenuItem("Add funds", 'a', () =>
                {
                    double amount;
                    do {
                        amount = -1.0;
                        String response = Displayer.PromptUserAndGetResponse("How much do you want to add?");
                        if (!double.TryParse(response, out amount))
                        {
                            amount = -1.0;
                        }
                        if (amount < 0)
                        {
                            Displayer.ShowMessage("Please enter a valid non-negative number.");
                        }
                    } while (amount < 0);

                    Store.Customer.Wallet.Fill(amount);
                    Displayer.ShowMessage(String.Format("{0:C} has been deducted from your bank account and added to the wallet.\nThank you.", amount));
                    Console.ReadKey(true);
                    return this;
                }),
                GetBackMenuItem()
            };
        }

        public override MenuItem this[char c]
        {
            get
            {
                foreach (MenuItem menuItem in _menuItems)
                {
                    if (menuItem.Shortcut == c)
                    {
                        return menuItem;
                    }
                }
                throw new ArgumentOutOfRangeException();
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
                return String.Format(DESCRIPTION_FMT_STRING, Store.Customer.Wallet.Cash);
            }
        }

        public override int Length
        {
            get
            {
                return _menuItems.Count();
            }
        }
    }
}
