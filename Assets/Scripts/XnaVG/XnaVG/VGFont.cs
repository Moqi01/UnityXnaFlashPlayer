using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaVG.Rendering;
using XnaVG.Rendering.Tesselation;
using XnaVG.Rendering.Text;
using XnaVG.Rendering.Text.Strings;

namespace XnaVG
{
    public sealed class VGFont : IDisposable
    {
        private const string _invalidOpError = "Only dynamic font can be modified!";
        public const char MissingGlyphCode = '\0';

        private Glyph[] _glyphs = new Glyph[char.MaxValue + 1];
        private VertexBuffer _bufferedGlyphs = null;
        private Vector4 _maxGlyphExtents = new Vector4();

        public VGDevice Device { get; private set; }
        public IEnumerable<char> Characters
        {
            get
            {
                for (int i = 0; i < _glyphs.Length; i++)
                    if (_glyphs[i] != null)
                        yield return (char)i;
            }
        }
        public VGFillRule FillRule { get; private set; }
        public VGFontMode FontMode { get; private set; }
        public float EmSquareSize { get; private set; }
        public float LeadingSize { get; private set; }
        public bool IsStatic { get { return _bufferedGlyphs != null || Vertices != null; } }
        public VGKerningTable KerningTable { get; set; }

        internal Glyph this[char c] { get { return _glyphs[c] ?? _glyphs[MissingGlyphCode]; } }
        internal Glyph MissingGlyph { get { return _glyphs[MissingGlyphCode]; } }
        internal Vector4 MaxExtents { get { return _maxGlyphExtents; } }
        internal int BufferVertices { get { return _bufferedGlyphs == null ? 0 : _bufferedGlyphs.VertexCount; } }
        internal Texture2D Vertices { get; private set; }

        internal VGFont(VGDevice device, VGFillRule fillRule, float emSquareSize, float leadingSize)
        {
            Device = device;
            FillRule = fillRule;
            EmSquareSize = emSquareSize;
            LeadingSize = leadingSize;
            FontMode = VGFontMode.CPU;
        }

        internal VGFont(VGDevice device, VGFillRule fillRule, float emSquareSize, float leadingSize, IDictionary<char, VGGlyphInfo> glyphs, VGFontMode mode)
            : this(device, fillRule, emSquareSize, leadingSize)
        {
            StencilVertex[] vertices;
            CharBuffer[] defs;
            CombineVertices(glyphs, out vertices, out defs, out _maxGlyphExtents);

            FontMode = mode;
            if (mode == VGFontMode.CPU)
            {
                _bufferedGlyphs = new VertexBuffer(device.GraphicsDevice, StencilVertex.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
                _bufferedGlyphs.SetData(vertices);
            }
            else
            {
                int h = vertices.Length / device.MaxTextureSize;
                int s = device.MaxTextureSize * h;
                if (h > 0)
                {
                    Vertices = new Texture2D(device.GraphicsDevice, device.MaxTextureSize, 1 + h, false, SurfaceFormat.Vector4);
                    Vertices.SetData(0, new Rectangle(0, 0, device.MaxTextureSize, h), vertices, 0, s);
                    Vertices.SetData(0, new Rectangle(0, h, vertices.Length - s, 1), vertices, s, vertices.Length - s);
                }
                else
                {
                    Vertices = new Texture2D(device.GraphicsDevice, vertices.Length, 1, false, SurfaceFormat.Vector4);
                    Vertices.SetData(vertices);                    
                }
            }

            for (int i = 0; i < defs.Length; i++)
            {
                var def = defs[i];
                var info = glyphs[def.Character];
                _glyphs[def.Character] = new BufferGlyph(_bufferedGlyphs, def.Offset, def.Triangles, info.Advance);
            }
        }

        internal VGFont(VGDevice device, Loaders.VGFontData fontData, VGFontMode mode)
            : this(device, fontData.FillRule, fontData.EmSquareSize, fontData.LeadingSize)
        {
            _maxGlyphExtents = fontData.Extents;

            KerningTable = fontData.Kerning;
            FontMode = mode;
            if (mode == VGFontMode.CPU)
            {
                _bufferedGlyphs = new VertexBuffer(device.GraphicsDevice, StencilVertex.VertexDeclaration, fontData.Vertices.Length, BufferUsage.WriteOnly);
                _bufferedGlyphs.SetData(fontData.Vertices);
            }
            else
            {
                int h = fontData.Vertices.Length / device.MaxTextureSize;
                int s = device.MaxTextureSize * h;
                Vertices = new Texture2D(device.GraphicsDevice, device.MaxTextureSize, 1 + h, false, SurfaceFormat.Vector4);
                if (h > 0) Vertices.SetData(0, new Rectangle(0, 0, device.MaxTextureSize, h), fontData.Vertices, 0, s);
                Vertices.SetData(0, new Rectangle(0, h, fontData.Vertices.Length - s, 1), fontData.Vertices, s, fontData.Vertices.Length - s);
            }

            foreach(var def in fontData.Glyphs)
                _glyphs[def.Character] = new BufferGlyph(_bufferedGlyphs, def.Offset, def.Triangles, def.Escape);
        }

        internal bool SetBuffer()
        {
            if (_bufferedGlyphs == null)
                return false;

            _bufferedGlyphs.GraphicsDevice.SetVertexBuffer(_bufferedGlyphs);
            return true;
        }

        public void SetGlyph(char glyph, VGPath path, float escapeX, float escapeY)
        {
            if (IsStatic) throw new InvalidOperationException(_invalidOpError);

            ClearGlyph(glyph);
            _glyphs[glyph] = new PathGlyph(Device.PreparePath(path, VGPaintMode.Fill), new Vector2(escapeX, escapeY));

            Vector4 extents = _glyphs[glyph].Extents;
            VectorMath.ExpandExtents(ref _maxGlyphExtents, ref extents);            
        }
        public void ClearGlyph(char glyph)
        {
            if (IsStatic) throw new InvalidOperationException(_invalidOpError);

            if (_glyphs[glyph] != null)
                _glyphs[glyph].Dispose();
            _glyphs[glyph] = null;
        }
        public VGString CreateString()
        {
            switch (FontMode)
            {
                case VGFontMode.GPU_PreferRAM:
                    return new GPUBufferedString(this, false);
                case VGFontMode.GPU_PreferVRAM:
                    return new GPUBufferedString(this, true);
                default:
                    return new CPUString(this);
            }
        }

        public void Dispose()
        {
            if (_bufferedGlyphs != null)
            {
                _bufferedGlyphs.Dispose();
                _bufferedGlyphs = null;
            }

            if (Vertices != null)
            {
                Vertices.Dispose();
                Vertices = null;
            }

            if (_glyphs == null)
                return;

            foreach (var g in _glyphs)
                if (g != null)
                    g.Dispose();

            _glyphs = null;
        }

        #region Utils

        public VGMatrix PtToScaling(float ptSize)
        {
            ptSize *= Rendering.Constants.PixelsPerPoint;
            ptSize /= EmSquareSize;
            return VGMatrix.Scale(ptSize, -ptSize);
        }

        internal static void CombineVertices(IDictionary<char, VGGlyphInfo> glyphs, out StencilVertex[] vertices, out CharBuffer[] definitions, out Vector4 extents)
        {
            int tris;
            int count = glyphs.Count;
            var verts = new List<StencilVertex>(256 * count);
            var defs = new List<CharBuffer>(count);
            extents = new Vector4();

            foreach (var glyph in glyphs)
            {
                var path = glyph.Value.Path;
                if (path == null)
                    continue;

                if (!path.IsEmpty)
                {
                    if (!FillMesh.Make(path, out vertices, out tris))
                        continue;

                    defs.Add(new CharBuffer { Character = glyph.Key, Offset = verts.Count, Triangles = tris });
                    verts.AddRange(vertices);
                    path.ExpandExtents(ref extents);
                }
                else
                    defs.Add(new CharBuffer { Character = glyph.Key, Offset = 0, Triangles = 0 });                    
            }

            definitions = defs.ToArray();
            vertices = verts.ToArray();
        }

        internal struct CharBuffer
        {
            public char Character;
            public int Offset;
            public int Triangles;
        }

        #endregion
    }
}
