using System;
using System.Text;


namespace SciSharp
{
    public static class FormattingExtensions
    {
        public static string Formatted(this string format, params object[] args)
        {
            return String.Format(format, args);
        }

        public static string Repeated(this string str, int times)
        {
            if (times < 0)
                throw new ArgumentOutOfRangeException("times", "Must be greater or equal to zero.");

            if (times == 0)
                return string.Empty;

            if (times == 1)
                return str;

            var builder = new StringBuilder(str.Length * times);

            for (int i = 0; i < times; i++)
                builder.Append(str);

            return builder.ToString();
        }

        public static string Ordinal(this int index)
        {
            string post;

            if (index == 1)
                post = "st";
            else if (index == 2)
                post = "nd";
            else if (index == 3)
                post = "rd";
            else
                post = "th";

            return index + post;
        }

        public static string[] Lines(this string s)
        {
            return s.Split(new[] {"\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] SplitSpaces(this string s)
        {
            return s.Split(new[] {" ", "\t"}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
