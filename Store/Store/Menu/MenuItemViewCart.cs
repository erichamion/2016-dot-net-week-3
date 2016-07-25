using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Menu
{
    class MenuItemViewCart : MenuItem
    {
        const String DESCRIPTION = "view cart";
        private static char[] _allowedShortcuts = new char[] { 'c', 'v', 'w', 'r', 't' };

        public MenuItemViewCart()
        {
            _description = DESCRIPTION;
            _shortcutIndex = _description.IndexOf(_allowedShortcuts[0]);
        }

        public override List<char> AllowedShortcuts
        {
            get
            {
                return new List<char>(_allowedShortcuts);
            }
        }

        public override IMenu Execute()
        {
            throw new NotImplementedException();
        }
    }
}
