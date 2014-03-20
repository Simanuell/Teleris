using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Teleris.Systems;

namespace Teleris.Core.Utilities
{
    public static class ControllerInput
    {


        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        public static bool IsKeyDown(System.Windows.Forms.Keys key)
        {
            return (GetAsyncKeyState(key) & 0x8000) != 0;
        }

                
        //public void RenderControl_MouseWheel(object sender, MouseEventArgs e)


        /*
        private void RenderControl_MouseUp(object sender, MouseEventArgs e)
        {
            CameraManager.Instance.currentCamera.MouseUp(sender, e);
        }

        private void RenderControl_MouseDown(object sender, MouseEventArgs e)
        {
            CameraManager.Instance.currentCamera.MouseDown(sender, e);
        }

        private void RenderControl_MouseMove(object sender, MouseEventArgs e)
        {
            CameraManager.Instance.currentCamera.MouseMove(sender, e);
        }
        */
        /*
        void RenderControl_MouseWheel(object sender, MouseEventArgs e)
        {
            System.Console.WriteLine("MouseWheel");
        }
        */
        /*
        private void RenderControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                CameraManager.Instance.CycleCameras();
            }
            else if (e.KeyCode == Keys.F2)
            {
                RenderManager.Instance.SwitchSyncInterval();
            }

            CameraManager.Instance.currentCamera.KeyUp(sender, e);
        }

        private void RenderControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            CameraManager.Instance.currentCamera.KeyPress(sender, e);
        }

        private void RenderControl_KeyDown(object sender, KeyEventArgs e)
        {
            CameraManager.Instance.currentCamera.KeyDown(sender, e);
        }
        */
    
    
    
    
    
    }
}
