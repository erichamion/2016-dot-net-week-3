using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Menu
{
    class MenuItemProductCategories : MenuItem
    {
        const String DESCRIPTION = "product categories";
        private static char[] _allowedShortcuts = new char[] { 'p', 'c', 'g' };

        public MenuItemProductCategories()
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
