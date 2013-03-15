using System;


namespace SciSharp.Language.Expressions
{
    public abstract class ConstantAction : ExpressionBase
    {
        private static readonly Type ReturnTypeInstance = typeof (void);

        public override Type ReturnType
        {
            get { return ReturnTypeInstance; }
        }

        public override int ArgsCount
        {
            get { return 0; }
        }

        public override Type[] ArgsType
        {
            get { return Type.EmptyTypes; }
        }
    }
}
