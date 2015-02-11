using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Xml.Serialization;

namespace OP.General.Serialisation
{

    /// <summary>
    /// Class with methods to handle symetrical encryption using rijndaelCipher
    /// </summary>

    public class SerialiseDeserialise
    {

        #region object serialisation / deserialisation
        /// <summary>
        /// generic method to handle the serialisation of an object
        /// </summary>
        /// <typeparam name="T">type of object to serialise</typeparam>
        /// <param name="obj">object to serialise</param>
        /// <returns>a string representation of a serialised object</returns>
        /// <example>string serialised = clsUtilities.SaveDataObjecttoXMLString<clsTransmission>(dTransmit);</example>

        public static string SaveDataObjecttoXMLString<T>(T obj)
        {
            MemoryStream _ms = new MemoryStream();
            // Xml Serializer outputs to stream
            (new XmlSerializer(typeof(T))).Serialize(_ms, obj);
            // Go to beginning of stream and read out to string result
            _ms.Position = 0;
            return (new StreamReader(_ms)).ReadToEnd();
        }

        /// <summary>
        /// generic method to gandle the deserialisation of a method
        /// </summary>
        /// <typeparam name="T">type of object to deserialise</typeparam>
        /// <param name="objdefinition">string representing the serialised object</param>
        /// <returns>a deserialised object</returns>
        /// <example>clsTransmission retrans = new clsTransmission(); retrans = clsUtilities.RestoreDataObjectfromString<clsTransmission>(serialised);</example>

        public static T RestoreDataObjectfromString<T>(String objdefinition)
        {
            // If there is an object in string then read into stream
            MemoryStream _ms = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(objdefinition));
            // Cast object from serialised stream
            return (T)(new XmlSerializer(typeof(T))).Deserialize(_ms);
        }


        /// <summary>
        /// generic method to gandle the deserialisation of a method
        /// </summary>
        /// <typeparam name="T">type of object to deserialise</typeparam>
        /// <param name="objdefinition">string representing the serialised object</param>
        /// <returns>a deserialised object</returns>
        /// <example>clsTransmission retrans = new clsTransmission(); retrans = clsUtilities.RestoreDataObjectfromString<clsTransmission>(serialised);</example>

        public static T RestoreDataObjectfromString<T>(String objdefinition, Boolean tryandAddXMlHeader)
        {
           // if no xml header
            if (tryandAddXMlHeader)
            {
                string headdertolookfor = "<?xml version=\"1.0\"?>";
                if (!objdefinition.Contains(headdertolookfor))
                {
                    objdefinition = headdertolookfor + objdefinition;
                }
            }

            // If there is an object in string then read into stream
            MemoryStream _ms = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(objdefinition));
            // Cast object from serialised stream
            return (T)(new XmlSerializer(typeof(T))).Deserialize(_ms);
        }
        #endregion

    }
}
