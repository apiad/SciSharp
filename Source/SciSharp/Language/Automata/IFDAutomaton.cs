namespace SciSharp.Language.Automata
{
    public interface IFDAutomaton : IAutomaton
    {
        FDADescription Description { get; }
    }
}
