using XnaVG;

namespace XnaFlash.Swf.Structures.Filters
{
    public class ConvolutionFilter : Filter
    {
        public override Filter.ID FilterID { get { return ID.Convolution; } }
        
        public byte Width { get; private set; }
        public byte Height { get; private set; }
        public float Divisor { get; private set; }
        public float Bias { get; private set; }
        public float[] Matrix { get; private set; }
        public VGColor DefaultColor { get; private set; }
        public bool Clamp { get;private set;}
        public bool PreserveAlpha { get;private set;}

        public ConvolutionFilter(SwfStream stream)
        {
            Width = stream.ReadByte();
            Height = stream.ReadByte();
            Divisor = stream.ReadSingle();
            Bias = stream.ReadSingle();
            Matrix = stream.ReadSingleArray(Width * Height);
            DefaultColor = stream.ReadRGBA();

            byte flags = stream.ReadByte();
            Clamp = (flags & 0x02) != 0;
            PreserveAlpha = (flags & 0x01) != 0;
        }
    }
}
