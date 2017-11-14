using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cry
{
    public static class DictionaryHelper
    {

        public static IEnumerable<(TKey Key1, TKey Key2, TValue Value)> GetKeyCombinations<TKey,TValue>
            (IDictionary<TKey, TValue> d,
            Func<TValue, TValue, TValue> action)
        {
            var processed = new Collection<TKey>();
            var keys = d.Keys;
            foreach (var key1 in keys)
            {
                processed.Add(key1);
                foreach (var key2 in keys)
                {
                    if (processed.Contains(key2)) continue;
                    yield return (key1, key2, action(d[key1], d[key2]));
                }
            }
        }


    }
}
