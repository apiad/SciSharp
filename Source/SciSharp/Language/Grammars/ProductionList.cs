using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace SciSharp.Language.Grammars
{
    public class ProductionList<T> : Rule<T>, IEnumerable<ProductionRule<T>>
        where T : Node, new()
    {
        internal List<ProductionRule<T>> List;

        internal ProductionList()
        {
            List = new List<ProductionRule<T>>();
        }

        #region IEnumerable<ProductionRule<T>> Members

        public IEnumerator<ProductionRule<T>> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public static Def<T> operator %(Def<T> left, ProductionList<T> right)
        {
            if (left.Grammar != right.Grammar)
                throw new InvalidOperationException("Both rules must be defined in the same grammar.");

            left.List = right;
            left.Grammar.Productions.Add(new Production<T>(left, right));
            return left;
        }

        public static ProductionList<T> operator |(Def<T> left, ProductionList<T> right)
        {
            if (left.Grammar != right.Grammar)
                throw new InvalidOperationException("Both rules must be defined in the same grammar.");

            var rule = new ProductionRule<T>();
            rule.Grammar = left.Grammar;
            rule.Defs.Add(left);

            right.List.Insert(0, rule);
            return right;
        }

        public static ProductionList<T> operator |(ProductionList<T> left, Def<T> right)
        {
            if (left.Grammar != right.Grammar)
                throw new InvalidOperationException("Both rules must be defined in the same grammar.");

            var rule = new ProductionRule<T>();
            rule.Grammar = right.Grammar;
            rule.Defs.Add(right);

            left.List.Add(rule);
            return left;
        }

        public static ProductionList<T> operator |(ProductionRule<T> left, ProductionList<T> right)
        {
            if (left.Grammar != right.Grammar)
                throw new InvalidOperationException("Both rules must be defined in the same grammar.");

            right.List.Insert(0, left);
            return right;
        }

        public static ProductionList<T> operator |(ProductionList<T> left, ProductionRule<T> right)
        {
            if (left.Grammar != right.Grammar)
                throw new InvalidOperationException("Both rules must be defined in the same grammar.");

            left.List.Add(right);
            return left;
        }

        public static ProductionList<T> operator |(ProductionList<T> left, ProductionList<T> right)
        {
            if (left.Grammar != right.Grammar)
                throw new InvalidOperationException("Both rules must be defined in the same grammar.");

            left.List.AddRange(right.List);
            return left;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(List[0]);

            for (int i = 1; i < List.Count; i++)
                sb.Append(" | " + List[i].ToString());

            return sb.ToString();
        }

        internal override ProductionList<T> AsList()
        {
            return this;
        }
    }
}
