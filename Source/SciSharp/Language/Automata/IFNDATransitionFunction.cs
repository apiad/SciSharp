using SciSharp.Collections;


namespace SciSharp.Language.Automata
{
    public interface IFNDATransitionFunction
    {
        IFiniteSet<int> Evaluate(char character, int state);
    }
}
