using System;
using System.Collections.Generic;


namespace SciSharp.Language.Grammars
{
    public abstract class Rule<T> : GrammarItem<T>
        where T : Node, new()
    {
        private readonly List<Func<ParameterList<T>, bool>> semanticPredicates;

        protected Rule()
        {
            semanticPredicates = new List<Func<ParameterList<T>, bool>>();
        }


        protected internal void AddPredicate(Func<ParameterList<T>, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            semanticPredicates.Add(predicate);
        }

        protected internal void AddPredicate(Func<T, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            semanticPredicates.Add(p => predicate(p[0]));
        }

        protected internal void AddPredicate(Func<bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            AddPredicate((ParameterList<T> x) => predicate());
        }


        protected bool CheckPredicates(T[] items)
        {
            foreach (var predicate in semanticPredicates)
                if (!predicate(new ParameterList<T>(items)))
                    return false;

            return true;
        }

        protected bool CheckPredicates(T items)
        {
            foreach (var predicate in semanticPredicates)
                if (!predicate(new ParameterList<T>(items)))
                    return false;

            return true;
        }

        internal abstract ProductionList<T> AsList();
    }
}
