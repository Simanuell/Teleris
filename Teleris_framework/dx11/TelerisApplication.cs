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

using Teleris;
using Teleris.Core;
using Teleris.Core.Managers;
using Teleris.Nodes;
using Teleris.Nodes.Nodes;
using Teleris.Systems;
using Teleris.Entities;
using Teleris.Components;
using Teleris.Systems.Systems;
using Teleris.Core.Utilities;


namespace Teleris
{
    
 
    public class TelerisApplication
    {
        

        static string mMainWindowCaption = "Teleris";
        public static RenderForm mMainWindow = new RenderForm(mMainWindowCaption);
        
        
        public void Init()
        {
            DeviceManager.Instance.Init(mMainWindow);
            LoadData();
        }

        //Set entities and systems
        public void LoadData() 
        {


            Engine = new Engine<NodeGroupManager>();

            //EngineTimer Timer = new EngineTimer();

            #region entities
            //Add system

            ISystem Camera = new CameraSystem();
            //ISystem Printer = new PrinterSystem();
            ISystem ResourceUpdate = new ResourceSystem();
            ISystem Render = new RenderSystem();


            Engine.AddSystem(Camera, 1);
            Engine.AddSystem(ResourceUpdate, 2);
            Engine.AddSystem(Render, 3);

            Entity MainCamera = EntityManager.Camera("MainCamera");
            Engine.AddEntity(MainCamera);

            for (int i = 1; i <= 1; i++)
            {
                //Create and Entities
                string Name = i.ToString();

                Entity Triangle = EntityManager.Triangle("Triangle"+Name);
                //Entity L2 = EntityManager.CreateShip2("L2");

                Engine.AddEntity(Triangle);
                //Engine.AddEntity(L2);
            }
            #endregion
                    
        }
        
        //Run Msg Loop
        public void Run()
        {

            mTimer = new EngineTimer();
            mTimer.Reset();
            //MsgProcedure();
         
            RenderLoop.Run(mMainWindow, () =>
            {
                //Application.DoEvents();
                
                mTimer.Tick();

                if (!mAppPaused)
                {

                    CalculateFrameStats();
                    Engine.Update(mTimer.DeltaTime());
                }


            });

        }

        public virtual void MsgProcedure()
        {

            mMainWindow.Activated += new System.EventHandler(this.Active);
            mMainWindow.Deactivate += new System.EventHandler(this.Inactive);
            mMainWindow.ResizeBegin += new System.EventHandler(this.ResizeBegin);
            mMainWindow.ResizeEnd += new System.EventHandler(this.ResizeEnd);
            //mMainWindow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pp);

        }

        protected virtual void CalculateFrameStats()
        {

            // Code computes the average frames per second, and also the 
            // average time it takes to render one frame.  These stats 
            // are appended to the window caption bar.

            frameCnt++;

            // Compute averages over one second period.
            if ((mTimer.TotalTime() - timeElapsed) >= 1.0f)
            {

                float fps = (float)frameCnt;
                float mspf = 1000.0f / fps;

                var s = string.Format("{0} FPS: {1} Frame Time: {2} (ms)", mMainWindowCaption, fps, mspf);
                mMainWindow.Text = s;

                // Reset for next average.
                frameCnt = 0;
                timeElapsed += 1.0f;

            }

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
            Debug.WriteLine("fffff");
            mAppPaused = true;
            mResizing = true;
            mTimer.Stop();
        }

        public virtual void ResizeEnd(object sender, EventArgs e)
        {

            mAppPaused = false;
            mResizing = false;
            mTimer.Start();
            mClientWidth = mMainWindow.Width;
            mClientHeight = mMainWindow.Height;
            //DeviceManager.Instance.OnResize(mMainWindow);

        }


        #endregion
    
        #region DATA
        //protected string mMainWindowCaption;

        protected IEngine Engine;

        protected bool mAppPaused;
        protected bool mMinimized;
        protected bool mMaximized;
        protected bool mResizing;
        protected uint m4xMsaaQuality;

        protected EngineTimer mTimer;
        protected int frameCnt = 0;
        protected float timeElapsed = 0.0f;

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
