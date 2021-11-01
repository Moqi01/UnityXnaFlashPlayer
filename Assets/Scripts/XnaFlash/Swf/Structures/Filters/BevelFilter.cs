using XnaVG;

namespace XnaFlash.Swf.Structures.Filters
{
    public class BevelFilter : Filter
    {
        private byte mFlags;

        public override Filter.ID FilterID { get { return ID.Bevel; } }
        
        public VGColor ShadowColor { get; private set; }
        public VGColor HighlightColor { get; private set; }
        public decimal BlurX { get; private set; }
        public decimal BlurY { get; private set; }
        public decimal Angle { get; private set; }
        public decimal Distance { get; private set; }
        public decimal Strength { get; private set; }
        public bool InnerShadow { get { return (mFlags & 0x80) != 0; } }
        public bool Knockout { get { return (mFlags & 0x40) != 0; } }
        public bool CompositeSource { get { return (mFlags & 0x20) != 0; } }
        public bool OnTop { get { return (mFlags & 0x10) != 0; } }
        public int Passes { get { return mFlags & 0x0F; } }

        public BevelFilter(SwfStream stream)
        {
            ShadowColor = stream.ReadRGBA();
            HighlightColor = stream.ReadRGBA();
            BlurX = stream.ReadFixed();
            BlurY = stream.ReadFixed();
            Angle = stream.ReadFixed();
            Distance = stream.ReadFixed();
            Strength = stream.ReadFixedHalf();
            mFlags = stream.ReadByte();
        }
    }
}