namespace SciSharp.GameTheory
{
    public interface IPlay<in TBoard>
    {
        int Player { get; }

        void Apply(TBoard board);

        void Undo(TBoard board);
    }
}
