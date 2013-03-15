namespace SciSharp.GameTheory
{
    public delegate double MonteCarloSelection<TGame, TBoard, TPlay>(GameTree<TGame, TBoard, TPlay> parent, GameTree<TGame, TBoard, TPlay> child)
        where TPlay : IPlay<TBoard>
        where TGame : IGame<TBoard, TPlay>;
}
