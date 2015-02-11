using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace OP.General.Extensions
{
    public static class ByteExtensions
    {
        public static Image ToImage(this byte[] array)
        {
            try
            {
                MemoryStream ms = new MemoryStream(array);
                Image returnImage = Image.FromStream(ms, false, false);
                return returnImage;
            }
            catch (Exception err)
            {
                return null;
            }
        }

        public static string ToHexString(this byte[] array)
        {
            // Create a new StringBuilder to collect the bytes and create a string.
            var sBuilder = new StringBuilder();

            // Convert to a hex string.
            for (int i = 0; i < array.Length; i++)
                sBuilder.Append(array[i].ToString("x2"));

            // Return the hex string.
            return sBuilder.ToString();
        }

        public static byte[] Encrypt(this byte[] array)
        {

            if (array != null)
            {
                return Encryption.Encryption.EncryptByte(array);
            }

            return array;
        }

        public static byte[] Decrypt(this byte[] array)
        {

            if (array != null)
            {
                return Encryption.Encryption.DecryptByte(array);
            }

            return array;
        }

        public static MemoryStream ToStream(this byte[] array)
        {
            MemoryStream ms = new MemoryStream(array);
            return ms;
        }
    }
}
