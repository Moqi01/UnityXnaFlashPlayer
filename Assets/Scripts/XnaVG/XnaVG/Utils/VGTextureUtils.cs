using System;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace XnaVG.Utils
{
    public sealed class VGTextureUtils : IDisposable
    {
        public VGDevice Device { get; private set; }
        private MethodInfo _getDataMethodInfo;
        private MethodInfo _setDataMethodInfo;
        private MethodInfo _getBackBufferDataMethodInfo;
        private SpriteBatch _spriteBatch;

        public VGTextureUtils(VGDevice device)
        {
            Device = device;
            _spriteBatch = new SpriteBatch(device.GraphicsDevice);
            _getDataMethodInfo = typeof(Texture2D).GetMethods().Where(m => m.Name == "GetData" && m.GetParameters().Length == 1).FirstOrDefault();
            _setDataMethodInfo = typeof(Texture2D).GetMethods().Where(m => m.Name == "SetData" && m.GetParameters().Length == 1).FirstOrDefault();
            _getBackBufferDataMethodInfo = typeof(GraphicsDevice).GetMethods().Where(m => m.Name == "GetBackBufferData" && m.GetParameters().Length == 1).FirstOrDefault();
        }

        public Type GetTypeBySurfaceFormat(SurfaceFormat format)
        {
            switch (format)
            {
                case SurfaceFormat.Color: return typeof(Color);
                case SurfaceFormat.Bgr565: return typeof(Bgr565);
                case SurfaceFormat.Bgra5551: return typeof(Bgra5551);
                case SurfaceFormat.Bgra4444: return typeof(Bgra4444);
                case SurfaceFormat.NormalizedByte2: return typeof(NormalizedByte2);
                case SurfaceFormat.NormalizedByte4: return typeof(NormalizedByte4);
                case SurfaceFormat.Rgba1010102: return typeof(Rgba1010102);
                case SurfaceFormat.Rg32: return typeof(Rg32);
                case SurfaceFormat.Rgba64: return typeof(Rgba64);
                case SurfaceFormat.Alpha8: return typeof(Alpha8);
                case SurfaceFormat.Single: return typeof(Single);
                case SurfaceFormat.Vector2: return typeof(Vector2);
                case SurfaceFormat.Vector4: return typeof(Vector4);
                case SurfaceFormat.HalfSingle: return typeof(HalfSingle);
                case SurfaceFormat.HalfVector2: return typeof(HalfVector2);
                case SurfaceFormat.HdrBlendable:
                case SurfaceFormat.HalfVector4: return typeof(HalfVector4);                
                case SurfaceFormat.Dxt1:
                case SurfaceFormat.Dxt3:
                case SurfaceFormat.Dxt5:
                    throw new NotSupportedException("Dxt surfaces are not supported!");
                default:
                    throw new ArgumentException("Format is not valid surface format!", "format");
            }
        }

        public Texture2D CloneTexture(Texture2D texture)
        {
            if (_getDataMethodInfo == null || _setDataMethodInfo == null)
                throw new NotSupportedException("This operation is not supported on this machine!");

            if (texture == null)
                throw new ArgumentNullException("texture");

            var type = GetTypeBySurfaceFormat(texture.Format);
            var clone = new Texture2D(Device.GraphicsDevice, texture.Width, texture.Height, false, texture.Format);
            var data = new object[] { Array.CreateInstance(type, texture.Width * texture.Height) };
            _getDataMethodInfo.MakeGenericMethod(type).Invoke(texture, data);
            _setDataMethodInfo.MakeGenericMethod(type).Invoke(clone, data);
            return clone;        
        }

        public Texture2D CloneBackBuffer()
        {
            if (_setDataMethodInfo == null || _getBackBufferDataMethodInfo == null)
                throw new NotSupportedException("This operation is not supported on this machine!");

            var pp = Device.GraphicsDevice.PresentationParameters;
            var type = GetTypeBySurfaceFormat(pp.BackBufferFormat);
            var clone = new Texture2D(Device.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat);
            var data = new object[] { Array.CreateInstance(type, clone.Width * clone.Height) };
            _getBackBufferDataMethodInfo.MakeGenericMethod(type).Invoke(Device.GraphicsDevice, data);
            _setDataMethodInfo.MakeGenericMethod(type).Invoke(clone, data);
            return clone;
        }

        public RenderTarget2D CloneTextureToRenderTarget(Texture2D texture, Effect effect)
        {
            var rtt = new RenderTarget2D(Device.GraphicsDevice, texture.Width, texture.Height, false, texture.Format, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            CopyToRenderTarget(rtt, texture, ColorWriteChannels.All, null, null, effect);
            return rtt;
        }

        public void CopyToRenderTarget(RenderTarget2D target, Texture2D source, ColorWriteChannels channels, Rectangle? sourceRect, Rectangle? targetRect, Effect effect)
        {
            var device = Device.GraphicsDevice;
            if (target != null && device != target.GraphicsDevice)
                throw new InvalidOperationException("RenderTarget2D does not belong to this VGDevice!");

            if (device != source.GraphicsDevice)
                throw new InvalidOperationException("Source does not belong to this VGDevice!");

            if (!targetRect.HasValue)
            {
                if (target != null)
                    targetRect = target.Bounds;
                else
                    targetRect = source.GraphicsDevice.PresentationParameters.Bounds;
           }


            var rtts = device.GetRenderTargets();
            var blend = new BlendState
            {
                AlphaDestinationBlend = Blend.Zero,
                AlphaSourceBlend = Blend.One,
                ColorDestinationBlend = Blend.Zero,
                ColorSourceBlend = Blend.One,
                ColorWriteChannels = channels
            };                      

            try
            {
                device.SetRenderTarget(target);
                _spriteBatch.Begin(SpriteSortMode.Immediate, blend, null, null, null, effect, Matrix.Identity);
                _spriteBatch.Draw(source, targetRect.Value, sourceRect, Color.White);
                _spriteBatch.End();
            }
            finally
            {
                device.SetRenderTargets(rtts);
            }
        }

        public void Dispose()
        {
            if (_spriteBatch != null)
            {
                _spriteBatch.Dispose();
                _spriteBatch = null;
            }
        }
    }
}
