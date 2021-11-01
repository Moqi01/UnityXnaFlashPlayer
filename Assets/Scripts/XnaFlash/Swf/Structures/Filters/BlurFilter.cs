
namespace XnaFlash.Swf.Structures.Filters
{
    public class BlurFilter : Filter
    {
        public override Filter.ID FilterID { get { return ID.Blur; } }
        
        public decimal BlurX { get; private set; }
        public decimal BlurY { get; private set; }
        public byte Passes { get; private set; }
        
        public BlurFilter(SwfStream stream)
        {
            BlurX = stream.ReadFixed();
            BlurY = stream.ReadFixed();
            Passes = (byte)(stream.ReadByte() >> 3);
        }
    }
}
