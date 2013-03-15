namespace SciSharp.Language.Automata
{
    public interface IFDATransitionFunction
    {
        int Evaluate(char character, int state);
    }
}
