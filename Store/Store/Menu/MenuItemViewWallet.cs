using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Menu
{
    class MenuItemViewWallet : MenuItem
    {
        const String DESCRIPTION = "view wallet";
        private static char[] _allowedShortcuts = new char[] { 'w', 'v', 'l', 't'};

        public MenuItemViewWallet()
        {
            _description = DESCRIPTION;
            Shortcut = _allowedShortcuts[0];
        }

        public override List<char> AllowedShortcuts
        {
            get
            {
                return new List<char>(_allowedShortcuts);
            }
        }

        public override char? Shortcut
        {
            get
            {
                return base.Shortcut;
            }

            set
            {
                if (value == 'w')
                {
                    // We want the second w (the start of the word 'wallet'),
                    // not the first w. The second occurrence happens to also be
                    // the last occurrence
                    _shortcutIndex = _description.LastIndexOf(value ?? 'w');
                }
                else
                {
                    base.Shortcut = value;
                }
            }
        }

        public override IMenu Execute()
        {
            throw new NotImplementedException();
        }
    }
}
