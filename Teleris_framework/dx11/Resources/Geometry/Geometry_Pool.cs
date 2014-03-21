﻿using System;
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

    public class GeometryMesh
    {
        InputElement[] m_inputElements;
        public InputElement[] InputElements {get; set;}

        InputLayout m_inputLayout;
        public InputLayout InputLayout { get; set; }

        int m_vertexSize;
        public int VertexSize {get; set;}

        Buffer m_vertexBuffer;
        public Buffer VertexBuffer {get; set;}

        Buffer m_indexBuffer;
        public Buffer IndexBuffer {get; set;}

        int m_vertexCount;
        public int VertexCount {get; set;}

        int m_indexCount;
        public int IndexCount { get; set; }

        int m_primitiveCount;
        public int PrimitiveCount { get; set; }

        PrimitiveTopology m_primitiveTopology;
        public PrimitiveTopology PrimitiveTopology { get; set; }

    }


    public class GeometryModel
    {
        
        public string olo;
        private Buffer _vertexBuffer;
        private Buffer _indexBuffer;
        private InputElement[] _elements;
        

        public GeometryModel() 
        {
            olo = "olo000";
            Debug.WriteLine("Geopool"); 
        }


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
            _models = new Dictionary<string, GeometryModel>();
            
            GeometryModel testipala = new GeometryModel();
            _models.Add("testipala", testipala); 
            Debug.WriteLine("Geopool");
        }
    }
}
