using System;


namespace SciSharp.Numerics
{
    public class LinearCombinationFunction<T> : IFunction<T, double>
    {
        private readonly Vector coeficients;
        private readonly IFunction<T, double>[] functions;

        public LinearCombinationFunction(Vector coeficients, params IFunction<T, double>[] functions)
        {
            if (coeficients == null)
                throw new ArgumentNullException("coeficients");

            if (functions == null)
                throw new ArgumentNullException("functions");

            if (coeficients.Dimension != functions.Length)
                throw new ArgumentOutOfRangeException();

            this.coeficients = coeficients;
            this.functions = functions;
        }

        public Vector Coeficients
        {
            get { return coeficients; }
        }

        #region IFunction<T,double> Members

        public double Value(T x)
        {
            double result = 0;

            for (int i = 0; i < coeficients.Dimension; i++)
                result += coeficients[i]*functions[i].Value(x);

            return result;
        }

        #endregion
    }
}
