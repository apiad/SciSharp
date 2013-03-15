using System.Collections.Generic;


namespace SciSharp.Language.Grammars.Generation
{
    public delegate T Selector<T>(IEnumerable<T> set);
}
