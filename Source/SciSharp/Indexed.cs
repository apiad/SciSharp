using System;
using System.Collections.Generic;


namespace SciSharp
{
    public class Indexed<T> : IEquatable<Indexed<T>>, IComparable<Indexed<T>>
    {
        private readonly int index;
        private readonly T value;

        public Indexed(int index, T value)
        {
            this.index = index;
            this.value = value;
        }

        public int Index
        {
            get { return index; }
        }

        public T Value
        {
            get { return value; }
        }

        #region IComparable<Indexed<T>> Members

        public int CompareTo(Indexed<T> other)
        {
            return index - other.index;
        }

        #endregion

        #region IEquatable<Indexed<T>> Members

        public bool Equals(Indexed<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return index == other.index && EqualityComparer<T>.Default.Equals(value, other.value);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Index: {0}, Data: {1}", Index, Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Indexed<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (index*397) ^ EqualityComparer<T>.Default.GetHashCode(value);
            }
        }

        public static bool operator ==(Indexed<T> left, Indexed<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Indexed<T> left, Indexed<T> right)
        {
            return !Equals(left, right);
        }
    }
}
