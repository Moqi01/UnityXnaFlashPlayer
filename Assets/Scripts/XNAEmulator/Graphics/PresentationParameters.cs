using System;

namespace Microsoft.Xna.Framework.Graphics
{
    // Token: 0x0200011E RID: 286
    public class PresentationParameters
    {
        // Token: 0x0600059C RID: 1436 RVA: 0x0003E01C File Offset: 0x0003D41C
        public PresentationParameters()
        {
            settings = new Settings(0);
        }

        // Token: 0x0600059D RID: 1437 RVA: 0x0003E038 File Offset: 0x0003D438
        public PresentationParameters Clone()
        {
            return new PresentationParameters
            {
                settings = this.settings,
                BackBufferWidth = this.BackBufferWidth ,
                BackBufferHeight = this.BackBufferHeight,
        };
        }

        // Token: 0x1700014A RID: 330
        // (get) Token: 0x0600059E RID: 1438 RVA: 0x0003E058 File Offset: 0x0003D458
        // (set) Token: 0x0600059F RID: 1439 RVA: 0x0003E070 File Offset: 0x0003D470
        public int BackBufferWidth
        {
            get
            {
                return this.settings.BackBufferWidth;
            }
            set
            {
                this.settings.BackBufferWidth = value;
            }
        }

        // Token: 0x1700014B RID: 331
        // (get) Token: 0x060005A0 RID: 1440 RVA: 0x0003E08C File Offset: 0x0003D48C
        // (set) Token: 0x060005A1 RID: 1441 RVA: 0x0003E0A4 File Offset: 0x0003D4A4
        public int BackBufferHeight
        {
            get
            {
                return this.settings.BackBufferHeight;
            }
            set
            {
                this.settings.BackBufferHeight = value;
            }
        }

        // Token: 0x1700014C RID: 332
        // (get) Token: 0x060005A2 RID: 1442 RVA: 0x0003E0C0 File Offset: 0x0003D4C0
        // (set) Token: 0x060005A3 RID: 1443 RVA: 0x0003E0D8 File Offset: 0x0003D4D8
        public SurfaceFormat BackBufferFormat
        {
            get
            {
                return this.settings.BackBufferFormat;
            }
            set
            {
                this.settings.BackBufferFormat = value;
            }
        }

        // Token: 0x1700014D RID: 333
        // (get) Token: 0x060005A4 RID: 1444 RVA: 0x0003E0F4 File Offset: 0x0003D4F4
        // (set) Token: 0x060005A5 RID: 1445 RVA: 0x0003E10C File Offset: 0x0003D50C
        public DepthFormat DepthStencilFormat
        {
            get
            {
                return this.settings.DepthStencilFormat;
            }
            set
            {
                this.settings.DepthStencilFormat = value;
            }
        }

        // Token: 0x1700014E RID: 334
        // (get) Token: 0x060005A6 RID: 1446 RVA: 0x0003E128 File Offset: 0x0003D528
        // (set) Token: 0x060005A7 RID: 1447 RVA: 0x0003E140 File Offset: 0x0003D540
        public int MultiSampleCount
        {
            get
            {
                return this.settings.MultiSampleCount;
            }
            set
            {
                this.settings.MultiSampleCount = value;
            }
        }

        // Token: 0x1700014F RID: 335
        // (get) Token: 0x060005A8 RID: 1448 RVA: 0x0003E15C File Offset: 0x0003D55C
        // (set) Token: 0x060005A9 RID: 1449 RVA: 0x0003E174 File Offset: 0x0003D574
        public DisplayOrientation DisplayOrientation
        {
            get
            {
                return this.settings.DisplayOrientation;
            }
            set
            {
                this.settings.DisplayOrientation = value;
            }
        }

        // Token: 0x17000150 RID: 336
        // (get) Token: 0x060005AA RID: 1450 RVA: 0x0003E190 File Offset: 0x0003D590
        // (set) Token: 0x060005AB RID: 1451 RVA: 0x0003E1A8 File Offset: 0x0003D5A8
        public PresentInterval PresentationInterval
        {
            get
            {
                return this.settings.PresentationInterval;
            }
            set
            {
                this.settings.PresentationInterval = value;
            }
        }

        // Token: 0x17000151 RID: 337
        // (get) Token: 0x060005AC RID: 1452 RVA: 0x0003E1C4 File Offset: 0x0003D5C4
        // (set) Token: 0x060005AD RID: 1453 RVA: 0x0003E1DC File Offset: 0x0003D5DC
        public RenderTargetUsage RenderTargetUsage
        {
            get
            {
                return this.settings.RenderTargetUsage;
            }
            set
            {
                this.settings.RenderTargetUsage = value;
            }
        }

        // Token: 0x17000152 RID: 338
        // (get) Token: 0x060005AE RID: 1454 RVA: 0x0003E1F8 File Offset: 0x0003D5F8
        // (set) Token: 0x060005AF RID: 1455 RVA: 0x0003E210 File Offset: 0x0003D610
        public IntPtr DeviceWindowHandle
        {
            get
            {
                return this.settings.DeviceWindowHandle;
            }
            set
            {
                this.settings.DeviceWindowHandle = value;
            }
        }

        // Token: 0x17000153 RID: 339
        // (get) Token: 0x060005B0 RID: 1456 RVA: 0x0003E22C File Offset: 0x0003D62C
        // (set) Token: 0x060005B1 RID: 1457 RVA: 0x0003E24C File Offset: 0x0003D64C
        public bool IsFullScreen
        {
            get
            {
                return this.settings.IsFullScreen != 0;
            }
            set
            {
                this.settings.IsFullScreen = (value ? 1 : 0);
            }
        }

        // Token: 0x17000154 RID: 340
        // (get) Token: 0x060005B2 RID: 1458 RVA: 0x0003E26C File Offset: 0x0003D66C
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(0, 0, this.settings.BackBufferWidth, this.settings.BackBufferHeight);
            }
        }

        // Token: 0x0400035B RID: 859
        internal PresentationParameters.Settings settings;

        // Token: 0x0200011F RID: 287
        internal struct Settings
        {
            public Settings (int a)
            {
                BackBufferWidth = UnityEngine.Screen.width;
                BackBufferHeight = UnityEngine.Screen.height;
                BackBufferFormat = SurfaceFormat.Rgba32;
                DepthStencilFormat = DepthFormat.Depth32;
                MultiSampleCount = 0;
                DisplayOrientation = DisplayOrientation.Default;
                PresentationInterval = PresentInterval.Default;
                RenderTargetUsage = RenderTargetUsage.PlatformContents;
                DeviceWindowHandle = IntPtr.Zero;
                IsFullScreen = 1;
            }
            // Token: 0x0400035C RID: 860
            public int BackBufferWidth;

            // Token: 0x0400035D RID: 861
            public int BackBufferHeight;

            // Token: 0x0400035E RID: 862
            public SurfaceFormat BackBufferFormat;

            // Token: 0x0400035F RID: 863
            public DepthFormat DepthStencilFormat;

            // Token: 0x04000360 RID: 864
            public int MultiSampleCount;

            // Token: 0x04000361 RID: 865
            public DisplayOrientation DisplayOrientation;

            // Token: 0x04000362 RID: 866
            public PresentInterval PresentationInterval;

            // Token: 0x04000363 RID: 867
            public RenderTargetUsage RenderTargetUsage;

            // Token: 0x04000364 RID: 868
            public IntPtr DeviceWindowHandle;

            // Token: 0x04000365 RID: 869
            public int IsFullScreen;
        }
    }
}
