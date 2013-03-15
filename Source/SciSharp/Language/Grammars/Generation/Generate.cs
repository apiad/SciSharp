using System.Collections.Generic;


namespace SciSharp.Language.Grammars.Generation
{
    public static class Generate
    {
        public static IEnumerable<T> DepthFirst<T>(Grammar<T> G)
            where T : Node, new()
        {
            var generator = new RecursiveGenerator<T>(G, Selectors.First);
            return generator.Generate();
        }
    }
}
