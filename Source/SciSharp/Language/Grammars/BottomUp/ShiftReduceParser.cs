using System;
using System.Collections.Generic;


namespace SciSharp.Language.Grammars.BottomUp
{
    [Serializable]
    public class ShiftReduceParser<T> : IParser<T>
        where T : Node, new()
    {
        private readonly Grammar<T> grammar;
        private readonly Stack<T> nodes;
        private readonly Stack<int> stack;
        private readonly IActionGotoTable<T> table;

        public ShiftReduceParser(IActionGotoTable<T> table, Grammar<T> grammar)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            this.table = table;
            this.grammar = grammar;
            stack = new Stack<int>();
            nodes = new Stack<T>();
        }

        #region IParser<T> Members

        public Grammar<T> Grammar
        {
            get { return grammar; }
        }

        public T Parse(TokenStream<T> str)
        {
            stack.Clear();
            nodes.Clear();
            stack.Push(0);

            Token<T> symbol = str.NextToken();

            while (true)
            {
                int state = stack.Peek();
                ParsingAction<T> action = table.Action(state, symbol);

                switch (action.Code)
                {
                    case ActionCode.Shift:
                        // Cambiar de estado
                        stack.Push(action.State);

                        // Meter el Node del token
                        nodes.Push(symbol.Node);

                        // Leer el siguiente símbolo
                        symbol = str.NextToken();
                        break;

                    case ActionCode.Reduce:
                        // Reducir A -> B.
                        Def<T> A = action.LeftHand;
                        ProductionRule<T> B = action.RightHand;

                        // Sacar |B| símbolos de la pila
                        for (int i = 0; i < B.Count; i++)
                            stack.Pop();

                        // Sacar |B| nodes de la otra pila
                        var n = new T[B.Count];

                        // Salen en orden inverso
                        for (int i = 0; i < B.Count; i++)
                            n[B.Count - i - 1] = nodes.Pop();

                        // Aplicar las reglas semánticas
                        T node = B.ApplyRule(n);

                        // Nuevo estado en el tope de la pila
                        int t = stack.Peek();

                        // Meter GOTO[t,A] en la pila
                        stack.Push(table.Goto(t, A));

                        // Meter el Node de A
                        nodes.Push(node);
                        A.Node = node;
                        break;

                    case ActionCode.Accept:
                        // Reducir S' -> S.
                        // Me puedo quedar con S porque S' no importa
                        Def<T> S = action.RightHand.Defs[0];

                        // Ya las reglas semánticas de S se aplicaron
                        return S.Node;

                    case ActionCode.Error:
                        throw new Language.UnexpectedTokenException(symbol.Node.Match);

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion
    }
}
