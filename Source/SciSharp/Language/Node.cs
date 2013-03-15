namespace SciSharp.Language
{
    public class Node
    {
        public string Match { get; internal set; }
        public string Line { get; internal set; }
        public string Column { get; internal set; }
        public bool Eof { get; internal set; }
        public bool Error { get; internal set; }
    }
}
