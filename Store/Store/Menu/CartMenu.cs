using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreProgram.Store;
using StoreProgram.UI;

namespace StoreProgram.Menu
{
    class CartMenu : BaseInventoryMenu
    {
        private const String DISPLAY_FMT_STRING = "Shopping Cart: {0}-{1} of {2} unique items (Sorted by: {3}). Total cost {4:C}";

        private MenuItem _checkoutMenuItem;

        public CartMenu(Store.Store store, IMenuDisplayer displayer, Stack<Menu> breadcrumbs = null) : 
            base(store, displayer, breadcrumbs)
        {
            _checkoutMenuItem = new MenuItem("Checkout", 'c', () => { throw new NotImplementedException(); });

            RefreshInventory();
        }

        public override string Description
        {
            get
            {
                return String.Format(DISPLAY_FMT_STRING, FirstIndex + 1, LastIndex + 1, ItemCount, SortMethod, Store.Customer.Cart.GetTotalPrice());
            }
        }

        protected override List<Product> GetInventory()
        {
            return Store.Customer.Cart.GetAllProducts();
        }

        protected override string GetMenuItemDescription(Product product)
        {
            int productCount = Store.Customer.Cart.GetProductCount(product);
            double extendedPrice = product.Price * productCount;
            return String.Format("Name:           {0}\n   Price Per Unit: {1:C}\n   Units:          {2}\n   Extended Price: {3:C}",
                product.Name, product.Price, productCount, extendedPrice);
        }

        protected override int GetExtraMenuItemCount()
        {
            // One extra menu item: Checkout
            return base.GetExtraMenuItemCount() + 1;
        }

        protected override void AppendMenuItems(AddOnlyCollection<MenuItem> menuItems)
        {

            menuItems.Add(_checkoutMenuItem);
            base.AppendMenuItems(menuItems);
        }

        protected override MenuItem.Executable GetProductAction(Product product)
        {
            return () => 
            {
                int count = -1;
                do
                {
                    String response = Displayer.PromptUserAndGetResponse(String.Format("How many units of '{0}' do you want to remove?", product.Name));
                    if (!int.TryParse(response, out count))
                    {
                        count = -1;
                    }
                    if (count < 0)
                    {
                        Displayer.ShowMessage("Please enter a valid non-negative number.");
                    }
                } while (count < 0);

                if (count > 0)
                {
                    Store.Customer.Cart.SubtractProduct(product, count);
                    Displayer.ShowMessage(String.Format("Removing {0} units of '{1}' from cart.\nPress any key...", count, product.Name));
                    Console.ReadKey(true);
                    RefreshInventory();
                }
                // else count == 0, do nothing

                return this;
            };
        }
    }
}
