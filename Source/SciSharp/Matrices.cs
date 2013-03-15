using System;
using System.Collections.Generic;
using System.Linq;


namespace SciSharp
{
    public static class Matrices
    {
        #region Methods

        public static Matrix Basis(int dimension, params Vector[] basis)
        {
            if (basis == null)
                throw new ArgumentNullException("basis");

            if (basis.Length != dimension)
                throw new ArgumentException("The number of vectors don't match the dimension.");

            var elements = new double[dimension,dimension];

            for (int i = 0; i < dimension; i++)
            {
                if (basis[i].Dimension != dimension)
                    throw new ArgumentException("The " + i + "th vector doesn't match the dimension.");

                for (int j = 0; j < dimension; j++)
                    elements[i, j] = basis[i][j];
            }

            return new Matrix(elements);
        }

        public static Matrix Basis(IEnumerable<Vector> basis)
        {
            if (basis == null)
                throw new ArgumentNullException("basis");

            Vector[] vectors = basis.ToArray();

            return Basis(vectors.Length, vectors);
        }

        public static Matrix Zeros(int rows, int columns)
        {
            return new Matrix(rows, columns);
        }

        public static Matrix Ones(int rows, int columns)
        {
            var elements = new double[rows,columns];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    elements[i, j] = 1;

            return new Matrix(elements);
        }

        public static Matrix Identity(int order)
        {
            var elements = new double[order,order];

            for (int k = 0; k < order; k++)
                elements[k, k] = 1;

            return new Matrix(elements);
        }

        public static Matrix Random(int rows, int columns)
        {
            return Random(rows, columns, 15);
        }

        public static Matrix Random(int rows, int columns, int digits)
        {
            return Random(rows, columns, digits, Engine.R);
        }

        public static Matrix Random(int rows, int columns, Random random)
        {
            return Random(rows, columns, 15, random);
        }

        public static Matrix Random(int rows, int columns, int digits, Random random)
        {
            var values = new double[rows,columns];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    values[i, j] = Math.Round(random.NextDouble(), digits);

            return new Matrix(values);
        }

        #endregion
    }
}
