using System;
using System.Collections.Generic;

using SciSharp.Probabilities;


namespace SciSharp
{
    public static class Vectors
    {
        #region Methods

        public static IEnumerable<Vector> CanonicalBase(int dimension)
        {
            for (int i = 0; i < dimension; i++)
                yield return Canonical(dimension, i);
        }

        public static Vector Zeros(int dimension)
        {
            return new Vector(dimension);
        }

        public static Vector Ones(int dimension)
        {
            var elements = new double[dimension];

            for (int i = 0; i < elements.Length; i++)
                elements[i] = 1;

            return new Vector(elements);
        }

        public static Vector Canonical(int dimension, int index)
        {
            var elements = new double[dimension];

            elements[index] = 1;

            return new Vector(elements);
        }

        public static Vector Random(int dimension)
        {
            return Random(dimension, 15);
        }

        public static Vector Random(int dimension, int digits)
        {
            return Random(dimension, digits, Engine.R);
        }

        public static Vector Random(int dimension, Random random)
        {
            return Random(dimension, 15, random);
        }

        public static Vector Random(int dimension, int digits, Random random)
        {
            var elements = new double[dimension];

            for (int i = 0; i < dimension; i++)
                elements[i] = Math.Round(random.NextDouble(), digits);

            return new Vector(elements);
        }

        public static Vector RandomBox(int dimension, double min, double max)
        {
            return RandomBox(dimension, min, max, Engine.R);
        }

        public static Vector RandomBox(int dimension)
        {
            return RandomBox(dimension, Engine.R);
        }

        public static Vector RandomBox(int dimension, RandomEx random)
        {
            return RandomBox(dimension, -1d, 1d, random);
        }

        public static Vector RandomBox(int dimension, double min, double max, RandomEx random)
        {
            var v = new Vector(dimension);

            for (int i = 0; i < dimension; i++)
                v[i] = random.Uniform(min, max);

            return v;
        }

        public static Vector RandomBoxNormal(int dimension, double min, double max, RandomEx random, double mu = 0, double sigma = 1)
        {
            var v = new Vector(dimension);

            for (int i = 0; i < dimension; i++)
            {
                v[i] = random.Normal(mu, sigma);
                if (v[i] > max) v[i] = max;
                if (v[i] < min) v[i] = min;
            }

            return v;
        }

        public static Vector RandomSphere(int dimension)
        {
            return RandomSphere(dimension, Engine.R);
        }

        public static Vector RandomSphere(int dimension, RandomEx random)
        {
            var v = new Vector(dimension);

            for (int i = 0; i < dimension; i++)
                v[i] = random.Normal();

            return v.Normalize();
        }

        public static Vector RandomSphere(int dimension, double radius)
        {
            return RandomSphere(dimension, radius, Engine.R);
        }

        public static Vector RandomSphere(int dimension, double radius, RandomEx random)
        {
            return RandomSphere(dimension, random).Multiply(radius);
        }

        public static Vector RandomBall(int dimension)
        {
            return RandomBall(dimension, Engine.R);
        }

        public static Vector RandomBall(int dimension, RandomEx random)
        {
            Vector v = RandomSphere(dimension, random);
            double u = random.NextDouble();

            return v.Multiply(Math.Pow(u, 1d/dimension));
        }

        public static Vector RandomBall(int dimension, double radius, RandomEx random)
        {
            return RandomBall(dimension, random).Multiply(radius);
        }

        public static Vector RandomRing(int dimension, double min, double max)
        {
            return RandomRing(dimension, min, max, Engine.R);
        }

        public static Vector RandomRing(int dimension, double min, double max, RandomEx random)
        {
            Vector v = RandomSphere(dimension);

            min = Math.Pow(min, dimension);
            max = Math.Pow(max, dimension);

            double u = random.Uniform(min, max);

            return v.Multiply(Math.Pow(u, 1d/dimension));
        }

        public static Vector Staircase(int dimension)
        {
            var x = new Vector(dimension);
            double D = dimension;

            for (int i = 0; i < dimension; i++)
                x[i] = (i%2 == 0 ? i/D : -i/D);

            return x;
        }

        public static Vector RandomDiagonal(int dimension)
        {
            return RandomDiagonal(dimension, Engine.R);
        }

        public static Vector RandomDiagonal(int dimension, Random random)
        {
            var diagonal = new Vector(dimension);

            for (int i = 0; i < dimension; i++)
            {
                if (random.NextDouble() < 0.5d)
                    diagonal[i] = -1;
                else
                    diagonal[i] = 1;
            }

            return diagonal;
        }

        #endregion
    }
}
