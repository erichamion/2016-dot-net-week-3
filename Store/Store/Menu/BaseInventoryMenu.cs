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

        protected List<Store.Product> Products { get; set; }

        public BaseInventoryMenu(Store.Store store, UI.IMenuDisplayer displayer, Stack<Menu> breadcrumbs = null) : 
            base(store, displayer, breadcrumbs)
        {
            Breadcrumbs.Push(this);
            //RefreshInventory();
        }

        protected abstract List<Store.Product> GetInventory();
        protected abstract String GetMenuItemDescription(Store.Product product);
        protected abstract MenuItem.Executable GetProductAction(Store.Product product);

        protected sealed override int ItemCount { get { return Products.Count; } }

        // Sort menu item, and Back menu item
        protected sealed override int ExtraMenuItemCount { get { return GetExtraMenuItemCount() + 2; } }

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
                    return new SortMenu(Store, Displayer, Breadcrumbs);
                })
            };
        }

        protected sealed override ICollection<MenuItem> GetExtraMenuItemsAfter()
        {
            // Add any items from subclasses
            List<MenuItem> menuItems = new List<MenuItem>();
            AddOnlyCollection<MenuItem> addOnlyCollection = new AddOnlyCollection<MenuItem>(menuItems);
            AppendMenuItems(addOnlyCollection);

            // Add the Back option last
            menuItems.Add(GetBackMenuItem());
            return menuItems;
        }

        /**
        * Subclasses that add extra MenuItems must override this method to
        * indicate the number of MenuItems they add. Indirect subclasses
        * MUST call base.GetExtraMenuItemCount() and add their own count to
        * the results of that call. Direct subclasses do not need to call
        * the base method, but they are encouraged to do so for consistency.
        */
        protected virtual int GetExtraMenuItemCount()
        {
            return 0;
        }

        /**
         * Subclasses that need to add extra MenuItems should do so by
         * overriding this method. Subclasses are encouraged to call the
         * base method. Calling the base method is not necessary for 
         * direct subclasses, but it is needed for indirect subclasses.
         */
        protected virtual void AppendMenuItems(AddOnlyCollection<MenuItem> menuItems)
        {
            // Do nothing
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

        
        
        //private void AddProductToCart(Store.Product product)
        //{
        //    int count = -1;
        //    do
        //    {
        //        String response = Displayer.PromptUserAndGetResponse(String.Format("How many units of '{0}' would you like to add to the cart?", product.Name));
        //        bool isValid = int.TryParse(response, out count);
        //        if (!isValid) count = -1;

        //        if (count < 0)
        //        {
        //            Displayer.ShowMessage("Please enter a valid positive integer.");
        //        }
        //    } while (count < 0);

        //    String reserveResult;
        //    if (Store.ReserveProducts(product, count, out reserveResult))
        //    {
        //        // Success
        //        reserveResult = String.Format("Added {0} units of '{1}' to cart.", count, product.Name);
        //    }
        //    // else reserveResult has an error message.

        //    Displayer.ShowMessage(reserveResult);
            
        //}

        public enum SortFields
        {
            NONE, NAME, CATEGORY, PRICE
        }


    }
}
