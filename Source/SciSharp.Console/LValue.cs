using System;


namespace SciSharp.Console
{
    public class LValue : Expression
    {
        public virtual void SetValue(Context context, Object value) {}
    }
}
