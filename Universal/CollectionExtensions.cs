using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OnPoint.Universal
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Iterates over list, stopping if a non-null value is encountered.
        /// </summary>
        /// <param name="list">The IEnumerable object to check</param>
        /// <returns>True if <paramref name="list"/> is not null and has at least one non-null item; false otherwise</returns>
        public static bool AnyNonNulls(this IEnumerable list)
        {
            bool retVal = false;
            if (list != null)
            {
                foreach (object item in list)
                {
                    if (item != null)
                    {
                        retVal = true;
                        break;
                    }
                }
            }
            return retVal;
        }

        public static IEnumerable<T> NN<T>(this IEnumerable<T> list) => list?.Where(x => x != null);

        public static bool RemoveIfContains<T>(this ICollection<T> list, T item)
        {
            bool retVal = false;
            if (list?.Contains(item) == true)
            {
                list.Remove(item);
                retVal = true;
            }
            return retVal;
        }

        public static bool AddIfNotContains<T>(this ICollection<T> list, T item)
        {
            bool retVal = false;
            if (list?.Contains(item) == false)
            {
                list.Add(item);
                retVal = true;
            }
            return retVal;
        }

        public static IList<T> AddAll<T>(this IList<T> list, params T[] items)
        {
            if (list != null && items != null)
            {
                foreach (T item in items)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public static T[] AddToArray<T>(this T[] items, T item)
        {
            T[] retVal = items;
            if (retVal != null)
            {
                Array.Resize(ref retVal, retVal.Length + 1);
                retVal[retVal.Length - 1] = item;
            }
            return retVal;
        }

        public static bool Replace<T>(this IList<T> list, T originalCopy, T newCopy, bool addIfNotContains = true)
        {
            bool retVal = false;
            if (list != null)
            {
                if (list.Contains(originalCopy))
                {
                    list.Insert(list.IndexOf(originalCopy), newCopy);
                    list.Remove(originalCopy);
                    retVal = true;
                }
                else if (addIfNotContains)
                {
                    list.Add(newCopy);
                    retVal = true;
                }
            }
            return retVal;
        }

        public static void RemoveItems<T>(this ICollection<T> list, Func<T, bool> filter)
        {
            if (list != null && filter != null)
            {
                List<T> itemsToRemove = new List<T>();
                foreach (var item in list.Where(filter))
                {
                    itemsToRemove.Add(item);
                }
                foreach (var item in itemsToRemove)
                {
                    list.Remove(item);
                }
            }
        }

        public static T GetItemAtIndexOrLast<T>(this IList<T> list, int index) where T : class
        {
            T retVal = default;
            if (list.Count > 0)
            {
                if (index < list.Count)
                {
                    retVal = list[index];
                }
                else
                {
                    retVal = list.Last();
                }
            }
            return retVal;
        }

        public static T[] MakeArray<T>(this T item) => new T[] { item };
    }
}