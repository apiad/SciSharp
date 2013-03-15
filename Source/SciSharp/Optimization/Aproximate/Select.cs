using System.Collections.Generic;


namespace SciSharp.Optimization.Aproximate
{
    public static class Select
    {
        public static Vector Best(Vector current, IRealFunction function, IEnumerable<Vector> neighboors)
        {
            double value = function.Value(current);

            foreach (Vector vector in neighboors)
            {
                double val = function.Value(vector);

                if (val > value)
                {
                    current = vector;
                    value = val;
                }
            }

            return current;
        }

        public static Vector FirstBetter(Vector current, IRealFunction function, IEnumerable<Vector> neighboors)
        {
            double value = function.Value(current);

            foreach (Vector vector in neighboors)
            {
                double val = function.Value(vector);

                if (val > value)
                    return vector;
            }

            return current;
        }
    }
}
