using XnaFlash.Swf.Structures.Filters;

namespace XnaFlash.Swf.Structures
{
    public abstract class Filter
    {
        public abstract ID FilterID { get; }

        public static Filter Read(SwfStream stream)
        {
            ID id = (ID)stream.ReadByte();
            switch (id)
            {
                case ID.DropShadow: return new DropShadowFilter(stream);
                case ID.Blur: return new BlurFilter(stream);
                case ID.Glow: return new GlowFilter(stream);
                case ID.Bevel: return new BevelFilter(stream);
                case ID.GradientGlow: return new GradientGlowBevelFilter(stream, false);
                case ID.Convolution: return new ConvolutionFilter(stream);
                case ID.ColorMatrix: return new ColorMatrixFilter(stream);
                case ID.GradientBevel: return new GradientGlowBevelFilter(stream, true);
                default:
                    throw new SwfCorruptedException("Unknown filter found!");
            }
        }

        public static Filter[] ReadFilterList(SwfStream stream)
        {
            byte count = stream.ReadByte();
            var filters = new Filter[count];
            for (; count > 0; count--)
                filters[filters.Length - count] = Read(stream);
            return filters;
        }

        public enum ID : byte
        {
            DropShadow = 0,
            Blur = 1,
            Glow = 2,
            Bevel = 3,
            GradientGlow = 4,
            Convolution = 5,
            ColorMatrix = 6,
            GradientBevel = 7
        }
    }
}
