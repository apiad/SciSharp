using System;
using System.IO;


namespace SciSharp
{
    public class Scanner
    {
        #region Instance Fields

        private readonly TextReader input;
        private int pos;
        private string[] tokens;

        #endregion

        #region Constructors

        public Scanner(TextReader input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            this.input = input;
        }

        public Scanner()
            : this(Console.In) {}

        #endregion

        #region Members

        public string Next()
        {
            while (tokens == null || pos >= tokens.Length)
            {
                if (input.Peek() == -1)
                    throw new InvalidOperationException("Not more tokens available on the input stream.");

                tokens = input.ReadLine().SplitSpaces();
                pos = 0;
            }

            return tokens[pos++];
        }

        public string NextLine()
        {
            tokens = null;
            return input.ReadLine();
        }

        public int NextInt()
        {
            return int.Parse(Next());
        }

        public long NextLong()
        {
            return long.Parse(Next());
        }

        public double NextDouble()
        {
            return double.Parse(Next());
        }

        public float NextFloat()
        {
            return float.Parse(Next());
        }

        public decimal NextDecimal()
        {
            return decimal.Parse(Next());
        }

        public bool NextBool()
        {
            return bool.Parse(Next());
        }

        #endregion

        #region Read

        public Scanner Read(ref int x)
        {
            if (input.Peek() == -1)
                return this;

            x = NextInt();

            return this;
        }

        public Scanner Read(ref double x)
        {
            if (input.Peek() == -1)
                return this;

            x = NextDouble();

            return this;
        }

        public Scanner Read(ref float x)
        {
            if (input.Peek() == -1)
                return this;

            x = NextFloat();

            return this;
        }

        public Scanner Read(ref long x)
        {
            if (input.Peek() == -1)
                return this;

            x = NextLong();

            return this;
        }

        public Scanner Read(ref bool x)
        {
            if (input.Peek() == -1)
                return this;

            x = NextBool();

            return this;
        }

        public Scanner Read(ref string x)
        {
            if (input.Peek() == -1)
                return this;

            x = Next();

            return this;
        }

        public Scanner Read(ref decimal x)
        {
            if (input.Peek() == -1)
                return this;

            x = NextDecimal();

            return this;
        }

        #endregion

        #region Operators

        public static bool operator true(Scanner s)
        {
            return s.input.Peek() != -1;
        }

        public static bool operator false(Scanner s)
        {
            return s.input.Peek() == -1;
        }

        #endregion
    }
}
