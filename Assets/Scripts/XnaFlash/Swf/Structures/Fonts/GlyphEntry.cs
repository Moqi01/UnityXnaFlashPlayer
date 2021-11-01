using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XnaFlash.Swf.Paths;
using XnaFlash.Swf.Structures;
using XnaVG;

namespace XnaFlash.Swf.Structures.Fonts
{
    public class GlyphEntry
    {
        public uint GlyphIndex { get; private set; }
        public int GlyphAdvance { get; private set; }

        public GlyphEntry(SwfStream stream, int glyphBits, int advanceBits)
        {
            GlyphIndex = stream.ReadBitUInt(glyphBits);
            GlyphAdvance = stream.ReadBitInt(advanceBits);
        }
    }
}