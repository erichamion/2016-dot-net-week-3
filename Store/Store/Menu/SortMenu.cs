using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Menu
{
    class SortMenu : Menu
    {
        const String DESCRIPTION = "Sort by:";

        private List<MenuItem> _menuItems = new List<MenuItem>();

        /**
         * The last item pushed onto the breadcrumb stack MUST be an
         * instance of InventoryMenu. If not, will throw an
         * InvalidCastException.
         */
        public SortMenu(Stack<Menu> breadcrumbs) : base(breadcrumbs)
        {
            // Do NOT push self onto the breadcrumb stack.

            InitializeMenuItems((InventoryMenu)breadcrumbs.Peek());
        }

        public override MenuItem this[char c]
        {
            get
            {
                // No shortcut keys
                throw new IndexOutOfRangeException();
            }
        }

        public override MenuItem this[int i] { get { return _menuItems[i]; } }

        public override string Description { get { return DESCRIPTION; } }

        public override int Length { get { return _menuItems.Count; } }

        private void InitializeMenuItems(InventoryMenu returnMenu)
        {
            // Based on answer to "How do I enumerate an enum?" at
            // http://stackoverflow.com/questions/105372/how-do-i-enumerate-an-enum
            foreach (InventoryMenu.SortFields sortField in Enum.GetValues(typeof(InventoryMenu.SortFields)))
            {
                _menuItems.Add(new MenuItem(sortField.ToString(), null, () => 
                {
                    returnMenu.SortMethod = sortField;
                    return returnMenu;
                }));
            }
        }
    }
}
