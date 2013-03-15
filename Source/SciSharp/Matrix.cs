using System;
using System.Linq;
using System.Text;


namespace SciSharp
{
    /// <summary>
    /// Represents a double precision matrix.
    /// </summary>
    public class Matrix
    {
        #region Fields

        /// <summary>
        /// Stores the real indices for the columns,
        /// to make the implementation of <see cref="SwapColumns"/> in O(1).
        /// </summary>
        private readonly int[] columns;

        /// <summary>
        /// Stores the elements of the matrix.
        /// </summary>
        private readonly double[,] elements;

        /// <summary>
        /// Stores the real indices for the rows,
        /// to make the implementation of <see cref="SwapRows"/> in O(1).
        /// </summary>
        private readonly int[] rows;

        /// <summary>
        /// Determines if the the matrix is transposed
        /// to make the implementation of <see cref="Transpose()"/> in O(1).
        /// </summary>
        private bool transposed;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class <see cref="Matrix"/>
        /// with the specified elements.
        /// </summary>
        /// <param name="rows">The number of rows in the matrix. Must be greater than 0.</param>
        /// <param name="columns">The number of columns in the matrix. Must be greater than 0.</param>
        /// <param name="elements">The values of the elements of the matrix.
        /// The length of the array must be (<paramref name="rows"/>*<paramref name="columns"/>).</param>
        public Matrix(int rows, int columns, params double[] elements)
        {
            if (elements == null)
                throw new ArgumentNullException("elements");

            if (elements.Length != rows*columns)
                throw new ArgumentException("Dimensions don't with the number of elements.");

            this.elements = new double[rows,columns];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    this.elements[i, j] = elements[i*columns + j];

            this.rows = DefaultArray(rows);
            this.columns = DefaultArray(columns);
        }

        /// <summary>
        /// Initializes a new instance of the class <see cref="Matrix"/> with 
        /// the specified dimensions and all elements set to 0.
        /// </summary>
        /// <param name="rows">The number of rows in the matrix. Must be greater than 0.</param>
        /// <param name="columns">The number of columns in the matrix. Must be greater than 0.</param>
        public Matrix(int rows, int columns)
            : this(new double[rows,columns]) {}

        /// <summary>
        /// Initializes a new instance of the class <see cref="Matrix"/> with 
        /// the specified elements.
        /// </summary>
        /// <param name="elements">An <see cref="Array"/> with the elements of the matrix.
        /// The matrix will have <code>Array.GetLength(0)</code> rows (<see cref="Rows"/>)
        /// and <code>Array.GetLength(1)</code> columns (<see cref="Columns"/>). Both dimensions
        /// must be greater than 0.
        /// </param>
        public Matrix(double[,] elements)
            : this(elements, DefaultArray(elements.GetLength(0)), DefaultArray(elements.GetLength(1))) {}

        /// <summary>
        /// Initializes a new instance of the class <see cref="Matrix"/> with 
        /// the specified elements.
        /// </summary>
        /// <param name="elements">An <see cref="Array"/> with the elements of the matrix.
        /// The matrix will have <code>Array.GetLength(0)</code> rows (<see cref="Rows"/>)
        /// and <code>Array.GetLength(1)</code> columns (<see cref="Columns"/>). Both dimensions
        /// must be greater than 0.
        /// </param>
        /// <param name="rows">Contains the values for the permuation of the rows.
        /// Its length must be <code>elements.GetLength(0)</code>.</param>
        /// <param name="columns">Contains the values for the permutation of the columns.
        /// Its length must be <code>elements.GetLength(1)</code>.</param>
        protected Matrix(double[,] elements, int[] rows, int[] columns)
        {
            if (elements == null)
                throw new ArgumentNullException("elements");

            if (rows == null)
                throw new ArgumentNullException("rows");

            if (columns == null)
                throw new ArgumentNullException("columns");

            if (rows.Length != elements.GetLength(0) || columns.Length != elements.GetLength(1))
                throw new ArgumentException("The arrays dimensions don't match.");

            this.elements = elements;
            this.rows = rows;
            this.columns = columns;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the number of rows in the matrix.
        /// </summary>
        public int Rows
        {
            get { return transposed ? columns.Length : rows.Length; }
        }

        /// <summary>
        /// Returns the number of columns in the matrix.
        /// </summary>
        public int Columns
        {
            get { return transposed ? rows.Length : columns.Length; }
        }

        public double this[int row, int column]
        {
            get
            {
                if (row < 0)
                    row += Rows;
                if (column < 0)
                    column += Columns;

                double value = transposed ? elements[rows[column], columns[row]] : elements[rows[row], columns[column]];

                Debug.ThrowOnNaN(value);

                return value;
            }
            set
            {
                if (row < 0)
                    row += Rows;
                if (column < 0)
                    column += Columns;

                Debug.ThrowOnNaN(value);

                if (transposed)
                    elements[rows[column], columns[row]] = value;
                else
                    elements[rows[row], columns[column]] = value;
            }
        }

        public Vector this[Wildcard w, int column]
        {
            get { return Column(column); }
            set
            {
                if (value.Dimension != Rows)
                    throw new ArgumentException("Sizes don't match.");

                for (int i = 0; i < Rows; i++)
                    this[i, column] = value[i];
            }
        }

        public Vector this[int row, Wildcard w]
        {
            get { return Row(row); }
            set
            {
                if (value.Dimension != Columns)
                    throw new ArgumentException("Sizes don't match.");

                for (int i = 0; i < Columns; i++)
                    this[row, i] = value[i];
            }
        }

        public Matrix this[int startRow, int endRow, int startColumn, int endColumn]
        {
            get
            {
                if (startColumn < 0)
                    startColumn += Columns;
                if (endColumn < 0)
                    endColumn += Columns + 1;
                if (startRow < 0)
                    startRow += Rows;
                if (endRow < 0)
                    endRow += Rows + 1;

                var m = new Matrix(endRow - startRow, endColumn - startColumn);

                for (int i = startRow; i < endRow; i++)
                    for (int j = startColumn; j < endColumn; j++)
                    {
                        Debug.ThrowOnNaN(this[i, j]);

                        m[i - startRow, j - startColumn] = this[i, j];
                    }

                return m;
            }
            set
            {
                if (startColumn < 0)
                    startColumn += Columns;
                if (endColumn < 0)
                    endColumn += Columns + 1;
                if (startRow < 0)
                    startRow += Rows;
                if (endRow < 0)
                    endRow += Rows + 1;

                for (int i = startRow; i < endRow; i++)
                    for (int j = startColumn; j < endColumn; j++)
                    {
                        Debug.ThrowOnNaN(value[i - startRow, j - startColumn]);

                        this[i, j] = value[i - startRow, j - startColumn];
                    }
            }
        }

        public bool Squared
        {
            get { return Rows == Columns; }
        }

        public double[,] Elements
        {
            get { return (double[,]) elements.Clone(); }
        }

        public ComponentWiseOperator _
        {
            get { return new ComponentWiseOperator(this); }
        }

        #endregion

        #region Instance Methods

        #region ICloneable<Matrix> Members

        public Matrix Clone()
        {
            var newElements = new double[Rows,Columns];

            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    newElements[i, j] = this[i, j];

            return new Matrix(newElements);
        }

        #endregion

        #region IEquatable<Matrix> Members

        public bool Equals(Matrix other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            if (Rows != other.Rows || Columns != other.Columns)
                return false;

            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    if (this[i, j] - other[i, j] > Engine.Epsilon)
                        return false;

            return true;
        }

        #endregion

        public Matrix SwapRows(int i, int j)
        {
            if (transposed)
            {
                int temp = columns[i];
                columns[i] = columns[j];
                columns[j] = temp;
            }
            else
            {
                int temp = rows[i];
                rows[i] = rows[j];
                rows[j] = temp;
            }

            return this;
        }

        public Matrix SwapColumns(int i, int j)
        {
            if (!transposed)
            {
                int temp = columns[i];
                columns[i] = columns[j];
                columns[j] = temp;
            }
            else
            {
                int temp = rows[i];
                rows[i] = rows[j];
                rows[j] = temp;
            }

            return this;
        }

        public Matrix Add(Matrix other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            if (other.Rows != Rows || other.Columns != Columns)
                throw new ArgumentException("Dimensions don't match.");

            if (!Engine.UseParallelOptimizations)
            {
                // Secuencial

                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Columns; j++)
                        this[i, j] += other[i, j];
            }
            else
            {
                // Paralelo

                //Parallel.For(0, Rows, i =>
                //                          {
                //                              for (int j = 0; j < Columns; j++)
                //                                  this[i, j] += other[i, j];
                //                          });
            }

            return this;
        }

        public Matrix Subtract(Matrix other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            if (other.Rows != Rows || other.Columns != Columns)
                throw new ArgumentException("Dimensions don't match.");

            if (!Engine.UseParallelOptimizations)
            {
                // Secuencial

                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Columns; j++)
                        this[i, j] -= other[i, j];
            }
            else
            {
                // Paralelo 

                //Parallel.For(0, Rows, i =>
                //                          {
                //                              for (int j = 0; j < Columns; j++)
                //                                  this[i, j] -= other[i, j];
                //                          });
            }

            return this;
        }

        public Matrix Multiply(double scalar)
        {
            Debug.ThrowOnNaN(scalar);

            if (!Engine.UseParallelOptimizations)
            {
                // Secuencial

                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Columns; j++)
                        this[i, j] *= scalar;
            }
            else
            {
                // Paralelo 

                //Parallel.For(0, Rows, i =>
                //                          {
                //                              for (int j = 0; j < Columns; j++)
                //                                  this[i, j] *= scalar;
                //                          });
            }

            return this;
        }

        public Matrix Divide(double scalar)
        {
            Debug.ThrowOnNaN(scalar);

            if (!Engine.UseParallelOptimizations)
            {
                // Secuencial

                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Columns; j++)
                        this[i, j] /= scalar;
            }
            else
            {
                // Paralelo

                //Parallel.For(0, Rows, i =>
                //                          {
                //                              for (int j = 0; j < Columns; j++)
                //                                  this[i, j] /= scalar;
                //                          });
            }

            return this;
        }

        public Matrix ComponentMultiply(Matrix other)
        {
            if (Rows != other.Rows || Columns != other.Columns)
                throw new InvalidOperationException("Dimensions must agree.");

            var values = new double[Rows,Columns];

            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    values[i, j] = this[i, j]*other[i, j];

            return new Matrix(values);
        }

        public Matrix ComponentDivide(Matrix other)
        {
            if (Rows != other.Rows || Columns != other.Columns)
                throw new InvalidOperationException("Dimensions must agree.");

            var values = new double[Rows,Columns];

            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    values[i, j] = this[i, j]/other[i, j];

            return new Matrix(values);
        }

        public Matrix Transpose()
        {
            transposed = !transposed;
            return this;
        }

        public Matrix RowPermutations()
        {
            var newElements = new double[Rows,Rows];

            if (!transposed)
                for (int i = 0; i < rows.Length; i++)
                    newElements[i, rows[i]] = 1;
            else
                for (int i = 0; i < columns.Length; i++)
                    newElements[columns[i], i] = 1;

            return new Matrix(newElements);
        }

        public Matrix ColumnPermutations()
        {
            var newElements = new double[Columns,Columns];

            if (transposed)
                for (int i = 0; i < rows.Length; i++)
                    newElements[i, rows[i]] = 1;
            else
                for (int i = 0; i < columns.Length; i++)
                    newElements[columns[i], i] = 1;

            return new Matrix(newElements);
        }

        public Matrix Diagonal()
        {
            if (!Squared)
                throw new InvalidOperationException("This matrix is not squared.");

            var newElements = new double[Rows,Columns];

            for (int k = 0; k < Rows; k++)
                newElements[k, k] = this[k, k];

            return new Matrix(newElements);
        }

        public Matrix UpperDiagonal()
        {
            var newElements = new double[Rows,Columns];

            for (int i = 0; i < Rows; i++)
                for (int j = i; j < Columns; j++)
                    newElements[i, j] = this[i, j];

            return new Matrix(newElements);
        }

        public Matrix LowerDiagonal()
        {
            var newElements = new double[Rows,Columns];

            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < i; j++)
                    newElements[i, j] = this[i, j];

            for (int k = 0; k < Rows; k++)
                newElements[k, k] = 1;

            return new Matrix(newElements);
        }

        public Matrix LowerSymetric()
        {
            if (!Squared)
                throw new InvalidOperationException("Matrix must be squared.");

            var newElements = new double[Rows, Rows];

            for (int i = 0; i < Rows; i++)
                for (int j = 0; j <= i; j++)
                    newElements[i, j] = newElements[j,i] = this[i, j];

            return new Matrix(newElements);
        }

        public Vector Row(int row)
        {
            var result = new double[Columns];

            for (int i = 0; i < result.Length; i++)
                result[i] = this[row, i];

            return new Vector(result);
        }

        public Vector Column(int column)
        {
            var result = new double[Rows];

            for (int i = 0; i < result.Length; i++)
                result[i] = this[i, column];

            return new Vector(result);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof (Matrix))
                return false;
            return Equals((Matrix) obj);
        }

        public override int GetHashCode()
        {
            return (elements != null ? elements.GetHashCode() : 0);
        }

        public string ToString(int digits)
        {
            digits = Math.Min(digits, 15);

            var str = new StringBuilder();

            for (int i = 0; i < Rows; i++)
            {
                str.Append("( ");

                for (int j = 0; j < Columns; j++)
                    str.AppendFormat(j == Columns - 1 ? "{0}" : "{0}, ", Math.Round(this[i, j], digits));

                str.Append(" )");

                str.AppendLine();
            }

            return str.ToString();
        }

        public override string ToString()
        {
            return ToString((int) (Engine.Epsilon > 0 ? -Math.Log10(Engine.Epsilon) : 15));
        }

        #endregion

        #region Static Methods

        private static int[] DefaultArray(int length)
        {
            var result = new int[length];

            for (int i = 0; i < length; i++)
                result[i] = i;

            return result;
        }

        public static Matrix Add(Matrix left, Matrix right)
        {
            if (left == null)
                throw new ArgumentNullException("left");

            if (!Engine.OperateOnSelf)
                left = left.Clone();

            return left.Add(right);
        }

        public static Matrix Subtract(Matrix left, Matrix right)
        {
            if (left == null)
                throw new ArgumentNullException("left");

            if (!Engine.OperateOnSelf)
                left = left.Clone();

            return left.Subtract(right);
        }

        public static Matrix Multiply(Matrix matrix, double scalar)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix");

            if (!Engine.OperateOnSelf)
                matrix = matrix.Clone();

            return matrix.Multiply(scalar);
        }

        public static Matrix Multiply(Matrix left, Matrix right)
        {
            if (left == null)
                throw new ArgumentNullException("left");

            if (right == null)
                throw new ArgumentNullException("right");

            if (left.Columns != right.Rows)
                throw new ArgumentException("Dimensions don't match.");

            var elements = new double[left.Rows,right.Columns];

            if (!Engine.UseParallelOptimizations)
            {
                // Secuencial

                for (int i = 0; i < left.Rows; i++)
                    for (int j = 0; j < right.Columns; j++)
                        for (int k = 0; k < left.Columns; k++)
                            elements[i, j] += left[i, k]*right[k, j];
            }
            else
            {
                // Paralelo

                //Parallel.For(0, left.Rows, i =>
                //                               {
                //                                   for (int j = 0; j < right.Columns; j++)
                //                                       for (int k = 0; k < left.Columns; k++)
                //                                           elements[i, j] += left[i, k] * right[k, j];
                //                               });
            }

            return new Matrix(elements);
        }

        public static Matrix Divide(Matrix matrix, double scalar)
        {
            if (!Engine.OperateOnSelf)
                matrix = matrix.Clone();

            return matrix.Divide(scalar);
        }

        public static Matrix Transpose(Matrix matrix)
        {
            if (!Engine.OperateOnSelf)
                matrix = matrix.Clone();

            return matrix.Transpose();
        }

        public static Matrix OrthoNormalize(Matrix matrix)
        {
            Wildcard _ = Wildcard.Get;

            var vectors = new Vector[matrix.Columns];

            for (int i = 0; i < matrix.Columns; i++)
                vectors[i] = matrix[_, i];

            for (int j = 0; j < vectors.Length; j++)
            {
                for (int i = 0; i < j; i++)
                    vectors[j].Subtract(Vector.Project(vectors[i], vectors[j]));

                vectors[j].Normalize();
            }

            var m = new Matrix(matrix.Rows, matrix.Columns);

            for (int i = 0; i < m.Columns; i++)
                m[_, i] = vectors[i];

            return m;
        }

        #endregion

        #region Operators

        public static Matrix operator +(Matrix left, Matrix right)
        {
            return Add(left, right);
        }

        public static Matrix operator +(Matrix m)
        {
            return 1*m;
        }

        public static Matrix operator -(Matrix left, Matrix right)
        {
            return Subtract(left, right);
        }

        public static Matrix operator -(Matrix m)
        {
            return -1*m;
        }

        public static Matrix operator *(Matrix left, Matrix right)
        {
            return Multiply(left, right);
        }

        public static Matrix operator *(Matrix matrix, double scalar)
        {
            return Multiply(matrix, scalar);
        }

        public static Matrix operator *(double scalar, Matrix matrix)
        {
            return Multiply(matrix, scalar);
        }

        public static Matrix operator /(Matrix matrix, double scalar)
        {
            return Divide(matrix, scalar);
        }

        public static bool operator ==(Matrix left, Matrix right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Matrix left, Matrix right)
        {
            return !Equals(left, right);
        }

        public static Builder operator |(Matrix left, Matrix right)
        {
            if (!left.Rows.Equals(right.Rows))
                throw new ArgumentException(
                    String.Format("The values of 'left.Rows' ({0}) and 'right.Rows' ({1}) must be equal.",
                                  left.Rows, right.Rows));

            return new HorizontalBuilder(new MatrixBuilder(left), new MatrixBuilder(right));
        }

        public static Builder operator |(Matrix left, Vector right)
        {
            return left | new VectorBuilder(right, false);
        }

        public static Builder operator |(Vector left, Matrix right)
        {
            return new VectorBuilder(left, false) | right;
        }

        public static Builder operator /(Matrix top, Matrix bottom)
        {
            if (!top.Columns.Equals(bottom.Columns))
                throw new ArgumentException(
                    String.Format("The values of 'top.Columns' ({0}) and 'bottom.Columns' ({1}) must be equal.",
                                  top.Columns, bottom.Columns));

            return new VerticalBuilder(new MatrixBuilder(top), new MatrixBuilder(bottom));
        }

        public static Builder operator /(Matrix top, Vector bottom)
        {
            return new MatrixBuilder(top)/new VectorBuilder(bottom, true);
        }

        public static Builder operator /(Vector top, Matrix bottom)
        {
            return new VectorBuilder(top, true)/new MatrixBuilder(bottom);
        }

        #endregion

        #region Nested Types

        #region Nested type: Builder

        public abstract class Builder
        {
            public readonly int Columns;
            public readonly int Rows;

            internal Builder(int rows, int columns)
            {
                Rows = rows;
                Columns = columns;
            }

            public Matrix Build()
            {
                var m = new Matrix(Rows, Columns);
                Build(m, 0, Rows, 0, Columns);
                return m;
            }

            internal abstract void Build(Matrix matrix, int startRow, int endRow, int startCol, int endCol);

            public static implicit operator Matrix(Builder builder)
            {
                return builder.Build();
            }

            public static Builder operator |(Builder left, Builder right)
            {
                if (!left.Rows.Equals(right.Rows))
                    throw new ArgumentException(
                        String.Format("The values of 'left.Rows' ({0}) and 'right.Rows' ({1}) must be equal.",
                                      left.Rows, right.Rows));

                return new HorizontalBuilder(left, right);
            }

            public static Builder operator |(Builder left, Matrix right)
            {
                return left | new MatrixBuilder(right);
            }

            public static Builder operator |(Matrix left, Builder right)
            {
                return new MatrixBuilder(left) | right;
            }

            public static Builder operator |(Builder left, Vector right)
            {
                return left | new VectorBuilder(right, false);
            }

            public static Builder operator |(Vector left, Builder right)
            {
                return new VectorBuilder(left, false) | right;
            }

            public static Builder operator /(Builder top, Builder bottom)
            {
                if (!top.Columns.Equals(bottom.Columns))
                    throw new ArgumentException(
                        String.Format("The values of 'top.Columns' ({0}) and 'bottom.Columns' ({1}) must be equal.",
                                      top.Columns, bottom.Columns));

                return new VerticalBuilder(top, bottom);
            }

            public static Builder operator /(Builder top, Vector bottom)
            {
                return top/new VectorBuilder(bottom, true);
            }

            public static Builder operator /(Vector top, Builder bottom)
            {
                return new VectorBuilder(top, true)/bottom;
            }

            public override string ToString()
            {
                return BuildString();
            }

            private string BuildString()
            {
                int width, height;
                Meassure(out width, out height);
                var str = new char[height,1 + width];

                for (int i = 0; i < height - 1; i++)
                    str[i, width] = '\n';

                BuildString(str, 0, height, 0, width);
                var s = new StringBuilder((width + 1)*height);

                foreach (char c in str)
                    s.Append(c);

                return s.ToString();
            }

            internal abstract void BuildString(char[,] str, int startRow, int endRow, int startCol, int endCol);

            internal abstract void Meassure(out int width, out int height);

            protected void CopyStrings(char[,] str, int startRow, int endRow, int startCol, int endCol,
                                       params string[] strings)
            {
                if (!strings.Length.Equals(endRow - startRow))
                    throw new ArgumentException(
                        String.Format(
                            "The values of 'strings.Length' ({0}) and 'endRow - startRow' ({1}) must be equal.",
                            strings.Length, endRow - startRow));

                for (int i = 0; i < strings.Length; i++)
                {
                    string s = strings[i];

                    for (int j = 0; j < s.Length; j++)
                        str[startRow + i, startCol + j] = s[j];
                }
            }
        }

        #endregion

        #region Nested type: ComponentWiseOperator

        public class ComponentWiseOperator
        {
            private readonly Matrix matrix;

            internal ComponentWiseOperator(Matrix matrix)
            {
                if (matrix == null)
                    throw new ArgumentNullException("matrix");

                this.matrix = matrix;
            }

            public static ComponentWiseOperator operator *(ComponentWiseOperator left, Matrix right)
            {
                return new ComponentWiseOperator(left.matrix.ComponentMultiply(right));
            }

            public static ComponentWiseOperator operator /(ComponentWiseOperator left, Matrix right)
            {
                return new ComponentWiseOperator(left.matrix.ComponentDivide(right));
            }

            public static implicit operator Matrix(ComponentWiseOperator op)
            {
                return op.matrix;
            }
        }

        #endregion

        #region Nested type: HorizontalBuilder

        internal class HorizontalBuilder : Builder
        {
            private readonly Builder left;
            private readonly Builder right;

            public HorizontalBuilder(Builder left, Builder right)
                : base(left.Rows, left.Columns + right.Columns)
            {
                this.left = left;
                this.right = right;
            }

            internal override void Build(Matrix matrix, int startRow, int endRow, int startCol, int endCol)
            {
                left.Build(matrix, startRow, endRow, startCol, startCol + left.Columns);
                right.Build(matrix, startRow, endRow, startCol + left.Columns, endCol);
            }

            internal override void BuildString(char[,] str, int startRow, int endRow, int startCol, int endCol)
            {
                int leftW, leftH;
                int rightW, rightH;

                left.Meassure(out leftW, out leftH);
                right.Meassure(out rightW, out rightH);

                left.BuildString(str, startRow, endRow, startCol, startCol + leftW);

                for (int i = 0; i < Math.Max(leftH, rightH); i++)
                    str[startRow + i, startCol + leftW + 1] = '|';

                right.BuildString(str, startRow, endRow, startCol + leftW + 3, endCol);
            }

            internal override void Meassure(out int width, out int height)
            {
                int leftW, leftH;
                int rightW, rightH;

                left.Meassure(out leftW, out leftH);
                right.Meassure(out rightW, out rightH);

                width = leftW + rightW + 3;
                height = Math.Max(leftH, rightH);
            }
        }

        #endregion

        #region Nested type: MatrixBuilder

        internal class MatrixBuilder : Builder
        {
            private readonly Matrix m;

            public MatrixBuilder(Matrix m)
                : base(m.Rows, m.Columns)
            {
                this.m = m;
            }

            internal override void Build(Matrix matrix, int startRow, int endRow, int startCol, int endCol)
            {
                matrix[startRow, endRow, startCol, endCol] = m;
            }

            internal override void BuildString(char[,] str, int startRow, int endRow, int startCol, int endCol)
            {
                string[] strings = m.ToString().Lines();
                CopyStrings(str, startRow, endRow, startCol, endCol, strings);
            }

            internal override void Meassure(out int width, out int height)
            {
                string[] strings = m.ToString().Lines();
                width = strings.Max(s => s.Length);
                height = strings.Length;
            }
        }

        #endregion

        #region Nested type: VectorBuilder

        internal class VectorBuilder : Builder
        {
            private readonly bool horizontal;
            private readonly Vector v;

            public VectorBuilder(Vector v, bool horizontal)
                : base(horizontal ? 1 : v.Dimension, horizontal ? v.Dimension : 1)
            {
                this.v = v;
                this.horizontal = horizontal;
            }

            internal override void Build(Matrix matrix, int startRow, int endRow, int startCol, int endCol)
            {
                matrix[startRow, endRow, startCol, endCol] = horizontal ? v.AsRow() : v.AsColumn();
            }

            internal override void BuildString(char[,] str, int startRow, int endRow, int startCol, int endCol)
            {
                if (horizontal)
                    CopyStrings(str, startRow, endRow, startCol, endCol, v.ToString());
                else
                {
                    string[] strings = v.AsColumn().ToString().Lines();
                    CopyStrings(str, startRow, endRow, startCol, endCol, strings);
                }
            }

            internal override void Meassure(out int width, out int height)
            {
                if (horizontal)
                {
                    width = v.ToString().Length;
                    height = 1;
                }
                else
                {
                    string[] strings = v.AsColumn().ToString().Lines();
                    width = strings.Max(s => s.Length);
                    height = strings.Length;
                }
            }
        }

        #endregion

        #region Nested type: VerticalBuilder

        internal class VerticalBuilder : Builder
        {
            private readonly Builder bottom;
            private readonly Builder top;

            public VerticalBuilder(Builder top, Builder bottom)
                : base(top.Rows + bottom.Rows, top.Columns)
            {
                if (!top.Columns.Equals(bottom.Columns))
                    throw new ArgumentException(
                        String.Format("The values of 'top.Columns' ({0}) and 'bottom.Columns' ({1}) must be equal.",
                                      top.Columns, bottom.Columns));

                this.top = top;
                this.bottom = bottom;
            }

            internal override void Build(Matrix matrix, int startRow, int endRow, int startCol, int endCol)
            {
                top.Build(matrix, startRow, startRow + top.Rows, startCol, endCol);
                bottom.Build(matrix, startRow + top.Rows, endRow, startCol, endCol);
            }

            internal override void BuildString(char[,] str, int startRow, int endRow, int startCol, int endCol)
            {
                int topW, topH;
                int botW, botH;

                top.Meassure(out topW, out topH);
                bottom.Meassure(out botW, out botH);

                top.BuildString(str, startRow, startRow + topH, startCol, endCol);

                for (int i = 0; i < Math.Max(topW, botW); i++)
                    str[startRow + topH, startCol + i] = '-';

                bottom.BuildString(str, startRow + topH + 1, endRow, startCol, endCol);
            }

            internal override void Meassure(out int width, out int height)
            {
                int topW, topH;
                int botW, botH;

                top.Meassure(out topW, out topH);
                bottom.Meassure(out botW, out botH);

                width = Math.Max(topW, botW);
                height = topH + botH + 1;
            }
        }

        #endregion

        #endregion

        public void Normalize()
        {
            var lengths = new double[Rows];
            var _ = Wildcard.Get;

            for (int r = 0; r < lengths.Length; r++)
                lengths[r] = this[r, _].Length;

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    this[r, c] /= lengths[r];
        }
    }
}
