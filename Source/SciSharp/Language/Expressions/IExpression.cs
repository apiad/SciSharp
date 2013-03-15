using System;


namespace SciSharp.Language.Expressions
{
    internal interface IExpression
    {
        int ArgsCount { get; }
        Type ReturnType { get; }
        Type[] ArgsType { get; }

        object Evaluate(params object[] args);
    }
}
