using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Menu
{
    abstract class BaseInventoryMenu : Menu
    {
        // 9 total rows max (because user has to select a 1-digit positive integer)
        // 1 row reserved for Back option
        // 1 row reserved for Sort option
        // Up to 2 rows reserved for next/previous page options
        // That leaves 5 rows remaining for products.
        const int MAX_PRODUCTS_PER_PAGE = 5;

        private readonly List<MenuItem> _menuItems = new List<MenuItem>();
        private SortFields _sortMethod = SortFields.NONE;

        public SortFields SortMethod
        {
            get { return _sortMethod; }
            set
            {
                bool isChanged = (_sortMethod != value);
                _sortMethod = value;
                if (isChanged)
                {
                    RefreshInventory();
                }
            }
        }

        public sealed override int Length
        {
            get
            {
                return _menuItems.Count;
            }
        }

        protected int FirstIndex { get; private set; }

        protected int LastIndex { get; private set; }

        protected Store.Store Store { get; }

        protected List<Store.Product> Products { get; set; }

        protected UI.IMenuDisplayer Displayer { get; }



        public BaseInventoryMenu(Store.Store store, UI.IMenuDisplayer displayer, Stack<Menu> breadcrumbs = null) : base(breadcrumbs)
        {
            Store = store;
            Displayer = displayer;
            Breadcrumbs.Push(this);
            RefreshInventory();
        }

        protected abstract List<Store.Product> GetInventory();
        protected abstract String GetMenuItemDescription(Store.Product product);
        protected abstract MenuItem.Executable GetProductAction(Store.Product product);


        public sealed override MenuItem this[char c]
        {
            get
            {
                foreach (MenuItem item in _menuItems)
                {
                    if (item.Shortcut == c) return item;
                }
                throw new IndexOutOfRangeException();
            }
        }

        public sealed override MenuItem this[int i] { get { return _menuItems[i]; } }

        protected void RefreshInventory()
        {
            FirstIndex = 0;
            Products = GetInventory();
            SortInventory();
            PopulateRows();
        }

        
        private void SortInventory()
        {
            switch (_sortMethod)
            {
                case SortFields.NAME:
                    Products.Sort(new Comparison<Store.Product>(CompareByName));
                    break;

                case SortFields.CATEGORY:
                    Products.Sort(new Comparison<Store.Product>(CompareByCategory));
                    break;

                case SortFields.PRICE:
                    Products.Sort(new Comparison<Store.Product>(CompareByPrice));
                    break;

                case SortFields.NONE:
                default:
                    // Do nothing
                    break;
            }
        }

        private int CompareByName(Store.Product x, Store.Product y)
        {
            return String.Compare(x.Name, y.Name);
        }

        private int CompareByCategory(Store.Product x, Store.Product y)
        {
            return String.Compare(x.Category, y.Category);
        }

        private int CompareByPrice(Store.Product x, Store.Product y)
        {
            return (int)(100 * (x.Price - y.Price));
        }

        
        protected virtual char? GetShortcutKey(Store.Product product)
        {
            return null;
        }

        
        private void PopulateRows()
        {
            // Start with a clean slate
            _menuItems.Clear();

            // Get MAX_PRODUCTS_PER_PAGE rows, or all the remaining rows if
            // there are not that many.
            int productRows = Math.Min(MAX_PRODUCTS_PER_PAGE, Products.Count - FirstIndex);
            LastIndex = FirstIndex + productRows - 1;

            // Add menu items for all the product rows
            for (int i = FirstIndex; i <= LastIndex; i++)
            {
                Store.Product product = Products[i];
                bool isAvailable = Store.Inventory.CheckAvailability(product);
                //String productDescription = product.PrettyPrint("", "   ", isAvailable ? "Available" : "Out of stock");
                String productDescription = GetMenuItemDescription(product);
                _menuItems.Add(new MenuItem(GetMenuItemDescription(product), GetShortcutKey(product), GetProductAction(product)));
            }

            // Sort
            _menuItems.Add(new MenuItem("Sort products", 's', () =>
            {
                //Start a menu to prompt for the type of sort
                return new SortMenu(Breadcrumbs);
            }));

            // Previous page
            if (FirstIndex > 0) {
                _menuItems.Add(new MenuItem("Previous page", 'p', () =>
                {
                    FirstIndex -= MAX_PRODUCTS_PER_PAGE;
                    // This should never result in a negative value (because we always
                    // increment and decrement by the same amount, and this item is not
                    // added if _firstIndex == 0), but be safe:
                    FirstIndex = (FirstIndex >= 0) ? FirstIndex : 0;
                    PopulateRows();
                    return this;
                }));
            }

            // Next page
            if (LastIndex < Products.Count - 1) {
                _menuItems.Add(new MenuItem("Next page", 'n', () =>
                {
                    FirstIndex += MAX_PRODUCTS_PER_PAGE;
                    // This should never equal/exceed the product count, but be safe:
                    FirstIndex = (FirstIndex >= Products.Count) ? Products.Count - 1 : FirstIndex;
                    PopulateRows();
                    return this;
                }));
            }

            // Back
            _menuItems.Add(GetBackMenuItem());
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

        public enum SortFields
        {
            NONE, NAME, CATEGORY, PRICE
        }


    }
}
