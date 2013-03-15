using System;
using System.Runtime.InteropServices;

using SlimDX;
using SlimDX.D3DCompiler;
using SlimDX.DXGI;
using SlimDX.Direct3D11;
using SlimDX.Windows;

using Buffer = SlimDX.Direct3D11.Buffer;
using Device = SlimDX.Direct3D11.Device;


namespace SciSharp.DirectCompute
{
    public class DxContext
    {
        private readonly Device device;
        private readonly DeviceContext context;
        
        private readonly InputLayout screenLayout;
        private readonly Buffer screenVertexes;

        #region Shaders

        private readonly VertexShader doNothing;
        private readonly PixelShader addMatrix;
        private readonly PixelShader multiplyMatrix;

        #endregion

        public DxContext()
        {
            var form = new RenderForm("Dummy Form - You should not be seeing this...");

            var desc = new SwapChainDescription
                       {
                           BufferCount = 1,
                           ModeDescription = new ModeDescription(form.ClientSize.Width, form.ClientSize.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm_SRGB),
                           IsWindowed = true,
                           OutputHandle = form.Handle,
                           SampleDescription = new SampleDescription(1, 0),
                           SwapEffect = SwapEffect.Discard,
                           Flags = SwapChainFlags.None,
                           Usage = Usage.RenderTargetOutput | Usage.UnorderedAccess
                       };

            SwapChain swapChain;
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, desc, out device, out swapChain);

            context = Device.ImmediateContext;

            #region Compile Shaders

            ShaderBytecode bytecode;
            doNothing = Helper.CompileVertexShader(device, "Shaders\\DoNothing.vs", out bytecode);
            addMatrix = Helper.CompilePixelShader(device, "Shaders\\AddMatrix.ps");
            multiplyMatrix = Helper.CompilePixelShader(device, "Shaders\\MultiplyMatrix.ps");

            #endregion

            screenLayout = new InputLayout(Device, bytecode, new[] {
                new InputElement ("POSITION", 0, Format.R32G32B32_Float, 0, 0)
            });

            screenVertexes = Helper.CreateVertexBuffer(device, new[] {
                                    new Vector3 (-1,1,1), new Vector3(-1,-1,1), new Vector3(1,-1,1),
                                    new Vector3 (-1,1,1), new Vector3(1,-1,1), new Vector3(1,1,1)
                                }, ResourceUsage.Default);
        }

        internal Device Device
        {
            get { return device; }
        }

        internal DeviceContext Context
        {
            get { return context; }
        }

        internal PixelShader ShaderAddMatrix
        {
            get { return addMatrix; }
        }

        internal PixelShader ShaderMultiplyMatrix
        {
            get { return multiplyMatrix; }
        }

        internal void SetShader(PixelShader shader)
        {
            if (shader == null) 
                throw new ArgumentNullException("shader");

            context.VertexShader.Set(doNothing);
            context.PixelShader.Set(shader);
        }

        internal void SetTargets(params UnorderedAccessView[] accessViews)
        {
            context.OutputMerger.SetTargets(null, null, 1, accessViews);
        }

        internal void SetViewport(int width, int height)
        {
            context.Rasterizer.SetViewports(new Viewport(0, 0, width, height));
        }

        internal void SetResources(params ShaderResourceView[] resourceViews)
        {
            context.PixelShader.SetShaderResources(resourceViews, 0, resourceViews.Length);
        }

        internal void SetConstantData<T>(int slot, T data)
            where T : struct 
        {
            Buffer buffer = Helper.CreateConstantBuffer<T>(device, data);
            context.PixelShader.SetConstantBuffer(buffer, slot);
        }

        internal void Dispatch()
        {
            context.Rasterizer.State = RasterizerState.FromDescription(Device, new RasterizerStateDescription
            {
                CullMode = CullMode.None,
                FillMode = FillMode.Solid
            });

            context.OutputMerger.DepthStencilState = DepthStencilState.FromDescription(Device, new DepthStencilStateDescription
            {
                DepthComparison = Comparison.LessEqual,
                DepthWriteMask = DepthWriteMask.All,
                IsDepthEnabled = false
            });

            context.InputAssembler.InputLayout = screenLayout;
            context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(screenVertexes, Marshal.SizeOf(typeof(Vector3)), 0));
            context.Draw(6, 0);
            context.Flush();
        }
    }
}