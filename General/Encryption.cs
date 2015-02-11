using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace OP.General.Encryption
{

    /// <summary>
    /// Class with methods to handle symetrical encryption using rijndaelCipher
    /// http://www.splinter.com.au/c-cryptography-encrypting-a-bunch-of-bytes/
    /// </summary>

    public class Encryption
    {
        private static byte[] Salt
        {
            get { return Encoding.ASCII.GetBytes(GetEncryptionKey().Length.ToString()); }
        }

        private static int _Saltcharstoadd
        {
            get { return 2; }
        }

        /// <summary>
        /// Static method to get the encryption key for the project
        /// SO: 22/11/11 - Now uses the web config to get the encryption key.
        /// </summary>
        /// <returns>encryption key string</returns>
        private static string GetEncryptionKey()
        {

            string saltappended = string.Empty;
            try
            {
                System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                saltappended = settingsReader.GetValue("SecondPartofEncryptionSalt", typeof(String)).ToString();
            }
            catch (Exception err)
            {
                //throw new Exception("No additional salt found in web.config for key 'SecondPartofEncryptionSalt'");
                return "Ek8";
            }

            return "ez_2015_KBJ%" + saltappended;
        }

        #region encrypt
        /// <summary>
        /// this method returns an encrypted string
        /// </summary>
        /// <param name="inputText">String required to be encrypted</param>
        /// <param name="generateAsHex">Return a string/hex representation</param>
        /// <returns>a string</returns>
        public static string encrypt(string inputText, Boolean generateAsHex)
        {
            try
            {
                Encryption instance = new Encryption();
                // prefix 1 random characters to the string to ensure that the end string is more random
                string prefixer = "";
                //Random _random = new Random();

                var cryptoProvider = new RNGCryptoServiceProvider();

                byte[] randomNumber = new byte[1];
                cryptoProvider.GetBytes(randomNumber);
                byte randomised = randomNumber[0];

                int cryptorandomnumber = (randomNumber[0] % 26);

                for (int i = 1; i <= _Saltcharstoadd; i++)
                {
                    prefixer = prefixer + Convert.ToChar(Convert.ToInt32(cryptorandomnumber + 65));
                }
                inputText = prefixer + inputText;

                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                byte[] plainText = Encoding.Unicode.GetBytes(inputText);
                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(GetEncryptionKey(), Salt);

                using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16)))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainText, 0, plainText.Length);
                            cryptoStream.FlushFinalBlock();

                            if (generateAsHex == true)
                            {
                                return getHexString(memoryStream.ToArray());

                            }
                            else
                            {
                                return Convert.ToBase64String(memoryStream.ToArray());
                            }
                        }
                    }
                }
            }
            catch
            {
                // throw the error so it is reported to us through the application error
                throw;
            }

        }

        #endregion

        #region decrypt
        /// <summary>
        /// decrypts an input string
        /// </summary>
        /// <param name="encryptedText">String to be decrypted</param>
        /// <param name="decryptFromHex">Is string a hex representation</param>
        /// <returns>a string</returns>
        public static string decrypt(string encryptedText, Boolean decryptFromHex)
        {
            if (!String.IsNullOrEmpty(encryptedText))
            {
                try
                {
                    byte[] encryptedData;

                    if (decryptFromHex == true)
                    {
                        encryptedData = getHexBytes(encryptedText);
                    }
                    else
                    {
                        encryptedData = Convert.FromBase64String(encryptedText);
                    }
                    RijndaelManaged rijndaelCipher = new RijndaelManaged();
                    //byte[] encryptedData = Convert.FromBase64String(inputText);
                    PasswordDeriveBytes secretKey = new PasswordDeriveBytes(GetEncryptionKey(), Salt);

                    using (ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] plainText = new byte[encryptedData.Length];
                                int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
                                // return and remove the first 3 characters
                                return (Encoding.Unicode.GetString(plainText, 0, decryptedCount)).Remove(0, _Saltcharstoadd);

                            }
                        }
                    }
                }
                catch
                {
                    // throw the error so it is reported to us through the application error
                    //throw;
                    return string.Empty;

                }

            }
            else
            {
                return string.Empty;
            }
        }
        #endregion


        public static string EncryptTripleDES(string toEncrypt, Boolean generateAsHex)
        {
            return EncryptTripleDES(toEncrypt, generateAsHex, false);
        }


        #region Encrypt TripleDES
        /// <summary>
        /// Method that encrypts a string
        /// </summary>
        /// <param name="toEncrypt">string ot encrypt</param>
        /// <param name="generateAsHex">generate result stringin hex format</param>
        /// <returns>an ecnrypted string</returns>
        public static string EncryptTripleDES(string toEncrypt, Boolean generateAsHex, Boolean addRandomCharacter)
        {
            if (!string.IsNullOrEmpty(toEncrypt))
            {

                var cryptoProvider = new RNGCryptoServiceProvider();

                var randomBytes = new byte[4];
                cryptoProvider.GetBytes(randomBytes);
                int randomNumber = Math.Abs(BitConverter.ToInt32(randomBytes, 0)) % 24;


                // prefix 1 random characters to the string to ensure that the end string is more random
                string prefixer = "";
                Random _random = new Random(DateTime.Now.Second * DateTime.Now.Hour);
                if (addRandomCharacter)
                {
                    for (int i = 1; i <= _Saltcharstoadd; i++)
                    {
                        prefixer = prefixer + Convert.ToChar(Convert.ToInt32(Math.Floor(randomNumber * _random.NextDouble() + 65)));
                    }
                }
                string inputText = prefixer + toEncrypt;

                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(inputText);

                string key = GetEncryptionKey();

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();
                if (generateAsHex == true)
                {
                    return getHexString(resultArray);

                }
                else
                {
                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
            }
            else
            {
                return String.Empty;
            }

        }
        #endregion



        public static string DecryptTripleDES(string toDecrypt, Boolean decryptFromHex)
        {
            return DecryptTripleDES(toDecrypt, decryptFromHex, false);
        }


        public static byte[] EncryptByte(byte[] input)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(GetEncryptionKey(), Salt); // Change this
            MemoryStream ms = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            CryptoStream cs = new CryptoStream(ms,
              aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.Close();
            return ms.ToArray();
        }
        public static byte[] DecryptByte(byte[] input)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(GetEncryptionKey(), Salt);
            MemoryStream ms = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            CryptoStream cs = new CryptoStream(ms,
              aes.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.Close();
            return ms.ToArray();
        }



        #region Decrypt TripleDES
        /// <summary>
        /// Method that decrypts an encrypted string
        /// </summary>
        /// <param name="toDecryptString to decrypt</param>
        /// <param name="decryptFromHex">decrypt froma hex representation</param>
        /// <returns>a decoded string</returns>
        public static string DecryptTripleDES(string toDecrypt, Boolean decryptFromHex, Boolean addRandomCharacter)
        {
            try
            {
                if (!string.IsNullOrEmpty(toDecrypt))
                {
                    byte[] keyArray;
                    byte[] toEncryptArray;

                    if (decryptFromHex == true)
                    {
                        toEncryptArray = getHexBytes(toDecrypt);
                    }
                    else
                    {
                        toEncryptArray = Convert.FromBase64String(toDecrypt);
                    }

                    string key = GetEncryptionKey();

                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    hashmd5.Clear();

                    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                    tdes.Key = keyArray;
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform cTransform = tdes.CreateDecryptor();
                    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                    tdes.Clear();
                    if (addRandomCharacter)
                    {
                        return UTF8Encoding.UTF8.GetString(resultArray, 0, resultArray.Length).Remove(0, _Saltcharstoadd);
                    }
                    else
                    {
                        return UTF8Encoding.UTF8.GetString(resultArray, 0, resultArray.Length);
                    }
                }
                else
                {
                    return String.Empty;
                }
            }
            catch
            {
                return toDecrypt;
            }
        }
        #endregion

        #region getHexString
        /// <summary>
        /// generates a hex representation from an array of bytes
        /// </summary>
        /// <param name="data">array of bytes</param>
        /// <returns>a string</returns>
        private static string getHexString(byte[] data)
        {
            StringBuilder results = new StringBuilder();
            foreach (byte b in data)
            {
                results.Append(b.ToString("X2"));
            }

            return results.ToString();
        }
        #endregion

        #region getHexBytes
        /// <summary>
        /// generates a byte array from a hex string
        /// </summary>
        /// <param name="data">a string hex representation</param>
        /// <returns>a byte array</returns>
        private static byte[] getHexBytes(string data)
        {
            // GetString encodes the hex-numbers with two digits
            byte[] results = new byte[data.Length / 2];
            for (int i = 0; i < data.Length; i += 2)
            {
                results[i / 2] = Convert.ToByte(data.Substring(i, 2), 16);
            }

            return results;
        }
        #endregion


        #region Encrypt with hashing
        /// <summary>
        /// Method that encrypts a string one way - useful for passwords
        /// </summary>
        /// <param name="literal">String to be encrypted</param>
        /// <returns>string representation of a hex result</returns>
        public static string encryptWithHashing(string encStr)
        {
            string retVal = null;
            // ensure that there is a string ot encrypt
            if (!String.IsNullOrEmpty(encStr))
            {
                SHA1 enc = SHA1.Create();
                // MD5 md5 = MD5.Create();
                byte[] byteBuffer = System.Text.Encoding.ASCII.GetBytes(encStr);
                byte[] hash = enc.ComputeHash(byteBuffer);
                // string hashHex = ByteArrayToHexString(hash);
                retVal = getHexString(hash);
            }
            return retVal;
        }
        #endregion


    }
}
