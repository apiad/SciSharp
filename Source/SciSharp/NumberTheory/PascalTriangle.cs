using System;


namespace SciSharp.NumberTheory
{
    public class PascalTriangle
    {
        private readonly int[,] coefficients;
        private int top;

        public PascalTriangle(int size)
        {
            coefficients = new int[size,size];
            coefficients[0, 0] = 1;
        }

        public int this[int n, int k]
        {
            get
            {
                if (k > n)
                    throw new ArgumentOutOfRangeException("k", "Must be less or equal than n.");

                if (top < n)
                {
                    for (int i = top + 1; i <= n; i++)
                        coefficients[i, 0] = coefficients[i, i] = 1;

                    for (int i = top + 1; i <= n; i++)
                        for (int j = 1; j < n; j++)
                            coefficients[i, j] = coefficients[i - 1, j - 1] + coefficients[i - 1, j];

                    top = n;
                }

                return coefficients[n, k];
            }
        }
    }
}
