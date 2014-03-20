using System;
using System.Collections.Generic;
using System.Linq;
using Teleris.Core;
using Teleris.Nodes;
using Teleris.Nodes.Nodes;
using Teleris.Entities;
using Teleris.Components;
using System.Diagnostics;

namespace Teleris.Systems
{
    class PrinterSystem : ISystem
    {
        #region Standard
        private IEngine _engine;

        public override void AddToGame(IEngine Engine)
        {
            _engine = Engine;
            
            //Create node type
            
            INodeGroupManager PrinterNodes = new NodeGroupManager();
            PrinterNodes.Setup(Engine, typeof(PrinterNode));
            Engine.GetNodeList(typeof(PrinterNode));
            Debug.WriteLine("Printer Added");
            
        }

        public override void RemoveFromGame(IEngine Engine)
        {
            System.Console.WriteLine("Printer removed!");
        }
        #endregion
        
        public override void Update(double time)
        {

            NodeList PrinterNodes = _engine.GetNodeList<PrinterNode>();
            
            for(var node = PrinterNodes.Head; node != null; node = node.Next)
            {
                
                TextComponent Text = (TextComponent)node.GetProperty("Text");
                //NumberComponent Number = (NumberComponent)node.GetProperty("Number");
                //Text.Text = "ölö";
                //Debug.WriteLine(time.ToString());
            }        
            
        }
    }
}
