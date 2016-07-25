using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Menu
{
    class MenuItemExitPrompt : MenuItem
    {
        const String DESCRIPTION = "exit";
        private static char[] _allowedShortcuts = new char[] { 'e', 'x' };

        private readonly IMenu _origMenu;

        public MenuItemExitPrompt(IMenu origMenu)
        {
            _description = DESCRIPTION;
            _origMenu = origMenu;
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
            return new ConfirmationMenu(null, _origMenu);
        }
    }
}
