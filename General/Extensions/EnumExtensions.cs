using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using OP.General.Attributes;

namespace OP.General.Extensions
{
    public static class EnumExtensions
    {
        public static int ConvertToInt(this Enum e)
        {
            return Convert.ToInt32(e);
        }

        public static SelectList ToEnumSelectList<TEnum>(this TEnum enumObj) where TEnum : struct, IConvertible
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = e.ToString() };

         
            return new SelectList(values, "Id", "Name", enumObj);
        }


        public static IEnumerable<SelectListItem> ToSelectListItems(this Enum enumValue)
        {
            return from Enum e in Enum.GetValues(enumValue.GetType())
                   select new SelectListItem
                   {
                       Selected = e.Equals(enumValue),
                       Text = e.ToDescription(),
                       Value = (e.ConvertToInt()).ToString()
                   };
        }

        public static string ToDescription(this Enum value)
        {
            var attributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static int GetOrder(this Enum value)
        {
            var attributes = (EnumOrderAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(EnumOrderAttribute), false);
            return attributes.Length > 0 ? attributes[0].Order : 0;
        }

        public static bool IsHidden(this Enum value)
        {
            var attributes = (EnumDisplayAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(EnumDisplayAttribute), false);
            return attributes.Length > 0 ?  attributes[0].IsHidded : false;
        }

    }
}
