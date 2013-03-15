using System;
using System.Collections.Generic;


namespace SciSharp.Collections
{
    public class Tree<T> : ITree<T>
    {
        private readonly List<Tree<T>> children = new List<Tree<T>>();

        public Tree<T> this[int index]
        {
            get
            {
                if (index < 0 || index > children.Count)
                    throw new ArgumentOutOfRangeException("index");

                return children[index];
            }
        }

        public int ChildrenCount
        {
            get { return children.Count; }
        }

        #region ITree<T> Members

        public T Value { get; set; }

        public IEnumerable<ITree<T>> Children
        {
            get { return children; }
        }

        #endregion

        public void Add(Tree<T> tree)
        {
            if (tree == null)
                throw new ArgumentNullException("tree");

            children.Add(tree);
        }

        public bool Remove(Tree<T> tree)
        {
            if (tree == null)
                throw new ArgumentNullException("tree");

            return children.Remove(tree);
        }
    }
}
