
using Teleris.Core;
using Teleris.Nodes;
using Teleris.Core.Managers;



using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buffer = SharpDX.Direct3D11.Buffer;
using VertexP = Teleris.Resources.Geometry.VertexP;
using SharpDX.Direct3D;
using Teleris.Resources;
using SharpDX.D3DCompiler;
using Teleris.Components.Components;
using System.Diagnostics;
using Teleris.Nodes.Nodes;
using System;
using Teleris.Core.Utilities;
using Teleris;
using System.Windows.Forms;


namespace Teleris.Systems
{
    class RenderSystem : ISystem
    {
        #region Standard


        private IEngine _engine;
        public AntRenderForm GUI;
        private float a = 0;
        VertexShaderData vsData;
        Buffer vertexConstantBuffer;
        float _time = 0.0f;
        bool _guiVisible;



        //data to pass to the vertex and pixel shader
        struct VertexShaderData
        {
            public Matrix worldViewProj;
            public Matrix worldView;
            public Matrix world;
        };
   

        public override void AddToGame(IEngine Engine)
        {

            //Engine
            _engine = Engine;

            //Create node type
            INodeGroupManager RenderNodes = new NodeGroupManager();
            NodeList CameraNodes = Engine.GetNodeList(typeof(CameraNode));

            //RenderNode list
            RenderNodes.Setup(Engine, typeof(RenderNode));
            Engine.GetNodeList(typeof(RenderNode));

            vertexConstantBuffer = new Buffer(DeviceManager.Instance.Device, Utilities.SizeOf<VertexShaderData>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            DeviceManager.Instance.Context.VertexShader.SetConstantBuffer(0, vertexConstantBuffer);
            //VertexShaderData vsData = new VertexShaderData();


            //Init GUI drawing

            GUI = new AntRenderForm("GUI");
            GUI.ClientSize = new System.Drawing.Size(960, 600);
            TweakBar.InitializeLibrary(DeviceManager.Instance.Device); 



            //TweakBar myBar1 = new TweakBar(GUI, "myTest1");
            //TweakBar myBar2 = new TweakBar(GUI, "myTest2");
            //TweakBar myBar3 = new TweakBar(GUI, "myTest3");
            //TweakBar myBar4 = new TweakBar(GUI, "myTest4");
            //TweakBar myBar5 = new TweakBar(GUI, "myTest5");

            //AntTweakBar.TwDefine("myTest1 color='5 5 5' alpha = 100 fontsize=3 text=light iconifiable=true fontscaling=5 contained=true  iconpos=tl iconmargin='20 20'");
            //AntTweakBar.TwDefine("myTest2 color='5 5 5' alpha = 100 fontsize=2 text=light iconifiable=true fontscaling=5 contained=true  iconpos=tl");
            //AntTweakBar.TwDefine("myTest3 color='5 5 5' alpha = 100 fontsize=1 text=light iconifiable=true fontscaling=5 contained=true  iconpos=tl"); 
            //myBar1.AddFloat("parmA", "parmA", "parmA", 0, 100, 30, 0.1, 2);

            //myBar1["parmA"].VariableChange += parmA;
            //myBar1.AddDirection("suunta", "suunta", "nimi", new Vector3(0,0,0));
            TweakBar.UpdateWindow(GUI);

            

        }
        
        public override void RemoveFromGame(IEngine Engine)
        {
            System.Console.WriteLine("Printer removed!");
        }
        #endregion

        public override void Update(double time)
        {

            
            NodeList RenderNodes = _engine.GetNodeList<RenderNode>();
            INode Camera = _engine.GetNodeList<CameraNode>().Head;

            string name = Camera.Entity.Name;
            var camera = (CameraComponent)Camera.GetProperty("Camera");

            //Debug.WriteLine(camera.View.Column3.X + camera.View.Column3.Y + camera.View.Column3.Z);
            //Debug.WriteLine("{0}, {1}, {2}", camera.Look.X, camera.Look.Y, camera.Look.Z);
            //string testipala = "testipala";
            //GeometryModel test = GeometryPool.Pool._models[testipala];
            //var testa = test.olo;
            //Debug.WriteLine(testa);
            

            DeviceManager.Instance.Context.ClearRenderTargetView(DeviceManager.Instance.mRenderTargetView, Color.Silver);
            DeviceManager.Instance.Context.ClearDepthStencilView(DeviceManager.Instance.mDepthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);

            #region RENDER LOOP

            for (var node = RenderNodes.Head; node != null; node = node.Next)
            {

               
                ShaderIDComponent ShaderID = (ShaderIDComponent)node.GetProperty("ShaderID");
                var Shader = ShaderID.ShaderID;
                //GeometryIDComponent GeoID = (GeometryIDComponent)node.GetProperty("GeometryID");
                //var Geo = GeoID.GeometryID;

                VertexShader VertexShader = EffectPool.Pool._effects[Shader].VertexShader;
                PixelShader PixelShader = EffectPool.Pool._effects[Shader].PixelShader;

                //GeometryModel Teapot = GeometryPool.Pool._models[Geo].model;
                



                ShaderSignature InputSignature = EffectPool.Pool._effects[Shader].InputSignature;

                VertexComponent VertexComponent = (VertexComponent)node.GetProperty("VertexComponent");
                var VertexBuffer = (Buffer)VertexComponent.VertexBuffer;
                var IndexBuffer = (Buffer)VertexComponent.IndexBuffer;
                var InputElements = (InputElement[])VertexComponent.InputElements;

                // configure the Input Assembler portion of the pipeline with the vertex data
                DeviceManager.Instance.Context.InputAssembler.InputLayout = new InputLayout(DeviceManager.Instance.Device, InputSignature, InputElements); ;
                DeviceManager.Instance.Context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

                DeviceManager.Instance.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, VertexP.Stride, 0));
                DeviceManager.Instance.Context.InputAssembler.SetIndexBuffer(IndexBuffer, Format.R32_UInt, 0);



                // Update transformation matrices
                vsData.world = Matrix.Identity * Matrix.RotationY(_time) * Matrix.Translation(0.0f,(float)Math.Sin(_time),0.0f); _time += 0.001f;
                vsData.worldView = vsData.world * camera.View;
                vsData.worldViewProj = vsData.world * camera.ViewProj;

                //transpose matrices before sending them to the shader
                vsData.world.Transpose();
                vsData.worldView.Transpose();
                vsData.worldViewProj.Transpose();
                //Debug.WriteLine("{0}, {1}, {2}", vsData.worldViewProj.Column2.X, vsData.worldViewProj.Column2.Y, vsData.worldViewProj.Column2.Z);
                //update vertex shader constant buffer
                DeviceManager.Instance.Context.UpdateSubresource(ref vsData, vertexConstantBuffer);


                // set the shaders
                DeviceManager.Instance.Context.VertexShader.Set(VertexShader);
                DeviceManager.Instance.Context.PixelShader.Set(PixelShader);
                
                DeviceManager.Instance.Context.DrawIndexed(36, 0, 0);

                //Draw GUI
                //Debug.WriteLine(AntTweakBar.TwGetBarCount());
               
                _guiVisible = ControllerInput.IsKeyDown(Keys.G) ? _guiVisible = true : _guiVisible = false;
                
                if (1==1)
                {
                    AntTweakBar.TwDraw();                              
                }

                
                DeviceManager.Instance.mSwapChain.Present(0, PresentFlags.None);
                
                //DrawScene();

            }

        }


        public void parmA(object sender, Variable variable)
        {
            Debug.WriteLine("parmA changed!"+variable.Value) ;
            
        }


        /*
        public void DrawScene()
        {
            DeviceManager.Instance.Context.ClearRenderTargetView(DeviceManager.Instance.mRenderTargetView, Color.Silver);
            DeviceManager.Instance.Context.ClearDepthStencilView(DeviceManager.Instance.mDepthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);
            DeviceManager.Instance.mSwapChain.Present(0, PresentFlags.None);

            // configure the Input Assembler portion of the pipeline with the vertex data
            DeviceManager.Instance.Context.InputAssembler.InputLayout = layout;
            DeviceManager.Instance.Context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            //DeviceManager.Instance.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, 12, 0));

            // set the shaders
            DeviceManager.Instance.Context.VertexShader.Set(VS);
            DeviceManager.Instance.Context.PixelShader.Set(PS);

            // render the triangle
            DeviceManager.Instance.Context.Draw(3, 0);
            
            
            System.Console.WriteLine("Rendered");
            
        
        }
        */
            #endregion
    }

}


