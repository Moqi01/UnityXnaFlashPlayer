using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaVG.Loaders;

namespace XnaVG.Rendering.Text.Strings
{
    internal sealed class StaticString : VGString
    {
        private VGPreparedPath _path;

        internal override bool IsEmpty { get { return _path == null; } }

        internal StaticString(VGDevice device, IEnumerable<KeyValuePair<Vector2, VGPath>> glyphs)
            : base(null)
        {
            VGPath result = new VGPath();
            foreach (var g in glyphs)
            {
                var p = g.Value.Clone();
                p.Offset(g.Key);
                result.Append(p);
            }
            _path = device.PreparePath(result, VGPaintMode.Fill);
            Extents = _path.Extents;
        }

        internal override void Prepare() { }        

        internal override void Stencil(Pipeline pipeline)
        {
            Vector2 zero = Vector2.Zero;
            pipeline.BeginCPUTextStenciling(pipeline.State.FillRule);
            pipeline.StencilGlyph(_path, ref zero);
        }

        internal override void Cover(Pipeline pipeline)
        {
            Vector2 offset = Vector2.Zero;
            Vector4 extents = _path.Extents;
            pipeline.CoverGlyphs(ref extents, ref offset);
        }

        public override void Dispose()
        {
            if (_path != null)
            {
                _path.Dispose();
                _path = null;
            }
        }

        protected override Vector2 SetString(string text)
        {
            throw new InvalidOperationException("Cannot change value of static string!");
        }

        public override string ToString()
        {
            return "StaticString";
        }
    }
}
