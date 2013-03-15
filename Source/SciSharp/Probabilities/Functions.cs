using System;

using SciSharp.Collections;


namespace SciSharp.Probabilities
{
    public static class Functions
    {
        /// <summary>
        /// Calculates the binomial probability p(k) equal to the
        /// probability of obtaining k successes out of n trials,
        /// where the probability of success is p.
        /// </summary>
        /// <param name="n">The number of trials.</param>
        /// <param name="k">The number of successes.</param>
        /// <param name="p">The probability that a single trial is a success.</param>
        /// <returns>The probaility of obtaining exactly k successes.</returns>
        public static double Binomial(int n, int k, double p)
        {
            return Combinatorics.Combinations(n, k)*Math.Pow(1 - p, n - k)*Math.Pow(p, k);
        }

        /// <summary>
        /// Calculates the geometric probability p(n) equal to the
        /// probability of getting the first success exactly at trial n,
        /// where the probability of success is p.
        /// </summary>
        /// <param name="n">The number of the trial of the first success.</param>
        /// <param name="p">The probability that a single trial is a success.</param>
        /// <returns>The probability of a first success exactly at trial n.</returns>
        public static double Geometric(int n, double p)
        {
            return Math.Pow(1 - p, n - 1)*p;
        }

        /// <summary>
        /// Calculates the poisson probability p(n) of
        /// a random variable with parameter p.
        /// </summary>
        /// <param name="n">The value of the argument.</param>
        /// <param name="p">The parameter of the poisson distribution.</param>
        /// <returns>The probability of p(n).</returns>
        public static double Poisson(int n, double p)
        {
            return Math.Exp(-p)*Math.Pow(p, n)/Combinatorics.Factorial(n);
        }
    }
}
