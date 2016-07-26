using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.UI
{
    interface IMenuDisplayer
    {
        Menu.Menu ShowMenuAndGetSelection(Menu.Menu menu);
    }
}
