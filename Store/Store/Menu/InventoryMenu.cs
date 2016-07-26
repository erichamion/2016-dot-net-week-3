using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Menu
{
    class InventoryMenu : Menu
    {
        const String DESCRIPTION_FMT_STRING = "Showing items {0}-{1} (Sorted by: {2})";
        const int TOTAL_LINES = 9;

        private List<MenuItem> _menuItems = new List<MenuItem>();
        private Store.Inventory _inventory;
        List<Store.Product> _products;
        private int _firstIndex = 0;
        private int _lastIndex;
        private SortFields _sortMethod = SortFields.NONE;

        private enum SortFields
        {
            NONE, NAME, CATEGORY, PRICE
        }

        public InventoryMenu(Store.Inventory inventory, Stack<Menu> breadcrumbs = null) : base(breadcrumbs)
        {
            _inventory = inventory;
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

        private void RefreshInventory()
        {
            _firstIndex = 0;
            _products = _inventory.GetAllProducts();
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
            _menuItems.Clear();
            int productRows = TOTAL_LINES;

            // Back option
            productRows--;

            // Sort
            productRows--;

            // Previous page
            if (_firstIndex > 0)
            {
                productRows--;
            }

            if (_products.Count - _firstIndex > productRows)
            {
                // Next page
                productRows--;
            }
            else
            {
                // Ensure we can't have more product rows than remaining products
                productRows = _products.Count - _firstIndex - 1;
            }

            _lastIndex = _firstIndex + productRows;
            for (int i = _firstIndex; i <= _lastIndex; i++)
            {
                //_menuItems.Add(new MenuItemAddToCart(_products[i], _inventory.CheckAvailability(_products[i])));
            }

            if (_firstIndex > 0)
            {
                //_menuItems.Add()
            }
        }
    }
}
