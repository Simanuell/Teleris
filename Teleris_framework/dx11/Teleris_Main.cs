using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpDX.D3DCompiler;


using Teleris;
using Teleris.Core;
using Teleris.Core.Managers;
using Teleris.Nodes;
using Teleris.Nodes.Nodes;
using Teleris.Systems;
using Teleris.Entities;
using Teleris.Components;


namespace Teleris
{
    static class Teleris_Main
    {
        [STAThread]   
        static void Main()
        {


            TelerisApplication Teleris = new TelerisApplication();
            Teleris.Init();
            Teleris.Run();
           
            DataStream vertices = new DataStream(12 * 3, true, true);
            vertices.Write(new Vector3(0.0f, 0.5f, 0.5f));
        
     
 
        }
    }
}




/*
RenderForm mMainWindow = new RenderForm();
//DeviceManager.Instance.Init(mMainWindow);
IEngine Engine = new Engine<NodeGroupManager>();
            
EngineTimer Timer = new EngineTimer();

#region entities
//Add system
ISystem Printer = new PrinterSystem();
ISystem Render = new RenderSystem();
Engine.AddSystem(Printer, 1);
Engine.AddSystem(Render, 2);

//Create and Entities
Entity L1 = EntityManager.CreateShip1("L1");
Entity L2 = EntityManager.CreateShip2("L2");

Engine.AddEntity(L1);
Engine.AddEntity(L2);
#endregion

/*
RenderLoop.Run(mMainWindow, () =>
{


     Engine.Update(1.0);
     //Debug.WriteLine("S");
                          

});
*/



/*
TextComponent texti = (TextComponent)L1.Get(typeof(TextComponent));
//System.Console.WriteLine(texti.Text);


//InitD3DApp InitD3DApp = new InitD3DApp();
//System.Console.WriteLine("jooo");
System.Console.ReadLine();
//InitD3DApp.Init();
//InitD3DApp.Run();
//InitD3DApp.ShutDown();
*/