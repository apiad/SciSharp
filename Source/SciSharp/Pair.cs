using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace SciSharp
{
    public struct Pair<T> : IEquatable<Pair<T>>
    {
        #region Constructors

        public Pair(T a, T b)
            : this()
        {
            A = a;
            B = b;
        }

        #endregion

        #region Properties

        public T A { get; set; }
        public T B { get; set; }

        #endregion

        #region IEquatable<Pair<T>> Members

        public bool Equals(Pair<T> other)
        {
            return Equals(other.A, A) && Equals(other.B, B);
        }

        #endregion

        #region Members

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(Pair<T>))
                return false;
            return Equals((Pair<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (A.GetHashCode() * 397) ^ B.GetHashCode();
            }
        }

        public static bool operator ==(Pair<T> left, Pair<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Pair<T> left, Pair<T> right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("A: {0}, B: {1}", A, B);
        }

        #endregion
    }

    public class Provider
    {
        private readonly Assembly[] assemblies;

        public Provider(params Assembly[] assemblies)
        {
            this.assemblies = assemblies;
        }

        public Provider()
            : this(Assembly.GetEntryAssembly(),
                   Assembly.GetExecutingAssembly())
        {

        }

        public T[] GetInstances<T>(params object[] args)
        {
            return (from assembly in assemblies
                    from type in assembly.GetTypes()
                    where typeof (T).IsAssignableFrom(type)
                    select (T) Activator.CreateInstance(type, args))
                .ToArray();
        }
    }
}
