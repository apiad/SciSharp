using System;


namespace SciSharp.Collections
{
    /// <summary>
    /// Provides extension methods for .NET arrays.
    /// </summary>
    public static class Arrays
    {
        /// <summary>
        /// Swaps two elements at given positions in an array.
        /// </summary>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        /// <param name="array">The array instance.</param>
        /// <param name="i">The index of the first element.</param>
        /// <param name="j">The index of the second element.</param>
        public static void Swap<T>(this T[] array, int i, int j)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        /// <summary>
        /// Creates a <see cref="ArraySlice{T}"/> with an specific
        /// array as back-end collection.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The instance of array to be used as back-end collection.</param>
        /// <param name="start">The starting (inclusive) index of the slice.</param>
        /// <param name="step">The step size of the slice.</param>
        /// <param name="end">The ending (inclusive) index of the silice.</param>
        /// <returns></returns>
        public static ArraySlice<T> Slice<T>(this T[] array, int start, int step, int end)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            return new ArraySlice<T>(array, start, step, end);
        }

        public static ArraySlice<T> Slice<T>(this T[] array, int start, int end)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            return new ArraySlice<T>(array, start, end);
        }

        public static ArraySlice<T> Slice<T>(this T[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            return new ArraySlice<T>(array);
        }

        public static T[] Fill<T>(int length)
        {
            return Fill(default(T), length);
        }

        /// <summary>
        /// Returns a new array with an specific value in every index.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="value">The value to set in every index.</param>
        /// <param name="length">The desired length of the array.</param>
        /// <returns>A new array of the specified length filled with the specified values.</returns>
        /// <remarks>The <paramref name="value"/> is copied with the default copy
        /// semantics, i.e., if it is a reference type then the same reference
        /// its shared for all the array elements.</remarks>
        public static T[] Fill<T>(T value, int length)
        {
            var t = new T[length];

            for (int i = 0; i < length; i++)
                t[i] = value;

            return t;
        }
    }
}
