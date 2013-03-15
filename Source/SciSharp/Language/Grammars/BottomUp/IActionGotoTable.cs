namespace SciSharp.Language.Grammars.BottomUp
{
    public interface IActionGotoTable<T>
        where T : Node, new()
    {
        ParsingAction<T> Action(int state, Token<T> symbol);

        int Goto(int state, Def<T> symbol);
    }
}
