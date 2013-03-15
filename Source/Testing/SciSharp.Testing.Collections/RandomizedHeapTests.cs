using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SciSharp.Collections;


namespace SciSharp.Testing.Collections
{
    [TestClass]
    public class RandomizedHeapTests
    {
        [TestMethod]
        public void HaveCountOneIfBuiltWithASingleElement()
        {
            var heap = new RandomizedHeap<int>(0);
            Assert.AreEqual(heap.Count, 1);
        }

        [TestMethod]
        public void PopTheRootWithASingleElement()
        {
            string str = "Hello World";
            var heap = new RandomizedHeap<string>(str);
            Assert.AreSame(heap.Pop(), str);
        }

        [TestMethod]
        public void UpdateTheCountWhenMerging()
        {
            var heap1 = new RandomizedHeap<int> {0, 2, 4, 6};
            var heap2 = new RandomizedHeap<int> {0, 1, 3, 5};

            RandomizedHeap<int> heap = RandomizedHeap<int>.Merge(heap1, heap2);

            Assert.AreEqual(heap.Count, heap1.Count + heap2.Count);
        }

        [TestMethod]
        public void ReturnItemsSorted()
        {
            var heap = new RandomizedHeap<int>();
            var rnd = new Random();

            for (int i = 0; i < 100; i++)
                heap.Add(rnd.Next());

            var ints = new List<int>(100);

            while (heap.Count > 0)
                ints.Add(heap.Pop());

            for (int i = 1; i < ints.Count; i++)
                Assert.IsTrue(ints[i] >= ints[i - 1]);
        }

        [TestMethod]
        public void CorrectlyMergeTwoHeaps()
        {
            var heap1 = new RandomizedHeap<int> {0, 2, 4, 6};
            var heap2 = new RandomizedHeap<int> {0, 1, 3, 5};

            RandomizedHeap<int> heap = RandomizedHeap<int>.Merge(heap1, heap2);

            var total = new HashSet<int>(heap);

            Assert.IsTrue(total.SetEquals(heap1.Concat(heap2)));
        }
    }
}
