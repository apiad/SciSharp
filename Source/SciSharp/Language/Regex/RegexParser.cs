using System;


namespace SciSharp.Language.Regex
{
    public class RegexParser
    {
        private RegexToken currentToken;
        private RegexLexer lexer;

        public IRegexNode Parse(string input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            lexer = new RegexLexer(input);
            currentToken = lexer.NextToken();

            IRegexNode node = MatchRegex();

            Match(RegexTokenType.End);

            return node;
        }

        private IRegexNode MatchRegex()
        {
            //var left = MatchUnion();
            //var right = MatchUnionAux();

            //return right == RegexEmptyNode.Node ? left : new RegexUnionNode(left, right);

            IRegexNode left = MatchUnion();

            while (currentToken.Type != RegexTokenType.End && currentToken.Type != RegexTokenType.CloseBracket)
            {
                Match(RegexTokenType.Union);
                IRegexNode right = MatchUnion();
                left = new RegexUnionNode(left, right);
            }

            return left;
        }

        //private IRegexNode MatchUnionAux()
        //{
        //    if (currentToken.Type == RegexTokenType.End || currentToken.Type == RegexTokenType.CloseBracket)
        //        return RegexEmptyNode.Node;

        //    Match(RegexTokenType.Union);

        //    var left = MatchUnion();
        //    var right = MatchUnionAux();

        //    return right == RegexEmptyNode.Node ? left : new RegexUnionNode(left, right);
        //}

        private IRegexNode MatchUnion()
        {
            //var left = MatchConcat();
            //var right = MatchConcatAux();

            //return right == RegexEmptyNode.Node ? left : new RegexConcatNode(left, right);

            IRegexNode left = MatchConcat();

            while (currentToken.Type != RegexTokenType.End && currentToken.Type != RegexTokenType.Union && currentToken.Type != RegexTokenType.CloseBracket)
            {
                IRegexNode right = MatchConcat();
                left = new RegexConcatNode(left, right);
            }

            return left;
        }

        //private IRegexNode MatchConcatAux()
        //{
        //    if (currentToken.Type == RegexTokenType.End || currentToken.Type == RegexTokenType.Union || currentToken.Type == RegexTokenType.CloseBracket)
        //        return RegexEmptyNode.Node;

        //    var left = MatchConcat();
        //    var right = MatchConcatAux();

        //    return right == RegexEmptyNode.Node ? left : new RegexConcatNode(left, right);
        //}

        private IRegexNode MatchConcat()
        {
            //var left = MatchFactor();
            //var right = MatchFactorAux();

            //return right == RegexEmptyNode.Node ? left : new RegexClosureNode(left);

            IRegexNode left = MatchFactor();

            while (currentToken.Type != RegexTokenType.End && currentToken.Type != RegexTokenType.OpenBracket && currentToken.Type != RegexTokenType.Union && currentToken.Type != RegexTokenType.Literal && currentToken.Type != RegexTokenType.CloseBracket)
            {
                Match(RegexTokenType.Clousure);
                left = new RegexClosureNode(left);
            }

            return left;
        }

        //private IRegexNode MatchFactorAux()
        //{
        //    if (currentToken.Type == RegexTokenType.End || currentToken.Type == RegexTokenType.OpenBracket || currentToken.Type == RegexTokenType.Union || currentToken.Type == RegexTokenType.Literal || currentToken.Type == RegexTokenType.CloseBracket)
        //        return RegexEmptyNode.Node;

        //    Match(RegexTokenType.Clousure);

        //    return new RegexLiteralNode("*");
        //}

        private IRegexNode MatchFactor()
        {
            if (currentToken.Type == RegexTokenType.OpenBracket)
            {
                Match(RegexTokenType.OpenBracket);
                IRegexNode node = MatchRegex();
                Match(RegexTokenType.CloseBracket);
                return node;
            }

            if (currentToken.Type == RegexTokenType.Literal)
            {
                var node = new RegexLiteralNode(currentToken.Value.ToString());
                Match(currentToken.Type);
                return node;
            }

            throw new Grammars.WrongTokenException<RegexToken>(currentToken, RegexTokenType.OpenBracket, RegexTokenType.Literal);
        }

        private void Match(RegexTokenType token)
        {
            if (currentToken.Type != token)
                throw new Grammars.WrongTokenException<RegexToken>(currentToken, token);

            currentToken = lexer.NextToken();
        }
    }
}
