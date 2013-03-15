using System.Collections.Generic;

using SciSharp.Collections;


namespace SciSharp.NumberTheory
{
    public static partial class Primes
    {
        /// <summary>
        /// Gets all the (6542) 16-bit prime numbers (smaller than 65536).
        /// </summary>
        public static ArraySlice<int> List
        {
            get { return new ArraySlice<int>(Primes16); }
        }

        /// <summary>
        /// Gets a range of the first <paramref name="length"/> prime numbers.
        /// </summary>
        /// <param name="length">The count of prime numbers to get. Must be smaller than or equal to 6542.</param>
        /// <returns></returns>
        public static ArraySlice<int> Range(int length)
        {
            return Range(0, length);
        }

        /// <summary>
        /// Gets a range of prime numbers.
        /// </summary>
        /// <param name="start">The index of the first prime number to get. Must be smaller than 6542.</param>
        /// <param name="length">The count of of numbers to get. Must be smaller than 6542 - <paramref name="start"/>.</param>
        /// <returns></returns>
        public static ArraySlice<int> Range(int start, int length)
        {
            return new ArraySlice<int>(Primes16, start, length);
        }

        public static bool[] Sieve(int length)
        {
            int count;
            return Sieve(length, out count);
        }

        public static bool[] Sieve(int length, out int count)
        {
            count = 0;

            var result = new bool[length];

            for (int i = 2; i < result.Length; i++)
                if (!result[i])
                {
                    count++;

                    for (int j = 2*i; j < result.Length; j += i)
                        result[j] = true;
                }

            return result;
        }

        /// <summary>
        /// Checks if the specified integer number is prime.
        /// </summary>
        /// <param name="n">The number to check for primality.</param>
        /// <returns>True if the number is prime, false otherwise.</returns>
        /// <remarks>This methods proves the primality by brute force
        /// checking among all the (at most 6552) primes smaller than the square
        /// root of the given number. However, since this primes are precalculated,
        /// this method is the fastest of the brute force aproaches.</remarks>
        public static bool Check(int n)
        {
            if (n < 2)
                return false;

            for (int i = 0; i < Primes16.Length; i++)
            {
                int p = Primes16[i];

                if (p*p > n)
                    break;

                if (n%p == 0)
                    return false;
            }

            return true;
        }

        public static bool RabinMiller(long p, IEnumerable<int> candidates)
        {
            if (p < 2 || p > 2 && p%2 == 0)
                return false;

            foreach (int n in candidates)
                if (n != p && ModularMath.Pow(n, p - 1, p) != 1)
                    return false;

            return true;
        }
    }
}
