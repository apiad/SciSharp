using System;

using SciSharp.Language.Automata;


namespace SciSharp.Language.Regex
{
    public abstract class RegexBinaryNode : IRegexNode
    {
        private readonly IRegexNode[] nodes;

        protected RegexBinaryNode(IRegexNode left, IRegexNode right)
        {
            if (left == null)
                throw new ArgumentNullException("left");

            if (right == null)
                throw new ArgumentNullException("right");

            nodes = new[] {left, right};
        }

        public IRegexNode Left
        {
            get { return nodes[0]; }
            set { throw new NotImplementedException(); }
        }

        public IRegexNode Right
        {
            get { return nodes[1]; }
        }

        #region IRegexNode Members

        public abstract FNDADescription Compile();

        public abstract int Priority { get; }

        public abstract IRegexNode Simplify();

        #endregion
    }
}
