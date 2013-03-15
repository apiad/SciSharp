using System;


namespace SciSharp.GameTheory
{
    public static class MonteCarloStrategies
    {
        public static double UpperConfidencePayoff<TGame, TBoard, TPlay>(GameTree<TGame, TBoard, TPlay> parent, GameTree<TGame, TBoard, TPlay> child, double balance)
            where TGame : IGame<TBoard, TPlay>
            where TPlay : IPlay<TBoard>
        {
            return child.TotalPayoff/child.Plays + balance*Math.Sqrt(2*Math.Log(parent.Plays)/child.Plays);
        }

        public static double UpperConfidencePayoff<TGame, TBoard, TPlay>(GameTree<TGame, TBoard, TPlay> parent, GameTree<TGame, TBoard, TPlay> child)
            where TGame : IGame<TBoard, TPlay>
            where TPlay : IPlay<TBoard>
        {
            return UpperConfidencePayoff(parent, child, 1);
        }

        public static MonteCarloSelection<TGame, TBoard, TPlay> UpperConfidencePayoff<TGame, TBoard, TPlay>(double balance)
            where TGame : IGame<TBoard, TPlay>
            where TPlay : IPlay<TBoard>
        {
            return (parent, child) => UpperConfidencePayoff(parent, child, balance);
        }

        public static double UpperConfidenceWins<TGame, TBoard, TPlay>(GameTree<TGame, TBoard, TPlay> parent, GameTree<TGame, TBoard, TPlay> child, double balance)
            where TGame : IGame<TBoard, TPlay>
            where TPlay : IPlay<TBoard>
        {
            return child.Wins*1d/child.Plays + balance*Math.Sqrt(2*Math.Log(parent.Plays)/child.Plays);
        }

        public static double UpperConfidenceWins<TGame, TBoard, TPlay>(GameTree<TGame, TBoard, TPlay> parent, GameTree<TGame, TBoard, TPlay> child)
            where TGame : IGame<TBoard, TPlay>
            where TPlay : IPlay<TBoard>
        {
            return UpperConfidenceWins(parent, child, 1);
        }

        public static MonteCarloSelection<TGame, TBoard, TPlay> UpperConfidenceWins<TGame, TBoard, TPlay>(double balance)
            where TGame : IGame<TBoard, TPlay>
            where TPlay : IPlay<TBoard>
        {
            return (parent, child) => UpperConfidenceWins(parent, child, balance);
        }
    }
}
