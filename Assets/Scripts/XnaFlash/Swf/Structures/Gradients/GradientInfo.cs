using System.Collections.Generic;
using System.Linq;
using XnaVG;

namespace XnaFlash.Swf.Structures.Gradients
{
    public class GradientInfo
    {
        public Padding PadMode { get; private set; }
        public Interpolation InterpolationMode { get; private set; }
        public GradRecord[] GradientStops { get; private set; }

        public GradientInfo(SwfStream swf, bool hasAlpha)
        {
            swf.Align();
            PadMode = (Padding)swf.ReadBitUInt(2);
            InterpolationMode = (Interpolation)swf.ReadBitUInt(2);
            GradientStops = new GradRecord[swf.ReadBitUInt(4)];

            for (int i = 0; i < GradientStops.Length; i++)
                GradientStops[i] = new GradRecord(swf, hasAlpha);
        }

        public IEnumerable<KeyValuePair<byte, VGColor>> AsEnumerable()
        {
            return GradientStops.Select(g => g.AsKeyValuePair());
        }

        public enum Padding
        {
            Pad = 0,
            Reflect = 1,
            Repeat = 2
        }

        public enum Interpolation
        {
            Normal = 0,
            Linear = 1
        }
    }
}
