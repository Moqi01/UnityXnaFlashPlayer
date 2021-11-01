using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace XnaVG.Rendering.Text.Strings
{
    internal sealed class CPUString : VGString
    {
        private Glyph[] _glyphs = new Glyph[0];
        private Vector2[] _offsets = new Vector2[0];

        internal override bool IsEmpty { get { return _glyphs.Length == 0; } }

        internal CPUString(VGFont font)
            : base(font)
        { }

        internal override void Prepare()
        {
            Font.SetBuffer();
        }

        internal override void Stencil(Pipeline pipeline)
        {
            int len = _glyphs.Length;
            pipeline.BeginCPUTextStenciling(Font.FillRule);
            for (int i = 0; i < len; i++)
                pipeline.StencilGlyph(_glyphs[i], ref _offsets[i]);
        }

        internal override void Cover(Pipeline pipeline)
        {
            Vector2 offset = Vector2.Zero;
            Vector4 extents = Extents;
            pipeline.CoverGlyphs(ref extents, ref offset);
        }
        
        public override void Dispose()
        { }

        protected override Vector2 SetString(string text)
        {
            int len = text.Length;
            List<Glyph> glyphs = new List<Glyph>(len);
            List<Vector2> offsets = new List<Vector2>(len);
            var gpi = new GlyphPositionInfo { Offset = Vector2.Zero, Size = Vector2.Zero };
            gpi.TabSize = ((Font.Device.ActiveContext != null) ? Font.Device.ActiveContext.State : Font.Device.State).TabSize * Font.EmSquareSize;

            foreach (var g in ProcessString<Glyph>(Font, text, gpi))
            {
                glyphs.Add(g);
                offsets.Add(gpi.Offset);
            }

            _glyphs = glyphs.ToArray();
            _offsets = offsets.ToArray();
            return gpi.Size;
        }
    }
}
