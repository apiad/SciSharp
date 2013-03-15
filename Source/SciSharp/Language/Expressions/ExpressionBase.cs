using System;


namespace SciSharp.Language.Expressions
{
    public abstract class ExpressionBase : IExpression
    {
        #region IExpression Members

        public abstract int ArgsCount { get; }
        public abstract Type ReturnType { get; }
        public abstract Type[] ArgsType { get; }

        public object Evaluate(params object[] args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            if (args.Length != ArgsCount)
                throw new ArgumentOutOfRangeException("args", args.Length, "Arguments count dont match the required: " + ArgsCount);

            for (int i = 0; i < args.Length; i++)
                if (!ArgsType[i].IsAssignableFrom(args[i].GetType()))
                    throw new ArgumentException(string.Format("Argument types don't match in index {0} of type {1} with required type {2}",
                                                              i, args[i].GetType(), ArgsType[i]), "args");

            object result = SafeEvaluate(args);

            if (ReturnType != typeof (void) && !ReturnType.IsAssignableFrom(result.GetType()))
                throw new ArgumentException(string.Format("Argument types don't match in evaluation result of type {0} with required type {1}",
                                                          result.GetType(), ReturnType));

            return result;
        }

        #endregion

        protected abstract object SafeEvaluate(object[] args);
    }
}
