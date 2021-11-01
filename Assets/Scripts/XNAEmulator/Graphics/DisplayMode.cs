using System;
using System.Globalization;

namespace Microsoft.Xna.Framework.Graphics
{
    // Token: 0x02000119 RID: 281
    public class DisplayMode
    {
        // Token: 0x06000575 RID: 1397 RVA: 0x0003D8C4 File Offset: 0x0003CCC4
        public DisplayMode(int width, int height, SurfaceFormat format)
        {
            this._width = width;
            this._height = height;
            this._format = format;
        }

        // Token: 0x17000138 RID: 312
        // (get) Token: 0x06000576 RID: 1398 RVA: 0x0003D8EC File Offset: 0x0003CCEC
        public SurfaceFormat Format
        {
            get
            {
                return this._format;
            }
        }

        // Token: 0x17000139 RID: 313
        // (get) Token: 0x06000577 RID: 1399 RVA: 0x0003D900 File Offset: 0x0003CD00
        public int Height
        {
            get
            {
                return this._height;
            }
        }

        // Token: 0x1700013A RID: 314
        // (get) Token: 0x06000578 RID: 1400 RVA: 0x0003D914 File Offset: 0x0003CD14
        public int Width
        {
            get
            {
                return this._width;
            }
        }

        // Token: 0x1700013B RID: 315
        // (get) Token: 0x06000579 RID: 1401 RVA: 0x0003D928 File Offset: 0x0003CD28
        public float AspectRatio
        {
            get
            {
                if (this._height == 0 || this._width == 0)
                {
                    return 0f;
                }
                return (float)this._width / (float)this._height;
            }
        }

        // Token: 0x1700013C RID: 316
        // (get) Token: 0x0600057A RID: 1402 RVA: 0x0003D95C File Offset: 0x0003CD5C
        public Rectangle TitleSafeArea
        {
            get
            {
                return Viewport.GetTitleSafeArea(0, 0, this._width, this._height);
            }
        }

        // Token: 0x0600057B RID: 1403 RVA: 0x0003D97C File Offset: 0x0003CD7C
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{{Width:{0} Height:{1} Format:{2} AspectRatio:{3}}}", new object[]
            {
                this._width,
                this._height,
                this.Format,
                this.AspectRatio
            });
        }

        // Token: 0x0400034E RID: 846
        internal int _width;

        // Token: 0x0400034F RID: 847
        internal int _height;

        // Token: 0x04000350 RID: 848
        internal SurfaceFormat _format;
    }
}
