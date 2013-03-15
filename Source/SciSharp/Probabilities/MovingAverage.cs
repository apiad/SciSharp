using System.Collections.Generic;


namespace SciSharp.Probabilities
{
    public class MovingAverage
    {
        private readonly int maxCount;
        private readonly Queue<double> values;
        private double total;

        public MovingAverage(int maxCount)
        {
            this.maxCount = maxCount;
            values = new Queue<double>();
        }

        public int MaxCount
        {
            get { return maxCount; }
        }

        public int Count
        {
            get { return values.Count; }
        }

        public double Total
        {
            get { return total; }
        }

        public double Average
        {
            get { return total/values.Count; }
        }

        public void Add(double value)
        {
            values.Enqueue(value);
            total += value;

            if (values.Count > maxCount)
                total -= values.Dequeue();
        }

        public static implicit operator double(MovingAverage average)
        {
            return average.Average;
        }

        public static MovingAverage operator +(MovingAverage average, double value)
        {
            average.Add(value);
            return average;
        }

        public static MovingAverage operator -(MovingAverage average, double value)
        {
            average.Add(-value);
            return average;
        }

        public override string ToString()
        {
            return string.Format("Average: {0}", Average);
        }
    }
}
