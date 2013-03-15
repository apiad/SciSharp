using System;
using System.Collections.Generic;
using System.Linq;


namespace SciSharp.Collections
{
    public static class Trees
    {
        public static IEnumerable<T> PreOrder<T>(this ITree<T> tree)
        {
            if (tree == null)
                throw new ArgumentNullException("tree");

            yield return tree.Value;

            foreach (T item in tree.PreOrder())
                yield return item;
        }

        public static IEnumerable<T> PostOrder<T>(this ITree<T> tree)
        {
            if (tree == null)
                throw new ArgumentNullException("tree");

            foreach (T item in tree.PostOrder())
                yield return item;

            yield return tree.Value;
        }

        public static int Count<T>(this ITree<T> tree)
        {
            if (tree == null)
                throw new ArgumentNullException("tree");

            return 1 + tree.Children.Sum(child => Count(child));
        }

        public static int Height<T>(this ITree<T> tree)
        {
            if (tree == null)
                throw new ArgumentNullException("tree");

            return tree.Children.Select(child => 1 + child.Height()).Append(0).Max();
        }
    }
}
