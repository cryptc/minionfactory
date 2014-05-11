using System.Linq;
using Common.JetBrainsAnnotations;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    public static class EnumerableExtensions
    {
        [Pure]
        public static bool IsNull<T>([CanBeNull] this IEnumerable<T> enumeration)
        {
            return (enumeration == null);
        }

        [Pure]
        public static bool IsNotNull<T>([CanBeNull] this IEnumerable<T> enumeration)
        {
            return (enumeration != null);
        }

        [Pure]
        public static bool IsNullOrEmpty<T>([CanBeNull] this IEnumerable<T> enumeration)
        {
            return (enumeration == null || !enumeration.Any());
        }

        [Pure]
        public static bool IsNotNullOrEmpty<T>([CanBeNull] this IEnumerable<T> enumeration)
        {
            return (enumeration != null && enumeration.Any());
        }

        [Pure]
        public static bool ContainsAll([NotNull] this IEnumerable<int> enumeration, [CanBeNull] IEnumerable<int> itemsToLookFor)
        {
            if (itemsToLookFor == null)
                return false;

            return enumeration.All(itemsToLookFor.Contains);
        }

        [Pure]
        public static bool IsEmpty<T>([NotNull] this IEnumerable<T> enumeration)
        {
            return !enumeration.Any();
        }

        [Pure, CanBeNull]
        public static IEnumerable<string> ToLower([CanBeNull] this IEnumerable<string> enumeration)
        {
            if (enumeration == null)
                return null;

            return enumeration.Select(s => s.ToLower());
        }

        [Pure, CanBeNull]
        public static IEnumerable<string> ToUpper([CanBeNull] this IEnumerable<string> enumeration)
        {
            if (enumeration == null)
                return null;

            return enumeration.Select(s => s.ToUpper());
        }

        [Pure, CanBeNull]
        public static string ToCommaDelimitedString<T>([NotNull] this IEnumerable<T> enumeration, bool shouldIncludeSpaceAfterComma=false)
        {
            return ToDelimitedString(enumeration, shouldIncludeSpaceAfterComma ? ", " : ",");
        }

        //public static string ToLineDelimitedString<T>(this IEnumerable<T> enumeration)
        //{
        //    return ToDelimitedString(enumeration, "\n");
        //}

        //public static string ToHtmlLineDelimitedString<T>(this IEnumerable<T> enumeration)
        //{
        //    return ToDelimitedString(enumeration, "<br/>");
        //}
        
        [Pure, CanBeNull]
        public static string ToDelimitedString<T>([CanBeNull] this IEnumerable<T> enumeration, [NotNull] string delimiter)
        {
            if (enumeration == null)
                return null;

            return String.Join(delimiter, enumeration.ToArray());
        }

        public static void ForEach<T>([CanBeNull] this IEnumerable<T> enumeration, [NotNull] Action<T> action)
        {
            if (enumeration == null)
                return;

            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        [Pure, CanBeNull]
        public static IEnumerable<T> GetDuplicateItems<T>([CanBeNull] this IEnumerable<T> enumeration)
        {
            if (enumeration == null)
                return null;

            IEnumerable<T> duplicateItems = (
                from item in enumeration
                group item by item into grp
                where grp.Count() > 1
                select grp.Key);

            return duplicateItems;
        }

        [Pure, NotNull]
        public static IEnumerable<T> WhereNotNull<T>([NotNull] this IEnumerable<T?> enumeration)
            where T : struct
        {
// ReSharper disable once PossibleInvalidOperationException
            return enumeration.Where(i => i.HasValue).Select(i => i.Value);
        }

        [Pure, NotNull]
        public static IEnumerable<T> WhereNotNull<T>([NotNull] this IEnumerable<T> enumeration)
            where T : class
        {
            return enumeration.Where(i => i != null);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> enumeration, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (TSource element in enumeration)
            {
                if (seenKeys.Add(keySelector(element)))
                    yield return element;
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> enumeration, Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer)
        {
            var seenKeys = new HashSet<TKey>(comparer);
            foreach (TSource element in enumeration)
            {
                if (seenKeys.Add(keySelector(element)))
                    yield return element;
            }
        }

        [NotNull]
        public static IEnumerable<IEnumerable<T>> Chunk<T>([NotNull] this IEnumerable<T> enumeration, int chunkSize)
        {
            int skip = 0;
            yield return enumeration.Skip(skip).Take(chunkSize);
// ReSharper disable once RedundantAssignment
            skip += chunkSize;
        }

    }
}
