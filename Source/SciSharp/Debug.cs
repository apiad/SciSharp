using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace SciSharp
{
    public static class Debug
    {
        #region Static and Constant Fields

        private static readonly Stack<int> Timers;

        public static readonly List<string> Categories;

        #endregion

        #region Constructors

        static Debug()
        {
            IndentSize = 4;
            LogBuffer = TextWriter.Null;
            Timers = new Stack<int>(16);
            Categories = new List<string>();
            Verbosity = int.MaxValue;
        }

        #endregion

        #region Properties

        public static TextWriter LogBuffer { get; set; }

        public static int Verbosity { get; set; }

        public static int IndentLevel { get; set; }

        public static int IndentSize { get; set; }

        #endregion

        #region Members

        private static bool Filter(string category, int verbosity)
        {
            if (Categories.Count == 0)
                return verbosity <= Verbosity;

            if (Categories.Any(category.StartsWith))
                return verbosity <= Verbosity;

            return false;
        }

        [Conditional("DEBUG")]
        public static void Indent()
        {
            IndentLevel++;
        }

        [Conditional("DEBUG")]
        public static void Unindent()
        {
            IndentLevel--;
        }

        [Conditional("DEBUG")]
        public static void BeginTimer()
        {
            BeginTimerIf(Engine.Debugging);
        }

        [Conditional("DEBUG")]
        public static void BeginTimerIf(bool condition)
        {
            if (condition)
                Timers.Push(Environment.TickCount);
        }

        [Conditional("DEBUG")]
        public static void EndTimer()
        {
            EndTimerIf(Engine.Debugging);
        }

        [Conditional("DEBUG")]
        public static void EndTimerIf(bool condition)
        {
            if (condition)
            {
                int result = Environment.TickCount - Timers.Pop();
                Post("Elapsed: {0} ms.", result);
            }
        }

        [Conditional("DEBUG")]
        public static void Assert(bool condition, string format, params object[] args)
        {
            if (!condition)
                Post(format, 0, args);
        }

        [Conditional("DEBUG")]
        public static void Assert(bool condition, string message)
        {
            if (!condition)
                Post(message);
        }

        [Conditional("DEBUG")]
        public static void Post(string message)
        {
            PostIf(Engine.Debugging, message);
        }

        [Conditional("DEBUG")]
        public static void PostIf(bool condition, string message)
        {
            if (!condition)
                return;

            if (IndentLevel < Verbosity)
                LogBuffer.WriteLine(message.PadLeft(message.Length + IndentLevel*IndentSize));
        }

        [Conditional("DEBUG")]
        public static void PostIf(bool condition, string format, params object[] args)
        {
            if (!condition)
                return;

            if (IndentLevel <= Verbosity)
                LogBuffer.WriteLine(format.PadLeft(format.Length + IndentLevel*IndentSize), args);
        }

        [Conditional("DEBUG")]
        public static void Post(string format, params object[] args)
        {
            PostIf(Engine.Debugging, format, args);
        }

        [Conditional("DEBUG")]
        public static void ThrowOnNaN(double value, string message = "Value cannot be NaN.")
        {
            if (Engine.ThrowOnNaN && double.IsNaN(value))
                throw new ArithmeticException(message);
        }

        #endregion
    }
}
