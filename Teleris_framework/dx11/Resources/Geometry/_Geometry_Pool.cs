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
using Assimp;
using System.Diagnostics;

namespace Teleris.Resources
{
    //Base
    //C:/Users/ilkkaj/Documents/GitHub/Teleris/Teleris_framework/Models/teapot.obj

    public class GeometryModel
    {


        public GeometryModel(string FileName)
        {
            String fileName = FileName;
            ModelLoader modelLoader = new ModelLoader(DeviceManager.Instance.Device);
            Debug.WriteLine("joouuuu");
            model = modelLoader.Load(fileName);

        }

        public string jokudata = "aaaa";
        public Model model;
        public ShaderSignature _inputSignature;
        public Buffer _indexBuffer;
        public VertexShader _vertexShader;
        public PixelShader _pixelShader;

        public Buffer _vertexBuffer;
        public InputElement[] _elements;

        //Geometry Here

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

    public class GeometryPool
    {
        //effect name, the effect and it's input signature
        public Dictionary<string, GeometryModel> _models;


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
            _models = new Dictionary<string, GeometryModel>();

            //Simple Triangle
            GeometryModel Teapot = new GeometryModel("C:/Users/ilkkaj/Documents/GitHub/Teleris/Teleris_framework/Models/teapot.obj");
            _models.Add("Teapot", Teapot);

            Debug.WriteLine("jooo");

        }

    }
}
