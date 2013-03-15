using System;
using System.Collections.Generic;


namespace SciSharp.Language.Grammars.TopDown
{
    public class PredictiveParser<T> : IParser<T>
        where T : Node, new()
    {
        private readonly Grammar<T> grammar;

        private readonly Dictionary<Def<T>, ProductionList<T>> rules;

        public PredictiveParser(Grammar<T> grammar)
        {
            if (grammar == null)
                throw new ArgumentNullException("grammar");

            this.grammar = grammar;
            rules = new Dictionary<Def<T>, ProductionList<T>>();

            Initialize();
        }

        #region IParser<T> Members

        public Grammar<T> Grammar
        {
            get { return grammar; }
        }

        public T Parse(TokenStream<T> str)
        {
            throw new NotImplementedException();
        }

        #endregion

        private void Initialize() {}

        private T Parse(Def<T> symbol)
        {
            throw new NotImplementedException();
        }
    }
}
