using System;


namespace SciSharp.Language.Grammars
{
    public class Ref<T> : GrammarItem<T>
        where T : Node, new()
    {
        internal Rule<T> Reference;

        internal Ref() {}

        public static Ref<T> operator ==(Ref<T> left, Rule<T> right)
        {
            left.Reference = right;
            return left;
        }

        public static Ref<T> operator !=(Ref<T> left, Rule<T> right)
        {
            throw new InvalidOperationException("This operator cannot be used.");
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
