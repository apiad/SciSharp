namespace SciSharp
{
    public interface IAnaliticFunction : IRealFunction
    {
        Vector Gradient(Vector x);

        Matrix Hessian(Vector x);
    }
}
