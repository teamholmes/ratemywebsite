using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.General.Extensions
{
    public static class DoubleExtensions
    {


        public static Boolean IsDouble(this string input)
        {
            bool isDouble = false;
            if (!String.IsNullOrEmpty(input))
            {
                double n;
                isDouble = double.TryParse(input, out n);
            }

            return isDouble;

        }

    }
}
