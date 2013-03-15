using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace SciSharp.Language.Grammars
{
    public class ProductionRule<T> : Rule<T>, IEnumerable<Def<T>>
        where T : Node, new()
    {
        internal T Node;
        private Func<ParameterList<T>, T> semanticRule;

        internal ProductionRule()
        {
            Defs = new List<Def<T>>();
        }

        internal List<Def<T>> Defs { get; set; }

        public int Count
        {
            get { return Defs.Count; }
        }

        public Func<ParameterList<T>, T> SemanticRule
        {
            get { return semanticRule; }
        }

        public Def<T> this[int index]
        {
            get { return Defs[index]; }
        }

        #region IEnumerable<Def<T>> Members

        public IEnumerator<Def<T>> GetEnumerator()
        {
            return Defs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public static Def<T> operator %(Def<T> left, ProductionRule<T> right)
        {
            if (left.Grammar != right.Grammar)
                throw new InvalidOperationException("Both rules must be defined in the same grammar.");

            return left%right.AsList();
        }

        public static ProductionRule<T> operator +(Def<T> left, ProductionRule<T> right)
        {
            if (left.Grammar != right.Grammar)
                throw new InvalidOperationException("Both rules must be defined in the same grammar.");

            right.Defs.Insert(0, left);
            return right;
        }

        public static ProductionRule<T> operator +(string left, ProductionRule<T> right)
        {
            return right.Grammar.GetImplicitToken(left) + right;
        }

        public static ProductionRule<T> operator +(char left, ProductionRule<T> right)
        {
            return right.Grammar.GetImplicitToken(left) + right;
        }

        public static ProductionRule<T> operator +(ProductionRule<T> left, Def<T> right)
        {
            left.Defs.Add(right);
            return left;
        }

        public static ProductionRule<T> operator +(ProductionRule<T> left, string right)
        {
            return left + left.Grammar.GetImplicitToken(right);
        }

        public static ProductionRule<T> operator +(ProductionRule<T> left, char right)
        {
            return left + left.Grammar.GetImplicitToken(right);
        }

        public static ProductionList<T> operator |(Def<T> left, ProductionRule<T> right)
        {
            if (left.Grammar != right.Grammar)
                throw new InvalidOperationException("Both rules must be defined in the same grammar.");

            var rule = new ProductionRule<T>();
            rule.Defs.Add(left);
            rule.Grammar = left.Grammar;

            return rule | right;
        }

        public static ProductionList<T> operator |(ProductionRule<T> left, Def<T> right)
        {
            if (left.Grammar != right.Grammar)
                throw new InvalidOperationException("Both rules must be defined in the same grammar.");

            var rule = new ProductionRule<T>();
            rule.Defs.Add(right);
            rule.Grammar = left.Grammar;

            return left | rule;
        }

        public static ProductionList<T> operator |(ProductionRule<T> left, ProductionRule<T> right)
        {
            if (left.Grammar != right.Grammar)
                throw new InvalidOperationException("Both rules must be defined in the same grammar.");

            var plist = new ProductionList<T>();

            plist.List.Add(left);
            plist.List.Add(right);
            plist.Grammar = left.Grammar;

            return plist;
        }

        public ProductionList<T> With(Func<ParameterList<T>, T> rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            SetRule(rule);
            return AsList();
        }

        public ProductionList<T> With(Func<T> rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            SetRule(rule);
            return AsList();
        }

        public ProductionList<T> With(Action<ParameterList<T>, T> rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            SetRule(rule);
            return AsList();
        }

        public ProductionList<T> With(Action<T> rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            SetRule(rule);
            return AsList();
        }

        public ProductionList<T> With(Action rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            SetRule(rule);
            return AsList();
        }

        public ProductionList<T> With<TNode>(Action<ParameterList<T>, TNode> rule)
            where TNode : T, new()
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            SetRule(rule);
            return AsList();
        }

        public ProductionList<T> With<TNode>(Action<TNode> rule)
            where TNode : T, new()
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            SetRule(rule);
            return AsList();
        }

        public ProductionList<T> With<TNode>(Action rule)
            where TNode : T, new()
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            SetRule(rule);
            return AsList();
        }

        protected internal void SetRule(Func<ParameterList<T>, T> rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            semanticRule = rule;
        }

        protected internal void SetRule(Func<T> rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            semanticRule = p => rule();
        }

        protected internal void SetRule(Action<ParameterList<T>, T> rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            semanticRule = p =>
                           {
                               T node = Node ?? new T();
                               rule(p, node);
                               return node;
                           };
        }

        protected internal void SetRule(Action<T> rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            semanticRule = p =>
                           {
                               T node = Node ?? new T();
                               rule(node);
                               return node;
                           };
        }

        protected internal void SetRule(Action rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            semanticRule = p =>
                           {
                               T node = Node ?? new T();
                               rule();
                               return node;
                           };
        }

        protected internal void SetRule<TNode>(Action<ParameterList<T>, TNode> rule)
            where TNode : T, new()
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            semanticRule = p =>
                           {
                               TNode node = (Node as TNode) ?? new TNode();
                               rule(p, node);
                               return node;
                           };
        }

        protected internal void SetRule<TNode>(Action<TNode> rule)
            where TNode : T, new()
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            semanticRule = p =>
                           {
                               TNode node = (Node as TNode) ?? new TNode();
                               rule(node);
                               return node;
                           };
        }

        protected internal void SetRule<TNode>(Action rule)
            where TNode : T, new()
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            semanticRule = p =>
                           {
                               TNode node = (Node as TNode) ?? new TNode();
                               rule();
                               return node;
                           };
        }

        internal T ApplyRule(T[] rightHand)
        {
            var nodes = new T[Defs.Count];

            for (int i = 0; i < Defs.Count; i++)
            {
                Defs[i].Node = rightHand[i];
                nodes[i] = Defs[i].Node;
            }

            return semanticRule != null ? semanticRule(new ParameterList<T>(rightHand)) : new T();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(Defs[0]);

            for (int i = 1; i < Defs.Count; i++)
                sb.Append(" " + Defs[i].ToString());

            return sb.ToString();
        }

        public bool CheckPredicates()
        {
            throw new NotImplementedException();
        }

        internal override ProductionList<T> AsList()
        {
            return new ProductionList<T> {Grammar = Grammar, List = {this}};
        }

        public void ApplyRule(T[] rightHand, T node)
        {
            Node = node;
            Node = ApplyRule(rightHand);
        }
    }
}
