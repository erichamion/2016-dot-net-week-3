using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram
{
    class AddOnlyCollection<T>
    {
        private readonly ICollection<T> _collection;

        public AddOnlyCollection(ICollection<T> collection)
        {
            _collection = collection;
        }

        public void Add(T item)
        {
            _collection.Add(item);
        }
    }
}
