using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Menu
{
    abstract class BaseInventoryMenu : PagingMenu
    {
        
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

        protected sealed override int ItemCount { get { return Products.Count; } }

        // Sort menu item, and Back menu item
        protected sealed override int ExtraMenuItemCount { get { return 2; } }

        protected sealed override MenuItem GetMenuItemForIndex(int index)
        {
            Store.Product product = Products[index];
            bool isAvailable = Store.Inventory.CheckAvailability(product);
            return new MenuItem(GetMenuItemDescription(product), GetShortcutKey(product), GetProductAction(product));
        }

        protected sealed override ICollection<MenuItem> GetExtraMenuItemsBefore()
        {
            return new List<MenuItem>
            {
                new MenuItem("Sort products", 's', () =>
                {
                    //Start a menu to prompt for the type of sort
                    return new SortMenu(Breadcrumbs);
                })
            };
        }

        protected sealed override ICollection<MenuItem> GetExtraMenuItemsAfter()
        {
            return new List<MenuItem> { GetBackMenuItem() };
        }


        protected void RefreshInventory()
        {
            ResetFirstIndex();
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
