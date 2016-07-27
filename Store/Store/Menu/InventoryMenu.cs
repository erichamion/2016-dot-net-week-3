using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Menu
{
    class InventoryMenu : Menu
    {
        const String DESCRIPTION_FMT_STRING = "Showing items {0}-{1} (Sorted by: {2})";

        // 9 total rows max (because user has to select a 1-digit positive integer)
        // 1 row reserved for Back option
        // 1 row reserved for Sort option
        // Up to 2 rows reserved for next/previous page options
        // That leaves 5 rows remaining for products.
        const int MAX_PRODUCTS_PER_PAGE = 5;

        private List<MenuItem> _menuItems = new List<MenuItem>();
        private Store.Store _store;
        List<Store.Product> _products;
        private int _firstIndex = 0;
        private int _lastIndex;
        private SortFields _sortMethod = SortFields.NONE;

        public enum SortFields
        {
            NONE, NAME, CATEGORY, PRICE
        }

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
                    PopulateRows();
                }
            }
        }

        public override string Description
        {
            get
            {
                return String.Format(DESCRIPTION_FMT_STRING, _firstIndex + 1, _lastIndex + 1, _sortMethod);
            }
        }

        public override int Length
        {
            get
            {
                return _menuItems.Count;
            }
        }

        public InventoryMenu(Store.Store store, Stack<Menu> breadcrumbs = null) : base(breadcrumbs)
        {
            _store = store;
            _breadcrumbs.Push(this);
            RefreshInventory();
            PopulateRows();
        }

        public override MenuItem this[char c]
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

        public override MenuItem this[int i] { get { return _menuItems[i]; } }

        private void RefreshInventory()
        {
            _firstIndex = 0;
            _products = _store.Inventory.GetAllProducts();
            SortInventory();
        }

        private void SortInventory()
        {
            switch (_sortMethod)
            {
                case SortFields.NAME:
                    _products.Sort(new Comparison<Store.Product>(CompareByName));
                    break;

                case SortFields.CATEGORY:
                    _products.Sort(new Comparison<Store.Product>(CompareByCategory));
                    break;

                case SortFields.PRICE:
                    _products.Sort(new Comparison<Store.Product>(CompareByPrice));
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

        private void PopulateRows()
        {
            // Start with a clean slate
            _menuItems.Clear();

            // Get MAX_PRODUCTS_PER_PAGE rows, or all the remaining rows if
            // there are not that many.
            int productRows = Math.Min(MAX_PRODUCTS_PER_PAGE, _products.Count - _firstIndex);
            _lastIndex = _firstIndex + productRows - 1;

            // Add menu items for all the product rows
            for (int i = _firstIndex; i <= _lastIndex; i++)
            {
                bool isAvailable = _store.Inventory.CheckAvailability(_products[i]);
                String productDescription = _products[i].PrettyPrint("", "   ", isAvailable ? "Available" : "Out of stock");
                _menuItems.Add(new MenuItem(productDescription, null, () =>
                {
                    //Prompt for a number of items, then add the specified number to cart
                    throw new NotImplementedException();
                    return this;
                }));
            }

            // Sort
            _menuItems.Add(new MenuItem("Sort products", 's', () =>
            {
                //Start a menu to prompt for the type of sort
                return new SortMenu(_breadcrumbs);
            }));

            // Previous page
            if (_firstIndex > 0) {
                _menuItems.Add(new MenuItem("Previous page", 'p', () =>
                {
                    _firstIndex -= MAX_PRODUCTS_PER_PAGE;
                    // This should never result in a negative value (because we always
                    // increment and decrement by the same amount, and this item is not
                    // added if _firstIndex == 0), but be safe:
                    _firstIndex = (_firstIndex >= 0) ? _firstIndex : 0;
                    PopulateRows();
                    return this;
                }));
            }

            // Next page
            if (_lastIndex < _products.Count - 1) {
                _menuItems.Add(new MenuItem("Next page", 'n', () =>
                {
                    _firstIndex += MAX_PRODUCTS_PER_PAGE;
                // This should never equal/exceed the product count, but be safe:
                _firstIndex = (_firstIndex >= _products.Count) ? _products.Count - 1 : _firstIndex;
                    PopulateRows();
                    return this;
                }));
            }

            // Back
            _menuItems.Add(GetBackMenuItem());
        }
    }
}
