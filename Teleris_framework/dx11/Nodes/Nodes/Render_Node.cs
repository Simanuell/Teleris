using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teleris.Entities;
using Teleris.Components;
using Teleris.Nodes.Nodes;
using Teleris.Components.Components;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace Teleris.Nodes
{
    public class RenderNode : INode
    {
        public ShaderIDComponent ShaderID { get; set; }
        public VertexComponent VertexComponent { get; set; }

    }
}
