SciSharp - Example: Expressions Grammar
=======================================

This example shows how to create an expressions grammar using 
SciSharp formal languages processing tools, and build an LR 
parser to evaluate simple arithmetic expressions. 
The source code for this example is located in the folder 
`Examples/ExpressionsGrammar` in the root of the SciSharp distribution
folder.

The project consists of a console application with the grammar definition,
parser building, and a simple loop that reads a line from the standard
input and parses and evaluates the expression.


Definition of the grammar
-------------------------

The grammar is defined using the SciSharp grammars embedded DSL. The full definition is listed here:

    // Definition of the grammar
    var G = new Grammar<ExpressionNode>();

    // Definition of Non-terminals
    Def<ExpressionNode> E = G.Start("E"); // Names are for debugging reasons
    Def<ExpressionNode> T = G.Rule("T");
    Def<ExpressionNode> F = G.Rule("F");

    // Definition of Tokens
    Token<ExpressionNode> add = G.Token('+'); // You can define char tokens
    Token<ExpressionNode> sub = G.Token('-');
    Token<ExpressionNode> mul = G.Token('*');
    Token<ExpressionNode> div = G.Token('/');
    Token<ExpressionNode> lp = G.Token('(');
    Token<ExpressionNode> rp = G.Token(')');
    Token<ExpressionNode> cons = G.Token("const", @"[123456789]\d*");
    // You can also define tokens by regexes
    // The name is for debugging purposes

    // Definition of productions

    // Semantic rules defined by using the parameters list
    // p[0] is the reduced (left-hand) symbol
    // p[1], p[2], ..., are the right-hand symbols
    // Note that p[2] is the 'plus' token
    E %= (E + add + T).With((p, node) => node.Value = p[1].Value + p[3].Value) |
         (E + sub + T).With((p, node) => node.Value = p[1].Value - p[3].Value) |
         (T).With((p, node) => node.Value = p[1].Value);

    // Semantic rules defined using the symbol directly
    // x is the resulting Node instance
    // Notice that refering to 'T' is not ambiguous, the parser
    // will prefer the right-hand 'T'.
    T %= (T + mul + F).With(x => x.Value = T.Node.Value*F.Node.Value) |
         (T + div + F).With(x => x.Value = T.Node.Value/F.Node.Value) |
         (F).With(x => x.Value = F.Node.Value);

    // Semantic rules defined using the Ref() tool
    Ref<ExpressionNode> e = G.Ref();
    F %= (lp + E + rp).With(x => x.Value = E.Node.Value) |
         (cons).With(x => x.Value = Convert.ToDouble(cons.Regex));
