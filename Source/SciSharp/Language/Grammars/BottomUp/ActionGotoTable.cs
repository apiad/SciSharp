using System;
using System.Collections.Generic;
using System.Text;


namespace SciSharp.Language.Grammars.BottomUp
{
    [Serializable]
    public class ActionGotoTable<T> : IActionGotoTable<T>
        where T : Node, new()
    {
        private readonly Dictionary<ActionKey<T>, ParsingAction<T>> actionTable;

        private readonly List<LrError<T>> errors;
        private readonly Dictionary<GotoKey<T>, int> gotoTable;

        public ActionGotoTable()
        {
            errors = new List<LrError<T>>();
            actionTable = new Dictionary<ActionKey<T>, ParsingAction<T>>();
            gotoTable = new Dictionary<GotoKey<T>, int>();
        }

        public bool Correct
        {
            get { return errors.Count == 0; }
        }

        #region IActionGotoTable<T> Members

        public ParsingAction<T> Action(int state, Token<T> symbol)
        {
            ParsingAction<T> action;

            if (!actionTable.TryGetValue(new ActionKey<T>(state, symbol), out action))
                return new ParsingAction<T>(-1, ActionCode.Error);

            return action;
        }

        public int Goto(int state, Def<T> symbol)
        {
            return gotoTable[new GotoKey<T>(state, symbol)];
        }

        #endregion

        public void SetAction(int state, Token<T> symbol, ParsingAction<T> action)
        {
            var key = new ActionKey<T>(state, symbol);

            if (actionTable.ContainsKey(key))
                errors.Add(new LrError<T>(action, actionTable[key], symbol, state));

            actionTable[new ActionKey<T>(state, symbol)] = action;
        }

        public void SetGoto(int state, Def<T> symbol, int result)
        {
            gotoTable[new GotoKey<T>(state, symbol)] = result;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("ACTION:");

            foreach (var parsingAction in actionTable)
            {
                sb.Append(parsingAction.Key.ToString());
                sb.Append(" --> ");
                sb.AppendLine(parsingAction.Value.ToString());
                sb.AppendLine();
            }

            sb.AppendLine("GOTO:");

            foreach (var state in gotoTable)
                sb.AppendFormat("{0} --> {1}\n", state.Key, state.Value);

            return sb.ToString();
        }
    }
}
