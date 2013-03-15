namespace SciSharp.NumberTheory
{
    public static class ModularMath
    {
        public static long Multiply(long a, long b, long mod)
        {
            if (b == 0)
                return 0;

            long result = 2*Multiply(a, b/2, mod)%mod;

            if (b%2 == 1)
                result = (result + a)%mod;

            return result;
        }

        public static long Pow(long x, long n, long mod)
        {
            long result = 1;

            for (; n > 0; x = Multiply(x, x, mod), n >>= 1)
                if (n%2 == 1)
                    result = Multiply(result, x, mod);

            return result;
        }
    }
}
