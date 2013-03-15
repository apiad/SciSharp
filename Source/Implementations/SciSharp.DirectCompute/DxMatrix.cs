using System;

using SlimDX.DXGI;
using SlimDX.Direct3D11;

using Buffer = SlimDX.Direct3D11.Buffer;


namespace SciSharp.DirectCompute
{
    public class DxMatrix : IDisposable
    {
        private readonly DxContext context;

        private Buffer data;
        private ShaderResourceView dataView;
        private UnorderedAccessView dataAccess;

        private readonly int cols;
        private readonly int rows;

        private readonly float[] values;
        private bool uploaded;
        private bool downloaded;

        public bool Disposed { get; set; }

        private DxMatrix(DxContext context, float[] values, int rows, int cols)
        {
            this.context = context;
            this.values = values;

            this.rows = rows;
            this.cols = cols;
        }

        public DxMatrix(DxContext context, double[,] values)
            : this(context, DoubleToFloat(values), values.GetLength(0), values.GetLength(1)) { }

        public DxMatrix(DxContext context, int rows, int cols)
            : this(context, new float[rows * cols], rows, cols) { }

        public void Upload()
        {
            if (uploaded)
                return;

            data = Helper.CreateBuffer(context.Device, values, ResourceUsage.Default);
            dataView = new ShaderResourceView(context.Device, data);
            dataAccess = new UnorderedAccessView(context.Device, data,
                                                 new UnorderedAccessViewDescription
                                                 {
                                                     Flags = UnorderedAccessViewBufferFlags.None,
                                                     Dimension = UnorderedAccessViewDimension.Buffer,
                                                     Format = Format.Unknown,
                                                     ElementCount = rows*cols,
                                                 });

            uploaded = true;
        }

        public void Download()
        {
            if (downloaded)
                return;

            Helper.GetData(context.Device, data, values);

            downloaded = true;
        }

        private static float[] DoubleToFloat(double[,] doubles)
        {
            if (doubles == null)
                throw new ArgumentNullException("doubles");

            int rows = doubles.GetLength(0);
            int cols = doubles.GetLength(1);

            var floats = new float[rows * cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    floats[i * cols + j] = (float)doubles[i, j];

            return floats;
        }

        public static DxMatrix Add(DxMatrix left, DxMatrix right)
        {
            if (left == null)
                throw new ArgumentNullException("left");

            if (right == null)
                throw new ArgumentNullException("right");

            if (left.rows != right.rows || left.cols != right.cols)
                throw new InvalidOperationException("The dimensions must agree.");

            if (left.context != right.context)
                throw new InvalidOperationException("Matrices must be created with the same GpuContext instance.");

            var sum = new DxMatrix(left.context, left.rows, left.cols);

            left.Upload();
            right.Upload();
            sum.Upload();

            var context = sum.context;

            context.SetShader(context.ShaderAddMatrix);
            context.SetTargets(sum.dataAccess, left.dataAccess, right.dataAccess);
            context.SetViewport(sum.cols, sum.rows);
            context.SetConstantData(0, new AddMatrixInfo { Rows = sum.rows, Cols = sum.cols });
            context.Dispatch();

            return sum;
        }

        public static DxMatrix Multiply(DxMatrix left, DxMatrix right)
        {
            if (left == null)
                throw new ArgumentNullException("left");

            if (right == null)
                throw new ArgumentNullException("right");

            if (left.cols != right.rows)
                throw new InvalidOperationException("The dimensions must agree.");

            if (left.context != right.context)
                throw new InvalidOperationException("Matrices must be created with the same GpuContext instance.");

            var mult = new DxMatrix(left.context, left.rows, right.cols);

            left.Upload();
            right.Upload();
            mult.Upload();

            var context = mult.context;

            context.SetShader(context.ShaderMultiplyMatrix);
            context.SetTargets(mult.dataAccess, left.dataAccess, right.dataAccess);
            context.SetViewport(mult.cols, mult.rows);

            for (int i = 0; i < left.cols; i++)
            {
                context.SetConstantData(0, new MultiplyMatrixInfo
                                           {
                                               Rows = mult.rows,
                                               Cols = mult.cols,
                                               Mid = left.cols,
                                               Index = i
                                           });
                context.Dispatch();
            }

            return mult;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                if (data != null)
                {
                    data.Dispose();
                    data = null;
                }
                if (dataView != null)
                {
                    dataView.Dispose();
                    dataView.Dispose();
                }
                if (dataAccess != null)
                {
                    dataAccess.Dispose();
                    dataAccess.Dispose();
                }
            }

            Disposed = true;
        }
    }
}
