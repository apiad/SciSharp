namespace SciSharp.Language.Grammars.BottomUp
{
    public class LrError<T>
        where T : Node, new()
    {
        private readonly int state;
        private readonly Token<T> symbol;
        private ParsingAction<T> action1;
        private ParsingAction<T> action2;

        public LrError(ParsingAction<T> action1, ParsingAction<T> action2, Token<T> symbol, int state)
        {
            this.action1 = action1;
            this.state = state;
            this.symbol = symbol;
            this.action2 = action2;
        }

        public int State
        {
            get { return state; }
        }

        public Token<T> Symbol
        {
            get { return symbol; }
        }

        public ParsingAction<T> Action1
        {
            get { return action1; }
        }

        public ParsingAction<T> Action2
        {
            get { return action2; }
        }

        public LrErrorType Type
        {
            get
            {
                if (action1.Code == ActionCode.Shift || action2.Code == ActionCode.Shift)
                    return LrErrorType.ShiftReduce;

                return LrErrorType.ReduceReduce;
            }
        }
    }
}
