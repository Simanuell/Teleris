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
using Buffer = SharpDX.Direct3D11.Buffer;
using VertexP = Teleris.Resources.Geometry.VertexP;

using Teleris.Core.Managers;
using System.Runtime.InteropServices;



namespace Teleris.Components.Components
{
    public sealed class VertexComponent : IComponent
    {
        
        private Buffer _vertexBuffer;
        private Buffer _indexBuffer;
        private InputElement[] _elements;


        public VertexComponent()
        {
            //new DataStream(vertices, true, false)
            //DataStream VertexStream = new DataStream(12 * 8, true, false);

            VertexP[] vertices = new[] {
            new VertexP(new Vector3(-1.0f, -1.0f, -1.0f)),
            new VertexP(new Vector3(-1.0f, 1.0f, -1.0f)),
            new VertexP(new Vector3(1.0f,1.0f,-1.0f)),
            new VertexP(new Vector3(1.0f,-1.0f,-1.0f)),
            new VertexP(new Vector3(-1.0f,-1.0f,1.0f)),
            new VertexP(new Vector3(-1.0f,1.0f,1.0f)),
            new VertexP(new Vector3(1.0f,1.0f,1.0f)),
            new VertexP(new Vector3(1.0f,-1.0f,1.0f))
            };

            // create the vertex layout and buffer
            _elements = new[] { new InputElement("POSITION", 0, Format.R32G32B32_Float, 0) };

            int SizeInBytes = Marshal.SizeOf(typeof(VertexP));
            BufferDescription VertexBufferDescription = new BufferDescription(SizeInBytes * 8, ResourceUsage.Immutable, BindFlags.VertexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, SizeInBytes);
            _vertexBuffer = Buffer.Create<VertexP>(DeviceManager.Instance.Device, vertices, VertexBufferDescription);


            var indices = new uint[] {
            // front
            0,1,2,
            0,2,3,
            // back
            4,6,5,
            4,7,6,
            // left
            4,5,1,
            4,1,0,
            // right
            3,2,6,
            3,6,7,
            //top
            1,5,6,
            1,6,2,
            // bottom
            4,0,3,
            4,3,7
            };

            BufferDescription IndexBufferDescription = new BufferDescription(sizeof(uint) * 36, ResourceUsage.Immutable, BindFlags.IndexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, sizeof(uint));
            _indexBuffer = Buffer.Create<uint>(DeviceManager.Instance.Device, indices, IndexBufferDescription);

            

        }







        /*
        public VertexComponent()
        {
            DataStream vertices = new DataStream(12 * 3, true, true);
            vertices.Write(new Vector3(1.0f, -1.0f, 0.0f));
            vertices.Write(new Vector3(-1.0f, -1.0f, 0.0f));
            vertices.Write(new Vector3(0.0f, 1.0f, 0.0f));
            vertices.Position = 0;

            // create the vertex layout and buffer
            _elements = new[] { new InputElement("POSITION", 0, Format.R32G32B32_Float, 0) };

            _vertexBuffer = new Buffer(DeviceManager.Instance.Device, vertices, 12 * 3, 
            ResourceUsage.Default, BindFlags.VertexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            
        }
        */



        public Buffer VertexBuffer
        {
            get { return _vertexBuffer; }
        }

        public Buffer IndexBuffer
        {
            get { return _indexBuffer; }
        }

        public InputElement[] InputElements
        {
            get { return _elements; }
        }  
    
    }
}
