using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Menu
{
    interface IMenu
    {
        String Description { get; }
        int Length { get; }

        MenuItem this[int i] { get; }
        MenuItem this[char c] { get; }

        String FullPrompt { get; }
    }
}
