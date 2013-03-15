using System;

using SciSharp.Language.Automata;


namespace SciSharp.Language.Regex
{
    public abstract class RegexUnaryNode : IRegexNode
    {
        //private readonly IRegexNode[] nodes;

        protected RegexUnaryNode(IRegexNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            InternalNode = node;
        }

        public IRegexNode InternalNode { get; private set; }

        #region IRegexNode Members

        public abstract FNDADescription Compile();

        public abstract int Priority { get; }

        public abstract IRegexNode Simplify();

        #endregion
    }
}
