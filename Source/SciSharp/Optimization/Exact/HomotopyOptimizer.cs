using System;

using SciSharp.Numerics;


namespace SciSharp.Optimization.Exact
{
    public class HomotopyOptimizer : IExactOptimizer
    {
        private readonly Func<Integrator> integratorBuilder;
        private int paths;
        private double step;

        public HomotopyOptimizer(Func<Integrator> integratorBuilder, double step)
        {
            this.integratorBuilder = integratorBuilder;
            this.step = step;

            MaxPaths = 1;
            MaxValue = 1;
            MaxIterations = 1000;
        }

        public HomotopyOptimizer(Integrator integrator, double step)
            : this(() => integrator, step) {}

        public int MaxPaths { get; set; }

        public double MinValue { get; set; }

        public double MaxValue { get; set; }

        public int MaxIterations { get; set; }

        #region IExactOptimizer Members

        public OptimizerResult Run(IAnaliticFunction function, Vector startingPoint)
        {
            double t = 0;
            Vector x0 = startingPoint;
            Matrix I = Matrices.Identity(function.Dimension);

            Wildcard _ = Wildcard.Get;
            int iters = 0;

            Vector x = x0;
            double value = double.NaN;

            Integrator integrator = integratorBuilder();

            Vector eigenvalues = null;

            while (t < MaxValue)
            {
                // Si t ha disminuido demasiado, es un fallo
                if (t < MinValue)
                    return OptimizerResult.InternalError;

                if (iters++ > MaxIterations)
                    return OptimizerResult.MaxIterations;

                // Valores necesarios
                LdlSolution ldlResult = null;
                Matrix m = null;
                Vector y = null;

                bool ldlSuccess = true;
                LdlSolution bad = null;

                DifferentialFunction fDirect = (d, v) =>
                                               {
                                                   // Calcular todos los valores necesarios
                                                   value = function.Value(v);
                                                   Vector gradient = function.Gradient(v);
                                                   Matrix hessian = function.Hessian(v);

                                                   // Resolver el sistema lineal con LDL
                                                   m = hessian*d + (1 - d)*I;
                                                   y = gradient + (x0 - v);
                                                   ldlResult = LinearSystems.LdlSolve(m, -y);

                                                   ldlSuccess &= (ldlResult.Type == LinearSolutionType.Definite);

                                                   if (!ldlSuccess && bad == null)
                                                       bad = ldlResult;

                                                   return ldlResult.Solution;
                                               };

                Vector nextX = integrator(fDirect, t, x, step);
                double nextT = t + step;

                int column = -1;

                if (ldlSuccess)
                {
                    if (eigenvalues == null)
                    {
                        eigenvalues = Vector.Signs(ldlResult.Eigenvalues);

                        x = nextX;
                        t = nextT;

                        OnStep(new HomotopyEventArgs(iters, function, x, y, t));
                    }
                    else
                    {
                        Vector signs = Vector.Signs(ldlResult.Eigenvalues);
                        Vector diff = Vector.ComponentEquals(eigenvalues, signs);

                        int zeros = Vector.NumberOfZeros(diff);

                        if (zeros == 0)
                        {
                            eigenvalues = signs;

                            x = nextX;
                            t = nextT;

                            OnStep(new HomotopyEventArgs(iters, function, y, x, t));
                        }
                        else if (zeros > 1)
                            return OptimizerResult.InternalError;
                        else
                        {
                            // Buscar el punto de singularidad
                            Vector x1 = x;
                            double t1 = t;

                            Vector x2 = nextX;
                            double t2 = nextT;

                            while (!Engine.Equals(t2, t1) && x1 != x2)
                            {
                                x = (x1 + x2)/2;
                                t = (t1 + t2)/2;

                                // Calcular todos los valores necesarios
                                value = function.Value(x);
                                Vector gradient = function.Gradient(x);
                                Matrix hessian = function.Hessian(x);

                                // Resolver el sistema lineal con LDL
                                m = hessian*t + (1 - t)*I;
                                y = gradient + (x0 - x);
                                ldlResult = LinearSystems.LdlSolve(m, -y);

                                // Si no hay singularidad, buscar el cambio de signo
                                if (ldlResult)
                                {
                                    signs = Vector.Signs(ldlResult.Eigenvalues);
                                    diff = Vector.ComponentEquals(eigenvalues, signs);
                                    zeros = Vector.NumberOfZeros(diff);

                                    // Decidir el intervalo a explorar
                                    if (zeros == 0)
                                    {
                                        x1 = x;
                                        t1 = t;
                                    }
                                    else
                                    {
                                        x2 = x;
                                        t2 = t;
                                    }
                                }
                                else
                                    break;
                            }

                            column = Vector.FirstZero(diff);
                            ldlSuccess = false;
                        }
                    }
                }
                else
                    column = bad.SingularColumn;

                if (!ldlSuccess)
                {
                    Console.WriteLine("Singular point at {0}, {1}", x, t);

                    // Si nos pasamos de la cantidad de caminos, es un fallo
                    if (paths == MaxPaths)
                        return OptimizerResult.MaxIterations;

                    // Hacer el cambio de variables
                    t = x.Swap(column, t);

                    LinearSolution pluResult = null;
                    bool pluSuccess = true;

                    DifferentialFunction fIndirect = (d, v) =>
                                                     {
                                                         d = v.Swap(column, d);

                                                         // Calcular todos los valores necesarios
                                                         value = function.Value(v);
                                                         Vector gradient = function.Gradient(v);
                                                         Matrix hessian = function.Hessian(v);

                                                         // Construir la homotopia
                                                         m = hessian*d + (1 - d)*I;
                                                         y = gradient + (x0 - v);

                                                         // Cambiar la matriz
                                                         Vector c = m[_, column];
                                                         m[_, column] = y;
                                                         y = c;

                                                         // Resolver con PLU, pues perdimos la simetría
                                                         pluResult = LinearSystems.PluSolve(m, -y,
                                                                                            LinearSystems.
                                                                                                ScaledPivot(m));

                                                         pluSuccess &= (pluResult.Type ==
                                                                        LinearSolutionType.Definite);

                                                         // Volver a cambiar
                                                         v.Swap(column, d);

                                                         return pluResult.Solution;
                                                     };

                    // Integrar hasta salir de la singularidad
                    do
                    {
                        pluSuccess = true;

                        if (iters++ > MaxIterations)
                            return OptimizerResult.MaxIterations;

                        // Integrar un paso
                        nextX = integrator(fIndirect, t, x, step);
                        nextT = t + step;

                        // Si hay un error, fallar
                        if (!pluSuccess)
                            return OptimizerResult.InternalError;
                    } while (Engine.IsZero(pluResult.Solution[column]));

                    // Actualizar
                    x = nextX;
                    t = nextT;

                    // Actualizar los valores propios
                    eigenvalues = null;

                    // Cambiar de nuevo
                    t = x.Swap(column, t);

                    OnStep(new HomotopyEventArgs(iters, function, x, y, t));

                    // Invertir el signo del paso
                    step *= -1;

                    if (step > 0)
                        paths++;
                }
            }

            Debug.ThrowOnNaN(value);

            return new OptimizerResult(x, value);
        }

        #endregion

        public override string ToString()
        {
            return "Homotopy Optimizer ({0})".Formatted(integratorBuilder().Method.Name);
        }

        public event EventHandler<HomotopyEventArgs> Step;

        protected virtual void OnStep(HomotopyEventArgs e)
        {
            EventHandler<HomotopyEventArgs> handler = Step;
            if (handler != null)
                handler(this, e);
        }
    }
}
