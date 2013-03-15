using System.Linq;


namespace SciSharp.Probabilities
{
    public class Collection
    {
        private readonly int[] items;
        private int total;

        public Collection(params int[] items)
        {
            this.items = items;
            total = items.Sum();
        }
    }
}
