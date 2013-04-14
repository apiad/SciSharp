using System;
using System.Linq;
using System.Text;


namespace SciSharp
{
    /// <summary>
    /// Represents a double precision vector.
    /// </summary>
    public class Vector : ICloneable<Vector>, IEquatable<Vector>
    {
        #region Fields

        /// <summary>
        /// Stores the values of the vector.
        /// </summary>
        private readonly double[] elements;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class <see cref="Vector"/>
        /// with the specified dimension and all elements set to 0.
        /// </summary>
        /// <param name="dimension">The dimension of the vector.</param>
        public Vector(int dimension)
        {
            Dimension = dimension;
            elements = new double[dimension];
        }

        /// <summary>
        /// Initializes a new instance of the class <see cref="Vector"/>
        /// with the specified length and all the elements set to 0.
        /// </summary>
        /// <param name="elements">The values for the vector.</param>
        /// <exception cref="ArgumentNullException">If the value of the <paramref name="elements"/> array is null.</exception>
        public Vector(params double[] elements)
        {
            if (elements == null)
                throw new ArgumentNullException("elements");

            Dimension = elements.Length;
            this.elements = elements;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the dimension of the vector.
        /// </summary>
        public int Dimension { get; private set; }

        /// <summary>
        /// Returns the length (euclidan norm) of the vector.
        /// </summary>
        public double Length
        {
            get { return Math.Sqrt(this*this); }
        }

        /// <summary>
        /// Returns a reference to the elements of the vector in the form
        /// of an <see cref="Array"/>.  
        /// </summary>
        /// <remarks>
        /// The reference returned is the same used internally by the 
        /// <see cref="Vector"/> class to store the elements. If any
        /// element of this array is changed, it will be changed
        /// in the <see cref="Vector"/> instance as well. 
        /// In general this reference should be used as read-only.
        /// </remarks>
        public double[] Elements
        {
            get { return elements; }
        }

        public double LengthSqr
        {
            get { return this*this; }
        }

        /// <summary>
        /// Gets or sets the value of an element of the vector.
        /// </summary>
        /// <param name="index">The index of the element starting at 0.</param>
        /// <returns>The value of the specified coordinate (element) of the vector.</returns>
        /// <exception cref="IndexOutOfRangeException">If the value of <paramref name="index"/> 
        /// is less than 0 or greater equal than <see cref="Dimension"/>.</exception>
        public double this[int index]
        {
            get
            {
                if (index < 0)
                    index = Dimension + index;

                Debug.ThrowOnNaN(elements[index]);

                return elements[index];
            }
            set
            {
                if (index < 0)
                    index = Dimension + index;

                Debug.ThrowOnNaN(value);

                elements[index] = value;
            }
        }

        /// <summary>
        /// Gets or sets a sub-array. If any index is less than zero,
        /// it is taken starting form the end, so that the last position
        /// is -1.
        /// </summary>
        /// <param name="start">The inclusive starting position.</param>
        /// <param name="end">The exclusive final position.</param>
        /// <returns>The elements in the specified subvector.</returns>
        public Vector this[int start, int end]
        {
            get
            {
                if (start < 0)
                    start += Dimension;
                if (end < 0)
                    end += Dimension + 1;

                var v = new Vector(end - start);

                for (int i = start; i < end; i++)
                    v[i - start] = this[i];

                return v;
            }
            set
            {
                if (start < 0)
                    start += Dimension;
                if (end < 0)
                    end += Dimension + 1;

                for (int i = start; i < end; i++)
                    this[i] = value[i - start];
            }
        }

        public Vector this[int start, int step, int end]
        {
            get
            {
                if (start < 0)
                    start += Dimension;
                if (end < 0)
                    end += Dimension + 1;

                var v = new Vector((end - start)/step);

                for (int i = 0; i < v.Dimension; i++)
                    v[i] = this[start + i*step];

                return v;
            }
            set
            {
                if (start < 0)
                    start += Dimension;
                if (end < 0)
                    end += Dimension + 1;

                if (value.Dimension != (end - start)/step)
                    throw new ArgumentException("The size of the vector and the slice don't match.");

                for (int i = 0; i < value.Dimension; i++)
                    this[start + i*step] = value[i];
            }
        }

        public ComponentWiseOperator _
        {
            get { return new ComponentWiseOperator(this); }
        }

        #endregion

        #region Instance Methods

        #region ICloneable<Vector> Members

        public Vector Clone()
        {
            var result = new Vector(Dimension);

            for (int i = 0; i < Dimension; i++)
                result[i] = this[i];

            return result;
        }

        #endregion

        #region IEquatable<Vector> Members

        public bool Equals(Vector other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Dimension != other.Dimension)
                return false;

            return Length - other.Length <= Engine.Epsilon;
        }

        #endregion

        public Vector Add(Vector other)
        {
            if (Dimension != other.Dimension)
                throw new ArgumentException("Dimensions must agree.");

            for (int i = 0; i < Dimension; i++)
                this[i] += other[i];

            return this;
        }

        public Vector Subtract(Vector other)
        {
            if (Dimension != other.Dimension)
                throw new ArgumentException("Dimensions must agree.");

            for (int i = 0; i < Dimension; i++)
                this[i] -= other[i];

            return this;
        }

        public Vector Multiply(double scalar)
        {
            Debug.ThrowOnNaN(scalar);

            for (int i = 0; i < Dimension; i++)
                this[i] *= scalar;

            return this;
        }

        public Vector Divide(double scalar)
        {
            Debug.ThrowOnNaN(scalar);

            for (int i = 0; i < Dimension; i++)
                this[i] /= scalar;

            return this;
        }

        public Vector Modulate(Vector other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            if (other.Dimension != Dimension)
                throw new ArgumentException("Dimensions don't match.");

            for (int i = 0; i < Dimension; i++)
                this[i] *= other[i];

            return this;
        }

        public Vector Complement()
        {
            for (int i = 0; i < Dimension; i++)
                this[i] = 1 - this[i];

            return this;
        }

        public double DistanceSqrTo(Vector vector)
        {
            return (this - vector).LengthSqr;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof (Vector))
                return false;
            return Equals((Vector) obj);
        }

        public override string ToString()
        {
            return ToString((int) (Engine.Epsilon > 0 ? -Math.Log10(Engine.Epsilon) : 15));
        }

        public string ToString(int digits)
        {
            digits = Math.Min(digits, 15);

            var str = new StringBuilder("( ", Dimension*70 + 4);

            for (int i = 0; i < Dimension - 1; i++)
                str.Append(Math.Round(elements[i], digits) + ", ");

            if (Dimension > 0)
                str.Append(Math.Round(elements[elements.Length - 1], digits) + " ");

            str.Append(")");

            return str.ToString();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (elements.GetHashCode()*397) ^ Dimension;
            }
        }

        public Matrix AsRow()
        {
            var m = new Matrix(1, Dimension);

            for (int i = 0; i < Dimension; i++)
                m[0, i] = this[i];

            return m;
        }

        public Matrix AsColumn()
        {
            var m = new Matrix(Dimension, 1);

            for (int i = 0; i < Dimension; i++)
                m[i, 0] = this[i];

            return m;
        }

        public double Swap(int index, double value)
        {
            double temp = this[index];
            this[index] = value;
            return temp;
        }

        public void Swap(int i, int j)
        {
            this[j] = Swap(i, this[j]);
        }

        public Vector Normalize()
        {
            double length = Length;

            for (int i = 0; i < Dimension; i++)
                this[i] /= length;

            return this;
        }

        public Vector Clamp(double min, double max)
        {
            for (int i = 0; i < Dimension; i++)
            {
                if (this[i] < min)
                    this[i] = min;
                else if (this[i] > max)
                    this[i] = max;
            }

            return this;
        }

        #endregion

        #region Static Methods

        public static Vector Add(Vector left, Vector right)
        {
            if (!Engine.OperateOnSelf)
                left = left.Clone();

            return left.Add(right);
        }

        public static Vector Subtract(Vector left, Vector right)
        {
            if (!Engine.OperateOnSelf)
                left = left.Clone();

            return left.Subtract(right);
        }

        public static Vector Multiply(Vector vector, double scalar)
        {
            if (!Engine.OperateOnSelf)
                vector = vector.Clone();

            return vector.Multiply(scalar);
        }

        public static Vector Divide(Vector vector, double scalar)
        {
            if (!Engine.OperateOnSelf)
                vector = vector.Clone();

            return vector.Divide(scalar);
        }

        public static Vector Multiply(Vector vector, Matrix matrix)
        {
            if (vector == null)
                throw new ArgumentNullException("vector");

            if (matrix == null)
                throw new ArgumentNullException("matrix");

            if (vector.Dimension != matrix.Rows)
                throw new ArgumentException("Dimensions don't match.");

            var result = new double[matrix.Columns];

            if (!Engine.UseParallelOptimizations)
            {
                // Secuencial

                for (int j = 0; j < matrix.Columns; j++)
                    for (int k = 0; k < vector.Dimension; k++)
                        result[j] += vector[k]*matrix[k, j];
            }
            else
            {
                // Paralelo

                //Parallel.For(0, matrix.Columns, j =>
                //                                    {
                //                                        for (int k = 0; k < vector.Dimension; k++)
                //                                            result[j] += vector[k] * matrix[k, j];
                //                                    });
            }

            return new Vector(result);
        }

        public static Vector Multiply(Matrix matrix, Vector vector)
        {
            if (vector == null)
                throw new ArgumentNullException("vector");

            if (matrix == null)
                throw new ArgumentNullException("matrix");

            if (vector.Dimension != matrix.Columns)
                throw new ArgumentException("Dimensions don't match.");

            var result = new double[matrix.Rows];

            if (!Engine.UseParallelOptimizations)
            {
                // Secuencial

                for (int i = 0; i < matrix.Rows; i++)
                    for (int k = 0; k < vector.Dimension; k++)
                        result[i] += vector[k]*matrix[i, k];
            }
            else
            {
                //Parallel.For(0, matrix.Rows, i =>
                //                                 {
                //                                     for (int k = 0; k < vector.Dimension; k++)
                //                                         result[i] += vector[k] * matrix[i, k];
                //                                 });
            }

            return new Vector(result);
        }

        public static double Dot(Vector left, Vector right)
        {
            if (left.Dimension != right.Dimension)
                throw new ArgumentException("Dimensions must agree.");

            double result = 0;

            for (int i = 0; i < left.Dimension; i++)
                result += left[i]*right[i];

            return result;
        }

        public static Vector Concat(Vector left, Vector right)
        {
            var v = new Vector(left.Dimension + right.Dimension);

            v[0, left.Dimension] = left;
            v[left.Dimension, -1] = right;

            return v;
        }

        public static Vector Modulate(Vector left, Vector right)
        {
            if (!Engine.OperateOnSelf)
                left = left.Clone();

            return left.Modulate(right);
        }

        public static int NumberOfZeros(Vector v)
        {
            if (v == null)
                throw new ArgumentNullException("v");

            int result = 0;

            for (int i = 0; i < v.Dimension; i++)
                if (Engine.Sign(v[i]) == 0)
                    result++;

            return result;
        }

        public static int NumberOfPositives(Vector v)
        {
            if (v == null)
                throw new ArgumentNullException("v");

            int result = 0;

            for (int i = 0; i < v.Dimension; i++)
                if (Engine.Sign(v[i]) == 1)
                    result++;

            return result;
        }

        public static int NumberOfNegatives(Vector v)
        {
            if (v == null)
                throw new ArgumentNullException("v");

            int result = 0;

            for (int i = 0; i < v.Dimension; i++)
                if (Engine.Sign(v[i]) == -1)
                    result++;

            return result;
        }

        public static int FirstZero(Vector v)
        {
            if (v == null)
                throw new ArgumentNullException("v");

            for (int i = 0; i < v.Dimension; i++)
                if (Engine.Sign(v[i]) == 0)
                    return i;

            return -1;
        }

        public static int FirstPositive(Vector v)
        {
            if (v == null)
                throw new ArgumentNullException("v");

            for (int i = 0; i < v.Dimension; i++)
                if (Engine.Sign(v[i]) == 1)
                    return i;

            return -1;
        }

        public static int FirstNegative(Vector v)
        {
            if (v == null)
                throw new ArgumentNullException("v");

            for (int i = 0; i < v.Dimension; i++)
                if (Engine.Sign(v[i]) == -1)
                    return i;

            return -1;
        }

        public static Vector Signs(Vector v)
        {
            if (v == null)
                throw new ArgumentNullException("v");

            var result = new Vector(v.Dimension);

            for (int i = 0; i < v.Dimension; i++)
                result[i] = Engine.Sign(v[i]);

            return result;
        }

        public static Vector ComponentEquals(Vector left, Vector right)
        {
            var result = new Vector(left.Dimension);

            for (int i = 0; i < result.Dimension; i++)
                result[i] = Engine.Equals(left[i], right[i]) ? 1 : 0;

            return result;
        }

        public static Vector Complement(Vector v)
        {
            if (!Engine.OperateOnSelf)
                v = v.Clone();

            return v.Complement();
        }

        public static Vector Project(Vector v, Vector u)
        {
            return ((v*u)/(u*u))*u;
        }

        #endregion

        #region Operators

        public static Vector operator <(Vector left, Vector right)
        {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            var result = new Vector(left.Dimension);

            for (int i = 0; i < result.Dimension; i++)
                result[i] = Engine.Sign(left[i] - right[i]) < 0 ? 1 : 0;

            return result;
        }

        public static Vector operator >(Vector left, Vector right)
        {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            var result = new Vector(left.Dimension);

            for (int i = 0; i < result.Dimension; i++)
                result[i] = Engine.Sign(left[i] - right[i]) > 0 ? 1 : 0;

            return result;
        }

        public static Vector operator <=(Vector left, Vector right)
        {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            var result = new Vector(left.Dimension);

            for (int i = 0; i < result.Dimension; i++)
                result[i] = Engine.Sign(left[i] - right[i]) <= 0 ? 1 : 0;

            return result;
        }

        public static Vector operator >=(Vector left, Vector right)
        {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            var result = new Vector(left.Dimension);

            for (int i = 0; i < result.Dimension; i++)
                result[i] = Engine.Sign(left[i] - right[i]) >= 0 ? 1 : 0;

            return result;
        }

        public static Vector operator ~(Vector v)
        {
            return Complement(v);
        }

        public static Vector operator ^(Vector left, Vector right)
        {
            return Modulate(left, right);
        }

        public static Vector operator +(Vector left, Vector right)
        {
            return Add(left, right);
        }

        public static Builder operator |(Vector left, Vector right)
        {
            return new ConcatBuilder(new VectorBuilder(left), new VectorBuilder(right));
        }

        public static Builder operator |(Vector left, double right)
        {
            return new ConcatBuilder(new VectorBuilder(left), new DoubleBuilder(right));
        }

        public static Builder operator |(double left, Vector right)
        {
            return new ConcatBuilder(new DoubleBuilder(left), new VectorBuilder(right));
        }

        public static Vector operator +(Vector v)
        {
            return v.Clone();
        }

        public static Vector operator -(Vector left, Vector right)
        {
            return Subtract(left, right);
        }

        public static Vector operator -(Vector v)
        {
            return -1*v;
        }

        public static double operator *(Vector left, Vector right)
        {
            return Dot(left, right);
        }

        public static Vector operator *(Vector vector, Matrix matrix)
        {
            return Multiply(vector, matrix);
        }

        public static Vector operator *(Matrix matrix, Vector vector)
        {
            return Multiply(matrix, vector);
        }

        public static Vector operator *(Vector vector, double value)
        {
            return Multiply(vector, value);
        }

        public static Vector operator *(double value, Vector vector)
        {
            return Multiply(vector, value);
        }

        public static Vector operator /(Vector vector, double value)
        {
            return Divide(vector, value);
        }

        public static Matrix.Builder operator /(Vector top, Vector bottom)
        {
            return new Matrix.VerticalBuilder(new Matrix.VectorBuilder(top, true),
                                              new Matrix.VectorBuilder(bottom, true));
        }

        public static Vector operator ^(Vector left, int right)
        {
            var v = new Vector(left.Dimension*right);

            for (int i = 0; i < v.Dimension; i++)
                v[i] = left[i%left.Dimension];

            return v;
        }

        #endregion

        #region Nested Types

        #region Nested type: Builder

        public abstract class Builder
        {
            public readonly int Dimension;

            internal Builder(int dimension)
            {
                Dimension = dimension;
            }

            public Vector Build()
            {
                var v = new Vector(Dimension);
                Build(v, 0, Dimension);
                return v;
            }

            internal abstract void Build(Vector vector, int start, int end);

            public static implicit operator Vector(Builder b)
            {
                return b.Build();
            }

            public static Builder operator |(Builder left, Builder right)
            {
                return new ConcatBuilder(left, right);
            }

            public static Builder operator |(Builder left, double right)
            {
                return new ConcatBuilder(left, new DoubleBuilder(right));
            }

            public static Builder operator |(double left, Builder right)
            {
                return new ConcatBuilder(new DoubleBuilder(left), right);
            }

            public override string ToString()
            {
                return BuildString();
            }

            private string BuildString()
            {
                int width = Meassure();
                var s = new StringBuilder(width);
                BuildString(s);
                return s.ToString();
            }

            internal abstract void BuildString(StringBuilder sb);

            internal abstract int Meassure();
        }

        #endregion

        #region Nested type: ComponentWiseOperator

        public class ComponentWiseOperator
        {
            private readonly Vector vector;

            public ComponentWiseOperator(Vector vector)
            {
                if (vector == null)
                    throw new ArgumentNullException("vector");

                this.vector = vector;
            }

            public static ComponentWiseOperator operator *(ComponentWiseOperator left, Vector right)
            {
                return new ComponentWiseOperator(left.vector.ComponentMultiply(right));
            }

            public static ComponentWiseOperator operator /(ComponentWiseOperator left, Vector right)
            {
                return new ComponentWiseOperator(left.vector.ComponentDivide(right));
            }

            public static Vector operator +(ComponentWiseOperator left, Vector right)
            {
                return left.vector + right;
            }

            public static Vector operator -(ComponentWiseOperator left, Vector right)
            {
                return left.vector - right;
            }

            public static implicit operator Vector(ComponentWiseOperator op)
            {
                return op.vector;
            }
        }

        #endregion

        #region Nested type: ConcatBuilder

        private class ConcatBuilder : Builder
        {
            private readonly Builder left;
            private readonly Builder right;

            public ConcatBuilder(Builder left, Builder right)
                : base(left.Dimension + right.Dimension)
            {
                this.left = left;
                this.right = right;
            }

            internal override void Build(Vector vector, int start, int end)
            {
                left.Build(vector, start, start + left.Dimension);
                right.Build(vector, start + left.Dimension, end);
            }

            internal override void BuildString(StringBuilder sb)
            {
                left.BuildString(sb);
                sb.Append(" | ");
                right.BuildString(sb);
            }

            internal override int Meassure()
            {
                return left.Meassure() + right.Meassure() + 3;
            }
        }

        #endregion

        #region Nested type: DoubleBuilder

        private class DoubleBuilder : Builder
        {
            private readonly double value;

            public DoubleBuilder(double value)
                : base(1)
            {
                this.value = value;
            }

            internal override void Build(Vector vector, int start, int end)
            {
                vector[start] = value;
            }

            internal override void BuildString(StringBuilder sb)
            {
                sb.Append(value.ToString());
            }

            internal override int Meassure()
            {
                return value.ToString().Length;
            }
        }

        #endregion

        #region Nested type: VectorBuilder

        private class VectorBuilder : Builder
        {
            private readonly Vector v;

            public VectorBuilder(Vector v)
                : base(v.Dimension)
            {
                this.v = v;
            }

            internal override void Build(Vector vector, int start, int end)
            {
                vector[start, end] = v;
            }

            internal override void BuildString(StringBuilder sb)
            {
                sb.Append(v);
            }

            internal override int Meassure()
            {
                return v.ToString().Length;
            }
        }

        #endregion

        #endregion

        public Vector ComponentDivide(Vector other)
        {
            var result = new Vector(Dimension);

            for (int i = 0; i < Dimension; i++)
                result.Elements[i] = Elements[i]/other.Elements[i];

            return result;
        }

        public Vector ComponentMultiply(Vector other)
        {
            var result = new Vector(Dimension);

            for (int i = 0; i < Dimension; i++)
                result.Elements[i] = Elements[i]*other.Elements[i];

            return result;
        }

        public Vector Clamped(Vector min, Vector max)
        {
            if (min == null)
                throw new ArgumentNullException("min");

            if (max == null)
                throw new ArgumentNullException("max");

            if (min.Dimension != Dimension || max.Dimension != Dimension)
                throw new ArgumentException("The min and max vectors must have dimension {0}.".Formatted(Dimension));

            var values = new double[Dimension];

            for (int i = 0; i < Dimension; i++)
                values[i] = Math.Max(max[i], Math.Min(min[i], this[i]));

            return new Vector(values);
        }

        public double Sum()
        {
            return elements.Sum();
        }
    }
}
