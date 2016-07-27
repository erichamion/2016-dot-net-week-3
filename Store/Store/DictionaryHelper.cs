using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProgram
{
    static class DictionaryHelper
    {
        public static TValue GetWithDefault<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
        {
            TValue result = default(TValue);
            bool hasValue = (key != null) ? dict.TryGetValue(key, out result) : false;
            if (!hasValue)
            {
                result = default(TValue);
            }

            return result;
        }

        public static TValue GetWithDefault<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key)
        {
            return GetWithDefault(dict, key, default(TValue));
        }
    }
}
