namespace SciSharp.Probabilities
{
    public class EventData<T>
    {
        private readonly T data;
        private readonly double probability;

        public EventData(T data, double probability)
        {
            this.data = data;
            this.probability = probability;
        }

        public T Data
        {
            get { return data; }
        }

        public double Probability
        {
            get { return probability; }
        }
    }
}
