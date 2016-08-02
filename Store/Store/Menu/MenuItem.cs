using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram.Menu
{
    public class MenuItem
    {
        public String Description { get; }
        public char? Shortcut { get; }

        public delegate Menu Executable();
        private Executable _onExecute;

        public MenuItem(String description, char? shortcut, Executable onExecute)
        {
            _onExecute = onExecute;
            Description = description;
            Shortcut = shortcut;
        }
        


        public Menu Execute()
        {
            return _onExecute();
        }
    }
}
