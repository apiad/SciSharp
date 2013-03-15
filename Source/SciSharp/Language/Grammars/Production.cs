namespace SciSharp.Language.Grammars
{
    internal struct Production<T>
        where T : Node, new()
    {
        internal Def<T> Definition;
        internal ProductionList<T> List;

        public Production(Def<T> definition, ProductionList<T> rule)
        {
            Definition = definition;
            List = rule;
        }

        public override string ToString()
        {
            return Definition.ToString() + " -> " + List;
        }
    }
}
