using System;


namespace SciSharp.Language.Grammars
{
    public class GrammarBuilder<T> : GrammarItem<T>
        where T : Node, new()
    {
        private int count;

        public GrammarBuilder(Grammar<T> grammar)
        {
            Grammar = grammar;
        }

        public Def<T> this[params Rule<T>[] items]
        {
            get
            {
                if (items.Length == 0)
                    throw new ArgumentException("Must provide at least one rule.");

                Def<T> def = Grammar.Rule("R" + count++);

                if (items.Length == 1)
                    return def%(items[0].AsList() | Grammar.Epsilon);

                ProductionList<T> list = items[0].AsList();

                for (int i = 1; i < items.Length; i++)
                    list |= items[i].AsList();

                return def%list;
            }
        }

        public static Def<T> operator *(Rule<T> left, GrammarBuilder<T> right)
        {
            int index = right.count++;

            Def<T> def = right.Grammar.Rule("R" + index)%left.AsList();
            Def<T> list = right.Grammar.Rule("R" + index + "*");

            return list%(def + list | right.Grammar.Epsilon);
        }

        public static Def<T> operator +(Rule<T> left, GrammarBuilder<T> right)
        {
            int index = right.count++;

            Def<T> def = right.Grammar.Rule("R" + index)%left.AsList();
            Def<T> list = right.Grammar.Rule("R" + index + "+");

            return list%(def + list | def);
        }
    }
}
