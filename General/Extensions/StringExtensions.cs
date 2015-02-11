using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OP.General.Extensions
{
    public static class StringExtensions
    {
        public static String TrimforUI(this string input, int numchars, Boolean addtrailing = true)
        {

            try
            {
                string retval = string.Empty;

                string appender = "...";

                if (String.IsNullOrEmpty(input)) return retval;

                if (input.Length < numchars) return input;

                if (input.Length >= numchars) retval = input.Substring(0, numchars);

                if (addtrailing) retval += "...";

                return retval;
            }
            catch (Exception)
            {
                return input;
            }
        }

        public static String TrimCheckForNull(this string input)
        {

            string retval = input;

            if (!String.IsNullOrEmpty(retval))
            {
                retval = retval.Trim();
            }

            return retval;
        }


        public static String GetNthCharacter(this string input, int position)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            return string.Format("{0}",input[position]);
        }


        public static String ToFormattedPostCode(this string input, Boolean removeallspaces = false)
        {

            if (String.IsNullOrEmpty(input)) return input;

            StringBuilder output = new StringBuilder();


            input = input.Replace(" ", "").ToUpperCheckForNull();

            if (input.Length == 6)
            {
                output.Append(input.Substring(0, 3));
                output.Append(" ").Append(input.Substring(3, 3));
            }
            else if (input.Length == 7)
            {
                output.Append(input.Substring(0, 4));
                output.Append(" ").Append(input.Substring(4, 3));
            }
            else
            {
                output.Append(input);
            }
            
            if (removeallspaces)
            {
                return output.ToString().Replace(" ", "");
            }

            
            return output.ToString();
        }

        public static String RemoveAllSpaces(this string input)
        {

            string retval = input;

            if (!String.IsNullOrEmpty(retval))
            {
                retval = retval.Trim().Replace(" ", "");
            }

            return retval;
        }

        public static Boolean IsInteger(this string input)
        {
            bool isNumeric = false;
            if (!String.IsNullOrEmpty(input))
            {
                int n;
                isNumeric = int.TryParse(input, out n);
            }

            return isNumeric;

        }

        public static String ToLowerCheckForNull(this string input)
        {

            string retval = input;

            if (!String.IsNullOrEmpty(retval))
            {
                retval = retval.ToLower();
            }

            return retval;

        }

        public static String ToUpperCheckForNull(this string input)
        {

            string retval = input;

            if (!String.IsNullOrEmpty(retval))
            {
                retval = retval.ToUpper();
            }

            return retval;

        }

        public static Boolean IsNotNullOrEmpty(this string input)
        {

            if (String.IsNullOrEmpty(input)) return false;

            return true;
        }

        public static int NumberOfUpperCaseCharacters(this string input)
        {
            if (String.IsNullOrEmpty(input)) return -1;

            return input.Where(char.IsUpper).Count();
        }



        public static Boolean IsNullOrEmpty(this string input)
        {

            if (String.IsNullOrEmpty(input)) return true;

            return false;
        }

        public static string ReplaceIfNullOrEmpty(this string input, string replacementtext = "--")
        {

            if (String.IsNullOrEmpty(input))
            {
                input = replacementtext;
                return replacementtext;
            }

            return input;
        }


        public static string Encrypt(this string input)
        {

            if (String.IsNullOrEmpty(input)) return input;

            return Encryption.Encryption.EncryptTripleDES(input, true, true);
        }

        public static string Encrypt(this string input, Boolean addrandomcharacter)
        {

            if (String.IsNullOrEmpty(input)) return input;

            return Encryption.Encryption.EncryptTripleDES(input, true, addrandomcharacter);
        }

        public static string Decrypt(this string input)
        {
            if (String.IsNullOrEmpty(input)) return input;

            return Encryption.Encryption.DecryptTripleDES(input, true, true);
        }

        public static string Decrypt(this string input, Boolean addrandomcharacter)
        {
            if (String.IsNullOrEmpty(input)) return input;

            return Encryption.Encryption.DecryptTripleDES(input, true, addrandomcharacter);
        }

        public static int DecryptToInt(this string input)
        {

            if (String.IsNullOrEmpty(input))
            {
                throw new Exception("Cannot decrypt an empty string");
            }

            string decryptedretval = Encryption.Encryption.DecryptTripleDES(input, true, true);

            return int.Parse(decryptedretval);
        }

        public static int DecryptToInt(this string input, Boolean addrandomcharacter)
        {

            if (String.IsNullOrEmpty(input))
            {
                throw new Exception("Cannot decrypt an empty string");
            }

            string decryptedretval = Encryption.Encryption.DecryptTripleDES(input, true, addrandomcharacter);

            return int.Parse(decryptedretval);
        }

        public static Boolean DecryptToBoolean(this string input)
        {

            if (String.IsNullOrEmpty(input))
            {
                throw new Exception("Cannot decrypt an empty string");
            }

            string decryptedretval = Encryption.Encryption.DecryptTripleDES(input, true, true);

            return Boolean.Parse(decryptedretval);
        }

        public static long DecryptToLong(this string input)
        {

            if (String.IsNullOrEmpty(input))
            {
                throw new Exception("Cannot decrypt an empty string");
            }

            return long.Parse(Encryption.Encryption.DecryptTripleDES(input, true, true));
        }

        public static string[] SplitOnPipe(this string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                throw new Exception("Cannot Split on a empty string");
            }

            string[] obitems = input.Split('|');
            return obitems;
        }

        public static int[] ConvertStringArrayToIntArray(this string[] input)
        {
            if (input == null || input.Length == 0)
            {
                throw new Exception("Cannot Split on a empty string");
            }

            List<int> listofints = new List<int>();
            foreach (string str in input)
            {
                listofints.Add(int.Parse(str));
            }
            return listofints.ToArray();
        }

        public static string RemoveAllInvalidFilenameChars(this string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }

            return Path.GetInvalidFileNameChars().Aggregate(input, (current, c) => current.Replace(c.ToString(), string.Empty));

        }


        public static string ToAllowableFirstNameSurnameCharacters(this string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                string pattern = "[\\~#%&*{}/:<>?|\"]";
                string replacement = "";

                Regex regEx = new Regex(pattern);
                return regEx.Replace(input, replacement);
            }
            else
            {
                return null;
            }

        }

        public static string PleuraliseByAddingS(this string input, int number)
        {
            if (!String.IsNullOrEmpty(input))
            {
                if (number == 1) return input;

                return string.Format("{0}s", input);
            }
            else
            {
                return null;
            }

        }


        public static string GetFileExtention(this string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                return input.Remove(0, input.LastIndexOf('.') + 1);
            }
            else
            {
                return null;
            }

        }

        public static string RemoveFileExtention(this string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                return input.Remove(input.LastIndexOf('.'), input.Length - input.LastIndexOf('.'));
            }
            else
            {
                return null;
            }
        }


        public static string RandomWords(this string input, int numwords)
        {
            string[] words = new string[] { "Lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipisicing", "elit", "sed", "do", "eiusmod", "tempor", "incididunt", "ut", "labore", "et", "dolore", "magna", "aliqua", "Ut", "enim", "ad", "minim", "veniam", "quis", "nostrud", "exercitation", "ullamco", "laboris", "nisi", "ut", "aliquip", "ex", "ea", "commodo", "consequat", "Duis", "aute", "irure", "dolor", "in", "reprehenderit", "in", "voluptate", "velit", "esse", "cillum", "dolore", "eu", "fugiat", "nulla", "pariatur", "Excepteur", "sint", "occaecat", "cupidatat", "non", "proident", "sunt", "in", "culpa", "qui", "officia", "deserunt", "mollit", "anim", "id", "est", "laborum" };

            string retval = string.Empty;

            int wordcount = words.Count();

            for (int i = 1; i <= numwords; i++)
            {
                int maxroute = 32;

                RNGCryptoServiceProvider randomcrypto = new RNGCryptoServiceProvider("test");
                byte[] randomBytes = new byte[maxroute * sizeof(int)];
                randomcrypto.GetBytes(randomBytes);
                int randomval = 0;
                for (int j = 0; j < maxroute; ++j)
                {
                    randomval = BitConverter.ToInt32(randomBytes, j * 4);
                    randomval &= 0x7fffffff;
                }

                int randomNumber = randomval % wordcount;

                retval += words[randomNumber];

                if (i < numwords) retval += " ";
            }
            return retval;

        }

        public static String AppendDDMMYYYYFilename(this string input, string extension)
        {
            if (!String.IsNullOrEmpty(input) && !String.IsNullOrEmpty(extension))
            {
                string formattedextension = extension;

                if (extension.Contains(".")) formattedextension = extension.Replace(".", "");

                return String.Format("{0}_{1}_{2}_{3}.{4}", input, DateTime.Now.FormattedYYYY(), DateTime.Now.ZeroFormattedMM(), DateTime.Now.ZeroFormattedDD(), formattedextension);
            }
            else
            {
                throw new Exception("GetDMYFilename - input or extension parameter is null");
            }

        }



        public static String ReplaceCheckForNull(this string input, string searchfor, string replacewith)
        {

            string retval = input;

            if (!String.IsNullOrEmpty(retval))
            {
                retval = input.Replace(searchfor, replacewith);
            }

            return retval;

        }




    }
}
