using System;
using System.Collections.Generic;
using System.Linq;


namespace SciSharp.Collections
{
    public static class Combinatorics
    {
        #region Combinations

        public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> set, int size,
                                                       Predicate<T> elementFilter, Predicate<T[]> subsetFilter)
        {
            if (set == null)
                throw new ArgumentNullException("set");

            T[] items = set is T[] ? (T[]) set : set.ToArray();

            if (size < 0 || size >= items.Length)
                throw new ArgumentOutOfRangeException("size");

            return Combinations(items, size, elementFilter, subsetFilter, 0, new List<T>());
        }

        public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> set, int size,
                                                       Predicate<T> elementFilter)
        {
            return Combinations(set, size, elementFilter, null);
        }

        public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> set, int size,
                                                       Predicate<T[]> subsetFilter)
        {
            return Combinations(set, size, null, subsetFilter);
        }

        public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> set, int size)
        {
            return Combinations(set, size, null, null);
        }

        public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> set,
                                                       Predicate<T> elementFilter, Predicate<T[]> subsetFilter)
        {
            if (set == null)
                throw new ArgumentNullException("set");

            return Combinations(set, set.Count(), elementFilter, subsetFilter);
        }

        public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> set, Predicate<T[]> subsetFilter)
        {
            if (set == null)
                throw new ArgumentNullException("set");

            return Combinations(set, set.Count(), null, subsetFilter);
        }

        public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> set, Predicate<T> elementFilter)
        {
            if (set == null)
                throw new ArgumentNullException("set");

            return Combinations(set, set.Count(), elementFilter, null);
        }

        public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> set)
        {
            if (set == null)
                throw new ArgumentNullException("set");

            return Combinations(set, set.Count(), null, null);
        }

        private static IEnumerable<T[]> Combinations<T>(T[] set, int size,
                                                        Predicate<T> elementFilter, Predicate<T[]> subsetFilter, int actual, List<T> combination)
        {
            if (combination.Count == size)
            {
                T[] comb = combination.ToArray();

                if (subsetFilter == null || subsetFilter(comb))
                    yield return comb;
            }

            for (int i = actual; i < set.Length; i++)
            {
                T current = set[i];

                if (elementFilter == null || elementFilter(current))
                {
                    combination.Add(current);

                    foreach (var c in Combinations(set, size, elementFilter, subsetFilter, i + 1, combination))
                        yield return c;

                    combination.RemoveAt(combination.Count - 1);
                }
            }
        }

        #endregion

        #region Variations

        public static IEnumerable<T[]> Variations<T>(this IEnumerable<T> set, int size, Predicate<T[]> subsetFilter)
        {
            return Variations(set, size, null, subsetFilter);
        }

        public static IEnumerable<T[]> Variations<T>(this IEnumerable<T> set, int size)
        {
            return Variations(set, size, null, null);
        }

        public static IEnumerable<T[]> Variations<T>(this IEnumerable<T> set, int size, Predicate<T> elementFilter)
        {
            return Variations(set, size, elementFilter, null);
        }

        public static IEnumerable<T[]> Variations<T>(this IEnumerable<T> set)
        {
            return Variations(set, null, null);
        }

        public static IEnumerable<T[]> Variations<T>(this IEnumerable<T> set, Predicate<T[]> subsetFilter)
        {
            return Variations(set, null, subsetFilter);
        }

        public static IEnumerable<T[]> Variations<T>(this IEnumerable<T> set, Predicate<T> elementFilter)
        {
            return Variations(set, elementFilter, null);
        }

        public static IEnumerable<T[]> Variations<T>(this IEnumerable<T> set,
                                                     Predicate<T> elementFilter, Predicate<T[]> subsetFilter)
        {
            if (set == null)
                throw new ArgumentNullException("set");

            return Variations(set, set.Count(), elementFilter, subsetFilter);
        }

        public static IEnumerable<T[]> Variations<T>(this IEnumerable<T> set, int size,
                                                     Predicate<T> elementFilter, Predicate<T[]> subsetFilter)
        {
            if (set == null)
                throw new ArgumentNullException("set");

            if (size < 0 || size >= set.Count())
                throw new ArgumentOutOfRangeException("size");

            return Variations(set, size, elementFilter, subsetFilter, new bool[set.Count()], new List<T>());
        }

        private static IEnumerable<T[]> Variations<T>(IEnumerable<T> set, int size,
                                                      Predicate<T> elementFilter, Predicate<T[]> subsetFilter, bool[] used, List<T> variation)
        {
            if (variation.Count == size)
            {
                T[] var = variation.ToArray();

                if (subsetFilter == null || subsetFilter(var))
                    yield return var;
            }

            for (int i = 0; i < set.Count(); i++)
            {
                if (!used[i])
                {
                    T current = set.ElementAt(i);

                    if (elementFilter == null || elementFilter(current))
                    {
                        variation.Add(current);
                        used[i] = true;

                        foreach (var v in Variations(set, size, elementFilter, subsetFilter, used, variation))
                            yield return v;

                        variation.RemoveAt(variation.Count() - 1);
                        used[i] = false;
                    }
                }
            }
        }

        #endregion

        #region VariationsWithRepetition

        public static IEnumerable<T[]> VariationsWithRepetition<T>(this IEnumerable<T> set, int size, Predicate<T[]> subsetFilter)
        {
            return VariationsWithRepetition(set, size, null, subsetFilter);
        }

        public static IEnumerable<T[]> VariationsWithRepetition<T>(this IEnumerable<T> set, int size)
        {
            return VariationsWithRepetition(set, size, null, null);
        }

        public static IEnumerable<T[]> VariationsWithRepetition<T>(this IEnumerable<T> set, int size, Predicate<T> elementFilter)
        {
            return VariationsWithRepetition(set, size, elementFilter, null);
        }

        public static IEnumerable<T[]> VariationsWithRepetition<T>(this IEnumerable<T> set, Predicate<T[]> subsetFilter)
        {
            return VariationsWithRepetition(set, null, subsetFilter);
        }

        public static IEnumerable<T[]> VariationsWithRepetition<T>(this IEnumerable<T> set)
        {
            return VariationsWithRepetition(set, null, null);
        }

        public static IEnumerable<T[]> VariationsWithRepetition<T>(this IEnumerable<T> set, Predicate<T> elementFilter)
        {
            return VariationsWithRepetition(set, elementFilter, null);
        }

        public static IEnumerable<T[]> VariationsWithRepetition<T>(this IEnumerable<T> set,
                                                                   Predicate<T> elementFilter, Predicate<T[]> subsetFilter)
        {
            if (set == null)
                throw new ArgumentNullException("set");

            return VariationsWithRepetition(set, set.Count(), elementFilter, subsetFilter);
        }

        public static IEnumerable<T[]> VariationsWithRepetition<T>(this IEnumerable<T> set, int size,
                                                                   Predicate<T> elementFilter, Predicate<T[]> subsetFilter)
        {
            if (set == null)
                throw new ArgumentNullException("set");

            if (size < 0 || size >= set.Count())
                throw new ArgumentOutOfRangeException("size");

            return VariationsWithRepetition(set, size, elementFilter, subsetFilter, new List<T>());
        }

        private static IEnumerable<T[]> VariationsWithRepetition<T>(IEnumerable<T> set, int size,
                                                                    Predicate<T> elementFilter, Predicate<T[]> subsetFilter, List<T> variation)
        {
            if (variation.Count == size)
            {
                T[] var = variation.ToArray();

                if (subsetFilter == null || subsetFilter(var))
                    yield return var;
            }

            for (int i = 0; i < set.Count(); i++)
            {
                T current = set.ElementAt(i);

                if (elementFilter == null || elementFilter(current))
                {
                    variation.Add(current);

                    foreach (var v in VariationsWithRepetition(set, size, elementFilter, subsetFilter, variation))
                        yield return v;

                    variation.RemoveAt(variation.Count() - 1);
                }
            }
        }

        #endregion

        // BUG: for (int i = 0; i < set.Count(); i++) is O(N^2) !!
        // TODO: Fix the core methods to avoid multiple enumerations and use T[] instead of IEnumerable<T>

        public static long Factorial(long n)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException("n");

            long fact = 1;

            while (n > 0)
                fact *= n--;

            return fact;
        }

        public static long Combinations(long n, long k)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException("n");

            if (k < 0)
                throw new ArgumentOutOfRangeException("k");

            if (k > n)
                return 0;

            if (k == 0 || k == n)
                return 1;

            var pascal = new long[n,n];

            pascal[0, 0] = 1;

            for (int i = 0; i < n; i++)
            {
                pascal[i, 0] = 1;
                pascal[i, i] = 1;

                for (int j = 1; j < i; j++)
                    pascal[i, j] = pascal[i - 1, j - 1] + pascal[i - 1, j];
            }

            return pascal[n, k];
        }
    }
}
