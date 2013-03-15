using SciSharp.Language.Grammars;


namespace SciSharp.Language
{
    public interface IParser<T>
        where T : Node, new()
    {
        Grammar<T> Grammar { get; }

        T Parse(TokenStream<T> str);
    }
}
