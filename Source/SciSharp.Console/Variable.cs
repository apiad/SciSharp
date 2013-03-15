namespace SciSharp.Console
{
    public class Variable : LValue
    {
        private readonly string name;

        public Variable(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }

        public override object Evaluate(Context context)
        {
            return context.Get<object>(name);
        }

        public override void SetValue(Context context, object value)
        {
            context.Set(name, value);
        }
    }
}
