namespace SciSharp
{
    internal abstract class Context : IContext
    {
        #region IContext Members

        public void Dispose()
        {
            CloseContext();
        }

        public bool Closed { get; private set; }

        public void CloseContext()
        {
            if (Closed)
                return;

            SafeEnd();

            Closed = true;
        }

        #endregion

        protected abstract void SafeEnd();
    }
}
