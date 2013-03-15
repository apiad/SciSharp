namespace SciSharp.Language.Grammars
{
    public abstract class GrammarItem<T>
        where T : Node, new()
    {
        internal Grammar<T> Grammar;
    }
}
