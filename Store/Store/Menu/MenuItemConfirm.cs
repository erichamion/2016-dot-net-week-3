using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Menu
{
    class MenuItemConfirm : MenuItem
    {
        private readonly IMenu _result;

        public MenuItemConfirm(IMenu result, String desc, int shortcutIndex)
        {
            _result = result;
            _description = desc.ToLower();
            _shortcutIndex = shortcutIndex;
        }

        public override List<char> AllowedShortcuts
        {
            get
            {
                return (_shortcutIndex >= 0) ? 
                    new List<char> { Description[_shortcutIndex] } :
                    null;
            }
        }

        public override IMenu Execute()
        {
            return _result;
        }
    }
}
