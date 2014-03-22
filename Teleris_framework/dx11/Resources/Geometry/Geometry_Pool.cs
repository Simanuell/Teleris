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
using System.Runtime.InteropServices;

namespace Teleris.Resources
{
    //Base
    //C:/Users/ilkkaj/Documents/GitHub/Teleris/Teleris_framework/Models/teapot.obj

    public class GeometryMesh
    {
        
        InputElement[] _inputElements;
        public InputElement[] InputElements {get; set;}

        InputLayout _inputLayout;
        public InputLayout InputLayout { get; set; }

        int _vertexSize;
        public int VertexSize {get; set;}

        Buffer _vertexBuffer;
        public Buffer VertexBuffer {get; set;}

        Buffer _indexBuffer;
        public Buffer IndexBuffer {get; set;}

        int _vertexCount;
        public int VertexCount {get; set;}

        int _indexCount;
        public int IndexCount { get; set; }

        int _primitiveCount;
        public int PrimitiveCount { get; set; }

        PrimitiveTopology _primitiveTopology;
        public PrimitiveTopology PrimitiveTopology { get; set; }

    }


    public class GeometryModel
    {
        
        public List<GeometryMesh> _meshes;
        
        bool _inputLayoutSet;

        public BoundingBox _BoundingBox;

        Vector3 _aaBoxMin;
        public Vector3 AABoxMin { get; set; }

        Vector3 _aaBoxMax;
        public Vector3 AABoxMax { get; set; }

        Vector3 _aaBoxCentre;
        public Vector3 AABoxCentre { get; set; }

        public void AddMesh(ref GeometryMesh mesh)
        {
            _meshes.Add(mesh);
        }

        public void SetAABox(Vector3 min, Vector3 max)
        {
            _BoundingBox.Minimum = min;
            _BoundingBox.Maximum = max; 
            _aaBoxCentre = 0.5f * (min + max);
        }

        //Constructor
        public GeometryModel(string FileName)
        {
            //Debug.WriteLine(FileName);
            //ModelLoader modelLoader = new ModelLoader(DeviceManager.Instance.Device);
            //GeometryModel model = modelLoader.Load(FileName);

            _meshes = new List<GeometryMesh>();
            _BoundingBox = new BoundingBox();
            _inputLayoutSet = false;

        }     

    }


    public class GeometryData 
    {
        public ModelLoader _loader;
        public GeometryModel _Geometrymodel;

        public GeometryData(string fileName) 
        {
            _loader = new ModelLoader(DeviceManager.Instance.Device);
            _Geometrymodel = _loader.Load(fileName);

        }
       
    }

    public class GeometryPool
    {
        //effect name, the effect and it's input signature
        public Dictionary<string, GeometryData> _models;


        #region Singleton Pattern
        public static GeometryPool pool = null;
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
            _models = new Dictionary<string, GeometryData>();

            //GeometryData testipala = new GeometryData("C:/Users/ilkka.jahnukainen/Desktop/own project/from_home/Teleris/Teleris_framework/Models/teapot.obj");
            GeometryData teapot = new GeometryData("C:/Users/ilkkaj/Documents/GitHub/Teleris/Teleris_framework/Models/teapot.obj");

            _models.Add("Teapot", teapot);
           Debug.WriteLine("Geopool");
        }
    }
}


