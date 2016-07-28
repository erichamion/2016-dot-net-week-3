using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Menu
{
    abstract class PagingMenu : Menu
    {
        // 9 total rows max (because user has to select a 1-digit positive integer)
        // 1 row reserved for Back option
        // 1 row reserved for Sort option
        // Up to 2 rows reserved for next/previous page options
        // That leaves 7 rows remaining for products and extra menu items.
        protected const int MAX_ITEMS_PER_PAGE = 7;

        private readonly List<MenuItem> _menuItems = new List<MenuItem>();

        public sealed override int Length
        {
            get
            {
                return _menuItems.Count;
            }
        }

        protected int FirstIndex { get; private set; }

        protected int LastIndex { get; private set; }

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

        protected abstract int ItemCount { get; }
        protected abstract int ExtraMenuItemCount { get; }
        protected abstract MenuItem GetMenuItemForIndex(int index);

        // Shown before the previous/next items
        protected abstract ICollection<MenuItem> GetExtraMenuItemsBefore();

        // Shown after the previous/next items
        protected abstract ICollection<MenuItem> GetExtraMenuItemsAfter();

        public PagingMenu(Store.Store store, UI.IMenuDisplayer displayer, Stack<Menu> breadcrumbs = null) : 
            base(store, displayer, breadcrumbs)
        { }

        protected void ResetFirstIndex()
        {
            FirstIndex = 0;
        }

        protected void PopulateRows()
        {
            // Start with a clean slate
            _menuItems.Clear();

            // Get maximum allowed rows, or all the remaining rows if
            // there are not that many.
            int maxItemRows = MAX_ITEMS_PER_PAGE - ExtraMenuItemCount;
            int itemRows = Math.Min(maxItemRows, ItemCount - FirstIndex);
            LastIndex = FirstIndex + itemRows - 1;

            // Add menu items for all the product rows
            for (int i = FirstIndex; i <= LastIndex; i++)
            {
                _menuItems.Add(GetMenuItemForIndex(i));
            }

            // Extra items shown before the next/previous options
            ICollection<MenuItem> beforeItems = GetExtraMenuItemsBefore();
            if (beforeItems != null && beforeItems.Count > 0)
            {
                _menuItems.AddRange(beforeItems);
            }

            // Previous page
            if (FirstIndex > 0)
            {
                _menuItems.Add(new MenuItem("Previous page", 'p', () =>
                {
                    FirstIndex -= maxItemRows;
                    // This should never result in a negative value (because we always
                    // increment and decrement by the same amount, and this item is not
                    // added if _firstIndex == 0), but be safe:
                    FirstIndex = (FirstIndex >= 0) ? FirstIndex : 0;
                    PopulateRows();
                    return this;
                }));
            }

            // Next page
            if (LastIndex < ItemCount - 1)
            {
                _menuItems.Add(new MenuItem("Next page", 'n', () =>
                {
                    FirstIndex += maxItemRows;
                    // This should never equal/exceed the product count, but be safe:
                    FirstIndex = (FirstIndex >= ItemCount) ? ItemCount - 1 : FirstIndex;
                    PopulateRows();
                    return this;
                }));
            }

            // Extra items shown after the next/previous options
            ICollection<MenuItem> afterItems = GetExtraMenuItemsAfter();
            if (afterItems != null && afterItems.Count > 0)
            {
                _menuItems.AddRange(afterItems);
            }
        }

    }
}
