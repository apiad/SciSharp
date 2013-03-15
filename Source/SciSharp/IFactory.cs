namespace SciSharp
{
    public interface IFactory<out T>
    {
        #region Members

        T Create();

        #endregion
    }
}
