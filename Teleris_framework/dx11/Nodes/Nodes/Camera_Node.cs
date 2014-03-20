using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teleris.Entities;
using Teleris.Components;
using Teleris.Nodes.Nodes;
using Teleris.Components.Components;

namespace Teleris.Nodes
{
    public class CameraNode : INode
    {

        public CameraComponent Camera { get; set; }

    }
}