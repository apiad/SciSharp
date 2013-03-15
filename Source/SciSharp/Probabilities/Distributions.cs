namespace SciSharp.Probabilities
{
    public static class Distributions
    {
        public static Distribution Bernoulli(double p = 0.5d)
        {
            var random = new RandomEx();
            return () => random.Bernoulli(p) ? 1d : 0d;
        }

        public static Distribution Uniform(double a = 0d, double b = 1d)
        {
            var random = new RandomEx();
            return () => random.Uniform(a, b);
        }

        public static Distribution Uniform(int a = 0, int b = 1)
        {
            var random = new RandomEx();
            return () => random.Uniform(a, b);
        }

        public static Distribution Normal(double mu = 0d, double sigma = 1d)
        {
            var random = new RandomEx();
            return () => random.Normal(mu, sigma);
        }

        public static Distribution Exponential(double lambda = 1d)
        {
            var random = new RandomEx();
            return () => random.Exponential(lambda);
        }

        public static Distribution Poisson(int lambda = 1)
        {
            var random = new RandomEx();
            return () => random.Poisson(lambda);
        }
    }
}
