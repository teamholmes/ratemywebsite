using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.General.Conversion
{
    public static class EnumConverter
    {
        public static IEnumerable<TOut> ConvertEnumItemsTo<TEnumType, TOut>(Func<KeyValuePair<int, string>, TOut> func)
        {
            foreach (var status in Enum.GetValues(typeof(TEnumType)))
            {
                yield return func(new KeyValuePair<int, string>(Convert.ToInt32(status), status.ToString()));
            }
        }
    }
}
