using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Device = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;
using System;
using SharpDX.Direct3D;




namespace Teleris
{
    
    class D3DAppBase
    {

         [DllImport("user32.dll")]
         static extern bool SetWindowText(System.IntPtr hWnd, string windowName);

        
        //Constructor
        public D3DAppBase()
        {

        
            mMainWindowCaption = "D3D11 Application";
	        //md3dDriverType = DriverType.Hardware;
	        mClientWidth = 800;
	        mClientHeight = 600;
	        mEnable4xMsaa = false;
	        mMainWindow = null;
	        mAppPaused = false;
	        mMinimized = false;
	        mMaximized = false;
	        mResizing = false;
	        m4xMsaaQuality = 0;

            mTimer = new EngineTimer();
 
	        md3dDevice = null;
	        md3dImmediateContext = null;
	        mSwapChain = null;
	        mDepthStencilBuffer = null;
	        mRenderTargetView = null;
            mDepthStencilView = null;
        
        }
        
        //Run Msg Loop
        public virtual void Run()
        {
        

            mTimer.Reset();
            MsgProcedure();
            RenderLoop.Run(mMainWindow, () =>
            {

                mTimer.Tick();
                
                if (!mAppPaused) 
                {
                    
                    CalculateFrameStats();
                    UpdateScene(mTimer.DeltaTime());
                    DrawScene();
                          
                }
             

            });

        }

        public virtual void MsgProcedure()
        {

            mMainWindow.Activated += new System.EventHandler(this.Active);
            mMainWindow.Deactivate += new System.EventHandler(this.Inactive);
            mMainWindow.ResizeBegin += new System.EventHandler(this.ResizeBegin);
            mMainWindow.ResizeEnd += new System.EventHandler(this.ResizeEnd);


 
        }

        //Initialize API
        public virtual bool Init() 
        {

            if (!InitMainWindow())
                return false;

            if (!InitDirect3D())
                return false;

            return true;

        }
               
        //Update Scene
        public virtual void UpdateScene(float dt)
        {

        }
        
        //Draw Scene
        public virtual void DrawScene() { }

        //Create main window
        protected bool InitMainWindow()
        {

            mMainWindow = new RenderForm(mMainWindowCaption);
            

            return true;
        }
        
        float AspectRatio()
        {
            return mMainWindow.ClientSize.Width / mMainWindow.ClientSize.Height;
        }
        
        protected bool InitDirect3D()
        {


            var description = new SwapChainDescription()
            {
                BufferCount = 1,
                Usage = Usage.RenderTargetOutput,
                OutputHandle = mMainWindow.Handle,
                IsWindowed = true,
                ModeDescription = new ModeDescription(0, 0, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                SampleDescription = new SampleDescription(1, 0),
                Flags = SwapChainFlags.AllowModeSwitch,
                SwapEffect = SwapEffect.Discard
            };


            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, description, out md3dDevice, out mSwapChain);

            // create a view of our render target, which is the backbuffer of the swap chain we just created
            //RenderTargetView mRenderTargetView;
             mResource = Resource.FromSwapChain<Texture2D>(mSwapChain, 0);
             mRenderTargetView = new RenderTargetView(md3dDevice, mResource);

            // setting a viewport is required if you want to actually see anything
            md3dImmediateContext = md3dDevice.ImmediateContext;
            mScreenViewport = new Viewport(0, 0, mClientWidth, mClientHeight);
            md3dImmediateContext.OutputMerger.SetTargets(mRenderTargetView);
            md3dImmediateContext.Rasterizer.SetViewports(mScreenViewport);


            OnResize();

            return true;
            
        }

        //Resize buffers
        public virtual void OnResize()
        {

            
            if (mDepthStencilView != null)
            mDepthStencilView.Dispose();

            if (mDepthStencilBuffer != null)
            mDepthStencilBuffer.Dispose();

            if (mRenderTargetView != null)
            mRenderTargetView.Dispose();

            if (mResource != null)
            mResource.Dispose();
            
            //update aspectratio

            mSwapChain.ResizeBuffers(1, mClientWidth, mClientHeight, Format.R8G8B8A8_UNorm, SwapChainFlags.AllowModeSwitch);
  
            
            mResource = Texture2D.FromSwapChain<Texture2D>(mSwapChain, 0);
            mRenderTargetView = new RenderTargetView(md3dDevice, mResource);
            
            
            Texture2DDescription depthStencilDesc = new Texture2DDescription(){
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
            

            mDepthStencilBuffer = new Texture2D(md3dDevice, depthStencilDesc);  
            mDepthStencilView = new DepthStencilView(md3dDevice, mDepthStencilBuffer);

            md3dImmediateContext.OutputMerger.SetTargets(mDepthStencilView, mRenderTargetView);
            mScreenViewport = new Viewport(0, 0, mClientWidth, mClientHeight);
            //System.Console.WriteLine(mDepthStencilBuffer.CreationTime);
        }

        protected virtual void CalculateFrameStats()
        {
        
        	// Code computes the average frames per second, and also the 
	        // average time it takes to render one frame.  These stats 
	        // are appended to the window caption bar.

	        int frameCnt = 0;
	        float timeElapsed = 0.0f;

	        frameCnt++;
            //System.Console.WriteLine(mTimer.TotalTime());
	        // Compute averages over one second period.
            if ((mTimer.TotalTime() - timeElapsed) >= 1.0f)
            {
                //System.Diagnostics.Trace.WriteLine(mTimer.TotalTime() - timeElapsed);
                float fps = (float)frameCnt; // fps = frameCnt / 1
                float mspf = 1000.0f / fps;
                //System.Diagnostics.Trace.WriteLine(fps);

                SetWindowText((System.IntPtr)mMainWindow.Handle, string.Format("{0},{1},{2}", mMainWindowCaption,
                fps,
                mspf));

		        // Reset for next average.
                frameCnt = 0;
                timeElapsed += 1.0f;


                
            }

        }

        public virtual void ShutDown()
        {
            md3dDevice.Dispose();
            md3dImmediateContext.Dispose();
            mSwapChain.Dispose();
            mDepthStencilBuffer.Dispose();
            mRenderTargetView.Dispose();
            mResource.Dispose();
            mDepthStencilView.Dispose();
            mMainWindow.Dispose();
;
            
        }
       
        #region MESSAGES
        public virtual void Active(object sender, EventArgs e)
        {
            //System.Console.WriteLine("active");
            mAppPaused = false;
            
            mTimer.Start();
        }

        public virtual void Inactive(object sender, EventArgs e)
        {
            //System.Console.WriteLine("inactive");
            mAppPaused = true;
            
            mTimer.Stop();
        }

        public virtual void ResizeBegin(object sender, EventArgs e)
        {
            //System.Console.WriteLine("RSS");
            mAppPaused = true;
            mResizing = true;
            mTimer.Stop();
        }

        public virtual void ResizeEnd(object sender, EventArgs e)
        {
            //System.Console.WriteLine("RSE");
            mAppPaused = false;
            mResizing = false;
            mTimer.Start();
            //System.Console.WriteLine("RS");
            mClientWidth = mMainWindow.Width;
            mClientHeight = mMainWindow.Height;
            
            OnResize();
        }
        #endregion


        #region DATA
        protected string mMainWindowCaption;
        protected RenderForm mMainWindow;
     
        protected bool mAppPaused;
        protected bool mMinimized;
        protected bool mMaximized;
        protected bool mResizing;
        protected uint m4xMsaaQuality;

        protected EngineTimer mTimer;

        protected Device md3dDevice;
        protected DeviceContext md3dImmediateContext;
        protected SwapChain mSwapChain;
        protected Texture2D mDepthStencilBuffer;
        protected Texture2D mResource;
        protected RenderTargetView mRenderTargetView;
        protected DepthStencilView mDepthStencilView;
        protected Viewport mScreenViewport;

        
        protected int mClientWidth;
        protected int mClientHeight;

        protected bool mEnable4xMsaa;
        #endregion

    }

}

