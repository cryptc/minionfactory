using Common.JetBrainsAnnotations;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class SystemExtensions
    {
        [Pure]
        public static bool IsNull([CanBeNull] this object obj)
        {
            return (obj == null);
        }

        [Pure]
        public static bool IsNotNull([CanBeNull] this object obj)
        {
            return (obj != null);
        }

        [Pure]
        public static bool IsValueType<T>(this T obj)
        {
            return typeof(T).IsValueType;
        }
        
        //[Pure]
        //public static bool IsBetween<T>(this T item, T lowerBound, T upperBound)
        //    where T : IComparable<T>
        //{
        //    return item.CompareTo(lowerBound) >= 0 && item.CompareTo(upperBound) < 0;
        //}

        //[Pure, NotNull]
        //public static List<T> ToListFromCommaDelimitedString<T>([CanBeNull] this string commaDelimitedString)
        //{
        //    var newList = new List<T>();

        //    if (commaDelimitedString.IsNullOrEmpty())
        //        return newList;

        //    // ReSharper disable once PossibleNullReferenceException
        //    List<string> strings = commaDelimitedString.Split(',').ToList();

        //    foreach (string str in strings)
        //    {
        //        string trimmedStr = str.Trim();

        //        if (trimmedStr.Length == 0)
        //            continue;

        //        T newType = (T)Convert.ChangeType(trimmedStr, typeof(T));

        //        newList.Add(newType);
        //    }

        //    return newList;
        //}

    }
}