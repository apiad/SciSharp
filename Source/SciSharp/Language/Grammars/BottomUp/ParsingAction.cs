namespace SciSharp.Language.Grammars.BottomUp
{
    public struct ParsingAction<T>
        where T : Node, new()
    {
        private readonly ActionCode code;

        private readonly Def<T> leftHand;
        private readonly ProductionRule<T> rightHand;
        private readonly int state;

        public ParsingAction(int state, ActionCode code, Def<T> leftHand = null, ProductionRule<T> rightHand = null)
        {
            this.state = state;
            this.code = code;
            this.leftHand = leftHand;
            this.rightHand = rightHand;
        }

        public Def<T> LeftHand
        {
            get { return leftHand; }
        }

        public ProductionRule<T> RightHand
        {
            get { return rightHand; }
        }

        public int State
        {
            get { return state; }
        }

        public ActionCode Code
        {
            get { return code; }
        }

        public override string ToString()
        {
            if (Code == ActionCode.Accept)
                return "Accept";

            if (Code == ActionCode.Reduce)
                return string.Format("Reduce {0} -> {1}", leftHand, rightHand);

            if (Code == ActionCode.Shift)
                return string.Format("Shift {0}", state);

            return "ERROR";
        }
    }
}
