using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
namespace Microsoft.Xna.Framework.Graphics
{
    public class Texture3D :Texture ,IDisposable
    {
        protected GraphicsDevice _parent;

        public int Depth
        {
            
            get
            {
                return this.depth;
            }
           
            private set
            {
                this.depth = value;
            }
        }

        public Texture3D(UnityEngine.Texture2D unityTexture)
        {
            // TODO: Complete member initialization
            this.unityTexture = unityTexture;
        }

        public Texture3D(GraphicsDevice graphicsDevice, int width, int height, int depth, bool mipMap, SurfaceFormat format)
        {
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException("graphicsDevice");
            }
            _parent = graphicsDevice;
            this.Width = width;
            this.Height = height;
            this.Depth = depth;
            this.LevelCount = (mipMap ? CalculateMipLevels(width, height, 0) : 1);
            this.Format = format;
            //this.texture = _parent.GLDevice.CreateTexture3D(format, width, height, depth, base.LevelCount);
            UnityEngine.Debug.Log("Texture3D");
        }

        private int depth;
       

        public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct
        {
            this.SetData<T>(0, 0, 0, this.Width, this.Height, 0, this.Depth, data, startIndex, elementCount);
        }

        public void SetData<T>(int level, int left, int top, int right, int bottom, int front, int back, T[] data, int startIndex, int elementCount) where T : struct
        {
            //this.ValidateParams<T>(level, left, top, right, bottom, front, back, data, startIndex, elementCount);
            int width = right - left;
            int height = bottom - top;
            int depth = back - front;
            this.PlatformSetData<T>(level, left, top, right, bottom, front, back, data, startIndex, elementCount, width, height, depth);
        }

        private void PlatformSetData<T>(int level, int left, int top, int right, int bottom, int front, int back, T[] data, int startIndex, int elementCount, int width, int height, int depth)
        {
            int num = 0;
            Debug.Log(" Texture3D SetData");
            //GCHandle gchandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            //try
            //{
            //    IntPtr datapointer = (IntPtr)(gchandle.AddrOfPinnedObject().ToInt64() + (long)(startIndex * num));
            //    int pitch = base.GetPitch(width);
            //    int slicePitch = pitch * height;

            //}
            //finally
            //{
            //    gchandle.Free();
            //}
            SetTexture<T>(data, width, height);

        }
    }
}

