using System.Collections.Generic;


namespace SciSharp.Language.Grammars.Generation
{
    public class RecursiveGenerator<T> : Generator<T>
        where T : Node, new()
    {
        public RecursiveGenerator(Grammar<T> grammar, Selector<ProductionRule<T>> productionSelector)
            : base(grammar)
        {
            ProductionSelector = productionSelector;
        }

        public Selector<ProductionRule<T>> ProductionSelector { get; set; }

        protected override IEnumerable<Token<T>> GenerateSentence(Def<T> symbol)
        {
            ProductionRule<T> rule = ProductionSelector(symbol.List);

            // Creo los nodos nuevos para esta producción
            var nodes = new T[rule.Count];

            for (int i = 0; i < nodes.Length; i++)
                nodes[i] = new T();

            // Aplico las reglas semánticas de esta producción
            rule.ApplyRule(nodes, symbol.Node);

            var list = new List<Token<T>>();

            // Recursivamente derivo cada uno de los nodos
            foreach (var def in rule)
            {
                if (def.IsNonTerminal)
                    list.AddRange(GenerateSentence(def));
                else
                    list.Add((Token<T>) def);
            }

            return list;
        }
    }
}
