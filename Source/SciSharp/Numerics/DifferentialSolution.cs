using System;
using System.Collections;
using System.Collections.Generic;


namespace SciSharp.Numerics
{
    public class DifferentialSolution : IEnumerable<Vector>
    {
        private readonly IEnumerable<Vector> integrationPath;

        private IEnumerator<Vector> enumerator;
        private int lastIteration = int.MaxValue;

        public DifferentialSolution(IEnumerable<Vector> integrationPath)
        {
            if (integrationPath == null)
                throw new ArgumentNullException("integrationPath");

            this.integrationPath = integrationPath;
        }

        #region IEnumerable<Vector> Members

        public IEnumerator<Vector> GetEnumerator()
        {
            return integrationPath.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public Vector Iterate(int iterations)
        {
            if (iterations <= 0)
                throw new ArgumentOutOfRangeException("iterations", "Must be greater than zero.");

            if (lastIteration > iterations)
            {
                lastIteration = 1;
                enumerator = integrationPath.GetEnumerator();
                enumerator.MoveNext();
            }

            while (lastIteration < iterations)
            {
                lastIteration++;
                enumerator.MoveNext();
            }

            return enumerator.Current;
        }
    }
}
