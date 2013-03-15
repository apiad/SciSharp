using System;
using System.Linq;
using System.Text;


namespace SciSharp.Collections
{
    public static class Sets
    {
        public static ISet<T> FromMethod<T>(Predicate<T> predicate)
        {
            return new MethodSet<T>(predicate);
        }

        public static ISet<T> Union<T>(this ISet<T> left, ISet<T> right)
        {
            return FromMethod<T>(x => left.Contains(x) || right.Contains(x));
        }

        public static ISet<T> Intersection<T>(this ISet<T> left, ISet<T> right)
        {
            return FromMethod<T>(x => left.Contains(x) && right.Contains(x));
        }

        public static ISet<T> Difference<T>(this ISet<T> left, ISet<T> right)
        {
            return FromMethod<T>(x => left.Contains(x) && !right.Contains(x));
        }

        public static IFiniteSet<int> Displaced(this IFiniteSet<int> set, int displace)
        {
            return new DisplacedIntSet(set, displace);
        }

        public static string ToString<T>(this IFiniteSet<T> set)
        {
            var s = new StringBuilder("{ ", 10*set.Count);

            bool first = true;

            foreach (T item in set)
            {
                if (!first)
                    s.AppendFormat(", {0}", item);
                else
                    s.Append(item);

                first = false;
            }

            s.Append(" }");

            return s.ToString();
        }

        public static bool Equals<T>(this IFiniteSet<T> set, IFiniteSet<T> other)
        {
            return set.Count == other.Count && set.All(other.Contains);
        }
    }
}
