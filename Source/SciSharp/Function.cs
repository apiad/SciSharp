using System;


namespace SciSharp
{
    public abstract partial class Function : IRealFunction
    {
        private readonly int dimension;

        protected Function(int dimension)
        {
            this.dimension = dimension;
        }

        public Function this[int index]
        {
            get { return new ProjectionFunction(index, dimension); }
        }

        #region IRealFunction Members

        public double Value(Vector x)
        {
            if (x == null)
                throw new ArgumentNullException("x");

            if (x.Dimension != dimension)
                throw new ArgumentException("The argument x must have dimension {0}".Formatted(dimension));

            return SafeValue(x);
        }

        public int Dimension
        {
            get { return dimension; }
        }

        #endregion

        protected abstract double SafeValue(Vector vector);
    }
}
