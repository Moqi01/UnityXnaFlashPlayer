using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaVG.Paints;
using XnaVG.Rendering;
using XnaVG.Rendering.Effects;
using XnaVG.Rendering.States;
using XnaVG.Rendering.Tesselation;
using XnaVG.Rendering.Text;
using XnaVG.Utils;

namespace XnaVG
{
    public sealed class VGDevice : IVGDevice
    {
        public IVGRenderContext ActiveContext { get; private set; }
        public VGTextureUtils TextureUtils { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }               
        public VGSurface WindowSurface { get; private set; }
        public VGState State { get; private set; }
        public int MaxTextureSize { get; private set; }

        internal EffectManager EffectManager { get; private set; }
        internal BlendStates BlendStates { get; private set; }

        private VGDevice(GraphicsDevice device, IServiceProvider services)
        {
            ActiveContext = null;
            GraphicsDevice = device;
            EffectManager = new EffectManager(services);
            BlendStates = new BlendStates();
            WindowSurface = new VGSurface(this);
            State = new VGState(this);

            TextureUtils = new VGTextureUtils(this);
            MaxTextureSize = 2048;
        }

        public IVGRenderContext BeginRendering(VGSurface surface, bool restoreXNAStates)
        {
            return BeginRendering<object>(surface, State, null, restoreXNAStates);
        }
        public IVGRenderContext BeginRendering(VGSurface surface, VGState state, bool restoreXNAStates)
        {
            return BeginRendering<object>(surface, state, null, restoreXNAStates);
        }
        public IVGRenderContext<T> BeginRendering<T>(VGSurface surface, T userState, bool restoreXNAStates)
        {
            return BeginRendering(surface, State, userState, restoreXNAStates);
        }
        public IVGRenderContext<T> BeginRendering<T>(VGSurface surface, VGState state, T userState, bool restoreXNAStates)
        {
            var o = this.ActiveContext;
            var c = new VGRenderContext<T>(this, surface ?? WindowSurface, state ?? State, userState, restoreXNAStates, () => { ActiveContext = o; });
            ActiveContext = c;
            return c;
        }

        public VGSurface CreateSurface(RenderTarget2D renderTarget)
        {
            if (renderTarget == null) throw new ArgumentNullException("renderTarget");
            if (renderTarget.GraphicsDevice != GraphicsDevice) throw new ArgumentException("Render target belongs to another device!", "renderTarget");
            return new VGSurface(this, renderTarget);
        }
        public VGSurface CreateSurface(int width, int height, SurfaceFormat format)
        {
            return new VGSurface(this, width, height, format);
        }
        public VGState CreateState()
        {
            return new VGState(this);
        }
        public void SetState(VGState state)
        {
            if (state == null) throw new ArgumentNullException("state");
            State = state;
        }
        public VGColorPaint CreateColorPaint(VGColor color)
        {
            return new VGColorPaint(this, color);
        }
        public VGPatternPaint CreatePatternPaint(VGImage image)
        {
            return new VGPatternPaint(this, image);
        }
        public VGLinearPaint CreateLinearPaint(IEnumerable<KeyValuePair<byte, VGColor>> stops, bool linearColorspace)
        {
            return new VGLinearPaint(this, linearColorspace, stops);
        }
        public VGRadialPaint CreateRadialPaint(IEnumerable<KeyValuePair<byte, VGColor>> stops, bool linearColorspace)
        {
            return new VGRadialPaint(this, linearColorspace, stops);
        }
        public VGPath CreatePath()
        {
            return new VGPath();
        }
        public VGPreparedPath PreparePath(VGPath path, VGPaintMode mode)
        {
            return PreparePath(path, mode, State);
        }
        public VGPreparedPath PreparePath(VGPath path, VGPaintMode mode, VGState state)
        {
            FillMesh f = null;
            StrokeMesh s = null;

            if ((mode & VGPaintMode.Fill) != 0)
                f = new FillMesh(GraphicsDevice, path);

            if ((mode & VGPaintMode.Stroke) != 0)
                s = new StrokeMesh(GraphicsDevice, path.Flatten(), state.StrokeStartCap, state.StrokeEndCap, state.StrokeJoin, state.StrokeMiterLimit);

            return new VGPreparedPath(f, s, state.StrokeStartCap, state.StrokeEndCap, state.StrokeJoin, state.StrokeMiterLimit, path.Extents);
        }
        public VGImage CreateImage(Texture2D texture, bool linearColorspace, bool premultiplied)
        {
            return new VGImage(this, texture, linearColorspace, premultiplied);
        }
        public VGFont CreateDynamicFont(VGFillRule fillRule, float emSquareSize, float leadingSize)
        {
            return new VGFont(this, fillRule, emSquareSize, leadingSize);
        }
        public VGFont CreateFont(VGFillRule fillRule, float emSquareSize, float leadingSize, IDictionary<char, VGGlyphInfo> glyphs, VGFontMode mode)
        {
            return new VGFont(this, fillRule, emSquareSize, leadingSize, glyphs, mode);
        }
        public VGFont CreateFont(Loaders.VGFontData fontData, VGFontMode mode)
        {
            if (fontData == null) throw new ArgumentNullException("fontData");
            return new VGFont(this, fontData, mode);
        }
        public VGString CreateStaticString(IEnumerable<KeyValuePair<Vector2, VGPath>> glyphs)
        {
            if (glyphs == null)
                return null;

            return new Rendering.Text.Strings.StaticString(this, glyphs);
        }

        public void Dispose()
        {
            if (WindowSurface != null)
            {
                WindowSurface.Dispose();
                WindowSurface = null;
            }

            if (EffectManager != null)
            {
                EffectManager.Dispose();
                EffectManager = null;
            }

            if (TextureUtils != null)
            {
                TextureUtils.Dispose();
                TextureUtils = null;
            }
        }

        public static VertexDeclaration SolidDeclaration { get { return Pipeline._solidDecl; } }
        public static IVGDevice Initialize(GraphicsDevice device, GameServiceContainer services)
        {
            var dev = new VGDevice(device, services);
            services.AddService(typeof(IVGDevice), dev);
            return dev;
        }
        public static IVGDevice Initialize(Game game)
        {
            return Initialize(game.GraphicsDevice, game.Services);
        }
    }
}