using System.IO;

using SciSharp.Language.Grammars;
using SciSharp.Language.Grammars.BottomUp;
using SciSharp.Language.Grammars.TopDown;


namespace SciSharp.Language
{
    public static class Parsers
    {
        public static IParser<T> BuildLl<T>(Grammar<T> grammar)
            where T : Node, new()
        {
            return new PredictiveParser<T>(grammar);
        }

        public static IParser<T> BuildLr<T>(Grammar<T> grammar)
            where T : Node, new()
        {
            LrState<T>[] states;
            return BuildLr(grammar, out states);
        }

        public static IParser<T> BuildLr<T>(Grammar<T> grammar, out LrState<T>[] states)
            where T : Node, new()
        {
            var builder = new LrParserBuilder<T>(grammar);
            return new ShiftReduceParser<T>(builder.Build(out states), grammar);
        }

        public static T ParseString<T>(this IParser<T> parser, string str)
            where T : Node, new()
        {
            return parser.Parse(new TokenStream<T>(new StringReader(str), parser.Grammar));
        }

        public static T Parse<T>(this IParser<T> parser, string path)
            where T : Node, new()
        {
            return parser.Parse(new TokenStream<T>(path, parser.Grammar));
        }

        public static T Parse<T>(this IParser<T> parser, Stream stream)
            where T : Node, new()
        {
            return parser.Parse(new TokenStream<T>(stream, parser.Grammar));
        }
    }
}
