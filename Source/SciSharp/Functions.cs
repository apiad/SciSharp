using System;
using System.Collections.Generic;
using System.Linq;


namespace SciSharp
{
    public static class Functions
    {
        public static Func<T, TResult> Max<T, TResult>(IEnumerable<Func<T, TResult>> functions)
            where TResult : IComparable<TResult>
        {
            return x => functions.Max(f => f(x));
        }

        public static Func<T, TResult> Max<T, TResult>(params Func<T, TResult>[] functions)
            where TResult : IComparable<TResult>
        {
            return Max((IEnumerable<Func<T, TResult>>) functions);
        }

        public static Func<T, double> Sum<T>(IEnumerable<Func<T, double>> functions)
        {
            return x => functions.Max(f => f(x));
        }

        public static IFunction<T, TResult> FromMethod<T, TResult>(Func<T, TResult> function)
        {
            return new MethodFunction<T, TResult>(function);
        }

        #region Nested type: MethodFunction

        public class MethodFunction<T, TResult> : IFunction<T, TResult>
        {
            private readonly Func<T, TResult> function;

            public MethodFunction(Func<T, TResult> function)
            {
                this.function = function;
            }

            #region IFunction<T,TResult> Members

            public TResult Value(T x)
            {
                return function(x);
            }

            #endregion
        }

        #endregion
    }
}
