using System;
using System.Collections.Generic;
using System.Linq;


namespace SciSharp.Collections
{
    internal class DisplacedIntSet : FiniteSetBase<int>
    {
        private readonly int displace;
        private readonly IFiniteSet<int> internalSet;

        public DisplacedIntSet(IFiniteSet<int> internalSet, int displace)
        {
            if (internalSet == null)
                throw new ArgumentNullException("internalSet");

            this.internalSet = internalSet;
            this.displace = displace;
        }

        public override int Count
        {
            get { return internalSet.Count; }
        }

        public override bool Contains(int item)
        {
            return internalSet.Contains(item - displace);
        }

        public override IEnumerator<int> GetEnumerator()
        {
            return internalSet.Select(i => i + displace).GetEnumerator();
        }
    }
}
