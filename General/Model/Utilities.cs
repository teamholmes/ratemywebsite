using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Serialization;


namespace OP.General.Model
{
    /// <summary>
    /// Generic utilities class
    /// </summary>
    public class Utilities
    {
        /// <summary>
        /// Constant that defines a text block that is rendered out ot the screen - visually show where the system if pulling text from.
        /// If this text is seen, then the system is p[ulling data from the resource files.
        /// </summary>
        public const String multilingualDisplayStart = "(";
        public const String multilingualDisplayEnd = ")";



        public static T SetandGetCache<T>(string uniquecacheName, T objectocache, int cacheduration = 60 )
        {
                Utilities.SetCache(uniquecacheName, objectocache, cacheduration);
                return (T)Utilities.GetFromCache(uniquecacheName);
        }


        #region Data Cache
        /// <summary>
        /// Returns an object that has been stored in the data cache 
        /// </summary>
        /// <param name="cacheName">Name to reference the cached object</param>
        /// <returns>An object which should be cast accordingly</returns>
        public static object GetFromCache(string cacheName)
        {
            try
            {
                return ((object)HttpRuntime.Cache[cacheName.ToUpper()]);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// caches an object for a set period of time
        /// </summary>
        /// <param name="cacheName">Name to reference the cached object</param>
        /// <param name="objectToCache">Object to be stored</param>
        /// <param name="duration">Duration in minutes for cache to persist</param>
        public static void SetCache(string cacheName, Object objectToCache, int duration = 15)
        {
            HttpRuntime.Cache.Insert(cacheName.ToUpper(), objectToCache, null, DateTime.Now.AddMinutes(duration), TimeSpan.Zero);
        }

        /// <summary>
        /// Removes a cached object
        /// </summary>
        /// <param name="cacheName">Name of cacheed object</param>
        public static void ClearCache(string cacheName)
        {
            HttpRuntime.Cache.Remove(cacheName.ToUpper());
        }
        #endregion

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


        /// <summary>
        /// Maps the property values from one object to another object
        /// </summary>
        /// <typeparam name="K">The source object class</typeparam>
        /// <typeparam name="T">The destination object class</typeparam>
        /// <param name="sourceclass">The instance of the source class</param>
        /// <param name="propertynameconversion">Optional dictionary containing a mapping of source property names and destination property names (see example in method for more info).</param>
        /// <returns>A Populated object of the of type destination</returns>
        public static T MapPropertyValuesTo<K, T>(K sourceclass, Dictionary<string, string> propertynameconversion = null)
        {

            //example //b = OP.General.Model.Utilities.MapPropertyValuesTo<ClassA, ClassB>(a, new Dictionary<string, string>() { { "str", "strmodified" } });

            object returnobject = Activator.CreateInstance(typeof(T));

            object sourceobject = Activator.CreateInstance(typeof(K));

            PropertyInfo[] sourceproperties = sourceclass.GetType().GetProperties();

            PropertyInfo[] destinationproperties = returnobject.GetType().GetProperties();

            foreach (PropertyInfo sourceproperty in sourceproperties)
            {
                if (sourceproperty != null)
                {
                    string sourcepropertyname = sourceproperty.Name.ToUpper();
                    Type sourcetype = sourceproperty.PropertyType;

                    foreach (PropertyInfo destinationproperty in destinationproperties)
                    {
                        if (sourcepropertyname == destinationproperty.Name.ToUpper())
                        {
                            if (sourcetype == destinationproperty.PropertyType)
                            {
                                // matched property name and type
                                destinationproperty.SetValue(returnobject, sourceproperty.GetValue(sourceclass, null), null);
                                break;
                            }
                        }
                    }
                }
            }

            // perform any mapping requirements
            if (propertynameconversion != null)
            {
                foreach (KeyValuePair<string, string> pair in propertynameconversion)
                {
                    foreach (PropertyInfo sourceproperty in sourceproperties)
                    {
                        Type sourcetype = sourceproperty.PropertyType;
                        // found a match on the source
                        if (sourceproperty != null && sourceproperty.Name.ToUpper() == pair.Key.ToUpper())
                        {
                            foreach (PropertyInfo destinationproperty in destinationproperties)
                            {
                                // found a match on the destination
                                if (destinationproperty.Name.ToUpper() == pair.Value.ToUpper())
                                {
                                    if (sourcetype == destinationproperty.PropertyType)
                                    {
                                        destinationproperty.SetValue(returnobject, sourceproperty.GetValue(sourceclass, null), null);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }

            return ((T)returnobject);
        }




    }

}