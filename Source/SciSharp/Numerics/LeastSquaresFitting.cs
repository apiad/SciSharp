using System;
using System.Collections.Generic;
using System.Linq;


namespace SciSharp.Numerics
{
    public class LeastSquaresFitting<T>
    {
        private readonly List<KeyValuePair<T, double>> samples;

        public LeastSquaresFitting()
        {
            samples = new List<KeyValuePair<T, double>>();
        }

        public void Add(T x, double y)
        {
            samples.Add(new KeyValuePair<T, double>(x, y));
        }

        public LinearCombinationFunction<T> Fit(out double error, params IFunction<T, double>[] bases)
        {
            LinearCombinationFunction<T> func = Fit(bases);

            error = samples.Sum(pair => Math.Pow(func.Value(pair.Key) - pair.Value, 2))/samples.Count;
            error = Math.Sqrt(error);

            return func;
        }

        public LinearCombinationFunction<T> Fit(params IFunction<T, double>[] bases)
        {
            if (bases == null)
                throw new ArgumentNullException("bases");

            var matrix = new Matrix(bases.Length, bases.Length);
            var vector = new Vector(bases.Length);

            for (int i = 0; i < matrix.Rows; i++)
                for (int j = i; j < matrix.Columns; j++)
                    matrix[i, j] = matrix[j, i] = samples.Sum(pair => bases[i].Value(pair.Key)*bases[j].Value(pair.Key));

            for (int i = 0; i < vector.Dimension; i++)
                vector[i] = samples.Sum(pair => bases[i].Value(pair.Key)*pair.Value);

            Vector coefs = LinearSystems.PluSolve(matrix, vector, LinearSystems.ScaledPivot(matrix)).Solution;

            return new LinearCombinationFunction<T>(coefs, bases);
        }
    }
}
