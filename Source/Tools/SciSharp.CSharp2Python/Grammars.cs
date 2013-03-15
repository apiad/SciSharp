using SciSharp.Language.Grammars;


namespace ScientificTools.Tools.CSharp2Python
{
    public static class Grammars
    {
        public static Grammar<LanguageNode> BuildPythonGrammar()
        {
            var G = new Grammar<LanguageNode>();
            GrammarBuilder<LanguageNode> _ = G.Builder;

            // Declarations
            Def<LanguageNode> classdef = G.Rule("classdef");
            Def<LanguageNode> arglist = G.Rule("arglist");
            Def<LanguageNode> suite = G.Rule("suite");

            // Tokens
            Token<LanguageNode> IDENTIFIER = G.Token("NAME", "[a-z,A-Z]");

            // Productions
            classdef %= "class" + IDENTIFIER + _['(' + _[arglist] + ')'] + ':' + suite;

            return G;
        }
    }
}
