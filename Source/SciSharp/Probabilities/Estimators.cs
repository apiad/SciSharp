using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SciSharp.Probabilities
{
    /// <summary>
    /// Contains methods for the estimation of common parameters
    /// of random distributions.
    /// </summary>
    public static class Estimators
    {
        /// <summary>
        /// Estimantes the mean of a random variable based on sampling.
        /// </summary>
        /// <param name="variable">The variable to estimate.</param>
        /// <param name="samples">The number of samples to perform the estimation.
        /// The greater the number, the more accurate the estimation.</param>
        /// <returns>The estimated mean of <paramref name="variable"/>.</returns>
        /// <remarks>This method uses and unbiassed estimator, which means
        /// that the estimated value converges to the real value
        /// as the number of samples increases.</remarks>
        /// <seealso cref="StdDev(SciSharp.Probabilities.RandomVariable,int)">
        /// The unbiassed estimator for the standard deviation.</seealso>
        public static double Mean(RandomVariable variable, int samples)
        {
            double mean = 0d;

            for (int i = 0; i < samples; i++)
                mean += variable;

            return mean / samples;
        }

        public static double StdDev(RandomVariable variable, int samples)
        {
            return StdDev(variable, Mean(variable, samples), samples);
        }

        public static double StdDev(RandomVariable variable, double mean, int samples)
        {
            double dev = 0d;

            for (int i = 0; i < samples; i++)
                dev += Math.Pow(mean - variable, 2);

            return Math.Sqrt(dev / (samples - 1));
        }
    }

    public class Histogram
    {
        private readonly double min;
        private readonly double max;
        private readonly int slices;
        private readonly int[] data;

        public Histogram(double min, double max, int slices)
        {
            this.min = min;
            this.max = max;
            this.slices = slices;
            this.data = new int[slices];
        }

        public bool AllowOutliers { get; set; }

        public void Add(double value)
        {
            if (value < min || value >= max)
                if (!AllowOutliers)
                    throw new ArgumentOutOfRangeException("value");
                else
                    return;

            double ratio = (value - min) / (max - min);
            int slice = (int)(ratio * slices);

            data[slice]++;
        }

        public void Add(RandomVariable variable, int samples)
        {
            for (int i = 0; i < samples; i++)
                Add(variable);
        }

        public override string ToString()
        {
            return ToString();
        }

        public string ToString(string sliceFormat = "{0:0.00}", string separator = " | ", string bar = "+", int count = 50)
        {
            int size = sliceFormat.Formatted(max).Length;
            int greater = Math.Max(1, data.Max());
            double step = (max - min) / slices;

            var builder = new StringBuilder();

            for (int i = 0; i < slices; i++)
                builder.AppendLine("{0}{1}{2}".Formatted(sliceFormat.Formatted(min + step*i).PadLeft(size),
                                                         separator, bar.Repeated(data[i]*count/greater)));

            return builder.ToString();
        }
    }
}
