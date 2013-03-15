using System;
using System.Collections.Generic;


namespace SciSharp.Numerics
{
    //TODO : Step Size Control
    // Implementar Step Size Control.
    // IDEA: Usar un delegate para computar el siguiente step.

    //TODO : Chequear si OperateOnNew == true
    // Modificar los métodos en consecuencia.

    //TODO: Chequear si UseParallelOptimizations = true
    // Modificar los métodos en consecuencia.
    // Creo que esto va mejor en Matrix y Vector.

    /// <summary>
    /// Contiene implementaciones para solvers de ecuaciones diferenciales.
    /// </summary>
    public static class DifferentialFunctions
    {
        private static IEnumerable<Vector> SolveInternal(DifferentialFunction function, double x, Vector y, double step,
                                                         SingleStepMethod method)
        {
            Debug.Post("DifferentialEquations.Solve (SingleStep)");
            Debug.Indent();
            Debug.Post("Dimension = {0}.", y.Dimension);
            Debug.Unindent();

            while (true)
            {
                y = method(function, x, y, step);
                x += step;

                yield return y;
            }
        }

        private static IEnumerable<Vector> SolveInternal(DifferentialFunction function, double x, Vector y, double step,
                                                         SingleStepMethod starter, MultiStepMethod method,
                                                         int iterations)
        {
            Debug.Post("DifferentialEquations.Solve (MultiStep)");
            Debug.Indent();
            Debug.Post("Dimension = {0}.", y.Dimension);

            var values = new Vector[iterations];

            for (int i = 0; i < iterations; i++)
            {
                y = starter(function, x, y, step);
                values[iterations - i - 1] = y;
                x += step;

                yield return y;
            }

            while (true)
            {
                Vector result = method(function, x, values, step);

                for (int j = values.Length - 1; j >= 1; j--)
                    values[j] = values[j - 1];

                values[0] = result;
                x += step;

                yield return result;
            }
        }

        private static IEnumerable<Vector> SolveInternal(DifferentialFunction function, double x, Vector y, double step,
                                                         SingleStepMethod predictor, SingleStepCorrector corrector,
                                                         int iterations)
        {
            Debug.Post("DifferentialEquations.Solve (Predictor-Corrector)");
            Debug.Indent();
            Debug.Post("Dimension = {0}.", y.Dimension);
            Debug.Unindent();

            while (true)
            {
                Vector yIter = predictor(function, x, y, step);

                for (int j = 0; j < iterations; j++)
                {
                    Vector result = corrector(function, x, y, yIter, step);
                    yIter = result;

                    if ((result - yIter).Length < Engine.Epsilon)
                        break;
                }

                yield return yIter;

                y = yIter;
                x += step;
            }
        }

        private static IEnumerable<Vector> SolveInternal(DifferentialFunction function, double x, Vector y, double step,
                                                         SingleStepMethod starter, MultiStepMethod predictor,
                                                         MultiStepMethod corrector,
                                                         int starterIterations, int iterations)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Vector> SolveInternal(Integrator integrator, DifferentialFunction function, double x0,
                                                         Vector y0, double step)
        {
            while (true)
            {
                yield return y0;

                y0 = integrator(function, x0, y0, step);
                x0 += step;
            }
        }

        public static DifferentialSolution Solve(this DifferentialFunction function, double x, Vector y, double step,
                                                 SingleStepMethod method)
        {
            return new DifferentialSolution(SolveInternal(function, x, y, step, method));
        }

        public static DifferentialSolution Solve(this DifferentialFunction function, double x, Vector y, double step,
                                                 SingleStepMethod starter, MultiStepMethod method, int iterations)
        {
            return new DifferentialSolution(SolveInternal(function, x, y, step, starter, method, iterations));
        }

        public static DifferentialSolution Solve(this DifferentialFunction function, double x, Vector y, double step,
                                                 SingleStepMethod predictor, SingleStepCorrector corrector,
                                                 int iterations)
        {
            return new DifferentialSolution(SolveInternal(function, x, y, step, predictor, corrector, iterations));
        }

        public static DifferentialSolution Solve(this DifferentialFunction function, double x, Vector y, double step,
                                                 SingleStepMethod starter, MultiStepMethod predictor,
                                                 MultiStepMethod corrector,
                                                 int starterIterations, int iterations)
        {
            return
                new DifferentialSolution(SolveInternal(function, x, y, step, starter, predictor, corrector,
                                                       starterIterations, iterations));
        }

        public static DifferentialSolution Solve(this Integrator integrator, DifferentialFunction function, double x,
                                                 Vector y, double step)
        {
            return new DifferentialSolution(SolveInternal(integrator, function, x, y, step));
        }
    }
}
