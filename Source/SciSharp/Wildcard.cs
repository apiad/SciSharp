namespace SciSharp
{
    public class Wildcard
    {
        private static readonly Wildcard Instance;

        static Wildcard()
        {
            Instance = new Wildcard();
        }

        private Wildcard() {}

        public static Wildcard Get
        {
            get { return Instance; }
        }
    }
}
