namespace SciSharp.Console
{
    public class Constant : Expression
    {
        private readonly object value;

        public Constant(object value)
        {
            this.value = value;
        }

        public override object Evaluate(Context context)
        {
            return value;
        }
    }
}
