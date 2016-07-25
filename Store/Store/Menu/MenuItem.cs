using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Menu
{
    abstract class MenuItem
    {
        protected String _description = "temp";
        protected int _shortcutIndex = 0;

        public String Description
        {
            get
            {
                if (_shortcutIndex < 0)
                {
                    // No shortcut. Return the unaltered description
                    return _description;
                }

                // Capitalize the shortcut character
                String startString = (_shortcutIndex > 0) ?
                    _description.Substring(0, _shortcutIndex).ToLower() :
                    String.Empty;
                char shortcut = Char.ToUpper(_description[_shortcutIndex]);
                String endString = (_shortcutIndex < _description.Length - 1) ?
                    _description.Substring(_shortcutIndex + 1) :
                    String.Empty;

                return String.Format("{0}{1}{2}", startString, shortcut, endString);
            }
        }
        public virtual char? Shortcut
        {
            get { return (_shortcutIndex >= 0) ? _description[_shortcutIndex] :  (char?) null; }
            set
            {
                // Make a non-null, lowercase copy for convenience
                char effectiveValue = value ?? '\0';    
                effectiveValue = Char.ToLower(effectiveValue);
                if (value == null || !AllowedShortcuts.Contains(effectiveValue))
                {
                    _shortcutIndex = -1;
                }
                else
                {
                    _shortcutIndex = _description.ToLower().IndexOf(effectiveValue);
                }
            }
        }

        /**
         * Implementations are encouraged to return a new List or a copy of
         * an internally stored List in order to avoid unexpected changes.
         * All entries should be lowercase.
         */
        public abstract List<char> AllowedShortcuts { get; }


        public abstract IMenu Execute();
    }
}
