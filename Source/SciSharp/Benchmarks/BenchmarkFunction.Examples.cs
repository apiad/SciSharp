//using System;
//using ScientificTools.Algebra;

//namespace ScientificTools.Benchmarks
//{
//    public partial class BenchmarkFunction
//    {
//        public static BenchmarkFunction ExtendedPenalty(int n)
//        {
//            Variable[] v = Variable.List(n);

//            Func f = Func.Sum(1, n - 1, (i, xi) => Func.Sqr(xi - 1), v) +
//                     Func.Sqr(Func.Sum((i, xi) => Func.Sqr(xi)) - 0.25d);

//            Vector start = Vectors.Ones(n);
//            start[-1] = 0.5d;

//            return new BenchmarkFunction("Extended Penalty", new FunctionWrapper(n, f, v), start, Vectors.Ones(n)*0.5d);
//        }

//        public static BenchmarkFunction RandomQuadratic(int n)
//        {
//            return RandomQuadratic(n, Environment.TickCount);
//        }

//        public static BenchmarkFunction RandomQuadratic(int n, int randomSeed)
//        {
//            Variable[] v = Variable.List(n);
//            var r = new Random(randomSeed);

//            var coef = new double[n];

//            for (int i = 0; i < n; i++)
//                coef[i] = r.NextDouble() - 0.5d;

//            Func f = Func.Sum((i, xi) => coef[i - 1]*xi*xi + 0.1d*Func.Pow(xi, 4), v);

//            return new BenchmarkFunction("Random Quadratic", new FunctionWrapper(n, f, v), 0.5d*Vectors.Zeros(n));
//        }

//        public static BenchmarkFunction NegativeQuadratic(int n)
//        {
//            Variable[] v = Variable.List(n);

//            Func f = Func.Sum((i, xi) => - i*Func.Sqr(xi) + 0.1d*Func.Pow(xi, 4), v);

//            return new BenchmarkFunction("Negative Quadratic", new FunctionWrapper(n, f, v), Vectors.Ones(n));
//        }

//        public static BenchmarkFunction Cubic(int n)
//        {
//            var v0 = new Variable("Y");
//            Variable[] vs = Variable.List(n - 1);

//            var v = new Variable[n];
//            v[0] = v0;
//            Array.Copy(vs, 0, v, 1, vs.Length);

//            Func f = Func.Pow(v0, 3) + Func.Sum(2, n, (i, xi) => Func.Sqr(xi), vs);

//            return new BenchmarkFunction("Cubic", new FunctionWrapper(n, f, v), Vectors.Ones(n));
//        }

//        public static BenchmarkFunction RandomNegativeQuadratic(int n)
//        {
//            return RandomNegativeQuadratic(n, Environment.TickCount);
//        }

//        public static BenchmarkFunction RandomNegativeQuadratic(int n, int randomSeed)
//        {
//            Variable[] v = Variable.List(n);
//            var r = new Random(randomSeed);

//            var coef = new double[n];

//            for (int i = 0; i < n; i++)
//                coef[i] = r.NextDouble() - 1d;

//            Func f = Func.Sum((i, xi) => coef[i - 1]*xi*xi + 0.001d*Func.Pow(xi, 4), v);

//            return new BenchmarkFunction("Random Negative Quadratic", new FunctionWrapper(n, f, v), Vectors.Zeros(n));
//        }

//        public static BenchmarkFunction PerturbedQuadratic(int n)
//        {
//            Variable[] v = Variable.List(n);

//            Func f = Func.Sum((i, xi) => i*Func.Sqr(xi), v) +
//                     1d/100*Func.Sqr(Func.Sum((i, xi) => xi, v));

//            return new BenchmarkFunction("Pertubed Quadratic", new FunctionWrapper(n, f, v), Vectors.Zeros(n),
//                                         Vectors.Ones(n)*0.5d);
//        }

//        public static BenchmarkFunction Raydan1(int n)
//        {
//            Variable[] v = Variable.List(n);

//            Func f = Func.Sum((i, xi) => i/10d*(Func.Exp(xi) - xi), v);

//            return new BenchmarkFunction("Raydan 1", new FunctionWrapper(n, f, v), Vectors.Zeros(n), Vectors.Ones(n));
//        }

//        public static BenchmarkFunction Raydan2(int n)
//        {
//            Variable[] v = Variable.List(n);

//            Func f = Func.Sum((i, xi) => Func.Exp(xi) - xi, v);

//            return new BenchmarkFunction("Raydan 2", new FunctionWrapper(n, f, v), Vectors.Zeros(n), Vectors.Ones(n));
//        }

//        public static BenchmarkFunction ExtendedTrigonometric(int n)
//        {
//            Variable[] v = Variable.List(n);

//            Func f =
//                Func.Sum(
//                    (i, xi) => Func.Sqr(n - Func.Sum(xj => Func.Cos(xj), v) + i*(1 - Func.Cos(xi)) - Func.Sin(xi)), v);

//            return new BenchmarkFunction("Extended Trigonometric", new FunctionWrapper(n, f, v), 0.2*Vectors.Ones(n));
//        }

//        public static BenchmarkFunction ExtendedRosenbrock(int n)
//        {
//            if (n%2 != 0)
//                throw new ArgumentException("Dimension must be even.");

//            Variable[] v = Variable.List(n);

//            Func f = Func.Sum(1, n/2, i => 100*Func.Sqr(v[2*i - 1] - Func.Sqr(v[2*i - 2])) + Func.Sqr(1 - v[2*i - 2]));

//            Vector start = Vectors.Ones(n);
//            start[0, 2, -1] = Vectors.Ones(n/2)*-1.2;

//            return new BenchmarkFunction("Extended Rosenbrock", new FunctionWrapper(n, f, v), Vectors.Ones(n), start);
//        }
//    }
//}


