using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Microsoft.Xna.Framework.Graphics
{
    public class GraphicsDevice
    {
        Viewport viewport = new Viewport();
        private DrawQueue drawQueue;
        public bool IsDeviceLost;
        public event EventHandler<EventArgs> DeviceReset;
        public SamplerStateCollection SamplerStates { get; private set; }
        public GraphicsDeviceStatus GraphicsDeviceStatus;
        public Color BlendFactor;
        public int MultiSampleMask;
        public int ReferenceStencil;
        public DisplayMode DisplayMode;
        public GraphicsAdapter Adapter;

        internal void DrawPrimitives(PrimitiveType triangleList, int vertexOffset, int triangleCount)
        {
            //Debug.Log("DrawPrimitives");
            DrawGL.ins.SetPrimitiveTypes(triangleList);
            DrawGL.ins.SetVectors ( m_VertexBuffer.vertices);
            
        }
        private VertexBuffer m_VertexBuffer;

        public void SetVertexBuffer(VertexBuffer _bufferedGlyphs)
        {
            m_VertexBuffer = _bufferedGlyphs;

        }

        public DrawQueue DrawQueue
        {
            get { return drawQueue; }
            set { drawQueue = value; }
        }

        public bool isDisposed;
        public bool IsDisposed
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                return this.isDisposed;
            }
        }

        public void SetRenderTarget(RenderTarget2D renderTarget)
        {
            //UnityEngine.Debug.Log("SetRenderTarget");
            if (renderTarget != null)
            {
                renderTarget.GraphicsDevice = this;
                RenderTargetBinding renderTargetBinding = new RenderTargetBinding(renderTarget);
                this.SetRenderTargets(new RenderTargetBinding[] { renderTargetBinding }, 1);

                UnityEngine.Graphics.SetRenderTarget(renderTarget.UnityTexture as RenderTexture);
                GL.Clear(true, true, UnityEngine.Color.green);
                //GL.PushMatrix();
                GL.LoadPixelMatrix(0, renderTarget.UnityTexture.width, renderTarget.UnityTexture.height, 0);
            }
            else
            {
                //this.SetRenderTargets(null, 0);
                //GL.PopMatrix();
                UnityEngine.Graphics.SetRenderTarget(null);
                //  GL.PushMatrix();
                GL.LoadPixelMatrix(0, Screen.width, Screen.height, 0);
            }
        }
        private RenderTargetBinding[] currentRenderTargetBindings;
        private int currentRenderTargetCount=1;
        
        internal VertexBufferBinding[] currentVertexBuffers;

        // Token: 0x040000D0 RID: 208
        internal int currentVertexBufferCount=1;

        internal VertexBufferBinding[] GetVertexBuffers()
        {
            int num = this.currentVertexBufferCount;
            VertexBufferBinding[] array = new VertexBufferBinding[num];
            //Array.Copy(this.currentVertexBuffers, array, num);
            return array;
        }

        internal void OnDeviceReset()
        {
            if (this.DeviceReset != null)
            {
                this.DeviceReset(this, EventArgs.Empty);
            }
        }

        internal void SetRenderTargets(RenderTargetBinding[] renderTargets,int i=1)
        {
            currentRenderTargetBindings = renderTargets;
        }

        internal RenderTargetBinding[] GetRenderTargets()
        {
            int num = this.currentRenderTargetCount;
            RenderTargetBinding[] array = new RenderTargetBinding[num];
            //Array.Copy(this.currentRenderTargetBindings, array, num);
            return array;
        }

        internal void SetVertexBuffers(VertexBufferBinding[] vertexBuffers)
        {
            currentVertexBuffers = vertexBuffers;
        }

        public void DrawUserPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int primitiveCount) where T : struct, IVertexType
        {
            // this.DrawUserPrimitives<T>(primitiveType, vertexData, vertexOffset, primitiveCount, VertexDeclarationCache<T>.VertexDeclaration);
            UnityEngine.Debug.Log("DrawUserPrimitives");
           

        }

        internal void Reset(PresentationParameters parameters)
        {
            PresentationParameters = parameters.Clone();
        }

        internal void DrawUserPrimitives(PrimitiveType triangleStrip, Vector2[] rectangle, int v1, int v2, VertexDeclaration solidDecl)
        {
            //DrawGL.ins.SetVectors(rectangle);
        }

        internal void Clear(ClearOptions clearOptions, Vector4 vector4, float v1, int v2)
        {
            //GL.Clear(true, true, new UnityEngine.Color(vector4.X, vector4.Y, vector4.Z, vector4.W));
            DrawGL.ins.Clear(new UnityEngine.Color(vector4.X, vector4.Y, vector4.Z, vector4.W));
        }



        public GraphicsDevice(DrawQueue drawQueue, PresentationParameters presentationParameters=null)
        {
            // TODO: Complete member initialization
            this.drawQueue = drawQueue;
            if (presentationParameters == null)
            {
                UnityEngine.Debug.Log("PresentationParameters");
                presentationParameters = new PresentationParameters();
                presentationParameters.BackBufferWidth = UnityEngine.Screen.width;
                presentationParameters.BackBufferHeight = UnityEngine.Screen.height;
            }
            pPublicCachedParams = presentationParameters.Clone();
            viewport = new Viewport(0, 0, UnityEngine.Screen.width, UnityEngine.Screen.height);
            pTextureCollection = new TextureCollection(this,4,false);
            SamplerStates = new SamplerStateCollection(this, 4, false);
        }

        internal void DrawIndexedPrimitives(PrimitiveType primitiveType, int v1, int v2, int vertexCount, int startVertex, int primitiveCount)
        {
            throw new NotImplementedException();
        }

        public GraphicsDevice(GraphicsAdapter Adapter, GraphicsProfile Profile, PresentationParameters parameters)
        {
            this.drawQueue = new DrawQueue();
            SamplerStates = new SamplerStateCollection(this, 4, false);
            this.Adapter = Adapter;
            DisplayMode =Adapter.CurrentDisplayMode;
            pPublicCachedParams = parameters.Clone();
            _graphicsProfile = Profile;
        }

        public Viewport Viewport
        {
            get { return viewport; }
            set { viewport = value; }
        }


        public  void Clear(Color color)
        {
            GL.Clear(true, true, UnityEngine.Color.yellow);

        }
        private TextureCollection pTextureCollection ;
        public TextureCollection Textures
        {
            get
            {
                if (pTextureCollection == null)
                    pTextureCollection = new TextureCollection(this, 4, false);
                return this.pTextureCollection;
            }
        }

        public GraphicsProfile _graphicsProfile;
        public BlendState BlendState;
        public DepthStencilState DepthStencilState;
        public RasterizerState RasterizerState;

        public GraphicsProfile GraphicsProfile
        {
            get
            {
                return this._graphicsProfile;
            }
        }

        public Rectangle ScissorRectangle
        {
            get
            {
                return new Rectangle();
            }
            set
            {

            }
        }

        public  void Dispose()
        {
            //this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private PresentationParameters pPublicCachedParams;
        public PresentationParameters PresentationParameters
        {
            get
            {
                return this.pPublicCachedParams;
            }
            set { pPublicCachedParams = value; }
        }

        public IndexBuffer Indices { get; internal set; }

        internal void Present(Rectangle sourceRectangle, object p, IntPtr handle)
        {
            ResetPools();
            if (lastMaterial != null)
                lastMaterial.SetPass(0);
        }
        private Material lastMaterial;
        //private DynamicAtlas atlas;
        public void ResetPools()
        {
            //_materialPool.Reset();
            _meshPool.Reset();
        }
        private readonly MeshPool _meshPool = new MeshPool();
        public Material GetMat(Texture2D text)
        {
            return _materialPool.Get(text);
        }

        private readonly MaterialPool _materialPool = new MaterialPool();
        public MeshHolder GetMesh(int primCCount)
        {
            return _meshPool.Get(primCCount);
        }

       
    }
    public class MeshHolder
    {
        public readonly int SpriteCount;
        public readonly Mesh Mesh;

        public readonly UnityEngine.Vector3[] Vertices;
        public readonly UnityEngine.Vector2[] UVs;
        public readonly Color32[] Colors;

        public MeshHolder(int spriteCount)
        {
            Mesh = new Mesh();
            Mesh.MarkDynamic(); //Seems to be a win on wp8

            SpriteCount = NextPowerOf2(spriteCount);
            int vCount = SpriteCount * 4;

            Vertices = new UnityEngine.Vector3[vCount];
            UVs = new UnityEngine.Vector2[vCount];
            Colors = new Color32[vCount];

            //Put some random crap in this so we can just set the triangles once
            //if these are not populated then unity totally fucks up our mesh and never draws it
            for (var i = 0; i < vCount; i++)
            {
                Vertices[i] = new UnityEngine.Vector3(1, i);
                UVs[i] = new UnityEngine.Vector2(0, i);
                Colors[i] = new Color32(255, 255, 255, 255);
            }

            var triangles = new int[SpriteCount * 6];
            for (var i = 0; i < SpriteCount; i++)
            {
                /*
                 *  TL    TR
                 *   0----1 0,1,2,3 = index offsets for vertex indices
                 *   |   /| TL,TR,BL,BR are vertex references in SpriteBatchItem.
                 *   |  / |
                 *   | /  |
                 *   |/   |
                 *   2----3
                 *  BL    BR
                 */
                // Triangle 1
                triangles[i * 6 + 0] = i * 4;
                triangles[i * 6 + 1] = i * 4 + 1;
                triangles[i * 6 + 2] = i * 4 + 2;
                // Triangle 2
                triangles[i * 6 + 3] = i * 4 + 1;
                triangles[i * 6 + 4] = i * 4 + 3;
                triangles[i * 6 + 5] = i * 4 + 2;
            }

            Mesh.vertices = Vertices;
            Mesh.uv = UVs;
            Mesh.colors32 = Colors;
            Mesh.triangles = triangles;
        }

        internal void Populate(ClassicUO.Renderer.PositionNormalTextureColor[] vertexData, int numVertices)
        {
            for (int i = 0; i < numVertices; i++)
            {
                var p = vertexData[i].Position;
                Vertices[i] = new UnityEngine.Vector3(p.X, p.Y, p.Z);

                var uv = vertexData[i].TextureCoordinate;
                UVs[i] = new UnityEngine.Vector2(uv.X, /*1 - */uv.Y);

                var c = vertexData[i].Normal;
                // Colors[i] = new Color32( c.R, c.G, c.B, c.A );
            }
            //we could clearly less if we remembered how many we used last time
            Array.Clear(Vertices, numVertices, Vertices.Length - numVertices);

            Mesh.vertices = Vertices;
            Mesh.uv = UVs;
            //Mesh.colors32 = Colors;
        }
        public void Populate(ClassicUO.Renderer.SpriteVertex[] vertexData, int numVertices)
        {
            for (int i = 0; i < numVertices; i++)
            {
                var p = vertexData[i].Position;
                Vertices[i] = new UnityEngine.Vector3(p.X, p.Y, p.Z);

                var uv = vertexData[i].TextureCoordinate;
                UVs[i] = new UnityEngine.Vector2(uv.X, /*1 - */uv.Y);

                var c = vertexData[i].Normal;
                // Colors[i] = new Color32( c.R, c.G, c.B, c.A );
            }
            //we could clearly less if we remembered how many we used last time
            Array.Clear(Vertices, numVertices, Vertices.Length - numVertices);

            Mesh.vertices = Vertices;
            Mesh.uv = UVs;
            //Mesh.colors32 = Colors;
        }


        public void Populate(VertexPositionColorTexture[] vertexData, int numVertices)
        {
            for (int i = 0; i < numVertices; i++)
            {
                var p = vertexData[i].Position;
                Vertices[i] = new UnityEngine.Vector3(p.X, p.Y, p.Z);

                var uv = vertexData[i].TextureCoordinate;
                UVs[i] = new UnityEngine.Vector2(uv.X, 1 - uv.Y);

                var c = vertexData[i].Color;
                Colors[i] = new Color32(c.R, c.G, c.B, c.A);
            }
            //we could clearly less if we remembered how many we used last time
            Array.Clear(Vertices, numVertices, Vertices.Length - numVertices);

            Mesh.vertices = Vertices;
            Mesh.uv = UVs;
            Mesh.colors32 = Colors;
        }

        public int NextPowerOf2(int minimum)
        {
            int result = 1;

            while (result < minimum)
                result *= 2;

            return result;
        }
    }
    public class MaterialPool
    {
        private class MaterialHolder
        {
            public readonly Material Material;
            public readonly Texture2D Texture2D;

            public MaterialHolder(Material material, Texture2D texture2D)
            {
                Material = material;
                Texture2D = texture2D;
            }
        }

        private readonly List<MaterialHolder> _materials = new List<MaterialHolder>();
        private int _index;
        private readonly Shader _shader = Shader.Find("Unlit/HueShader");

        private MaterialHolder Create(Texture2D texture)
        {
            var mat = new Material(_shader);
            mat.mainTexture = texture.UnityTexture;
            //mat.SetTexture( "_HueTex", texture.GraphicDevice.Textures[1].UnityTexture );
            mat.renderQueue += _materials.Count;
            return new MaterialHolder(mat, texture);
        }

        public Material Get(Texture2D texture)
        {
            while (_index < _materials.Count)
            {
                if (_materials[_index].Texture2D == texture)
                {
                    _index++;
                    return _materials[_index - 1].Material;
                }

                _index++;
            }

            var material = Create(texture);
            _materials.Add(material);
            _index++;
            return _materials[_index - 1].Material;
        }

        public void Reset()
        {
            _index = 0;
        }
    }
    public class MeshPool
    {
        private List<MeshHolder> _unusedMeshes = new List<MeshHolder>();
        private List<MeshHolder> _usedMeshes = new List<MeshHolder>();

        private List<MeshHolder> _otherMeshes = new List<MeshHolder>();
        //private int _index;

        /// <summary>
        /// get a mesh with at least this many triangles
        /// </summary>
        public MeshHolder Get(int spriteCount)
        {
            MeshHolder best = null;
            int bestIndex = -1;
            for (int i = 0; i < _unusedMeshes.Count; i++)
            {
                var unusedMesh = _unusedMeshes[i];
                if ((best == null || best.SpriteCount > unusedMesh.SpriteCount) && unusedMesh.SpriteCount >= spriteCount)
                {
                    best = unusedMesh;
                    bestIndex = i;
                }
            }
            if (best == null)
            {
                best = new MeshHolder(spriteCount);
            }
            else
            {
                _unusedMeshes.RemoveAt(bestIndex);
            }
            _usedMeshes.Add(best);

            return best;
        }

        public void Reset()
        {
            //Double Buffer our Meshes (Doesnt seem to be a win on wp8)
            //Ref http://forum.unity3d.com/threads/118723-Huge-performance-loss-in-Mesh-CreateVBO-for-dynamic-meshes-IOS

            //meshes from last frame are now unused
            _unusedMeshes.AddRange(_otherMeshes);
            _otherMeshes.Clear();

            //swap our use meshes and the now empty other meshes
            var temp = _otherMeshes;
            _otherMeshes = _usedMeshes;
            _usedMeshes = temp;
        }
    }
    // Token: 0x02000113 RID: 275
    public sealed class SamplerStateCollection
    {
        // Token: 0x06000C93 RID: 3219 RVA: 0x00036138 File Offset: 0x00034338
        internal SamplerStateCollection(GraphicsDevice device, int maxSamplers, bool applyToVertexStage)
        {
            this._graphicsDevice = device;
            this._samplerStateAnisotropicClamp = SamplerState.AnisotropicClamp.Clone();
            this._samplerStateAnisotropicWrap = SamplerState.AnisotropicWrap.Clone();
            this._samplerStateLinearClamp = SamplerState.LinearClamp.Clone();
            this._samplerStateLinearWrap = SamplerState.LinearWrap.Clone();
            this._samplerStatePointClamp = SamplerState.PointClamp.Clone();
            this._samplerStatePointWrap = SamplerState.PointWrap.Clone();
            this._samplers = new SamplerState[maxSamplers];
            this._actualSamplers = new SamplerState[maxSamplers];
            this._applyToVertexStage = applyToVertexStage;
            this.Clear();
        }

        // Token: 0x170002D8 RID: 728
        public SamplerState this[int index]
        {
            get
            {
                return this._samplers[index];
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (this._samplers[index] == value)
                {
                    return;
                }
                this._samplers[index] = value;
                SamplerState samplerState = value;
                if (object.ReferenceEquals(value, SamplerState.AnisotropicClamp))
                {
                    samplerState = this._samplerStateAnisotropicClamp;
                }
                else if (object.ReferenceEquals(value, SamplerState.AnisotropicWrap))
                {
                    samplerState = this._samplerStateAnisotropicWrap;
                }
                else if (object.ReferenceEquals(value, SamplerState.LinearClamp))
                {
                    samplerState = this._samplerStateLinearClamp;
                }
                else if (object.ReferenceEquals(value, SamplerState.LinearWrap))
                {
                    samplerState = this._samplerStateLinearWrap;
                }
                else if (object.ReferenceEquals(value, SamplerState.PointClamp))
                {
                    samplerState = this._samplerStatePointClamp;
                }
                else if (object.ReferenceEquals(value, SamplerState.PointWrap))
                {
                    samplerState = this._samplerStatePointWrap;
                }
                samplerState.BindToGraphicsDevice(this._graphicsDevice);
                this._actualSamplers[index] = samplerState;
                this.PlatformSetSamplerState(index);
            }
        }

        // Token: 0x06000C96 RID: 3222 RVA: 0x000362A8 File Offset: 0x000344A8
        internal void Clear()
        {
            for (int i = 0; i < this._samplers.Length; i++)
            {
                this._samplers[i] = SamplerState.LinearWrap;
                this._samplerStateLinearWrap.BindToGraphicsDevice(this._graphicsDevice);
                this._actualSamplers[i] = this._samplerStateLinearWrap;
            }
            this.PlatformClear();
        }

        // Token: 0x06000C97 RID: 3223 RVA: 0x0000C76B File Offset: 0x0000A96B
        internal void Dirty()
        {
            this.PlatformDirty();
        }

        // Token: 0x06000C98 RID: 3224 RVA: 0x0000C773 File Offset: 0x0000A973
        private void PlatformSetSamplerState(int index)
        {
            this._d3dDirty |= 1 << index;
        }

        // Token: 0x06000C99 RID: 3225 RVA: 0x0000C788 File Offset: 0x0000A988
        private void PlatformClear()
        {
            this._d3dDirty = int.MaxValue;
        }

        // Token: 0x06000C9A RID: 3226 RVA: 0x0000C788 File Offset: 0x0000A988
        private void PlatformDirty()
        {
            this._d3dDirty = int.MaxValue;
        }

        // Token: 0x06000C9B RID: 3227 RVA: 0x000362FC File Offset: 0x000344FC
        internal void PlatformSetSamplers(GraphicsDevice device)
        {
            if (this._applyToVertexStage)
            {
                return;
            }
            if (this._d3dDirty == 0)
            {
                return;
            }
            //CommonShaderStage commonShaderStage;
            //if (this._applyToVertexStage)
            //{
            //    commonShaderStage = device._d3dContext.VertexShader;
            //}
            //else
            //{
            //    commonShaderStage = device._d3dContext.PixelShader;
            //}
            for (int i = 0; i < this._actualSamplers.Length; i++)
            {
                int num = 1 << i;
                if ((this._d3dDirty & num) != 0)
                {
                    SamplerState samplerState = this._actualSamplers[i];
                    SamplerState sampler = null;
                    if (samplerState != null)
                    {
                        sampler = samplerState.GetState(device);
                    }
                    //commonShaderStage.SetSampler(i, sampler);
                    this._d3dDirty &= ~num;
                    if (this._d3dDirty == 0)
                    {
                        break;
                    }
                }
            }
            this._d3dDirty = 0;
        }

        // Token: 0x0400050E RID: 1294
        private readonly GraphicsDevice _graphicsDevice;

        // Token: 0x0400050F RID: 1295
        private readonly SamplerState _samplerStateAnisotropicClamp;

        // Token: 0x04000510 RID: 1296
        private readonly SamplerState _samplerStateAnisotropicWrap;

        // Token: 0x04000511 RID: 1297
        private readonly SamplerState _samplerStateLinearClamp;

        // Token: 0x04000512 RID: 1298
        private readonly SamplerState _samplerStateLinearWrap;

        // Token: 0x04000513 RID: 1299
        private readonly SamplerState _samplerStatePointClamp;

        // Token: 0x04000514 RID: 1300
        private readonly SamplerState _samplerStatePointWrap;

        // Token: 0x04000515 RID: 1301
        private readonly SamplerState[] _samplers;

        // Token: 0x04000516 RID: 1302
        private readonly SamplerState[] _actualSamplers;

        // Token: 0x04000517 RID: 1303
        private readonly bool _applyToVertexStage;

        // Token: 0x04000518 RID: 1304
        private int _d3dDirty;
    }



    public class TextureCollection
    {
        private readonly GraphicsDevice _graphicsDevice;

        // Token: 0x04000630 RID: 1584
        private readonly Texture[] _textures;

        // Token: 0x04000631 RID: 1585
        private readonly bool _applyToVertexStage;

        // Token: 0x04000632 RID: 1586
        private int _dirty;
        internal TextureCollection(GraphicsDevice graphicsDevice, int maxTextures, bool applyToVertexStage)
        {
            this._graphicsDevice = graphicsDevice;
            this._textures = new Texture[maxTextures];
            this._applyToVertexStage = applyToVertexStage;
            this._dirty = int.MaxValue;
            // this.PlatformInit();
        }

        public Texture this[int index]
        {
            get
            {
                return this._textures[index];
            }
            set
            {
                //if (this._applyToVertexStage && !this._graphicsDevice.GraphicsCapabilities.SupportsVertexTextures)
                //{
                //    throw new NotSupportedException("Vertex textures are not supported on this device.");
                //}
                if (this._textures[index] == value)
                {
                    return;
                }
                this._textures[index] = value;
                this._dirty |= 1 << index;
            }
        }

        // Token: 0x06000D9F RID: 3487 RVA: 0x0003A624 File Offset: 0x00038824
        internal void Clear()
        {
            for (int i = 0; i < this._textures.Length; i++)
            {
                this._textures[i] = null;
            }
            //this.PlatformClear();
            this._dirty = int.MaxValue;
        }

        // Token: 0x06000DA0 RID: 3488 RVA: 0x0000D50C File Offset: 0x0000B70C
        internal void Dirty()
        {
            this._dirty = int.MaxValue;
        }
    }

    public class VertexBuffer : GraphicsResource, IGraphicsResource
    {
        VertexDeclaration VertexDeclaration;
        public VertexBuffer(GraphicsDevice graphicsDevice, VertexDeclaration vertexDeclaration, int vertexCount, BufferUsage usage) : base()
        {
            VertexDeclaration = vertexDeclaration;
           this. graphicsDevice = graphicsDevice;
         _vertexCount= (uint)vertexCount;

    }

    public VertexBuffer()
        {

        }

        int IGraphicsResource.RecreateAndPopulateObject()
        {
            throw new NotImplementedException();
        }

        void IGraphicsResource.ReleaseNativeObject(bool disposeManagedResource)
        {
            throw new NotImplementedException();
        }

        int IGraphicsResource.SaveDataForRecreation()
        {
            throw new NotImplementedException();
        }

        public void SetData<T>(T[] data)
        {
            int elementCount;
            if (data != null)
            {
                elementCount = data.Length;
            }
            else
            {
                elementCount = 0;
            }
            UnityEngine.Debug.Log("SetData!");
            //this.SetData<T>(0, data, 0, elementCount, 0);
            vertices = data as XnaVG.Rendering.Tesselation.StencilVertex[];
        }

        internal void SetData<T>(T[] ts, int index, int count) where T : struct
        {
            SetData(ts);
        }

       public XnaVG.Rendering.Tesselation.StencilVertex[] vertices;

        internal uint _vertexCount;

        public int VertexCount
        {
            get
            {
                return (int)this._vertexCount;
            }
        }
    }

    public class DynamicVertexBuffer : VertexBuffer, IGraphicsResource, IDynamicGraphicsResource
    {
        public DynamicVertexBuffer(GraphicsDevice graphicsDevice, Type vertexType, int vertexCount, BufferUsage usage)
        {
            try
            {
                //VertexDeclaration vertexDeclaration = VertexDeclaration.FromType(vertexType);
                if (vertexCount <= 0)
                {
                    throw new ArgumentOutOfRangeException("vertexCount");
                }
                this._parent = graphicsDevice;
                //base.CreateBuffer(vertexDeclaration, vertexCount, 512, (_D3DPOOL)0);
                //graphicsDevice.Resources.AddTrackedObject(this, (void*)this.pComPtr, 0, this._internalHandle, ref this._internalHandle);
            }
            catch
            {
                base.Dispose(true);
                throw;
            }
        }

        public DynamicVertexBuffer(GraphicsDevice graphicsDevice, VertexDeclaration vertexDeclaration, int vertexCount, BufferUsage usage) : base(graphicsDevice, vertexDeclaration, vertexCount, usage)
        {
            _parent = graphicsDevice;
        }

        protected GraphicsDevice _parent;

        public bool IsContentLost
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                if (!this._contentLost)
                {
                    this._contentLost = this._parent.IsDeviceLost;
                }
                return this._contentLost;
            }
        }

        // Token: 0x040001FA RID: 506
        internal bool _contentLost;

        public event EventHandler<EventArgs> ContentLost;

        internal virtual void SetContentLost([MarshalAs(UnmanagedType.U1)] bool isContentLost)
        {
            this._contentLost = isContentLost;
            if (isContentLost)
            {
                //this.raise_ContentLost(this, EventArgs.Empty);
            }
        }

        void IDynamicGraphicsResource.SetContentLost(bool isContentLost)
        {
            throw new NotImplementedException();
        }
    }


    // Token: 0x02000118 RID: 280
    public struct VertexElement
    {
        // Token: 0x17000134 RID: 308
        // (get) Token: 0x06000567 RID: 1383 RVA: 0x0003D6D0 File Offset: 0x0003CAD0
        // (set) Token: 0x06000568 RID: 1384 RVA: 0x0003D6E4 File Offset: 0x0003CAE4
        public int Offset
        {
            get
            {
                return this._offset;
            }
            set
            {
                this._offset = value;
            }
        }

        // Token: 0x17000135 RID: 309
        // (get) Token: 0x06000569 RID: 1385 RVA: 0x0003D6F8 File Offset: 0x0003CAF8
        // (set) Token: 0x0600056A RID: 1386 RVA: 0x0003D70C File Offset: 0x0003CB0C
        public VertexElementFormat VertexElementFormat
        {
            get
            {
                return this._format;
            }
            set
            {
                this._format = value;
            }
        }

        // Token: 0x17000136 RID: 310
        // (get) Token: 0x0600056B RID: 1387 RVA: 0x0003D720 File Offset: 0x0003CB20
        // (set) Token: 0x0600056C RID: 1388 RVA: 0x0003D734 File Offset: 0x0003CB34
        public VertexElementUsage VertexElementUsage
        {
            get
            {
                return this._usage;
            }
            set
            {
                this._usage = value;
            }
        }

        // Token: 0x17000137 RID: 311
        // (get) Token: 0x0600056D RID: 1389 RVA: 0x0003D748 File Offset: 0x0003CB48
        // (set) Token: 0x0600056E RID: 1390 RVA: 0x0003D75C File Offset: 0x0003CB5C
        public int UsageIndex
        {
            get
            {
                return this._usageIndex;
            }
            set
            {
                this._usageIndex = value;
            }
        }

        // Token: 0x0600056F RID: 1391 RVA: 0x0003D770 File Offset: 0x0003CB70
        public VertexElement(int offset, VertexElementFormat elementFormat, VertexElementUsage elementUsage, int usageIndex)
        {
            this._offset = offset;
            this._usageIndex = usageIndex;
            this._format = elementFormat;
            this._usage = elementUsage;
        }

        // Token: 0x06000570 RID: 1392 RVA: 0x0003D79C File Offset: 0x0003CB9C
        public override int GetHashCode()
        {
            return 0;
            //return Helpers.SmartGetHashCode(this);
        }

        // Token: 0x06000571 RID: 1393 RVA: 0x0003D7BC File Offset: 0x0003CBBC
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{{Offset:{0} Format:{1} Usage:{2} UsageIndex:{3}}}", new object[]
            {
                this.Offset,
                this.VertexElementFormat,
                this.VertexElementUsage,
                this.UsageIndex
            });
        }

        // Token: 0x06000572 RID: 1394 RVA: 0x0003D818 File Offset: 0x0003CC18
        public override bool Equals(object obj)
        {
            return obj != null && !(obj.GetType() != base.GetType()) && this == (VertexElement)obj;
        }

        // Token: 0x06000573 RID: 1395 RVA: 0x0003D85C File Offset: 0x0003CC5C
        public static bool operator ==(VertexElement left, VertexElement right)
        {
            return left._offset == right._offset && left._usageIndex == right._usageIndex && left._usage == right._usage && left._format == right._format;
        }

        // Token: 0x06000574 RID: 1396 RVA: 0x0003D8AC File Offset: 0x0003CCAC
        public static bool operator !=(VertexElement left, VertexElement right)
        {
            return !(left == right);
        }

        // Token: 0x0400034A RID: 842
        public int _offset;

        // Token: 0x0400034B RID: 843
        public VertexElementFormat _format;

        // Token: 0x0400034C RID: 844
        public VertexElementUsage _usage;

        // Token: 0x0400034D RID: 845
        public int _usageIndex;

        
    }

    
}
