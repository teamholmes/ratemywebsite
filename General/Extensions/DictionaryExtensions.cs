using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.General.Extensions
{
    public static class DictionaryExtensions
    {

        public static T GetValueCheckKey<T>(this IDictionary<string, object> dict, string key)
        {

            key = key.ToUpperCheckForNull();

            var result = default(T);
            if (dict.ContainsKey(key))
            {
                result = (T)dict[key];
            }
            return result;
        }


        public static void SetValueCheckKey(this IDictionary<string, object> dict, string key, object value)
        {
            key = key.ToUpperCheckForNull();

            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }
    }



}
