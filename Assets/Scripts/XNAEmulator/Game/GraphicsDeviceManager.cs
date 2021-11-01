using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework
{
    public class GraphicsDeviceManager
    {
        private Game game;

        public GraphicsDevice GraphicsDevice
        {
            get
            {
                return game.GraphicsDevice;
            }
        }

        public GraphicsDeviceManager(Game game)
        {
            // TODO: Complete member initialization
            this.game = game;
            this._synchronizedWithVerticalRetrace = true;
        }

        public bool PreferMultiSampling
        {
            get
            {
                return this.allowMultiSampling;
            }
            set
            {
                this.allowMultiSampling = value;
                this.isDeviceDirty = true;
            }
        }
        private bool isFullScreen;

        private bool allowMultiSampling;
        private bool isDeviceDirty;

        // Token: 0x0400004D RID: 77
        public static readonly int DefaultBackBufferWidth = 800;

        // Token: 0x0400004E RID: 78
        public static readonly int DefaultBackBufferHeight = 480;
        // Token: 0x0400005A RID: 90
        private int backBufferWidth = GraphicsDeviceManager.DefaultBackBufferWidth;

        // Token: 0x0400005B RID: 91
        private int backBufferHeight = GraphicsDeviceManager.DefaultBackBufferHeight;

        // Token: 0x04000061 RID: 97
        private bool useResizedBackBuffer;

        private GraphicsProfile graphicsProfile;

        public bool IsFullScreen
        {
            get
            {
                return this.isFullScreen;
            }
            set
            {
                this.isFullScreen = value;
                this.isDeviceDirty = true;
            }
        }

        public int PreferredBackBufferWidth
        {
            get
            {
                return this.backBufferWidth;
            }
            set
            {
                if (value <= 0)
                {
                    UnityEngine.Debug.LogError("!");
                   // throw new ArgumentOutOfRangeException("value", Resources.BackBufferDimMustBePositive);
                }
                this.backBufferWidth = value;
                this.useResizedBackBuffer = false;
                this.isDeviceDirty = true;
            }
        }

        public int PreferredBackBufferHeight
        {
            get
            {
                return this.backBufferHeight;
            }
            set
            {
                if (value <= 0)
                {
                    UnityEngine.Debug.LogError("!");
                    //throw new ArgumentOutOfRangeException("value", Resources.BackBufferDimMustBePositive);
                }
                this.backBufferHeight = value;
                this.useResizedBackBuffer = false;
                this.isDeviceDirty = true;
            }
        }

        public GraphicsProfile GraphicsProfile
        {
            get
            {
                return this.graphicsProfile;
            }
            set
            {
                this.graphicsProfile = value;
                this.isDeviceDirty = true;
            }
        }

        private GraphicsDevice device;
        public void ApplyChanges()
        {
            if (this.device != null && !this.isDeviceDirty)
            {
                return;
            }
            this.ChangeDevice(false);
        }

        private DisplayOrientation currentWindowOrientation;
        private bool inDeviceTransition;
        private void ChangeDevice(bool forceCreate)
        {
            UnityEngine.Debug.Log("?");
            if (this.game == null)
            {
                //throw new InvalidOperationException(Resources.GraphicsComponentNotAttachedToGame);
            }
            this.inDeviceTransition = true;
            string screenDeviceName = this.game.Window.ScreenDeviceName;
            int width = this.game.Window.ClientBounds.Width;
            int height = this.game.Window.ClientBounds.Height;
            bool flag = false;
            try
            {
               
                this.isDeviceDirty = false;
            }
            finally
            {
                if (flag)
                {
                    this.game.Window.EndScreenDeviceChange(screenDeviceName, width, height);
                }
                this.currentWindowOrientation = this.game.Window.CurrentOrientation;
                this.inDeviceTransition = false;
            }
        }

        public void ToggleFullScreen()
        {
            this.IsFullScreen = !this.IsFullScreen;
            this.ChangeDevice(false);
        }

        public bool SynchronizeWithVerticalRetrace
        {
            get
            {
                return this._synchronizedWithVerticalRetrace;
            }
            set
            {
                this._shouldApplyChanges = true;
                this._synchronizedWithVerticalRetrace = value;
            }
        }
        // Token: 0x04000305 RID: 773
        private bool _synchronizedWithVerticalRetrace = true;
        // Token: 0x0400030B RID: 779
        private bool _shouldApplyChanges;
    }
}
