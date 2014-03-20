using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using Teleris.Core.Managers;
using System.Diagnostics;

namespace Teleris.Components.Components
{
    public sealed class CameraComponent : IComponent
    {

        public Vector3 Position { get; set; }
        public Vector3 Right { get; set; }
        public Vector3 Up { get; set; }
        public Vector3 Look { get; set; }

        public float NearZ { get; set; }
        public float FarZ { get; set; }

        public float Aspect { get; set; }

        public float FovY { get; set; }
        public float FovX
        {
            get
            {
                var halfWidth = 0.5f * NearWindowWidth;
                return 2.0f * (float)Math.Atan(halfWidth / NearZ);
            }
        }
        public float NearWindowWidth { get { return Aspect * NearWindowHeight; } }
        public float NearWindowHeight { get; set; }

        public float FarWindowWidth { get { return Aspect * FarWindowHeight; } }
        public float FarWindowHeight { get; set; }

        public Matrix View { get; set; }
        public Matrix Proj { get; set; }
        public Matrix ViewProj { get { return View * Proj; } }


        public CameraComponent()
        {

            Position = new Vector3(0, 0, -8);
            Right = new Vector3(1, 0, 0);
            Up = new Vector3(0, 1, 0);
            Look = new Vector3(0, 0, 1);

            View = Matrix.Identity;
            Proj = Matrix.Identity;

            FovY = 0.25f * (float)Math.PI;

            Aspect = DeviceManager.Instance.mScreenAspectRatio;
            //Debug.WriteLine(Aspect);
            NearZ = 0.01f;
            FarZ = 1000.0f;

            NearWindowHeight = 2.0f * NearZ * (float)Math.Tan(0.5f * FovY);
            FarWindowHeight = 2.0f * FarZ * (float)Math.Tan(0.5f * FovY);

            Proj = Matrix.PerspectiveFovLH(FovY, Aspect, NearZ, FarZ);

        }
    
    }

}
