using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XnaFlash.Swf.Paths;
using XnaFlash.Swf.Structures;
using XnaVG;

namespace XnaFlash.Swf.Structures.Fonts
{
    public class TextRecord
    {
        private byte _flags;
        
        public bool EndRecord { get { return _flags == 0; } }
        public bool HasFont { get { return (_flags & 8) != 0; } }
        public bool HasColor { get { return (_flags & 4) != 0; } }
        public bool HasYOffset { get { return (_flags & 2) != 0; } }
        public bool HasXOffset { get { return (_flags & 1) != 0; } }

        public ushort FontId { get; private set; }
        public ushort FontSize { get; private set; }
        public VGColor Color { get; private set; }
        public short XOffset { get; private set; }
        public short YOffset { get; private set; }
        public GlyphEntry[] Glyphs { get; private set; }

        public TextRecord(SwfStream stream, bool hasAlpha, int glyphBits, int advanceBits)
        {
            _flags = stream.ReadByte();
            if (EndRecord) return;

            if (HasFont) FontId = stream.ReadUShort();
            if (HasColor) Color = hasAlpha ? stream.ReadRGBA() : stream.ReadRGB();
            if (HasXOffset) XOffset = stream.ReadShort();
            if (HasYOffset) YOffset = stream.ReadShort();
            if (HasFont) FontSize = stream.ReadUShort();

            byte count = stream.ReadByte();
            Glyphs = new GlyphEntry[count];
            for (int i = 0; i < count; i++)
                Glyphs[i] = new GlyphEntry(stream, glyphBits, advanceBits);
        }
    }
}
