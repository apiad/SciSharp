using System;


namespace SciSharp.Language.Grammars
{
    public class Def<T> : Rule<T>
        where T : Node, new()
    {
        internal ProductionList<T> List;
        internal string Name;

        internal Def()
        {
            List = new ProductionList<T>();
        }

        public T Node { get; internal set; }

        public virtual bool IsNonTerminal
        {
            get { return true; }
        }

        public ProductionList<T> With(Func<ParameterList<T>, T> rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            return (new ProductionRule<T> {Defs = {this}, Grammar = Grammar}).With(rule);
        }

        public ProductionList<T> With(Func<T> rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            return (new ProductionRule<T> {Defs = {this}, Grammar = Grammar}).With(rule);
        }

        public ProductionList<T> With(Action<ParameterList<T>, T> rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            return (new ProductionRule<T> {Defs = {this}, Grammar = Grammar}).With(rule);
        }

        public ProductionList<T> With(Action<T> rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            return (new ProductionRule<T> {Defs = {this}, Grammar = Grammar}).With(rule);
        }

        public ProductionList<T> With(Action rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            return (new ProductionRule<T> {Defs = {this}, Grammar = Grammar}).With(rule);
        }

        public ProductionList<T> With<TNode>(Action<ParameterList<T>, TNode> rule)
            where TNode : T, new()
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            return (new ProductionRule<T> {Defs = {this}, Grammar = Grammar}).With(rule);
        }

        public ProductionList<T> With<TNode>(Action<TNode> rule)
            where TNode : T, new()
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            return (new ProductionRule<T> {Defs = {this}, Grammar = Grammar}).With(rule);
        }

        public ProductionList<T> With<TNode>(Action rule)
            where TNode : T, new()
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            return (new ProductionRule<T> {Defs = {this}, Grammar = Grammar}).With<TNode>(rule);
        }

        public static Def<T> operator %(Def<T> left, Def<T> right)
        {
            if (left.Grammar != right.Grammar)
                throw new InvalidOperationException("Both rules must be defined in the same grammar.");

            var rule = new ProductionRule<T>();
            rule.Defs.Add(right);
            rule.Grammar = left.Grammar;
            return left%rule;
        }

        public static ProductionRule<T> operator +(Def<T> left, Def<T> right)
        {
            if (left.Grammar != right.Grammar)
                throw new InvalidOperationException("Both rules must be defined in the same grammar.");

            var rule = new ProductionRule<T>();
            rule.Defs.Add(left);
            rule.Defs.Add(right);
            rule.Grammar = left.Grammar;
            return rule;
        }

        public static ProductionRule<T> operator +(Def<T> left, string right)
        {
            return left + left.Grammar.GetImplicitToken(right);
        }

        public static ProductionRule<T> operator +(string left, Def<T> right)
        {
            return right.Grammar.GetImplicitToken(left) + right;
        }

        public static ProductionRule<T> operator +(Def<T> left, char right)
        {
            return left + left.Grammar.GetImplicitToken(right);
        }

        public static ProductionRule<T> operator +(char left, Def<T> right)
        {
            return right.Grammar.GetImplicitToken(left) + right;
        }

        public static ProductionList<T> operator |(Def<T> left, Def<T> right)
        {
            return left.AsList() | right;
        }

        public override string ToString()
        {
            return Name;
        }

        internal override ProductionList<T> AsList()
        {
            return new ProductionList<T>
                   {
                       Grammar = Grammar,
                       List =
                       {
                           new ProductionRule<T>
                           {
                               Grammar = Grammar,
                               Defs = {this}
                           }
                       }
                   };
        }
    }
}
