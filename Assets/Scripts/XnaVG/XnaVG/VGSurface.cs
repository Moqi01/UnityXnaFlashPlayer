using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaVG
{
    public sealed class VGSurface : IDisposable
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public SurfaceFormat Format { get; private set; }
        public RenderTarget2D Target { get; private set; }
        public VGDevice Device { get; private set; }

        internal Vector2[] MSAAPattern { get; private set; }

        internal VGSurface(VGDevice device)
        {
            var displayMode = device.GraphicsDevice.DisplayMode;
            Width = displayMode.Width;
            Height = displayMode.Height;
            Format = displayMode.Format;
            Target = null;
            Device = device;
            UpdateMSAAPattern();
        }
        internal VGSurface(VGDevice device, RenderTarget2D renderTarget)
        {
            Width = renderTarget.Width;
            Height = renderTarget.Height;
            Format = renderTarget.Format;
            Target = renderTarget;
            Device = device;
            UpdateMSAAPattern();
        }
        internal VGSurface(VGDevice device, int width, int height, SurfaceFormat format)
            : this(device, new RenderTarget2D(
                device.GraphicsDevice, width, height, false, 
                format, DepthFormat.Depth24Stencil8, 
                device.GraphicsDevice.PresentationParameters.MultiSampleCount,
                RenderTargetUsage.PreserveContents))
        { }

        internal void MakeActive()
        {
            Device.GraphicsDevice.SetRenderTarget(Target);
        }

        public Texture2D ToTexture()
        {
            if (Target != null)
                return Device.TextureUtils.CloneTexture(Target);
            return Device.TextureUtils.CloneBackBuffer();
        }

        public void Dispose()
        {
            if (Target == null)
                return;
            
            Target.Dispose();
            Target = null;
        }

        private void UpdateMSAAPattern()
        {
            var orig = Rendering.Constants.MSAAPattern;
            Vector2 size = new Vector2(Width, Height);
            MSAAPattern = new Vector2[orig.Length];
            for (int i = 0; i < MSAAPattern.Length; i++)
                MSAAPattern[i] = orig[i] / (1.3f * size);
        }
    }
}
