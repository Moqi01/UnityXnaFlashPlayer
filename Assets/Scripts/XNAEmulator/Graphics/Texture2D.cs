using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
namespace Microsoft.Xna.Framework.Graphics
{
	public class Texture2D : Texture, IDisposable
	{
        public Texture2D()
        {

        }

        public Texture2D(UnityEngine.Texture2D unityTexture)
        {
            // TODO: Complete member initialization
            this.UnityTexture = unityTexture;
            m_width = unityTexture.width;
            m_height = unityTexture.height ;
            Debug.Log("New Texture2D no graphicsDevice");
        }

        public Texture2D(GraphicsDevice graphicsDevice, int width, int height)
        {
            m_width = width;
            m_height = height;
            this._parent = graphicsDevice;
            Debug.Log("New Texture2D");
        }

        public Texture2D(GraphicsDevice graphicsDevice, int width, int height, [MarshalAs(UnmanagedType.U1)] bool mipMap, SurfaceFormat format)
        {
            try
            {
                this.CreateTexture(graphicsDevice, width, height, mipMap, 0, (_D3DPOOL)1, format);
            }
            catch
            {
               // base.Dispose(true);
                throw;
            }
        }

        internal static Texture2D FromStream(GraphicsDevice graphicsDevice, MemoryStream memoryStream)
        {
            Texture2D texture = new Texture2D();
            byte[] date = memoryStream.ToArray();
            texture.SetData(date, 0, date.Length);
            return texture;
        }

        private void PlatformSetData<T>(int level, int arraySlice, Rectangle rect, T[] data, int startIndex, int elementCount) where T : struct
        {
            Debug.Log("PlatformSetData");
            unityTexture.SetPixelData<T>(data ,level);
        }
        public void SetData<T>(int level, int arraySlice, Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct
        {
            Rectangle rect2;
            this.ValidateParams<T>(level, arraySlice, rect, data, startIndex, elementCount, out rect2);
            this.PlatformSetData<T>(level, arraySlice, rect2, data, startIndex, elementCount);
        }

        public void SetData<T>(int level, Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct
        {
            this.SetData<T>(level, 0, rect, data, startIndex, elementCount);
        }

        // Token: 0x06000C10 RID: 3088 RVA: 0x00034018 File Offset: 0x00032218
        public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct
        {
            this.SetData<T>(0, null, data, startIndex, elementCount);
        }

        public void SetData(byte[] data, int startIndex, int elementCount) 
        {
            byte[] dataNew ;
            if (data .Length >elementCount)
            {
                dataNew = new byte[elementCount];
                for (int i = startIndex; i < elementCount; i++)
                    dataNew[i] = data[i];
            }
            else
            {
                dataNew = data;
            }
            
            if (Format==SurfaceFormat.Color)
            {
                //unityTexture = new UnityEngine.Texture2D(m_width, m_height, TextureFormat.ARGB4444, MipMap);
                unityTexture = new UnityEngine.Texture2D(m_width, m_height, TextureFormat.RGBA32, MipMap);
                //byte[] t = ImageConversion.EncodeArrayToPNG(dataNew, UnityEngine.Experimental.Rendering.GraphicsFormat.None, (uint)m_width, (uint)m_height);
                //unityTexture.LoadImage(t);
            }
            else if (Format == SurfaceFormat.Alpha8)
            {
                unityTexture = new UnityEngine.Texture2D(m_width, m_height, TextureFormat.R8, MipMap);
                //unityTexture = new UnityEngine.Texture2D(m_width, m_height, TextureFormat.Alpha8, MipMap);
            }
            unityTexture.SetPixelData(dataNew, 0);
            unityTexture.filterMode = FilterMode.Point;
            unityTexture.Apply(updateMipmaps: true);
            //OutputRt(unityTexture.EncodeToPNG(), num.ToString ());
        }

        internal void GetData(uint[] data)
        {
            //data=(uint[]) this.UnityTexture.EncodeToPNG();
        }

        internal void GetData(byte[] data)
        {
            data = this.UnityTexture.EncodeToPNG();

        }

        static int num = 100; 
        public static void OutputRt(byte[] dataBytes, string idx = "0")
        {
            if (num > 0)
            {
                string strSaveFile = Application.dataPath+"/../" + "/texture/" + System.DateTime.Now.Minute + "_" + System.DateTime.Now.Second + "_" + idx + ".png";
                FileStream fs = File.Open(strSaveFile, FileMode.OpenOrCreate);
                fs.Write(dataBytes, 0, dataBytes.Length);
                fs.Flush();
                fs.Close();
                num--;
            }
        }


        public void SetData(Color[] data)
        {
            if (data == null)
            {
                Debug.Log("SetData(Color[] data data null)");
                throw new ArgumentNullException("data");
            }
           
            this.UnityTexture.SetPixels(GetColors(data));
        }

        protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_1)
        {

        }

        public void SaveAsJpeg(Stream stream, int width, int height)
        {
            // this.SaveAsImage(stream, (XnaImageFormat)0, width, height);
            Debug.Log(" SaveAsJpeg");
        }

        // Token: 0x0600022A RID: 554 RVA: 0x00016928 File Offset: 0x00015D28
        public void SaveAsPng(Stream stream, int width, int height)
        {
            //this.SaveAsImage(stream, (XnaImageFormat)2, width, height);
            Debug.Log("SaveAsPng");
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(0, 0, this.Width, this.Height);
            }
        }

        public void SetData<T>(T[] data,string name="",int index=0) where T : struct
        {
            //Color[] mydata;
            // Rectangle? rect = null;
            //int elementCount;
            //if (data != null)
            //{
            //    elementCount = data.Length;
            //}
            //else
            //{
            //    elementCount = 0;
            //}
            //this.SetData<T>(0, rect, data, 0, elementCount);
            SetTexture<T>(data, m_width, m_height, name,  index );
        }

        public void GetData<T>(int level, Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct
        {
            this.GetData<T>(level, 0, rect, data, startIndex, elementCount);
        }

        public void GetData<T>(int level, int arraySlice, Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct
        {
            Debug.Log(" Texture2D GetData");
            Rectangle rect2;
            this.ValidateParams<T>(level, arraySlice, rect, data, startIndex, elementCount, out rect2);
            this.PlatformGetData<T>(level, arraySlice, rect2, data, startIndex, elementCount);
        }

        private void PlatformGetData<T>(int level, int arraySlice, Rectangle rect, T[] data, int startIndex, int elementCount) where T : struct
        {
            int val = this.Format == SurfaceFormat.Rgb32 ? 4 : 1;
            int num = Math.Max(this.m_width >> level, val);
            int num2 = Math.Max(this.m_height >> level, val);

        }

        private void ValidateParams<T>(int level, int arraySlice, Rectangle? rect, T[] data, int startIndex, int elementCount, out Rectangle checkedRect) where T : struct
        {
            Rectangle rectangle = new Rectangle(0, 0, Math.Max(this.m_width >> level, 1), Math.Max(this.m_height >> level, 1));
            checkedRect = (rect ?? rectangle);
        }

        public void SetData<T>(int level, Rectangle? rect, T[] data, int startIndex, int elementCount,string name,int index) where T : struct
        {
            string asset = "texture\\" + name + "(" + index + ")";
            asset = asset.Replace("\\", "/");
           // UnityEngine.Object a = Resources.Load(asset);
            this.unityTexture = Resources.Load<UnityEngine.Texture2D>(asset);
           // this.unityTexture = a as UnityEngine.Texture2D;
           // this.CopyData<T>(level, rect, data, startIndex, elementCount, 0, true);
        }

        private void CopyData<T>(int level, Rectangle? rect, T[] data, int startIndex, int elementCount, uint options, [MarshalAs(UnmanagedType.U1)] bool isSetting) where T : struct
        {
            Debug.Log(" Texture2D SetData");
            SetTexture<T>(data, m_width, m_height,rect);
        }
        private bool MipMap;
        public void CreateTexture(GraphicsDevice graphicsDevice, int width, int height, bool mipMap, int v, _D3DPOOL d3DPOOL, SurfaceFormat format)
        {
            m_width = width;
            m_height = height;
            //Debug.Log("CreateTexture New Texture2D");
            this._parent = graphicsDevice;
            Format = format;
            MipMap = mipMap;
        }
        protected GraphicsDevice _parent;
       
    }
}
