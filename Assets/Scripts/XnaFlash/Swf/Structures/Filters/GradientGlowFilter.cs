using XnaVG;

namespace XnaFlash.Swf.Structures.Filters
{
    public class GradientGlowBevelFilter : Filter
    {
        private byte mFlags;
        private ID mId;

        public override Filter.ID FilterID { get { return mId; } }
        
        public VGColor[] GlowColors { get; private set; }
        public byte[] GlowRatios { get; private set; }
        public decimal BlurX { get; private set; }
        public decimal BlurY { get; private set; }
        public decimal Angle { get; private set; }
        public decimal Distance { get; private set; }
        public decimal Strength { get; private set; }
        public bool InnerGlow { get { return (mFlags & 0x80) != 0; } }
        public bool Knockout { get { return (mFlags & 0x40) != 0; } }
        public bool CompositeSource { get { return (mFlags & 0x20) != 0; } }
        public bool OnTop { get { return (mFlags & 0x10) != 0; } }
        public int Passes { get { return mFlags & 0x0F; } }

        public GradientGlowBevelFilter(SwfStream stream, bool bevel)
        {
            mId = bevel ? ID.GradientBevel : ID.GradientGlow;

            byte numColors = stream.ReadByte();
            GlowColors = stream.ReadRGBAArray(numColors);
            GlowRatios = stream.ReadByteArray(numColors);
            BlurX = stream.ReadFixed();
            BlurY = stream.ReadFixed();
            Strength = stream.ReadFixedHalf();
            mFlags = stream.ReadByte();
        }
    }
}