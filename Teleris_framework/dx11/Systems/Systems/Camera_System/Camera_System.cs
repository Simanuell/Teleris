using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using Teleris.Core;
using Teleris.Nodes;

using SharpDX;
using SharpDX.Windows;
using Teleris.Components.Components;
using Teleris.Core.Utilities;
using System;



namespace Teleris.Systems
{
    class CameraSystem : ISystem
    {
        #region Standard
        private IEngine _engine;
        Point _lastMousePos;
        private Matrix _View;
        private float _X_angle;
        private float _Y_angle;


        public override void AddToGame(IEngine Engine)
        {

            //Engine
            _engine = Engine;
            _X_angle = 0.0f;
            _Y_angle = 0.0f;

            //Create node type
            INodeGroupManager CameraNodes = new NodeGroupManager();

            //RenderNode list
            CameraNodes.Setup(Engine, typeof(CameraNode));
            Engine.GetNodeList(typeof(CameraNode));

        }

        public override void RemoveFromGame(IEngine Engine)
        {
            System.Console.WriteLine("Camera removed!");
        }
        #endregion

        public override void Update(double time)
        {

            NodeList CameraNodes = _engine.GetNodeList<CameraNode>();



            for (var node = CameraNodes.Head; node != null; node = node.Next)
            {


                CameraComponent Camera = (CameraComponent)node.GetProperty("Camera");

                //Reset the camera
                if (ControllerInput.IsKeyDown(Keys.R))
                {
                    Camera.Position = new Vector3(0, 0, -8);
                    Camera.Right = new Vector3(1, 0, 0);
                    Camera.Up = new Vector3(0, 1, 0);
                    Camera.Look = new Vector3(0, 0, 1);
                }

                var Position = Camera.Position;
                var Right = Camera.Right;
                var Up = Camera.Up;
                var Look = Camera.Look;

                //Compute matrices and assign to the camera
                UpdateCameraPosition(ref Position, ref Right, ref Up, ref  Look, time);
                UpdateCameraPitchAndYaw(ref Right, ref Up, ref  Look, ref _lastMousePos);
                UpdateCameraViewMatrix( Position,  Right,  Up, Look);
                
                Camera.Position = Position;
                Camera.Right = Right;
                Camera.Up = Up;
                Camera.Look = Look;
                Camera.View = _View;

                //Build camera frustum object
                FrustumCameraParams FCP = new FrustumCameraParams();
                FCP.AspectRatio = Camera.Aspect;
                FCP.FOV = Camera.FovY;
                FCP.LookAtDir = Camera.Look;
                FCP.Position = Camera.Position;
                FCP.UpDir = Camera.Up;
                FCP.ZFar = Camera.FarZ;
                FCP.ZNear = Camera.NearZ;
                BoundingFrustum BB = BoundingFrustum.FromCamera(FCP);

                /*
                BoundingBox Bbox = new BoundingBox(new Vector3(-50,-50,-5), new Vector3(-60,-60,-10));
                BB.Intersects(ref Bbox);
                Debug.WriteLine(BB.Intersects(ref Bbox));
                */

            }

        }


        public void UpdateCameraViewMatrix(Vector3 Position, Vector3 Right, Vector3 Up, Vector3 Look)
        {
            var p = Position;
            var r = Right;
            var u = Up;
            var l = Look;


            l = Vector3.Normalize(l);
            u = Vector3.Normalize(Vector3.Cross(l, r));

            r = Vector3.Cross(u, l);

            var x = -Vector3.Dot(p, r);
            var y = -Vector3.Dot(p, u);
            var z = -Vector3.Dot(p, l);

            Right = r;
            Up = u;
            Look = l;

            var v = new Matrix();
            v[0, 0] = Right.X;
            v[1, 0] = Right.Y;
            v[2, 0] = Right.Z;
            v[3, 0] = x;

            v[0, 1] = Up.X;
            v[1, 1] = Up.Y;
            v[2, 1] = Up.Z;
            v[3, 1] = y;

            v[0, 2] = Look.X;
            v[1, 2] = Look.Y;
            v[2, 2] = Look.Z;
            v[3, 2] = z;

            v[0, 3] = v[1, 3] = v[2, 3] = 0;
            v[3, 3] = 1;

            _View = v;
        }

        public void UpdateCameraPosition(ref Vector3 Position, ref Vector3 Right, ref Vector3 Up, ref Vector3 Look, double time)
        {

            float Speed = 15.0f;

            if (ControllerInput.IsKeyDown(Keys.D) || ControllerInput.IsKeyDown(Keys.Right))
            {
                Position += Right * (float)time * Speed;
            }

            if (ControllerInput.IsKeyDown(Keys.A) || ControllerInput.IsKeyDown(Keys.Left))
            {
                Position += Right * (float)time * -Speed;
            }

            if (ControllerInput.IsKeyDown(Keys.W) || ControllerInput.IsKeyDown(Keys.Up))
            {
                Position += Look * (float)time * Speed;
            }

            if (ControllerInput.IsKeyDown(Keys.S) || ControllerInput.IsKeyDown(Keys.Down))
            {
                Position += Look * (float)time * -Speed;
            }

        }

        public void UpdateCameraPitchAndYaw(ref Vector3 Right, ref Vector3 Up, ref Vector3 Look, ref Point _lastMousePos)
        {
            
            Point CurrentMousePosition = RenderControl.MousePosition;

            if (RenderControl.MouseButtons == MouseButtons.Left)
            {

                
                _X_angle = SharpDX.MathUtil.DegreesToRadians(0.15f * (CurrentMousePosition.X - _lastMousePos.X));
                _Y_angle = SharpDX.MathUtil.DegreesToRadians(0.15f * (CurrentMousePosition.Y - _lastMousePos.Y));
                
                var rotation = Matrix.RotationAxis(Right, _Y_angle);
                Up = Vector3.TransformNormal(Up, rotation);
                Look = Vector3.TransformNormal(Look, rotation);

                rotation = Matrix.RotationY(_X_angle);
                Right = Vector3.TransformNormal(Right, rotation);
                Up = Vector3.TransformNormal(Up, rotation);
                Look = Vector3.TransformNormal(Look, rotation);
                
            }

            _lastMousePos = CurrentMousePosition;
        }


    }    
    
}


