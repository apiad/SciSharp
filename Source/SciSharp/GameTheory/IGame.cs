using System.Collections.Generic;


namespace SciSharp.GameTheory
{
    public interface IGame<out TBoard, out TPlay>
        where TPlay : IPlay<TBoard>
    {
        int Players { get; }
        TBoard Board { get; }

        bool Finished { get; }

        int CurrentPlayer { get; }

        IEnumerable<TPlay> ValidMoves();

        double Payoff(int player);

        IGameKey GetKey();
    }
}
