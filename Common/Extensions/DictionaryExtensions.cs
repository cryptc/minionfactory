using Common.JetBrainsAnnotations;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        [Pure]
        public static TValue TryGetValueOrDefault<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue item;
            if (!dictionary.TryGetValue(key, out item))
                item = default(TValue);

            return item;
        }

        [Pure]
        public static TValue TryGetValueOrDefaultWithNew<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, TKey key)
            where TValue : new()
        {
            TValue item;
            if (!dictionary.TryGetValue(key, out item))
                item = new TValue();

            return item;
        }

        public static T TryGetValueLogIfMissing<S, T>([NotNull] this IDictionary<S, T> thisDict, S key, [CanBeNull] string message=null)
        {
            throw new NotImplementedException();
            //T item;
            //if (thisDict.TryGetValue(key, out item))
            //    return item;

            //if (message.IsNullOrEmpty())
            //    Cat5NLogger.Warn("Could not find object of type {1} in dictionary for key: {0}".FormatString(key, typeof(T)));
            //else
            //    Cat5NLogger.Warn("{0} (Key: {1}, Type: {2})".FormatString(message, key, typeof(T)));

            //return item;
        }

        [NotNull]
        public static List<T> SafeGetItemsLogIfMissing<S, T>([NotNull] this IDictionary<S, T> thisDict, [NotNull] IEnumerable<S> keys)
            where T : class
        {
            return keys.Select(key => thisDict.TryGetValueLogIfMissing(key)).Where(item => item != null).ToList();
        }

        public static void AddToCollection<TKey, TValue, TCollection>([NotNull] this IDictionary<TKey, TCollection> dictionary, [CanBeNull] TKey key,
            [CanBeNull] TValue value, bool ignoreNullKey=false, bool ignoreNullValue=false)
            where TCollection : ICollection<TValue>, new()
        {
// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (!key.IsValueType() && ignoreNullKey && key == null)
                return;

// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (!value.IsValueType() && ignoreNullValue && value == null)
                return;

            TCollection dictValue;
            if (dictionary.TryGetValue(key, out dictValue))
            {
                dictValue.Add(value);
            }
            else
            {
                var newCollection = new TCollection()
                {
                    value,
                };

                dictionary.Add(key, newCollection);
            }
        }

        /// <summary> Do NOT use ignoreNullKey, nor ignoreNullValue, the respective parameter is a ValueType </summary>
        public static void AddRangeToCollectionIfItemNotNull<TKey, TValue, TCollection>([NotNull] this IDictionary<TKey, TCollection> dictionary,
            [CanBeNull] TKey key, [CanBeNull] TCollection collection, bool ignoreNullKey=false, bool ignoreNullValue=false)
            where TCollection : class, ICollection<TValue>, new()
        {
// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (!key.IsValueType() && ignoreNullKey && key == null)
                return;

            if (ignoreNullValue && collection == null)
                return;

            if (!dictionary.ContainsKey(key))
            {
                var newCollection = new TCollection();

                dictionary.Add(key, newCollection);
            }

            bool isValueAValueType = false;
            if (collection.Count > 0)  // NOTE:  This will blow up on a null if we are not checking for nulls.  That's by design.
                isValueAValueType = collection.First().IsValueType();

            foreach (var value in collection)
            {
// ReSharper disable once CompareNonConstrainedGenericWithNull
                if (!isValueAValueType && value == null)
                    continue;

                dictionary[key].Add(value);
            }
        }

        public static void AddIfValueNotNull<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, TKey key, [CanBeNull] TValue value)
            where TValue : class
        {
            if (value != null)
                dictionary.Add(key, value);
        }

        public static void AddSafelyByOverwritingExistingValue<TKey, TValue>([NotNull] this Dictionary<TKey, TValue> ciDictionary, TKey key,
            TValue value, bool ignoreNullKey=false, bool ignoreNullValue=false)
        {
// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (!key.IsValueType() && ignoreNullKey && key == null)
                return;

// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (!value.IsValueType() && ignoreNullValue && value == null)
                return;

            if (ciDictionary.ContainsKey(key))
                ciDictionary[key] = value;
            else
                ciDictionary.Add(key, value);
        }

        public static void AddSafelyByIgnoringNewValue<TKey, TValue>([NotNull] this Dictionary<TKey, TValue> ciDictionary, [CanBeNull] TKey key,
            [CanBeNull] TValue value, bool ignoreNullKey=false, bool ignoreNullValue=false)
        {
// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (!key.IsValueType() && ignoreNullKey && key == null)
                return;

// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (!value.IsValueType() && ignoreNullValue && value == null)
                return;

            if (ciDictionary.ContainsKey(key))
                return;

            ciDictionary.Add(key, value);
        }

    }
}