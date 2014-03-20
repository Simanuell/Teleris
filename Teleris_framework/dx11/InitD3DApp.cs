using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Device = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;




namespace Teleris
{
    class InitD3DApp : D3DAppBase
    {

        //Contructor
        public InitD3DApp() { }
        
        public override bool Init()
        {
            if (!base.Init())
                return false;
            
            return true;
        }
        public override void OnResize()
        {
            base.OnResize();
           
        }                
        public override void UpdateScene(float dt)
        {

        }
        public override void DrawScene()
        {
            // clear the render target to a soothing blue
            md3dImmediateContext.ClearRenderTargetView(mRenderTargetView, Color.Silver);
            md3dImmediateContext.ClearDepthStencilView(mDepthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);
            mSwapChain.Present(0, PresentFlags.None);

        }
    }
}

