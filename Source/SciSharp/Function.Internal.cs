using System;
using System.Linq;


namespace SciSharp
{
    public abstract partial class Function
    {
        #region Nested type: ConstantFunction

        private class ConstantFunction : Function
        {
            private readonly double value;

            public ConstantFunction(double value, int dimension)
                : base(dimension)
            {
                this.value = value;
            }

            protected override double SafeValue(Vector vector)
            {
                return value;
            }
        }

        #endregion

        #region Nested type: ProjectionFunction

        private class ProjectionFunction : Function
        {
            private readonly int index;

            public ProjectionFunction(int index, int dimension)
                : base(dimension)
            {
                this.index = index;
                if (index < 0 || index >= dimension)
                    throw new ArgumentOutOfRangeException("index", "Must be in the interval [0, {0}).".Formatted(dimension));
            }

            protected override double SafeValue(Vector vector)
            {
                return vector[index];
            }
        }

        #endregion

        #region Nested type: SumFunction

        private class SumFunction : Function
        {
            private readonly Function[] functions;

            public SumFunction(int dimension, params Function[] functions)
                : base(dimension)
            {
                if (functions == null)
                    throw new ArgumentNullException("functions");

                this.functions = functions;
            }

            protected override double SafeValue(Vector vector)
            {
                return functions.Sum(function => function.SafeValue(vector));
            }
        }

        #endregion
    }
}
