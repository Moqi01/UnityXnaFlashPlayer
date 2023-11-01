using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Microsoft.Xna.Framework.Graphics
{
    // Token: 0x020001EC RID: 492
    public abstract class Texture : GraphicsResource
    {
        // Token: 0x170002A6 RID: 678
        // (get) Token: 0x06000E92 RID: 3730 RVA: 0x00030C88 File Offset: 0x0002EE88
        // (set) Token: 0x06000E93 RID: 3731 RVA: 0x00030C90 File Offset: 0x0002EE90

        public UnityEngine.Texture unityTexture;
        public UnityEngine.Texture2D unityTexture2D
        {
            get
            {
                return (unityTexture as UnityEngine. Texture2D);
            }
        }

        public UnityEngine.Texture UnityTexture
        {
            get
            {
                if (unityTexture == null)
                    unityTexture= new UnityEngine.Texture2D(m_width, m_height);
                return unityTexture;
            }
            set { unityTexture = value; }
        }
        public int Width
        {
            get {
                //if (unityTexture != null)
                    //return unityTexture.width;
                    return m_width; }
            set { }
        }
        public int Height
        {
            get {
                //if (unityTexture != null)
                    //return unityTexture.height;
                return m_height; }
            set { }
        }

        public int m_width;
        public int m_height;

        public SurfaceFormat Format
        {
            [CompilerGenerated]
            get
            {
                return this.format;
            }
            [CompilerGenerated]
            protected set
            {
                this.format = value;
            }
        }

        // Token: 0x170002A7 RID: 679
        // (get) Token: 0x06000E94 RID: 3732 RVA: 0x00030C99 File Offset: 0x0002EE99
        // (set) Token: 0x06000E95 RID: 3733 RVA: 0x00030CA1 File Offset: 0x0002EEA1
        public int LevelCount
        {
            [CompilerGenerated]
            get
            {
                return this.levelCount;
            }
            [CompilerGenerated]
            protected set
            {
                this.levelCount = value;
            }
        }

        // Token: 0x06000E96 RID: 3734 RVA: 0x00030CAA File Offset: 0x0002EEAA
        //protected override void Dispose(bool disposing)
        //{
        //    if (!base.IsDisposed)
        //    {
        //        base.GraphicsDevice.GLDevice.AddDisposeTexture(this.texture);
        //    }
        //    base.Dispose(disposing);
        //}

        //// Token: 0x06000E97 RID: 3735 RVA: 0x00030CD1 File Offset: 0x0002EED1
        //protected internal override void GraphicsDeviceResetting()
        //{
        //}

        // Token: 0x06000E98 RID: 3736 RVA: 0x00030CD4 File Offset: 0x0002EED4
        internal static int GetFormatSize(SurfaceFormat format)
        {
            switch (format)
            {
                case SurfaceFormat.Color:
                case SurfaceFormat.NormalizedByte4:
                case SurfaceFormat.Rgba1010102:
                case SurfaceFormat.Rg32:
                case SurfaceFormat.Single:
                case SurfaceFormat.HalfVector2:
                case SurfaceFormat.ColorBgraEXT:
                    return 4;
                case SurfaceFormat.Bgr565:
                case SurfaceFormat.Bgra5551:
                case SurfaceFormat.Bgra4444:
                case SurfaceFormat.NormalizedByte2:
                case SurfaceFormat.HalfSingle:
                    return 2;
                case SurfaceFormat.Dxt1:
                    return 8;
                case SurfaceFormat.Dxt3:
                case SurfaceFormat.Dxt5:
                    return 16;
                case SurfaceFormat.Rgba64:
                case SurfaceFormat.Vector2:
                case SurfaceFormat.HalfVector4:
                    return 8;
                case SurfaceFormat.Alpha8:
                    return 1;
                case SurfaceFormat.Vector4:
                    return 16;
            }
            throw new ArgumentException("Should be a value defined in SurfaceFormat", "Format");
        }

        // Token: 0x06000E99 RID: 3737 RVA: 0x00030D60 File Offset: 0x0002EF60
        internal static int CalculateMipLevels(int width, int height = 0, int depth = 0)
        {
            int num = 1;
            int i = Math.Max(Math.Max(width, height), depth);
            while (i > 1)
            {
                i /= 2;
                num++;
            }
            return num;
        }

        // Token: 0x06000E9A RID: 3738 RVA: 0x00030D8C File Offset: 0x0002EF8C
        protected Texture()
        {
        }

        internal void SetTexture<T>(T[] data, int width, int height, string name = "", int index = 0)
        {
            m_width = width;
            m_height = height;
            if (data is Color[])
            {
                Color[] mydata = (Color[])Convert.ChangeType(data, typeof(Color[]));
               UnityEngine. Texture2D texture2D = unityTexture as UnityEngine. Texture2D;
                texture2D.SetPixels(GetColors(mydata));
                texture2D.Apply();

            }
            else if (data is uint[])
            {
                uint[] mydata = (uint[])Convert.ChangeType(data, typeof(uint[]));
                UnityEngine.Texture2D texture2D = unityTexture as UnityEngine.Texture2D;
                texture2D.SetPixels(GetABGRColors(mydata));
                texture2D.Apply();
            }
           else if (data is byte[])
            {
                byte[] mydata = (byte[])Convert.ChangeType(data, typeof(byte[]));
                UnityEngine.Texture2D texture2D = unityTexture as UnityEngine.Texture2D;
                texture2D.LoadRawTextureData(mydata);//流数据转换成Texture2D
                texture2D.Apply();

                //this.UnityTexture = texture;
                //string asset = "texture\\" + name + "(" + index + ")";
                //asset = asset.Replace("\\", "/");
                // UnityEngine.Object a = Resources.Load(asset);
                //this.unityTexture = Resources.Load<UnityEngine.Texture2D>(asset);
            }
            else
                UnityEngine.Debug.Log("SetData<T> T:" + typeof(T).Name);
        }

        internal void SetTexture<T>(T[] data, int width, int height, Rectangle? rect)
        {
            m_width = rect.Value .Width;
            m_height = rect.Value.Height;
            if (data is Color[])
            {
                Color[] mydata = (Color[])Convert.ChangeType(data, typeof(Color[]));
                UnityEngine.Texture2D texture2D = unityTexture as UnityEngine.Texture2D;

                texture2D.SetPixels(GetColors(mydata));
            }
            if (data is byte[])
            {
                byte[] mydata = (byte[])Convert.ChangeType(data, typeof(byte[]));
                UnityEngine.Texture2D texture = new UnityEngine.Texture2D(m_width, m_height);
                texture.LoadImage(mydata);//流数据转换成Texture2D

                // this.UnityTexture.SetPixel(0,0, texture.GetPixel(rect.Value.X, rect.Value.Y));
                this.UnityTexture = texture;
            }
            else
                UnityEngine.Debug.Log("SetData<T> T:" + typeof(T).Name);
        }

        public UnityEngine.Color[] GetColors(Color[] data)
        {
            UnityEngine.Color[] color = new UnityEngine.Color[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                color[i].a = data[i].A;
                color[i].b = data[i].B;
                color[i].g = data[i].G;
                color[i].r = data[i].R;
            }
            return color;
        }

        public UnityEngine.Color[] GetABGRColors(uint[] data)
        {
            UnityEngine.Color[] color = new UnityEngine.Color[data.Length];
            for (int i = data.Length - 1; i >= 0; i--)
            //for (int i = data.Length - 1; i >= 0; i--)
            {
                color[i].a = (data[i] >> 24) / 255f;
                color[i].b = ((data[i] & 0x00FF0000) >> 16) / 255f;
                color[i].g = ((data[i] & 0x0000FF00) >> 8) / 255f;
                color[i].r = (data[i] & 0x000000FF) / 255f;
            }
            return color;
        }


        // Token: 0x0400076F RID: 1903
        internal IGLTexture texture;

        // Token: 0x04000770 RID: 1904
        [CompilerGenerated]
        private SurfaceFormat format;

        // Token: 0x04000771 RID: 1905
        [CompilerGenerated]
        private int levelCount;
	}

   

    internal interface IGLTexture
    {
    }

}
