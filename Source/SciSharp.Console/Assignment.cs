namespace SciSharp.Console
{
    public class Assignment : Expression
    {
        private readonly LValue left;
        private readonly Expression right;

        public Assignment(LValue left, Expression right)
        {
            this.left = left;
            this.right = right;
        }

        public LValue Left
        {
            get { return left; }
        }

        public Expression Right
        {
            get { return right; }
        }

        public override object Evaluate(Context context)
        {
            object result = right.Evaluate(context);
            left.SetValue(context, result);
            return result;
        }
    }
}
