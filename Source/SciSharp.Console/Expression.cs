using System;

using SciSharp.Language;


namespace SciSharp.Console
{
    public class Expression : Node
    {
        public virtual Object Evaluate(Context context)
        {
            return null;
        }
    }
}
