using System;


namespace SciSharp.Language.Expressions
{
    public abstract class UnaryAction<T> : ExpressionBase
    {
        private static readonly Type ReturnTypeInstance = typeof (void);
        private static readonly Type[] ArgsTypeInstance = new[] {typeof (T)};

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
