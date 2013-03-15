using System;

using SciSharp.Language.Grammars;


namespace ScientificTools.Tools.CSharp2Python
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Grammar<LanguageNode> grammar = Grammars.BuildPythonGrammar();

            Console.WriteLine(grammar);
        }
    }
}
