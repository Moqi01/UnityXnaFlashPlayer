using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaVG.Rendering.Text.Strings
{
    internal sealed class GPUBufferedString : VGString
    {
        private VertexBuffer _vertices;
        private int _triangles;
        private bool _preferVRAM;

        internal Texture2D Glyphs { get; private set; }
        internal override bool IsEmpty { get { return _vertices == null; } }

        internal GPUBufferedString(VGFont font, bool preferVRAM)
            : base(font)
        {
            if (!font.IsStatic)
                throw new ArgumentException("Font must be static for full GPU rendering!", "font");

            _preferVRAM = preferVRAM;
        }

        internal override void Prepare()
        {
        }

        internal override void Stencil(Pipeline pipeline)
        {
            if (Glyphs == null || _vertices == null)
                return;

            pipeline.StencilInstancedGlyphs(Font.FillRule, this);
        }

        internal override void Cover(Pipeline pipeline)
        {
            Vector2 offset = Vector2.Zero;
            Vector4 extents = Extents;
            pipeline.CoverGlyphs(ref extents, ref offset);
        }

        public override void Dispose()
        {
            if (_vertices != null)
            {
                _vertices.Dispose();
                _vertices = null;
            }

            if (Glyphs != null)
            {
                Glyphs.Dispose();
                Glyphs = null;
            }
        }

        protected override Vector2 SetString(string text)
        {
            int i = 0, len = text.Length;
            List<Vector2> indices = new List<Vector2>(256 * len);
            List<Vector2> offsets = new List<Vector2>(len);
            var gpi = new GlyphPositionInfo { Offset = Vector2.Zero, Size = Vector2.Zero };
            gpi.TabSize = ((Font.Device.ActiveContext != null) ? Font.Device.ActiveContext.State : Font.Device.State).TabSize * Font.EmSquareSize;

            _triangles = 0;
            foreach (var g in ProcessString<BufferGlyph>(Font, text, gpi))
            {
                indices.AddRange(Enumerable.Range(g.VertexOffset, 3 * g.TriangleCount).Select(j => new Vector2(j, i)));
                offsets.Add(gpi.Offset);

                _triangles += g.TriangleCount;
                i++;
            }

            if (_vertices != null) _vertices.Dispose();
            if (Glyphs != null) Glyphs.Dispose();

            if (_triangles == 0)
            {
                _vertices = null;
                Glyphs = null;
                return gpi.Size;
            }

            if (_preferVRAM)
                _vertices = new VertexBuffer(Font.Device.GraphicsDevice, VertexDeclaration, indices.Count, BufferUsage.WriteOnly);
            else
               _vertices = new DynamicVertexBuffer(Font.Device.GraphicsDevice, VertexDeclaration, indices.Count, BufferUsage.WriteOnly);

            int h = offsets.Count / Font.Device.MaxTextureSize;
            int s = Font.Device.MaxTextureSize * h;
            var d = offsets.ToArray();

            if (h > 0)
            {
                Glyphs = new Texture2D(Font.Device.GraphicsDevice, Font.Device.MaxTextureSize, 1 + h, false, SurfaceFormat.Vector2);
                Glyphs.SetData(0, new Rectangle(0, 0, Font.Device.MaxTextureSize, h), d, 0, s);
                Glyphs.SetData(0, new Rectangle(0, h, d.Length - s, 1), d, s, d.Length - s);
            }
            else
            {
                Glyphs = new Texture2D(Font.Device.GraphicsDevice, d.Length, 1, false, SurfaceFormat.Vector2);
                Glyphs.SetData(d);
            }
            
            _vertices.SetData(indices.ToArray());
            return gpi.Size;
        }

        internal void Render()
        {
            _vertices.GraphicsDevice.SetVertexBuffer(_vertices);
            _vertices.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _triangles);
        }

        #region Vertex Declaration

        internal readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0)
        );   

        #endregion
    }
}
