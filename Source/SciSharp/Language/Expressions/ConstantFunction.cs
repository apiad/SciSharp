using System;


namespace SciSharp.Language.Expressions
{
    public abstract class ConstantFunction<T> : ExpressionBase
    {
        private static readonly Type ReturnTypeInstance = typeof (T);

        public override int ArgsCount
        {
            get { return 0; }
        }

        public override Type[] ArgsType
        {
            get { return Type.EmptyTypes; }
        }

        public override Type ReturnType
        {
            get { return ReturnTypeInstance; }
        }
    }
}
