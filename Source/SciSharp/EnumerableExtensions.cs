using System;
using System.Collections.Generic;


namespace SciSharp
{
    public static class EnumerableExtensions
    {
        public static T ArgMax<T>(this IEnumerable<T> items, Func<T, double> selector)
        {
            double value;
            return ArgMax(items, selector, out value);
        }

        public static T ArgMax<T>(this IEnumerable<T> items, Func<T, double> selector, out double value)
        {
            T max = default(T);
            value = double.NegativeInfinity;
            bool found = false;

            foreach (T item in items)
            {
                double itemValue = selector(item);

                if (!found || itemValue > value)
                {
                    found = true;
                    max = item;
                    value = itemValue;
                }
            }

            if (!found)
                throw new InvalidOperationException("The collection is empty.");

            return max;
        }

        public static int IndexMax<T>(this IEnumerable<T> items, Func<T, double> selector)
        {
            double value = double.NegativeInfinity;
            bool found = false;
            int idx = -1;
            int i = 0;

            foreach (T item in items)
            {
                double itemValue = selector(item);

                if (!found || itemValue > value)
                {
                    found = true;
                    value = itemValue;
                    idx = i;
                }

                i++;
            }

            if (!found)
                throw new InvalidOperationException("The collection is empty.");

            return idx;
        }

        public static T ArgMin<T>(this IEnumerable<T> items, Func<T, double> selector)
        {
            double value;
            return ArgMin(items, selector, out value);
        }

        public static T ArgMin<T>(this IEnumerable<T> items, Func<T, double> selector, out double value)
        {
            T min = default(T);
            value = double.PositiveInfinity;
            bool found = false;

            foreach (T item in items)
            {
                double itemValue = selector(item);

                if (!found || itemValue < value)
                {
                    found = true;
                    min = item;
                    value = itemValue;
                }
            }

            if (!found)
                throw new InvalidOperationException("The collection is empty.");

            return min;
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> items, T last)
        {
            foreach (T item in items)
                yield return item;

            yield return last;
        }

        public static IEnumerable<Indexed<T>> Enumerate<T>(this IEnumerable<T> items)
        {
            int index = 0;

            foreach (T item in items)
                yield return new Indexed<T>(index++, item);
        }

        public static void Consume<T>(this IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                // Consuming this item
            }
        }

        public static IEnumerable<IEnumerable<T>> Batches<T>(this IEnumerable<T> items, int size)
        {
            IEnumerator<T> enumerator = items.GetEnumerator();

            while (enumerator.MoveNext())
                yield return GetBatch(enumerator, size);
        }

        private static IEnumerable<T> GetBatch<T>(IEnumerator<T> enumerator, int size)
        {
            do
            {
                yield return enumerator.Current;
                size--;
            } while (size > 0 && enumerator.MoveNext());
        }
    }
}
