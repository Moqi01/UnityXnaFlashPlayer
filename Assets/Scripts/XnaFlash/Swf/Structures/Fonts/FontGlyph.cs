using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaVG;
using static XnaFlash.Swf.Paths.Shape;

namespace XnaFlash.Swf.Structures.Fonts
{
    public struct FontGlyph
    {
        public VGPath GlyphPath;
        public Vector2 ReferencePoint;
        public SubShape SubShape;
    }
}
