using System;
using System.Collections.Generic;
using System.Linq;


namespace SciSharp.Language.Grammars.Generation
{
    public static class Selectors
    {
        private static readonly Random Rand = new Random();

        public static T Random<T>(IEnumerable<T> set)
        {
            T[] items = set.ToArray();
            double prob = 1d/items.Length;

            double u = Rand.NextDouble();
            int i = 1;

            while (u > prob*i)
                i++;

            return items[i - 1];
        }

        public static T First<T>(IEnumerable<T> set)
        {
            return set.First();
        }

        public static ProductionRule<T> Filter<T>(IEnumerable<ProductionRule<T>> items)
            where T : Node, new()
        {
            return items.First(item => item.CheckPredicates());
        }
    }
}
