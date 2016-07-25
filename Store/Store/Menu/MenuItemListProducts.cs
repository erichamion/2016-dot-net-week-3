using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Menu
{
    class MenuItemListProducts : MenuItem
    {
        const String DESCRIPTION = "list all products";
        private static char[] _allowedShortcuts = new char[] { 'l', 'p', 'a', 's' };

        public MenuItemListProducts()
        {
            _description = DESCRIPTION;
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
