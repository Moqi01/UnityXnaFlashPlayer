using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework
{
    class UnityGameWindow : GameWindow
	{
        public override bool AllowUserResizing
        {
            get
            {
                UnityEngine.Debug.Log("?");
                return true;
            }
            set
            {
                UnityEngine.Debug.Log("?");
            }
        }
        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
            UnityEngine.Debug.Log("?");
        }
        public override Rectangle ClientBounds
        {
            get { UnityEngine.Debug.Log("?"); return new Rectangle(); }
        }
        public override void EndScreenDeviceChange(string screenDeviceName, int clientWidth, int clientHeight)
        {
            UnityEngine.Debug.Log("?");
        }
        public override IntPtr Handle
        {
            get { UnityEngine.Debug.Log("?");return IntPtr.Zero; }
        }
        public override string ScreenDeviceName
        {
            get { UnityEngine.Debug.Log("?");return ""; }
        }
        protected override void SetTitle(string title)
        {
            UnityEngine.Debug.Log("?");
        }
	}
}
