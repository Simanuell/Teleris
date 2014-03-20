using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpDX.D3DCompiler;



using Teleris.Core.Managers;



namespace Teleris.Components.Components
{
    public sealed class Position3DComponent : IComponent
    {
        
        private Vector3 _Position3D;

        public Vector3 Position3D
        {
            get { return _Position3D; }

            set { _Position3D = value; }
        }

    
    }
}
