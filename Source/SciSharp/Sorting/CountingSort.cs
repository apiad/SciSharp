using System;


namespace SciSharp.Sorting
{
    public class CountingSort : Sorter<int>
    {
        private readonly int high;
        private readonly int low;

        public CountingSort(int low, int high)
        {
            this.low = low;
            this.high = high;
        }

        protected override void SafeSort(int[] items, int start, int end, Comparison<int> comparison)
        {
            throw new NotImplementedException();

            //var count = new int[high - low + 1];

            //for (int i = start; i <= end; i++)
            //    count[items[i] - low]++;

            //for (int i = low + 1; i <= high; i++)
            //    count[i] = count[i] + count[i + 1];

            //for (int i = end; i >= start; i--)
            //{
            //    items[count[items[i - low]]] = count[items[i - low]];
            //    count[items[i - low]]--;
            //}
        }
    }

    public class RadixSort : Sorter<float>
    {
        protected override void SafeSort(float[] items, int start, int end, Comparison<float> comparison)
        {
            float[] temp = new float[items.Length];

            for (int i = 0, mod = 1; i < 20; i++, mod <<= 1)
            {
                int front = start;
                int back = end;

                for (int n = start; n <= end; n++)
                {
                    if (((int)(items[n] * 1000000) & mod) == 0)
                        temp[front++] = items[n];
                    else
                        temp[back--] = items[n];
                }

                for (int n = start; n < front; n++)
                    items[n] = temp[n];

                for (int n = end; n > back; n--)
                    items[front++] = temp[n];
            }
        }
    }
}
