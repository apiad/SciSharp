using System.Collections.Generic;


namespace SciSharp.Optimization.Aproximate
{
    public delegate Vector LocalSelector(Vector current, IRealFunction function, IEnumerable<Vector> neighboors);
}
