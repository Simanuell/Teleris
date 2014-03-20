using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Device = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;




using SharpDX.Direct3D;

using Teleris;
using System.Drawing;


namespace Teleris.Core.Managers
{
    public class DeviceManager
    {


        #region Singleton Pattern
        private static DeviceManager instance = null;
        public static DeviceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DeviceManager();
                }
                return instance;
            }
        }
        #endregion

        private DeviceManager()
        {
            
            mMainWindowCaption = "D3D11222 Application";
            //mClientWidth = 3;
            //mClientHeight = 3;

        }

        //Initialize API
        public bool Init(RenderForm MainWindow)
        {

            if (MainWindow==null)
                return false;

            if (!InitDirect3D(MainWindow))
                return false;


            MainWindow.ClientSize = new Size(960, 600);
            MainWindow.Resize += (sender, args) =>
            {
                OnResize(MainWindow);
            };

            System.Console.WriteLine("Init!");
            return true;

        }

        //Initialize DX
        protected bool InitDirect3D(RenderForm MainWindow)
        {
            #region VSYNC
            bool VerticalSyncEnabled = true;

            // Create a DirectX graphics interface factory.
            var factory = new Factory();
            // Use the factory to create an adapter for the primary graphics interface (video card).
            var adapter = factory.GetAdapter(0);
            // Get the primary adapter output (monitor).
            var monitor = adapter.Outputs[0];
            // Get modes that fit the DXGI_FORMAT_R8G8B8A8_UNORM display format for the adapter output (monitor).
            var modes = monitor.GetDisplayModeList(Format.R8G8B8A8_UNorm, DisplayModeEnumerationFlags.Interlaced);
            // Now go through all the display modes and find the one that matches the screen width and height.
            // When a match is found store the the refresh rate for that monitor, if vertical sync is enabled. 
            // Otherwise we use maximum refresh rate.
            var rational = new Rational(0, 1);
            if (VerticalSyncEnabled)
            {
                foreach (var mode in modes)
                {
                    //Debug.WriteLine(mode.Width);
                    if (mode.Width == mClientWidth && mode.Height == mClientWidth)
                    {
                        rational = new Rational(mode.RefreshRate.Numerator, mode.RefreshRate.Denominator);
                        break;
                    }
                }
            }
            #endregion


            var SwapChaindesc = new SwapChainDescription()
            {

                BufferCount = 1,
                Usage = Usage.RenderTargetOutput,
                OutputHandle = MainWindow.Handle,
                IsWindowed = true,
                ModeDescription = new ModeDescription(MainWindow.Width, MainWindow.Height, rational, Format.R8G8B8A8_UNorm) ,
                SampleDescription = new SampleDescription(1,0),
                Flags = SwapChainFlags.AllowModeSwitch,
                SwapEffect = SwapEffect.Discard
            };

            try
            {
                Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, SwapChaindesc, out Device, out mSwapChain);
            }
            catch (Exception ex)
            {
                MessageBox.Show("SwapChain creation failed\n" + ex.Message + "\n" + ex.StackTrace, "Error");
                return false;
            }

            
            
            
            // create a view of our render target, which is the backbuffer of the swap chain we just created
            //RenderTargetView mRenderTargetView
            mResource = Resource.FromSwapChain<Texture2D>(mSwapChain, 0);
            mRenderTargetView = new RenderTargetView(Device, mResource);


            Context = Device.ImmediateContext;           
            mScreenViewport = new Viewport(0, 0, mClientWidth, mClientHeight);
            Context.OutputMerger.SetTargets(mRenderTargetView);
            Context.Rasterizer.SetViewports(mScreenViewport);

            /*
            RasterizerStateDescription RasterizerDesc = new RasterizerStateDescription()
            {
                FillMode = FillMode.Solid,
                CullMode = CullMode.Back,
                FrontCounterClockwise = False,
                DepthBias	0,
                SlopeScaledDepthBias = 0.0f,
                DepthBiasClamp = 0.0f,
                DepthClipEnable = TRUE,
                ScissorEnable = False,
                MultisampleEnable = False,
                AntialiasedLineEnable = False,
                ForcedSampleCount = 0

            };
            */




            OnResize(MainWindow);

            return true;

        }
    
        //Resize buffers
        public void OnResize(RenderForm MainWindow)
        {

            mClientWidth = MainWindow.ClientSize.Width;
            mClientHeight =  MainWindow.ClientSize.Height;

            if (mDepthStencilView != null)
                mDepthStencilView.Dispose();

            if (mDepthStencilBuffer != null)
                mDepthStencilBuffer.Dispose();

            if (mRenderTargetView != null)
                mRenderTargetView.Dispose();

            if (mResource != null)
                mResource.Dispose();


            mSwapChain.ResizeBuffers(1, mClientWidth, mClientHeight, Format.R8G8B8A8_UNorm, SwapChainFlags.AllowModeSwitch);


            mResource = Texture2D.FromSwapChain<Texture2D>(mSwapChain, 0);
            mRenderTargetView = new RenderTargetView(Device, mResource);


            Texture2DDescription depthStencilDesc = new Texture2DDescription()
            {
                ArraySize = 1,
                MipLevels = 1,
                Format = Format.D32_Float,
                Width = mClientWidth,
                Height = mClientHeight,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default

            };
            

            mDepthStencilBuffer = new Texture2D(Device, depthStencilDesc);
            mDepthStencilView = new DepthStencilView(Device, mDepthStencilBuffer);
  
            Context.OutputMerger.SetTargets(mDepthStencilView, mRenderTargetView);
            mScreenAspectRatio = (float)mClientWidth / (float)mClientHeight;
            mScreenViewport = new Viewport(0, 0, mClientWidth, mClientHeight);
            Context.Rasterizer.SetViewports(mScreenViewport);
            
        }



        #region DATA
        public string mMainWindowCaption;
        //RenderForm mMainWindow;
        //protected EngineTimer mTimer;

        public Device Device;
        public DeviceContext Context;
        public SwapChain mSwapChain;
        public Texture2D mDepthStencilBuffer;
        public Texture2D mResource;
        public RenderTargetView mRenderTargetView;
        public DepthStencilView mDepthStencilView;
        public Viewport mScreenViewport;
        public float mScreenAspectRatio;


        protected int mClientWidth;
        protected int mClientHeight;

        public bool mEnable4xMsaa = true;
        protected int m4xMsaaQuality = 2;
        #endregion


    }
}
