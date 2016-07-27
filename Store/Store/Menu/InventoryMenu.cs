using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreProgram.Store;

namespace StoreProgram.Menu
{
    class InventoryMenu : BaseInventoryMenu
    {
        const String DESCRIPTION_FMT_STRING = "Showing items {0}-{1} (Sorted by: {2})";

        public override string Description
        {
            get
            {
                return String.Format(DESCRIPTION_FMT_STRING, FirstIndex + 1, LastIndex + 1, SortMethod);
            }
        }

        public InventoryMenu(Store.Store store, UI.IMenuDisplayer displayer, Stack<Menu> breadcrumbs = null) :
            base(store, displayer, breadcrumbs)
        {
            // Do nothing. It's all handled in the base class.
        }

        protected override List<Product> GetInventory()
        {
            return Store.Inventory.GetAllProducts();
        }

        protected override string GetMenuItemDescription(Product product)
        {
            bool isAvailable = Store.Inventory.CheckAvailability(product);
            return product.PrettyPrint("", "   ", isAvailable ? "Available" : "Out of stock");
        }

        protected override MenuItem.Executable GetProductAction(Product product)
        {
            return new MenuItem.Executable(() => 
            {
                //Prompt for a number of items, then add the specified number to cart
                AddProductToCart(product);
                Displayer.ShowMessage("Press any key to continue...");
                Console.ReadKey();

                return this;
            });
        }

        private void AddProductToCart(Product product)
        {
            int count = -1;
            do
            {
                String response = Displayer.PromptUserAndGetResponse(String.Format("How many units of '{0}' would you like to add to the cart?", product.Name));
                bool isValid = int.TryParse(response, out count);
                if (!isValid) count = -1;

                if (count < 0)
                {
                    Displayer.ShowMessage("Please enter a valid positive integer (or 0 to abort).");
                }
            } while (count < 0);

            if (count == 0)
            {
                Displayer.ShowMessage("Zero units selected. Aborting.");
            }
            else
            {
                Displayer.ShowMessage(String.Format("Adding {0} units of '{1}' to cart.", count, product.Name));
                Store.Customer.Cart.AddProduct(product, count);
            }
        }
    }
}
