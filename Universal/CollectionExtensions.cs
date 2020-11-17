using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OnPoint.Universal
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Iterates over <paramref name="target"/>, stopping if a non-null value is encountered.
        /// </summary>
        /// <param name="target">The IEnumerable object to check</param>
        /// <returns>True if <paramref name="target"/> is not null and has at least one non-null item; false otherwise</returns>
        public static bool AnyNonNulls(this IEnumerable target)
        {
            bool retVal = false;
            if (target != null)
            {
                foreach (object item in target)
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

        /// <summary>
        /// Return all non-null items in <paramref name="target"/>.
        /// </summary>
        /// <typeparam name="T">The type of items being stored</typeparam>
        /// <param name="target">The enumerable to filter</param>
        /// <returns>A filtered <paramref name="target"/> that contains no nulls</returns>
        public static IEnumerable<T> GetNonNulls<T>(this IEnumerable<T> target) => target?.Where(x => x != null);

        /// <summary>
        /// Remove the item from <paramref name="target"/> if it exists; otherwise no-op
        /// </summary>
        /// <typeparam name="T">The type of items being stored</typeparam>
        /// <param name="target">The collection to search</param>
        /// <param name="item">The item to remove</param>
        /// <returns>True if <paramref name="item"/> was both found and removed successfully from <paramref name="target"/></returns>
        public static bool RemoveIfContains<T>(this ICollection<T> target, T item)
        {
            bool retVal = false;
            if (target?.Contains(item) == true)
            {
                target.Remove(item);
                retVal = true;
            }
            return retVal;
        }

        /// <summary>
        /// Add <paramref name="item"/> to the <paramref name="target"/> if it is not already present.
        /// </summary>
        /// <typeparam name="T">The type of items being stored</typeparam>
        /// <param name="target">The collection to search</param>
        /// <param name="item">The item to add</param>
        /// <returns>True if <paramref name="item"/> was both not previously present and added successfully to <paramref name="target"/></returns>
        public static bool AddIfNotContains<T>(this ICollection<T> target, T item)
        {
            bool retVal = false;
            if (target?.Contains(item) == false)
            {
                target.Add(item);
                retVal = true;
            }
            return retVal;
        }

        /// <summary>
        /// Fluent method to add all <paramref name="items"/> to <paramref name="target"/>; acts as an "AddRange" method for the IList interface.
        /// </summary>
        /// <typeparam name="T">The type of items being stored</typeparam>
        /// <param name="target">The list to act on</param>
        /// <param name="items">The items to add</param>
        /// <returns><paramref name="target"/></returns>
        public static IList<T> AddAll<T>(this IList<T> target, params T[] items)
        {
            if (target != null && items != null)
            {
                foreach (T item in items)
                {
                    target.Add(item);
                }
            }
            return target;
        }

        /// <summary>
        /// Resize the given array and add the item to the end.
        /// </summary>
        /// <typeparam name="T">The type of items being stored</typeparam>
        /// <param name="items">The array to act on</param>
        /// <param name="item">The item to add</param>
        /// <returns><paramref name="items"/> resized with <paramref name="item"/> added to it</returns>
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

        /// <summary>
        /// Replace <paramref name="originalCopy"/> with <paramref name="newCopy"/>.
        /// </summary>
        /// <typeparam name="T">Generic Tyipe</typeparam>
        /// <param name="target">The list to search</param>
        /// <param name="originalCopy">The original item to replace</param>
        /// <param name="newCopy">The new version to insert or add</param>
        /// <param name="addIfNotContains">Determines whether <paramref name="newCopy"/> should be added if it is not already in the <paramref name="target"/></param>
        /// <returns>True if <paramref name="newCopy"/> was put in <paramref name="target"/></returns>
        public static bool Replace<T>(this IList<T> target, T originalCopy, T newCopy, bool addIfNotContains = true)
        {
            bool retVal = false;
            if (target != null)
            {
                if (target.Contains(originalCopy))
                {
                    target.Insert(target.IndexOf(originalCopy), newCopy);
                    target.Remove(originalCopy);
                    retVal = true;
                }
                else if (addIfNotContains)
                {
                    target.Add(newCopy);
                    retVal = true;
                }
            }
            return retVal;
        }

        /// <summary>
        /// Remove all items matching <paramref name="filter"/> from <paramref name="target"/>.
        /// </summary>
        /// <typeparam name="T">The type of items being stored</typeparam>
        /// <param name="target">The collection to search</param>
        /// <param name="filter">The search criteria</param>
        public static void RemoveItems<T>(this ICollection<T> target, Func<T, bool> filter)
        {
            if (target != null && filter != null)
            {
                List<T> itemsToRemove = new List<T>();
                foreach (var item in target.Where(filter))
                {
                    itemsToRemove.Add(item);
                }
                foreach (var item in itemsToRemove)
                {
                    target.Remove(item);
                }
            }
        }

        /// <summary>
        /// Create an array from a single item.
        /// </summary>
        /// <typeparam name="T">The type of items being stored</typeparam>
        /// <param name="item">The item to put in the array</param>
        /// <returns>A new array with only <paramref name="item"/>in it</returns>
        public static T[] MakeArray<T>(this T item) => new T[] { item };
    }
}