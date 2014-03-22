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
using SharpDX.Direct3D;
using SharpDX.D3DCompiler;
using Teleris.Core.Managers;
using System.IO;
using System.Diagnostics;

namespace Teleris.Resources
{

    public class Effect
    {

        ShaderSignature _inputSignature;
        VertexShader _vertexShader;
        PixelShader _pixelShader;

        public Effect(string EffectFile)
        {

            //string path = "C:/Users/ilkka.jahnukainen/Desktop/own project/from_home/Teleris/Teleris_framework/dx11/Resources/Effects/Shaders/";
            string path = "C:/Users/ilkkaj/Documents/GitHub/Teleris/Teleris_framework/dx11/Resources/Effects/Shaders/";
            // load and compile the vertex shader
            using (var bytecode = ShaderBytecode.CompileFromFile(path + EffectFile, "VShader", "vs_4_0", ShaderFlags.None, EffectFlags.None))
            {
                _inputSignature = ShaderSignature.GetInputSignature(bytecode);
                _vertexShader = new VertexShader(DeviceManager.Instance.Device, bytecode);
            }
            Debug.WriteLine("Compiled _vertexShader");
            // load and compile the pixel shader
            using (var bytecode = ShaderBytecode.CompileFromFile(path + EffectFile, "PShader", "ps_4_0", ShaderFlags.None, EffectFlags.None))
                _pixelShader = new PixelShader(DeviceManager.Instance.Device, bytecode);
            Debug.WriteLine("Compiled _pixelShader");
        }

        public ShaderSignature InputSignature
        {
            get { return _inputSignature; }

        }

        public VertexShader VertexShader
        {
            get { return _vertexShader; }

        }

        public PixelShader PixelShader
        {
            get { return _pixelShader; }

        }


    }


    public class EffectPool
    {
        //effect name, the effect and it's input signature
        public Dictionary<string, Effect> _effects;


        #region Singleton Pattern
        public static EffectPool pool = null;
        public static EffectPool Pool
        {
            get
            {
                if (pool == null)
                {
                    pool = new EffectPool();
                }
                return pool; 
            }
        }
        #endregion


        private EffectPool()
        {
            _effects = new Dictionary<string, Effect>();

            //Create an effect and add into the pool
            
            _effects.Add("triangle.fx", new Effect("triangle.fx"));
            _effects.Add("colored.fx", new Effect("colored.fx"));

        }


        public void Recompile(string EffectFile)
        {
            _effects[EffectFile] = null;
            Effect Effect = new Effect(EffectFile);
            _effects[EffectFile] = Effect;

        }

    }
}
