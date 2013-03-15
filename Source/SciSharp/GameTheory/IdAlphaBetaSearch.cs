using System;


namespace SciSharp.GameTheory
{
    public class IdAlphaBetaSearch<TGame, TBoard, TPlay> : AlphaBetaSearch<TGame, TBoard, TPlay>
        where TPlay : IPlay<TBoard>
        where TGame : IGame<TBoard, TPlay>
    {
        private readonly int maxDepth;
        private readonly int time;

        private double payoff;

        public IdAlphaBetaSearch(int maxDepth, int time, Func<TGame, int, double> heuristicPayoff, double payoffBias = 1d)
            : base(heuristicPayoff, payoffBias)
        {
            this.maxDepth = maxDepth;
            this.time = time;
        }

        public double Payoff
        {
            get { return payoff; }
        }

        public void StartSearch(TGame game, int player)
        {
            payoff = double.NegativeInfinity;

            for (int i = 0; i < maxDepth; i++)
                payoff = Max(game, player, i, double.NegativeInfinity, double.PositiveInfinity).Item2;
        }
    }
}
