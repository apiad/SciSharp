namespace SciSharp.Numerics
{
    public static class Integrators
    {
        public static Integrator SingleStep(SingleStepMethod method)
        {
            return (f, x, y, s) => method(f, x, y, s);
        }

        public static Integrator MultiStep(SingleStepMethod starter, MultiStepMethod method, int iterations)
        {
            var values = new Vector[iterations];

            return (function, x, y, step) =>
                   {
                       if (iterations > 0)
                       {
                           y = starter(function, x, y, step);
                           values[iterations - 1] = y;
                           iterations--;

                           return y;
                       }

                       Vector result = method(function, x, values, step);

                       for (int j = values.Length - 1; j >= 1; j--)
                           values[j] = values[j - 1];

                       values[0] = result;

                       return result;
                   };
        }

        public static Integrator PredictorCorrector(SingleStepMethod predictor, SingleStepCorrector corrector,
                                                    int iterations)
        {
            return (function, x, y, step) =>
                   {
                       Vector yIter = predictor(function, x, y, step);

                       for (int j = 0; j < iterations; j++)
                       {
                           Vector result = corrector(function, x, y, yIter, step);
                           yIter = result;

                           if ((result - yIter).Length < Engine.Epsilon)
                               break;
                       }

                       return yIter;
                   };
        }
    }
}
