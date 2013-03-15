using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;

using SlimDX.Direct3D11;
using SlimDX;
using SlimDX.D3DCompiler;
using SlimDX.DXGI;

using Buffer = SlimDX.Direct3D11.Buffer;
using Device = SlimDX.Direct3D11.Device;


namespace SciSharp.DirectCompute
{
    internal class Helper
    {
        public static Texture2D CreateDepthStencilBuffer(Device device, int width, int height)
        {
            return new Texture2D(device, new Texture2DDescription
            {
                BindFlags = BindFlags.DepthStencil,
                Format = Format.D32_Float,
                ArraySize = 1,
                MipLevels = 0,
                SampleDescription = new SampleDescription { Count = 1, Quality = 0 },
                CpuAccessFlags = CpuAccessFlags.None,
                Width = width,
                Height = height,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Default
            });
        }

        public static Buffer CreateBuffer<T>(Device device, T[] buffer, ResourceUsage usage) where T : struct
        {
            BufferDescription desc = new BufferDescription();
            desc.BindFlags = usage == ResourceUsage.Staging ? BindFlags.None : BindFlags.UnorderedAccess | BindFlags.ShaderResource;
            desc.OptionFlags = ResourceOptionFlags.StructuredBuffer;
            desc.CpuAccessFlags = usage == ResourceUsage.Default ? CpuAccessFlags.None : usage == ResourceUsage.Dynamic ? CpuAccessFlags.Write : CpuAccessFlags.Read;
            desc.Usage = usage;
            desc.SizeInBytes = buffer.Length * Marshal.SizeOf(typeof(T));
            desc.StructureByteStride = Marshal.SizeOf(typeof(T));

            DataStream ds = new DataStream(buffer, true, false);
            return new SlimDX.Direct3D11.Buffer(device, ds, desc);
        }

        public static Buffer CreateBuffer<T>(Device device, int bufferLength, ResourceUsage usage) where T : struct
        {
            BufferDescription desc = new BufferDescription();
            desc.BindFlags = usage == ResourceUsage.Staging ? BindFlags.None : BindFlags.UnorderedAccess | BindFlags.ShaderResource;
            desc.OptionFlags = ResourceOptionFlags.StructuredBuffer;
            desc.CpuAccessFlags = usage == ResourceUsage.Default ? CpuAccessFlags.None : usage == ResourceUsage.Dynamic ? CpuAccessFlags.Write : CpuAccessFlags.Read;
            desc.Usage = usage;
            desc.SizeInBytes = bufferLength * Marshal.SizeOf(typeof(T));
            desc.StructureByteStride = Marshal.SizeOf(typeof(T));

            return new SlimDX.Direct3D11.Buffer(device, desc);
        }

        public static Buffer CreateVertexBuffer<T>(Device device, T[] buffer, ResourceUsage usage) where T : struct
        {
            BufferDescription desc = new BufferDescription();
            desc.BindFlags = usage == ResourceUsage.Staging ? BindFlags.None : BindFlags.VertexBuffer;
            desc.OptionFlags = ResourceOptionFlags.None;
            desc.CpuAccessFlags = usage == ResourceUsage.Default ? CpuAccessFlags.None : usage == ResourceUsage.Dynamic ? CpuAccessFlags.Write : CpuAccessFlags.Read;
            desc.Usage = usage;
            desc.SizeInBytes = buffer.Length * Marshal.SizeOf(typeof(T));
            desc.StructureByteStride = Marshal.SizeOf(typeof(T));

            DataStream ds = new DataStream(buffer, true, false);
            return new SlimDX.Direct3D11.Buffer(device, ds, desc);
        }

        public static Buffer CreateIndexBuffer<T>(Device device, T[] buffer, ResourceUsage usage) where T : struct
        {
            BufferDescription desc = new BufferDescription();
            desc.BindFlags = usage == ResourceUsage.Staging ? BindFlags.None : BindFlags.IndexBuffer;
            desc.OptionFlags = ResourceOptionFlags.None;
            desc.CpuAccessFlags = usage == ResourceUsage.Default ? CpuAccessFlags.None : usage == ResourceUsage.Dynamic ? CpuAccessFlags.Write : CpuAccessFlags.Read;
            desc.Usage = usage;
            desc.SizeInBytes = buffer.Length * Marshal.SizeOf(typeof(T));
            desc.StructureByteStride = Marshal.SizeOf(typeof(T));

            DataStream ds = new DataStream(buffer, true, false);
            return new SlimDX.Direct3D11.Buffer(device, ds, desc);
        }

        public static Buffer CreateConstantBuffer<T>(Device device, T buffer) where T : struct
        {
            DataStream ds = new DataStream(new T[] { buffer }, true, true);
            ds.Write(buffer);
            ds.Position = 0;

            return new SlimDX.Direct3D11.Buffer(device, ds, new BufferDescription
            {
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = Marshal.SizeOf(typeof(T)),
                StructureByteStride = Marshal.SizeOf(typeof(T)),
                Usage = ResourceUsage.Dynamic
            });
        }

        public static void UpdateConstantBuffer<T>(Device device, Buffer buffer, T data) where T : struct
        {
            var map = device.ImmediateContext.MapSubresource(buffer, 0, Marshal.SizeOf(typeof(T)), MapMode.WriteDiscard, SlimDX.Direct3D11.MapFlags.None);
            map.Data.Write(data);
            device.ImmediateContext.UnmapSubresource(buffer, 0);
        }

        public static Texture2D CreateTexture2D<T>(Device device, T[,] entries, ResourceUsage usage) where T : struct
        {
            var format = Format.Unknown;

            switch (Marshal.SizeOf(typeof(T)))
            {
                case 1: format = Format.R8_UInt; break;
                case 2: format = Format.R16_UInt; break;
                case 4: format = typeof(T) == typeof(int) ? Format.R32_SInt : typeof(T) == typeof(uint) ? Format.R32_UInt : Format.R32_Float; break;
                case 8: format = Format.R32G32_Float; break;
                case 12: format = Format.R32G32B32_Float; break;
                case 16: format = Format.R32G32B32A32_Float; break;
                default: throw new ArgumentException();
            }

            return new Texture2D(device, new Texture2DDescription
            {
                BindFlags = BindFlags.ShaderResource | BindFlags.UnorderedAccess,
                ArraySize = 1,
                MipLevels = 1,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = format,
                Usage = usage,
                Width = entries.GetLength(1),
                Height = entries.GetLength(0),
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1, 0)
            }, new DataRectangle(Marshal.SizeOf(typeof(T)), new DataStream(entries.Cast<T>().ToArray(), true, true)));
        }

        public static Texture2D CreateTexture2D<T>(Device device, int width, int height, ResourceUsage usage) where T : struct
        {
            var format = Format.Unknown;

            switch (Marshal.SizeOf(typeof(T)))
            {
                case 1: format = Format.R8_UInt; break;
                case 2: format = Format.R16_UInt; break;
                case 4: format = typeof(T) == typeof(int) ? Format.R32_SInt : typeof(T) == typeof(uint) ? Format.R32_UInt : Format.R32_Float; break;
                case 8: format = Format.R32G32_Float; break;
                case 12: format = Format.R32G32B32_Float; break;
                case 16: format = Format.R32G32B32A32_Float; break;
                default: throw new ArgumentException();
            }

            return new Texture2D(device, new Texture2DDescription
            {
                BindFlags = BindFlags.ShaderResource | BindFlags.UnorderedAccess,
                ArraySize = 1,
                MipLevels = 1,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = format,
                Usage = usage,
                Width = width,
                Height = height,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1, 0)
            });
        }

        public static void GetData<T>(Device device, Buffer buffer, T[] data) where T : struct
        {
            SlimDX.Direct3D11.Buffer stagging = CreateBuffer<T>(device, data, ResourceUsage.Staging);
            device.ImmediateContext.CopyResource(buffer, stagging);
            var map = device.ImmediateContext.MapSubresource(stagging, 0, data.Length * Marshal.SizeOf(typeof(T)), MapMode.Read, SlimDX.Direct3D11.MapFlags.None);
            map.Data.ReadRange(data, 0, data.Length);
            device.ImmediateContext.UnmapSubresource(stagging, 0);
            stagging.Dispose();
        }

        public static VertexShader CompileVertexShader(Device device, string fileName, string entryPoint = "VS")
        {
            ShaderBytecode bytecode;
            return CompileVertexShader(device, fileName, out bytecode, entryPoint);
        }

        public static VertexShader CompileVertexShader(Device device, string fileName, out ShaderBytecode bytecode, string entryPoint = "VS")
        {
            return new VertexShader(device, bytecode = CompileBytecode(fileName, entryPoint, "vs_5_0"));
        }

        private static ShaderBytecode CompileBytecode(string fileName, string entryPoint, string version)
        {
            Logger.Log("Compiling '{0}'", fileName);

            string binFileName = Path.GetFileNameWithoutExtension(fileName) + ".bin";

            if (File.Exists(binFileName) && new FileInfo(fileName).LastWriteTime < new FileInfo(binFileName).LastWriteTime)
            {
                FileStream fs = new FileStream(binFileName, FileMode.Open);

                DataStream ds = new DataStream(fs.Length, true, true);

                fs.CopyTo(ds);

                fs.Close();

                ds.Position = 0;

                return new ShaderBytecode(ds);
            }
            else
            {
                var byteCode = ShaderBytecode.CompileFromFile(fileName, entryPoint, version, ShaderFlags.EnableStrictness, EffectFlags.SingleThreaded);

                FileStream fs = new FileStream(binFileName, FileMode.Create);

                byteCode.Data.CopyTo(fs);

                fs.Close();

                byteCode.Data.Position = 0;

                return byteCode;
            }
        }

        public static PixelShader CompilePixelShader(Device device, string fileName, string entryPoint = "PS")
        {
            ShaderBytecode bytecode;
            return CompilePixelShader(device, fileName, out bytecode, entryPoint);
        }

        public static PixelShader CompilePixelShader(Device device, string fileName, out ShaderBytecode bytecode, string entryPoint = "PS")
        {
            return new PixelShader(device, bytecode = CompileBytecode(fileName, entryPoint, "ps_5_0"));
        }
    }
}
