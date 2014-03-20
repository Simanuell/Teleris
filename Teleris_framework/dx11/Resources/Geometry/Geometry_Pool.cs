using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Device = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;
using Buffer = SharpDX.Direct3D11.Buffer;
using SharpDX.Direct3D;
using SharpDX.D3DCompiler;
using Teleris.Core.Managers;

namespace Teleris.Resources
{
    //Base
    public class Geometry
    {

        private ShaderSignature _inputSignature;
        private VertexShader _vertexShader;
        private PixelShader _pixelShader;

        private Buffer _vertexBuffer;
        private InputElement[] _elements;

        //Geometry Here

        virtual public Buffer VertexBuffer
        {
            get { return _vertexBuffer; }
        }

        virtual public InputElement[] InputElements
        {
            get { return _elements; }
        }  

    }

    //Procedural models
    public class Triangle:Geometry
    {

        ShaderSignature _inputSignature;
        VertexShader _vertexShader;
        PixelShader _pixelShader;

        Buffer _vertexBuffer;
        InputElement[] _elements;

        public Triangle()
        {

            DataStream vertices = new DataStream(12 * 3, true, true);
            vertices.Write(new Vector3(0.0f, 0.5f, 0.5f));
            vertices.Write(new Vector3(0.5f, -0.5f, 0.5f));
            vertices.Write(new Vector3(-0.5f, -0.5f, 0.5f));
            vertices.Position = 0;

            // create the vertex layout and buffer
            _elements = new[] { new InputElement("POSITION", 0, Format.R32G32B32_Float, 0) };

            _vertexBuffer = new Buffer(DeviceManager.Instance.Device, vertices, 12 * 3,
            ResourceUsage.Default, BindFlags.VertexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);

        }


        public override Buffer VertexBuffer
        {
            get { return _vertexBuffer; }
        }

        public override InputElement[] InputElements
        {
            get { return _elements; }
        }

    }

    //Models

    public class GeometryPool
    {
        //effect name, the effect and it's input signature
        public Dictionary<string, Geometry> _models;


        #region Singleton Pattern
        private static GeometryPool pool = null;
        public static GeometryPool Pool
        {
            get
            {
                if (pool == null)
                {
                    pool = new GeometryPool();
                }
                return pool;
            }
        }
        #endregion


        private GeometryPool()
        {
            _models = new Dictionary<string, Geometry>();

            //Simple Triangle
            Triangle BasicTriangle = new Triangle();
            _models.Add("Triangle", BasicTriangle);

        }

    }
}
