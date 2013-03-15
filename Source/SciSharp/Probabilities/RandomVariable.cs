using System;


namespace SciSharp.Probabilities
{
    public class RandomVariable
    {
        private readonly Distribution distribution;

        public RandomVariable(Distribution distribution)
        {
            if (distribution == null)
                throw new ArgumentNullException("distribution");

            this.distribution = distribution;
        }

        public double Next()
        {
            return distribution();
        }

        public static RandomVariable Uniform(double a = 0d, double b = 1d)
        {
            return new RandomVariable(Distributions.Uniform(a, b));
        }

        public static RandomVariable Uniform(int a, int b)
        {
            return new RandomVariable(Distributions.Uniform(a, b));
        }

        public static RandomVariable Exponential(double lambda = 1d)
        {
            return new RandomVariable(Distributions.Exponential(lambda));
        }

        public static RandomVariable Poisson(int lambda = 1)
        {
            return new RandomVariable(Distributions.Poisson(lambda));
        }

        public static RandomVariable Bernoulli(double p = 0.5d)
        {
            return new RandomVariable(Distributions.Bernoulli(p));
        }

        public static implicit operator double(RandomVariable var)
        {
            return var.Next();
        }

        public static implicit operator bool(RandomVariable var)
        {
            return var.Next() > 0;
        }

        public static implicit operator int(RandomVariable var)
        {
            return (int) var.Next();
        }

        public static RandomVariable operator +(RandomVariable left, double right)
        {
            return new RandomVariable(() => left.distribution() + right);
        }

        public static RandomVariable operator +(double left, RandomVariable right)
        {
            return right + left;
        }

        public static RandomVariable operator +(RandomVariable left, RandomVariable right)
        {
            return new RandomVariable(() => left.distribution() + right.distribution());
        }

        public static RandomVariable operator -(RandomVariable left, double right)
        {
            return new RandomVariable(() => left.distribution() - right);
        }

        public static RandomVariable operator -(double left, RandomVariable right)
        {
            return new RandomVariable(() => left - right.distribution());
        }

        public static RandomVariable operator -(RandomVariable left, RandomVariable right)
        {
            return new RandomVariable(() => left.distribution() - right.distribution());
        }

        public static RandomVariable operator *(RandomVariable left, double right)
        {
            return new RandomVariable(() => left.distribution()*right);
        }

        public static RandomVariable operator *(double left, RandomVariable right)
        {
            return right*left;
        }

        public static RandomVariable operator *(RandomVariable left, RandomVariable right)
        {
            return new RandomVariable(() => left.distribution()*right.distribution());
        }

        public static RandomVariable operator /(RandomVariable left, double right)
        {
            return new RandomVariable(() => left.distribution()/right);
        }

        public static RandomVariable operator /(double left, RandomVariable right)
        {
            return new RandomVariable(() => left/right.distribution());
        }

        public static RandomVariable operator /(RandomVariable left, RandomVariable right)
        {
            return new RandomVariable(() => left.distribution()/right.distribution());
        }
    }
}
