// ReSharper disable once CheckNamespace
namespace System
{
    public static class MathExtensions
    {
        public static bool IsOdd(this int integer)
        {
            return ((integer % 2) != 0);
        }

        public static bool IsEven(this int integer)
        {
            return ((integer % 2) == 0);
        }

        public static bool IsNullOrZero(this int? integer)
        {
            return !integer.HasValue || integer == 0;
        }

        public static bool IsNotNullOrZero(this int? integer)
        {
            return integer.HasValue && integer != 0;
        }

    }
}