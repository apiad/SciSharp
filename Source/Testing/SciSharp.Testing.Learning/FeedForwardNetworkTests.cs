using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SciSharp.Learning.Classification;


namespace SciSharp.Testing.Learning
{
    [TestClass]
    public class FeedForwardNetworkTests
    {
        [TestMethod]
        public void MajorityFunction()
        {
            var ffn = new FeedForwardNetwork(10, 1);

            var examples = new List<TrainingExample>();

            for (int i = 0; i < 100; i++)
            {
                var x = Vectors.Random(10);
                examples.Add(new TrainingExample(x,
                                                 new Vector(x.Sum() > 5 ? 1.0 : 0.0)));
            }

            ffn.Train(examples, 100);

            Assert.IsTrue(ffn.Test(examples).Length < 0.1);
        }
    }
}
