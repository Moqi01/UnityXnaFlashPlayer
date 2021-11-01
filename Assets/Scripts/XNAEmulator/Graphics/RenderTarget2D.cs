using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Graphics
{
    // Token: 0x020000A1 RID: 161
    public class RenderTarget2D : Texture2D, IDynamicGraphicsResource
    {
        public RenderTarget2D(GraphicsDevice graphicsDevice, int width, int height, [MarshalAs(UnmanagedType.U1)] bool mipMap, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat, int preferredMultiSampleCount, RenderTargetUsage usage)
        {
            try
            {
                this.CreateRenderTarget(graphicsDevice, width, height, mipMap, preferredFormat, preferredDepthFormat, preferredMultiSampleCount, usage);
            }
            catch
            {
                base.Dispose(true);
                throw;
            }
        }

        // Token: 0x06000347 RID: 839 RVA: 0x000130FC File Offset: 0x000124FC
        public RenderTarget2D(GraphicsDevice graphicsDevice, int width, int height, [MarshalAs(UnmanagedType.U1)] bool mipMap, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat)
        {
            try
            {
                this.CreateRenderTarget(graphicsDevice, width, height, mipMap, preferredFormat, preferredDepthFormat, 0, RenderTargetUsage.DiscardContents);
            }
            catch
            {
                base.Dispose(true);
                throw;
            }
        }

        // Token: 0x06000348 RID: 840 RVA: 0x000130AC File Offset: 0x000124AC
        public RenderTarget2D(GraphicsDevice graphicsDevice, int width, int height)
        {
            try
            {
                this.CreateRenderTarget(graphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            }
            catch
            {
                base.Dispose(true);
                throw;
            }
        }

        // Token: 0x1700009C RID: 156
        // (get) Token: 0x0600034A RID: 842 RVA: 0x0000A918 File Offset: 0x00009D18
        public DepthFormat DepthStencilFormat
        {
            get
            {
                return this.helper.depthFormat;
            }
        }

        // Token: 0x1700009B RID: 155
        // (get) Token: 0x0600034B RID: 843 RVA: 0x0000A938 File Offset: 0x00009D38
        public int MultiSampleCount
        {
            get
            {
                return this.helper.multiSampleCount;
            }
        }

        // Token: 0x1700009A RID: 154
        // (get) Token: 0x0600034C RID: 844 RVA: 0x0000A958 File Offset: 0x00009D58
        public RenderTargetUsage RenderTargetUsage
        {
            get
            {
                return this.helper.usage;
            }
        }

        // Token: 0x0600034D RID: 845 RVA: 0x00012ED0 File Offset: 0x000122D0
        public void CreateRenderTarget(GraphicsDevice graphicsDevice, int width, int height, [MarshalAs(UnmanagedType.U1)] bool mipMap, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat, int preferredMultiSampleCount, RenderTargetUsage usage)
        {
            UnityEngine.Debug.Log("?");
            if (graphicsDevice == null)
            {
                UnityEngine.Debug.LogError("!");
            }
            Format= preferredFormat;
            DepthFormat depthFormat;
            int multiSampleCount;
            //graphicsDevice.Adapter.QueryFormat(false, graphicsDevice._deviceType, graphicsDevice._graphicsProfile, preferredFormat, preferredDepthFormat, preferredMultiSampleCount, out format, out depthFormat, out multiSampleCount);
            //Texture2D.ValidateCreationParameters(graphicsDevice._profileCapabilities, width, height, format, mipMap);
            //RenderTargetHelper renderTargetHelper = new RenderTargetHelper(this, width, height, format, depthFormat, multiSampleCount, usage, graphicsDevice._profileCapabilities);
            //this.helper = renderTargetHelper;
            //renderTargetHelper.CreateSurfaces(graphicsDevice);
            base.CreateTexture(graphicsDevice, width, height, mipMap, 1, (_D3DPOOL)0, Format);
            this.renderTargetContentsDirty = true;
        }

        internal void GetData(Color[] data)
        {
            throw new NotImplementedException();
        }

        // Token: 0x0600034E RID: 846 RVA: 0x000186B0 File Offset: 0x00017AB0
        //internal override int SaveDataForRecreation()
        //{
        //    return 0;
        //}

        //// Token: 0x0600034F RID: 847 RVA: 0x00012F64 File Offset: 0x00012364
        //internal override int RecreateAndPopulateObject()
        //{
        //    if (this.pComPtr != null)
        //    {
        //        return -2147467259;
        //    }
        //    int num = (this._levelCount > 1) ? 1 : 0;
        //    base.CreateTexture(this._parent, this._width, this._height, (byte)num != 0, 1, (_D3DPOOL)0, this._format);
        //    this.helper.CreateSurfaces(this._parent);
        //    this.renderTargetContentsDirty = true;
        //    return 0;
        //}

        //// Token: 0x06000350 RID: 848 RVA: 0x00012CA0 File Offset: 0x000120A0
        //internal override void ReleaseNativeObject([MarshalAs(UnmanagedType.U1)] bool disposeManagedResource)
        //{
        //    base.ReleaseNativeObject(disposeManagedResource);
        //    RenderTargetHelper renderTargetHelper = this.helper;
        //    if (renderTargetHelper != null)
        //    {
        //        renderTargetHelper.ReleaseNativeObject();
        //    }
        //}

        // Token: 0x06000351 RID: 849 RVA: 0x0000C1F0 File Offset: 0x0000B5F0
        void IDynamicGraphicsResource.SetContentLost([MarshalAs(UnmanagedType.U1)] bool isContentLost)
        {
            this._contentLost = isContentLost;
            if (isContentLost)
            {
               // this.raise_ContentLost(this, EventArgs.Empty);
            }
        }
        

        // Token: 0x14000010 RID: 16
        // (add) Token: 0x06000352 RID: 850 RVA: 0x0000A978 File Offset: 0x00009D78
        // (remove) Token: 0x06000353 RID: 851 RVA: 0x0000A9A4 File Offset: 0x00009DA4
        //public virtual event EventHandler<EventArgs> ContentLost
        //{
        //    [MethodImpl(MethodImplOptions.Synchronized)]
        //    add
        //    {
        //        this.< backing_store > ContentLost = (EventHandler<EventArgs>)Delegate.Combine(this.< backing_store > ContentLost, value);
        //    }
        //    [MethodImpl(MethodImplOptions.Synchronized)]
        //    remove
        //    {
        //        this.< backing_store > ContentLost = (EventHandler<EventArgs>)Delegate.Remove(this.< backing_store > ContentLost, value);
        //    }
        //}

        // Token: 0x17000099 RID: 153
        // (get) Token: 0x06000355 RID: 853 RVA: 0x0000C218 File Offset: 0x0000B618
        public bool IsContentLost
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                if (!this._contentLost)
                {
                    UnityEngine.Debug.Log("!");
                    this._contentLost = this._parent.IsDeviceLost;
                }
                return this._contentLost;
            }
        }

        // Token: 0x06000356 RID: 854 RVA: 0x0000A9F4 File Offset: 0x00009DF4
        
        protected override void Dispose([MarshalAs(UnmanagedType.U1)] bool A_1)
        {
            if (A_1)
            {
                try
                {
                    return;
                }
                finally
                {
                    base.Dispose(true);
                }
            }
            base.Dispose(false);
        }

        // Token: 0x04000211 RID: 529
        internal RenderTargetHelper helper;
        protected GraphicsDevice _parent;
        // Token: 0x04000212 RID: 530
        internal bool _contentLost;
        private bool renderTargetContentsDirty;
        
        public event EventHandler<EventArgs> ContentLost;

        // Token: 0x04000213 RID: 531
        //private EventHandler<EventArgs> <backing_store>ContentLost;
    }
    public enum _D3DPOOL
    {
        node=0,
    }
    public interface IDynamicGraphicsResource
    {
        // Token: 0x1400000C RID: 12
        // (add) Token: 0x0600011E RID: 286
        // (remove) Token: 0x0600011F RID: 287
        event EventHandler<EventArgs> ContentLost;

        // Token: 0x1700001D RID: 29
        // (get) Token: 0x06000120 RID: 288
        bool IsContentLost { [return: MarshalAs(UnmanagedType.U1)] get; }

        // Token: 0x06000121 RID: 289
         void SetContentLost([MarshalAs(UnmanagedType.U1)] bool isContentLost);
    }
    public class RenderTargetHelper
    {
        internal DepthFormat depthFormat;
        internal int multiSampleCount;
        internal RenderTargetUsage usage;
    }
}
