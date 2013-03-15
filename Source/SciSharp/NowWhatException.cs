using System;


namespace SciSharp
{
    [Serializable]
    public class NowWhatException : Exception
    {
        public NowWhatException()
            : this("Now What ?!") {}

        public NowWhatException(string message)
            : base(message) {}
    }
}
