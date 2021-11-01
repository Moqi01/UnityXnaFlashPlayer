using System;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;

namespace XnaFlashPlayer
{
    public class GraphicsDeviceService : IGraphicsDeviceService
    {
        private static GraphicsDeviceService instance;
        private static int referenceCount;

        private PresentationParameters parameters;
        private GraphicsDevice graphicsDevice;

        public event EventHandler<EventArgs> DeviceCreated;
        public event EventHandler<EventArgs> DeviceDisposing;
        public event EventHandler<EventArgs> DeviceReset;
        public event EventHandler<EventArgs> DeviceResetting;

        public GraphicsDeviceService(IntPtr windowHandle, int width, int height)
        {
            parameters = new PresentationParameters();

            parameters.BackBufferWidth = Math.Max(width, 1);
            parameters.BackBufferHeight = Math.Max(height, 1);
            parameters.BackBufferFormat = SurfaceFormat.Color;
            parameters.DepthStencilFormat = DepthFormat.Depth24Stencil8;
            parameters.DeviceWindowHandle = windowHandle;
            parameters.PresentationInterval = PresentInterval.Immediate;
            parameters.MultiSampleCount = 16;
            parameters.IsFullScreen = false;

            graphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.HiDef, parameters);

            if (DeviceCreated != null)
                DeviceCreated(this, EventArgs.Empty);
        }
        
        public static GraphicsDeviceService AddRef(IntPtr windowHandle, int width, int height)
        {
            if (Interlocked.Increment(ref referenceCount) == 1)
                instance = new GraphicsDeviceService(windowHandle, width, height);
            return instance;
        }

        public void Release(bool disposing)
        {
            if (Interlocked.Decrement(ref referenceCount) == 0)
            {
                if (disposing)
                {
                    if (DeviceDisposing != null)
                        DeviceDisposing(this, EventArgs.Empty);

                    graphicsDevice.Dispose();
                }
                graphicsDevice = null;
            }
        }

        public void ResetDevice(int width, int height, bool? fullscreen)
        {
            if (DeviceResetting != null)
                DeviceResetting(this, EventArgs.Empty);

            parameters.BackBufferWidth = Math.Max(parameters.BackBufferWidth, width);
            parameters.BackBufferHeight = Math.Max(parameters.BackBufferHeight, height);
            if (fullscreen.HasValue) parameters.IsFullScreen = fullscreen.Value;

            graphicsDevice.Reset(parameters);

            if (DeviceReset != null)
                DeviceReset(this, EventArgs.Empty);
        }
        
        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }        
    }
}
