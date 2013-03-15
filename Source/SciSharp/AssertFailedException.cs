using System;


namespace SciSharp
{
    [Serializable]
    public class AssertFailedException : Exception
    {
        public AssertFailedException(string message)
            : base(message) {}
    }
}
