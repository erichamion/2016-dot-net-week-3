using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Menu
{
    class CategoryMenu : PagingMenu
    {
        private const String DESCRIPTION_FMT_STRING = "Categories: Showing {0}-{1}";

        private List<String> _categories;

        public CategoryMenu(Store.Store store, UI.IMenuDisplayer displayer, Stack<Menu> breadcrumbs) : 
            base(store, displayer, breadcrumbs)
        {
            var inventoryFilter = store.Inventory.GetAllProducts()
                .GroupBy(x => x.Category)
                .Select(g => g.Key)
                .OrderBy(x => x);
            _categories = inventoryFilter.ToList();

            Breadcrumbs.Push(this);
            PopulateRows();
        }

        public override string Description
        {
            get
            {
                return String.Format(DESCRIPTION_FMT_STRING, FirstIndex + 1, LastIndex + 1);
            }
        }

        // Back menu item
        protected override int ExtraMenuItemCount { get { return 1; } }

        protected override int ItemCount { get { return _categories.Count; } }

        protected override ICollection<MenuItem> GetExtraMenuItemsBefore()
        {
            return null;
        }

        protected override ICollection<MenuItem> GetExtraMenuItemsAfter()
        {
            return new List<MenuItem> { GetBackMenuItem() };
        }

        protected override MenuItem GetMenuItemForIndex(int index)
        {
            String category = _categories[index];
            return new MenuItem(category, null, () => new InventoryMenu(Store, Displayer, category, Breadcrumbs));
        }
    }
}
