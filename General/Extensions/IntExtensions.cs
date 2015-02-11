using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.General.Extensions
{
    public static class IntExtensions
    {


        public static string Encrypt(this int input)
        {

            return Encryption.Encryption.EncryptTripleDES(input.ToString(), true, true);
        }

        public static string Encrypt(this int input, Boolean addrandomcharacter)
        {

            return Encryption.Encryption.EncryptTripleDES(input.ToString(), true, addrandomcharacter);
        }

        public static bool IsOdd(this int i)
        {
            return (i % 2) != 0;
        }

        public static bool IsEven(this int i)
        {
            return (i % 2) == 0;
        }


        public static string nThOrdinal(this int position)
        {

            int j = position % 10;
            int k = position % 100;

            if (j == 1 && k != 11) return "st";
            else if (j == 2 && k != 12) return "nd";
            else if (j == 3 && k != 13) return "rd";

            return "th";


        }


    }
}
