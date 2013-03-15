using System;


namespace SciSharp.Language.Expressions
{
    public abstract class UnaryFunction<T, T1> : ExpressionBase
    {
        private static readonly Type ReturnTypeInstance = typeof (T);
        private static readonly Type[] ArgsTypeInstance = new[] {typeof (T1)};

        public override int ArgsCount
        {
            get { return 1; }
        }

        public override Type[] ArgsType
        {
            get { return ArgsTypeInstance; }
        }

        public override Type ReturnType
        {
            get { return ReturnTypeInstance; }
        }
    }
}
