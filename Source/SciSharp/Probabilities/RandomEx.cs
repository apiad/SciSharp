using System;
using System.Collections.Generic;

using SciSharp.Collections;


namespace SciSharp.Probabilities
{
    [Serializable]
    public class RandomEx : Random
    {
        protected const double TwoPi = 2 * Math.PI;
        protected const int BitsPerFloat = 53;

        protected static readonly double SgMagicConst = 1d + Math.Log(4.5d);
        protected static readonly double NvMagicConst = 4 * Math.Exp(-0.5d) / Math.Sqrt(2d);
        protected static readonly double Log4 = Math.Log(4d);
        protected static readonly double RecipBpf = Math.Pow(2, -BitsPerFloat);

        public static readonly RandomEx Instance = new RandomEx();

        public RandomEx(int seed)
            : base(seed) { }

        public RandomEx() { }

        public double Uniform(double a, double b)
        {
            return (a + (b - a) * Sample());
        }

        public bool Bernoulli(double p)
        {
            return Sample() < p;
        }

        public bool Flip()
        {
            return Bernoulli(0.5d);
        }

        public T[] Sample<T>(T[] items, int count)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            if (count < 0 || count > items.Length)
                throw new ArgumentOutOfRangeException("count", "Must be greater or equal to zero and less or equal to items.Length.");

            return (count < items.Length / 2) ? SampleTrue(items, count) : SampleFalse(items, count);
        }

        public T[] Sample<T>(IList<T> items, int count)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            if (count < 0 || count > items.Count)
                throw new ArgumentOutOfRangeException("count", "Must be greater or equal to zero and less or equal to items.Length.");

            return (count < items.Count / 2) ? SampleTrue(items, count) : SampleFalse(items, count);
        }

        public T Select<T>(T[] items)
        {
            return items[Next(items.Length)];
        }

        public T Select<T>(IList<T> items)
        {
            return items[Next(items.Count)];
        }

        private T[] SampleTrue<T>(IList<T> items, int count)
        {
            ICollection<int> selected = Select(items.Count, count);
            var result = new T[count];
            int pos = 0;

            foreach (int index in selected)
                result[pos++] = items[index];

            return result;
        }

        private ICollection<int> Select(int length, int count)
        {
            ICollection<int> selected = count < Math.Sqrt(length)
                                            ? new HashSet<int>()
                                            : (ICollection<int>)new RangeSet(length);

            while (count-- > 0)
            {
                int index = Next(length);

                while (selected.Contains(index))
                    index = Next(length);

                selected.Add(index);
            }

            return selected;
        }

        private T[] SampleFalse<T>(IList<T> items, int count)
        {
            ICollection<int> skipped = Select(items.Count, items.Count - count);
            var result = new T[count];
            int pos = 0;

            for (int i = 0; i < items.Count; i++)
                if (!skipped.Contains(i))
                    result[pos++] = items[i];

            return result;
        }

        public int Uniform(int a, int b)
        {
            return Next(a, b + 1);
        }

        public void Shuffle<T>(T[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            for (int i = 0; i < items.Length; i++)
                items.Swap(i, Uniform(i, items.Length - 1));
        }

        public double Triangular(double low = 0d, double high = 0d, double mode = 0.5d)
        {
            double u = Sample();

            if (u > mode)
            {
                u = 1d - u;
                mode = 1d - mode;

                double temp = low;
                low = high;
                high = temp;
            }

            return low + (high - low) * Math.Sqrt(u * mode);
        }

        public double Normal(double mu = 0d, double sigma = 1d)
        {
            double z;

            while (true)
            {
                double u1 = Sample();
                double u2 = 1d - Sample();

                z = NvMagicConst * (u1 - 0.5d) / u2;
                double zz = z / 4d;

                if (zz <= -Math.Log(u2))
                    break;
            }

            return mu + z * sigma;
        }

        public double LogNormal(double mu = 0d, double sigma = 1d)
        {
            return Math.Exp(Normal(mu, sigma));
        }

        public double Exponential(double lambda = 1d)
        {
            double u = Sample();

            while (u <= 1e-7)
                u = Sample();

            return -Math.Log(u) / lambda;
        }

        public int Poisson(int lambda = 1)
        {
            double param = 1d / lambda;

            double current = 0;
            int count = 0;

            while (current < lambda)
            {
                current += Exponential(param);
                count++;
            }

            return count - 1;
        }

        public double VonMisses(double mu = 0d, double kappa = 0d)
        {
            if (kappa <= 1e-6)
                return TwoPi * Sample();

            double a = 1d + Math.Sqrt(1d + 4d * kappa * kappa);
            double b = (a - Math.Sqrt(2d * a)) / (2d * kappa);
            double r = (1d + b * b) / (2d * b);

            double f;

            while (true)
            {
                double u1 = Sample();

                double z = Math.Cos(Math.PI * u1);
                f = (1.0 + r * z) / (r + z);
                double c = kappa * (r - f);

                double u2 = Sample();

                if (u2 < c * (2.0 - c) || u2 <= c * Math.Exp(1.0 - c))
                    break;
            }

            double u3 = Sample();
            double theta = u3 > 0.5d ? (mu % TwoPi) + Math.Acos(f) : (mu % TwoPi) - Math.Acos(f);
            return theta;
        }

        public T Roulette<T>(T[] items, double[] values)
        {
            double total = 0d;
            int l = values.Length;

            for (int i = 0; i < l; i++)
                total += values[i];

            for (int i = 0; i < l; i++)
                values[i] /= total;

            double u = Sample();
            int current = 0;
            double acum = 0d;

            while (acum < u && current < values.Length)
                acum += values[current++];

            return items[current - 1];
        }
    }
}
