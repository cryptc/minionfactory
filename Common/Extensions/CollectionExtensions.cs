using System;
using System.Collections.Generic;
using Common.JetBrainsAnnotations;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>([CanBeNull] this ICollection<T> collection, [CanBeNull] IEnumerable<T> items)
        {
            if (collection == null)
                return;

            if (items == null)
                return;

            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        public static void AddIfNotNull<T>([CanBeNull] this ICollection<T> enumeration, [CanBeNull] T item)
        {
            if (enumeration == null)
                return;

            if (Nullable.GetUnderlyingType(typeof(T)) == null)
            {
                enumeration.Add(item);
                return;
            }
            
// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (item != null)
                enumeration.Add(item);
        }

        public static void AddIfNotNull<T>([CanBeNull] this ICollection<T> enumeration, [CanBeNull] IEnumerable<T> items)
        {
            if (enumeration == null)
                return;

            if (items == null)
                return;

            if (Nullable.GetUnderlyingType(typeof(T)) == null)
            {
                enumeration.AddRange(items);
                return;
            }

            foreach (var item in items)
            {
// ReSharper disable once CompareNonConstrainedGenericWithNull
                if (item != null)
                    enumeration.Add(item);
            }
        }

        /// <returns> Collection of items removed </returns>
        [CanBeNull]
        public static TCollection RemoveWhere<TCollection, T>([CanBeNull] this TCollection collection, [NotNull] Func<T, bool> selector)
            where TCollection : class, ICollection<T>, new()
        {
            if (collection == null)
                return null;

            var removedItemsCollection = new TCollection();

            foreach (var item in collection)
            {
                bool removedSuccessfully = selector.Invoke(item);

                if (removedSuccessfully)
                    removedItemsCollection.Add(item);
            }

            return removedItemsCollection;
        }

    }
}