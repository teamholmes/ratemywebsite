using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.General.Testing
{
    public static class EqualityHelper
    {
        public static bool IsEqualMatch<T>(T actual, T expected)
        {
            if (actual == null && expected == null) return true;

            if (actual == null) return false;
            if (expected == null) return false;

            foreach (var property in typeof(T).GetProperties())
            {
                object actualPropertyValue = property.GetValue(actual, null);
                object expectedPropertyValue = property.GetValue(expected, null);

                if (actualPropertyValue == null && expectedPropertyValue == null) continue;
                if (actualPropertyValue == null || expectedPropertyValue == null)
                {
                    Debugger.Break();
                    return false;
                }

                if (IsObjectAGenericCollection(actualPropertyValue))
                {
                    if (AreCollectionsEqualSize(actualPropertyValue, expectedPropertyValue))
                        continue;
                }

                if (actualPropertyValue is DateTime || actualPropertyValue is DateTime?)
                {
                    if (IsEqualMatch(((DateTime)actualPropertyValue), ((DateTime)expectedPropertyValue), 2000))
                        continue;
                }

                if (actualPropertyValue is string)
                {
                    if (actualPropertyValue.Equals(expectedPropertyValue))
                        continue;
                }

                if (actualPropertyValue.Equals(expectedPropertyValue))
                {
                    continue;
                }

                Debug.WriteLine("actual is not equal to expected {0} {1}", actualPropertyValue, expectedPropertyValue);
                Debugger.Break();
                return false;
            }

            return true;
        }

        private static bool AreCollectionsEqualSize(object actualPropertyValue, object expectedPropertyValue)
        {
            var genericCollection = actualPropertyValue.GetType();

            var countProperty = (actualPropertyValue is Array && expectedPropertyValue is Array)?genericCollection.GetProperty("Length") : genericCollection.GetProperty("Count");

            return (Convert.ToInt32(countProperty.GetValue(actualPropertyValue, null)) == Convert.ToInt32(countProperty.GetValue(expectedPropertyValue, null)));
        }

        private static bool IsObjectAGenericCollection(object actualPropertyValue)
        {
            foreach (Type type in actualPropertyValue.GetType().GetInterfaces())
            {
                if (type.IsGenericType)
                {
                    if (type.GetGenericTypeDefinition() == typeof(ICollection<>))
                        return true;
                }
            }

            return false;
        }

        public static bool IsEqualMatch(DateTime actual, DateTime expected, int allowedMillisecondShift)
        {
            var timeDifference = Math.Abs((actual - expected).TotalMilliseconds);

            return (timeDifference <= allowedMillisecondShift);
        }

    }
}
