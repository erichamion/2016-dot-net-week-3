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
        private const String DESCRIPTION_FMT_STRING = "Showing items{0} {1}-{2} (Sorted by: {3})";
        
        private String _categoryFilterString;

        private String FilterDescriptionString
        {
            get
            {
                return (_categoryFilterString != null) ?
                    String.Format(" in Category '{0}'", _categoryFilterString) :
                    String.Empty;
            }
        }

        public override string Description
        {
            get
            {
                return String.Format(DESCRIPTION_FMT_STRING, FilterDescriptionString, FirstIndex + 1, LastIndex + 1, SortMethod);
            }
        }

        public InventoryMenu(Store.Store store, UI.IMenuDisplayer displayer, Stack<Menu> breadcrumbs = null) :
            this(store, displayer, null, breadcrumbs)
        { }

        public InventoryMenu(Store.Store store, UI.IMenuDisplayer displayer, String categoryFilter, Stack<Menu> breadcrumbs = null) :
            base(store, displayer, breadcrumbs)
        {
            _categoryFilterString = categoryFilter;
            RefreshInventory();

            // No need to push self onto breadcrumbs - this is done in BaseInventoryMenu
        }

        protected override List<Product> GetInventory()
        {
            List<Product> allProducts = Store.Inventory.GetAllProducts();
            List<Product> products;
            if (_categoryFilterString != null)
            {
                var filter = allProducts.Where(x => x.Category.Equals(_categoryFilterString));
                products = filter.ToList();
            }
            else
            {
                products = allProducts;
            }
            return products;
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

        private void AddProductToCart(Store.Product product)
        {
            int count = -1;
            do
            {
                String response = Displayer.PromptUserAndGetResponse(String.Format("How many units of '{0}' would you like to add to the cart?", product.Name));
                bool isValid = int.TryParse(response, out count);
                if (!isValid) count = -1;

                if (count < 0)
                {
                    Displayer.ShowMessage("Please enter a valid positive integer.");
                }
            } while (count < 0);

            String reserveResult;
            if (Store.ReserveProducts(product, count, out reserveResult))
            {
                // Success
                reserveResult = String.Format("Added {0} units of '{1}' to cart.", count, product.Name);
            }
            // else reserveResult has an error message.

            Displayer.ShowMessage(reserveResult);

        }
    }
}
