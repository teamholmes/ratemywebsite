using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace OP.General.Extensions
{
    public static class BooleanExtensions
    {
       

        public static String Encrypt(this Boolean input)
        {
            return Encryption.Encryption.EncryptTripleDES(input.ToString(), true, true);
        }

      


    }
}
