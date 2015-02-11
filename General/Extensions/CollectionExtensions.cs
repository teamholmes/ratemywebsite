using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OP.General.Extensions
{
    public static class CollectionExtensions
    {

        

        public static SelectList ToSelectList<T>(this IEnumerable<T> list, object selectedItem = null, bool addEmptyItem = true) where T : ISelectable
        {
            if (list == null) return null;

            List<T> items = new List<T>();

            if (addEmptyItem)
            {
                T defaultItem = Activator.CreateInstance<T>();
                defaultItem.Id = -1;
                defaultItem.Name = "Please Select";
                items.Add(defaultItem);
            }

            items.AddRange(list);

            return new SelectList(items, "Id", "Name");
        }

        

        public static SelectList ToVanillaSelectList<T>(this IEnumerable<T> list) where T : ISelectable
        {
            return ToSelectList(list, null, false);
        }

        public static T GetSelectedItem<T>(this IEnumerable<T> list, int selectedId) where T : ISelectable
        {
            return list.Where(x => x.Id == selectedId).SingleOrDefault();
        }

        public static void InitialiseCollection<T>(this IEnumerable<T> list, int length)
        {
            if (list == null)
                list = new List<T>();

            for (int i = 0; i < length; i++)
            {
                T newItem = default(T);
                ((List<T>)list).Add(newItem);
            }
        }

        public static void ResizeCollection<T>(this List<T> list, int length)
        {
            if (list == null)
                list = new List<T>();

            if (list.Count >= length)
            {
                list = list.Take(length).ToList();
                return;
            }

            int missingItems = length - list.Count;

            for (int i = 0; i < missingItems; i++)
            {
                T newItem = default(T);
                ((List<T>)list).Add(newItem);
            }
        }

        public static bool IsNullOrEmpty(this ICollection list)
        {
            if (list == null) 
                return true;

            if (list.Count == 0)
                return true;

            return false;
        }
    }
}