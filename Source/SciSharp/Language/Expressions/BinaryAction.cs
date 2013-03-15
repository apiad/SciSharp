using System;


namespace SciSharp.Language.Expressions
{
    public abstract class BinaryAction<T1, T2> : ExpressionBase
    {
        private static readonly Type ReturnTypeInstance = typeof (void);
        private static readonly Type[] ArgsTypeInstance = new[] {typeof (T1), typeof (T2)};

        public override int ArgsCount
        {
            get { return 2; }
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
