using System;


namespace SciSharp
{
    public interface IContext : IDisposable
    {
        bool Closed { get; }

        void CloseContext();
    }
}
