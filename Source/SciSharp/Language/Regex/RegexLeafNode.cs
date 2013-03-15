using SciSharp.Language.Automata;


namespace SciSharp.Language.Regex
{
    public abstract class RegexLeafNode : IRegexNode
    {
        #region IRegexNode Members

        public abstract FNDADescription Compile();

        public abstract int Priority { get; }

        public abstract IRegexNode Simplify();

        #endregion
    }
}
