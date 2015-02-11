using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.General.Extensions
{
    public static class LongExtensions
    {
      
       
        public static string Encrypt(this long input)
        {

            return Encryption.Encryption.EncryptTripleDES(input.ToString(),true,true);
        }

        public static double ConvertBytesToMegabytes(this long input)
        {
            return (input / 1024f) / 1024f;
        }

    }
}
