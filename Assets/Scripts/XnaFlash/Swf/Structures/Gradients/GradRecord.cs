using System.Collections.Generic;
using XnaVG;

namespace XnaFlash.Swf.Structures.Gradients
{
    public class GradRecord
    {
        public byte Ratio { get; private set; }
        public VGColor Color { get; private set; }

        public GradRecord(SwfStream swf, bool hasAlpha)
        {
            Ratio = swf.ReadByte();
            Color = hasAlpha ? swf.ReadRGBA() : swf.ReadRGB();
        }

        public KeyValuePair<byte, VGColor> AsKeyValuePair()
        {
            return new KeyValuePair<byte, VGColor>(Ratio, Color);
        }
    }
}
