namespace SciSharp.Language
{
    public interface ILexer<out T>
    {
        T NextToken();
    }
}
