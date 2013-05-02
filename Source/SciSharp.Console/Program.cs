using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using SciSharp.Benchmarks.Combinatorial;
using SciSharp.Language;
using SciSharp.Language.Grammars;
using SciSharp.Learning.Classification;
using SciSharp.Probabilities;
using SciSharp.Sorting;


namespace SciSharp.Console
{
    static class Program
    {
        public static void Main(string[] args)
        {
            Repl();
        }

        private static void GenerateTsp(int size, int iterations, bool simetric)
        {
            var factory = new TspInstanceWalkFactory(size, iterations, 100000);
            var tsp = factory.Create();

            double[,] matrix = tsp.Costs;

            int[,] costs = new int[size, size];

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    costs[i, j] = (int)matrix[i, j];

                    if (costs[i, j] == 0)
                        costs[i, j] = RandomEx.Instance.Uniform(1, 100);
                }

            if (simetric)
                for (int i = 0; i < size; i++)
                    for (int j = 0; j <= i; j++)
                        costs[i, j] = costs[j, i];

            var randomVariable = new RandomVariable(
                () =>
                {
                    int[] cities = Enumerable.Range(0, tsp.NumberCities).ToArray();
                    RandomEx.Instance.Shuffle(cities);

                    double cost = 0d;

                    for (int i = 1; i < cities.Length; i++)
                        cost += costs[cities[i - 1], cities[i]];

                    cost += costs[cities[tsp.NumberCities - 1], cities[0]];

                    return cost;
                });

            var histogram = new Histogram(0, 600000, 30);
            histogram.AllowOutliers = true;

            histogram.Add(randomVariable, 10000);

            System.Console.WriteLine(Estimators.Mean(randomVariable, 10000));
            System.Console.WriteLine(Estimators.StdDev(randomVariable, 10000));
            System.Console.WriteLine();
            System.Console.WriteLine(histogram);

            var file = new StreamWriter("TSP Costs ({0} cities).txt".Formatted(size));

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                    file.Write(costs[i, j] + " ");

                file.WriteLine();
            }

            file.Close();

            int[] ints = Enumerable.Range(0, tsp.NumberCities).ToArray();
            RandomEx.Instance.Shuffle(ints);

            var solution = new StreamWriter("TSP Solution ({0} cities).txt".Formatted(size));

            for (int i = 0; i < size; i++)
                solution.Write(ints[i] + " ");

            solution.WriteLine();
            solution.Close();
        }

        private static void PrintInitialMessage()
        {
            string title = string.Format("SciSharp {0} (running on {1} processors [{2} bits])",
                                         (Assembly.GetAssembly(typeof(Engine)).GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0] as AssemblyFileVersionAttribute).Version,
                                         Environment.ProcessorCount,
                                         Environment.Is64BitProcess ? 64 : 32);

            System.Console.WriteLine(title);
            System.Console.WriteLine("=".Repeated(title.Length));
            System.Console.WriteLine();
        }

        private static void Repl()
        {
            System.Console.WriteLine("Type 'help()' to enter help mode, or 'help(object)' for specific help.");
            System.Console.WriteLine();

            IParser<Expression> parser = BuildParser();
            var context = new Context();
            context.Set("__context__", context);
            context.Set("exit", new Quitter());
            int line = 1;

            while (true)
            {
                try
                {
                    System.Console.Write("[In  {0}] >>> ".Formatted(line));
                    Expression node = parser.ParseString(System.Console.ReadLine());
                    object result = node.Evaluate(context);
                    context.Set("_", result);
                    context.Set("_{0}".Formatted(line), result);
                    System.Console.WriteLine("[Out {0}] : {1}".Formatted(line, result));
                    line++;
                }
                catch (Exception exc)
                {
                    System.Console.WriteLine("\n{0}\n", exc);
                }
            }
        }

        private static IParser<Expression> BuildParser()
        {
            var g = new Grammar<Expression>();
            GrammarBuilder<Expression> _ = g.Builder;

            // Rules
            Def<Expression> expression = g.Start("expression");
            Def<Expression> arithmeticExpression = g.Start("arithmeticExpression");
            Def<Expression> lvalue = g.Rule("lvalue");
            Def<Expression> dottedName = g.Rule("dottedName");

            // Tokens
            Token<Expression> StringConst = g.Token("StringConst", "\"(.*)\"");
            Token<Expression> IntegerConst = g.Token("IntegerConst", "[\\+-]?[1-9][0-9]*");
            Token<Expression> Identifier = g.Token("Identifier", "[a-z,_,A-Z][0,-9,a-z,_,A-Z]*");

            // Productions
            expression %= (lvalue + "=" + expression)
                              .With(() => new Assignment((LValue)lvalue.Node, expression.Node)) |
                //(dottedName + '(' + _[expression + (',' + expression) * _] + ')')
                //    .With(() => null) |
                          arithmeticExpression
                              .With(() => arithmeticExpression.Node) |
                          IntegerConst
                              .With(() => new Constant(int.Parse(IntegerConst.Node.Match))) |
                          StringConst
                              .With(() => new Constant(IntegerConst.Node.Match)) |
                          Identifier
                              .With(() => new Variable(Identifier.Node.Match));

            lvalue %= Identifier.With(() => new Variable(Identifier.Node.Match));

            Logger.Log(g.ToString());

            // Build the parser
            return Parsers.BuildLr(g);
        }
    }
}
