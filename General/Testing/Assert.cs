using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace OP.General.Testing
{
    //[DebuggerNonUserCode]
    public class Assert
    {

        public static void IsEqualMatch<T>(T actual, T expected)
        {
            if (actual == null && expected == null) return;

            MAssert.IsNotNull(actual);
            MAssert.IsNotNull(expected);

            Type testType = typeof(T);

            var properties = testType.GetProperties();

            foreach (var property in properties)
            {
                object actualPropertyValue = property.GetValue(actual, null);
                object expectedPropertyValue = property.GetValue(expected, null);

                switch (Type.GetTypeCode(property.PropertyType))
                {
                    case TypeCode.DateTime:
                        IsEqualMatch(((DateTime)actualPropertyValue), ((DateTime)expectedPropertyValue), 2000);;
                        break;

                    default:

                        if (actualPropertyValue is Array && expectedPropertyValue is Array)
                        {
                            MAssert.AreEqual(((Array)actualPropertyValue).Length, ((Array)expectedPropertyValue).Length);
                            break;
                        }

                        MAssert.AreEqual(actualPropertyValue, expectedPropertyValue);
                        break;

                    //need to consider collections and recursively traverse
                }                
            }
        }

        public static void IsEqualMatch(DateTime actual, DateTime expected, int allowedMillisecondShift)
        {
            var timeDifference = Math.Abs((actual - expected).TotalMilliseconds);

            IsTrue(timeDifference <= allowedMillisecondShift);
        }

        public static void IsTrue(bool condition)
        {
            if(!condition) Fail();
        }

        public static void IsFalse(bool condition)
        {
            if (condition) Fail();
        }

        public static void Fail(string message)
        {
            MAssert.Fail();
        }

        public static void Fail()
        {
            Fail(null);
        }

        public static void IsNull(object o)
        {
            MAssert.IsTrue(o == null);
        }

        public static void IsNotNull(object o)
        {
            MAssert.IsTrue(o != null);
        }

    }
}
