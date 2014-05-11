using Common.JetBrainsAnnotations;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class StringExtensions
    {
        [Pure, NotNull, StringFormatMethod("formatString")]
        public static string Fs([NotNull] this string formatString, [NotNull] params object[] args)
        {
            return String.Format(formatString, args);
        }

        [Pure]
        public static bool ContainsAny([NotNull] this string str, [NotNull] string[] items)
        {
            foreach (var item in items)
            {
                if (str.Contains(item))
                    return true;
            }

            return false;
        }

        [Pure]
        public static bool IsNotNullOrEmpty([CanBeNull] this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        [Pure]
        public static bool IsNullOrWhiteSpace([CanBeNull] this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        [Pure]
        public static int Compare([NotNull] this string first, [NotNull] string second, StringComparison comparison=StringComparison.Ordinal)
        {
            return string.Compare(first, second, comparison);
        }

    }
}