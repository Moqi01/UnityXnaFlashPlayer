using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaVG.Paints;
using XnaVG.Utils;

namespace XnaVG
{
    public interface IVGDevice : IDisposable
    {
        IVGRenderContext ActiveContext { get; }
        VGTextureUtils TextureUtils { get; }
        GraphicsDevice GraphicsDevice { get; }               
        VGSurface WindowSurface { get; }
        VGState State { get; }
        int MaxTextureSize { get; }

        IVGRenderContext BeginRendering(VGSurface surface, bool restoreXNAStates);
        IVGRenderContext BeginRendering(VGSurface surface, VGState state, bool restoreXNAStates);
        IVGRenderContext<T> BeginRendering<T>(VGSurface surface, T userState, bool restoreXNAStates);
        IVGRenderContext<T> BeginRendering<T>(VGSurface surface, VGState state, T userState, bool restoreXNAStates);

        void SetState(VGState state);
        VGSurface CreateSurface(RenderTarget2D renderTarget);
        VGSurface CreateSurface(int width, int height, SurfaceFormat format);
        VGState CreateState();        
        VGPath CreatePath();        
        VGFont CreateDynamicFont(VGFillRule fillRule, float emSquareSize, float leadingSize);
        VGFont CreateFont(VGFillRule fillRule, float emSquareSize, float leadingSize, IDictionary<char, VGGlyphInfo> glyphs, VGFontMode mode);
        VGFont CreateFont(Loaders.VGFontData fontData, VGFontMode mode);
        VGImage CreateImage(Texture2D texture, bool linearColorspace, bool premultiplied);
        VGString CreateStaticString(IEnumerable<KeyValuePair<Vector2, VGPath>> glyphs);
        VGColorPaint CreateColorPaint(VGColor color);
        VGPatternPaint CreatePatternPaint(VGImage image);
        VGLinearPaint CreateLinearPaint(IEnumerable<KeyValuePair<byte, VGColor>> stops, bool linearColorspace);
        VGRadialPaint CreateRadialPaint(IEnumerable<KeyValuePair<byte, VGColor>> stops, bool linearColorspace);
        VGPreparedPath PreparePath(VGPath path, VGPaintMode mode);
        VGPreparedPath PreparePath(VGPath path, VGPaintMode mode, VGState state);
    }
}
