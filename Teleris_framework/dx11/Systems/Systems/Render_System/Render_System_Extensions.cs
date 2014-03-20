using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Teleris.Core;
using Teleris.Nodes;
using Teleris.Nodes.Nodes;
using Teleris.Entities;
using Teleris.Components;
using Teleris.Core.Managers;

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Device = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;
using SharpDX.Direct3D;

namespace Teleris.Systems
{
    public static class RenderSystemExtensions
    {

        public static void DrawScene()
        {
            DeviceManager.Instance.md3dImmediateContext.ClearRenderTargetView(DeviceManager.Instance.mRenderTargetView, Color.Silver);
            DeviceManager.Instance.md3dImmediateContext.ClearDepthStencilView(DeviceManager.Instance.mDepthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);
            DeviceManager.Instance.mSwapChain.Present(0, PresentFlags.None);
            //System.Console.WriteLine("Rendered");
        }
    
    }
}
