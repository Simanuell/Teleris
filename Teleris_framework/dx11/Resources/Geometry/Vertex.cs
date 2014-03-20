using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using System.Runtime.InteropServices;

namespace Teleris.Resources.Geometry
{


        public struct VertexPC
        {
            public Vector3 Pos;
            public Color4 Color;

            public VertexPC(Vector3 pos, Color color)
            {
                Pos = pos;
                Color = color;
            }

            public VertexPC(Vector3 pos, Color4 color)
            {
                Pos = pos;
                Color = color;
            }

            public static readonly int Stride = Marshal.SizeOf(typeof(VertexPC));
        }

        public struct VertexP
        {
            public Vector3 Pos;


            public VertexP(Vector3 pos)
            {
                Pos = pos;

            }

            public static readonly int Stride = Marshal.SizeOf(typeof(VertexP));
        }  
    
    

    
}
