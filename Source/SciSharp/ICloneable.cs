namespace SciSharp
{
    public interface ICloneable<out T>
    {
        T Clone();
    }
}
