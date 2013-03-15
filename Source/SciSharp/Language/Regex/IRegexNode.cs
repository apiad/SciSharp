using SciSharp.Language.Automata;


namespace SciSharp.Language.Regex
{
    public interface IRegexNode
    {
        int Priority { get; }

        FNDADescription Compile();

        IRegexNode Simplify();
    }
}
