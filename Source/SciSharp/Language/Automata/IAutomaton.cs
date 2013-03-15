namespace SciSharp.Language.Automata
{
    public interface IAutomaton
    {
        bool Recognized { get; }

        bool Step();

        void Feed(string input);
    }
}
