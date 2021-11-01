using XnaVG;

namespace XnaFlash.Swf.Structures.Filters
{
    public class GlowFilter : Filter
    {
        private byte mFlags;

        public override Filter.ID FilterID { get { return ID.Glow; } }
        
        public VGColor GlowColor { get; private set; }
        public decimal BlurX { get; private set; }
        public decimal BlurY { get; private set; }
        public decimal Angle { get; private set; }
        public decimal Distance { get; private set; }
        public decimal Strength { get; private set; }
        public bool InnerGlow { get { return (mFlags & 0x80) != 0; } }
        public bool Knockout { get { return (mFlags & 0x40) != 0; } }
        public bool CompositeSource { get { return (mFlags & 0x20) != 0; } }
        public int Passes { get { return mFlags & 0x1F; } }

        public GlowFilter(SwfStream stream)
        {
            GlowColor = stream.ReadRGBA();
            BlurX = stream.ReadFixed();
            BlurY = stream.ReadFixed();
            Strength = stream.ReadFixedHalf();
            mFlags = stream.ReadByte();
        }
    }
}