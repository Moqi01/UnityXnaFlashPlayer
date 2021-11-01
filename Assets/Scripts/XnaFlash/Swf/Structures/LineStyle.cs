using XnaVG;

namespace XnaFlash.Swf.Structures
{
    public class LineStyle
    {
        public int Index { get; private set; }
        public ushort Width { get; private set; }
        public VGLineCap StartCapStyle { get; private set; }
        public VGLineCap EndCapStyle { get; private set; }
        public VGLineJoin JoinStyle { get; private set; }
        public bool HasFill { get; private set; }
        public bool NoHScale { get; private set; }
        public bool NoVScale { get; private set; }
        public bool PixelHinting { get; private set; }
        public bool NoClose { get; private set; }
        public decimal MiterLimit { get; private set; }
        public VGColor Color { get; private set; }
        public FillStyle Fill { get; private set; }

        public LineStyle()
        {
            Index = 1;
            Width = 1;
            Color = Microsoft.Xna.Framework.Color.Red;
            StartCapStyle = VGLineCap.Round;
            EndCapStyle = VGLineCap.Round;
            JoinStyle = VGLineJoin.Round;
            HasFill = false;
            NoHScale = false;
            NoVScale = false;
            PixelHinting = false;
            NoClose = false;
            MiterLimit = 1m;
            Fill = null;
        }

        public LineStyle(SwfStream swf, bool hasAlpha, bool isExtended, int index)
        {
            Index = index;

            if (isExtended)
            {
                Width = swf.ReadUShort();
                StartCapStyle = ReadCap(swf);
                JoinStyle = ReadJoin(swf);                
                HasFill = swf.ReadBit();
                NoHScale = swf.ReadBit();
                NoVScale = swf.ReadBit();
                PixelHinting = swf.ReadBit();

                swf.ReadBitUInt(5); // Reserved
                
                NoClose = swf.ReadBit();
                EndCapStyle = ReadCap(swf);
                
                if (JoinStyle == VGLineJoin.Miter)
                    MiterLimit = swf.ReadFixedHalf();

                if (HasFill)
                    Fill = new FillStyle(swf, hasAlpha, -1);
                else
                    Color = swf.ReadRGBA();          
            }
            else
            {
                Width = swf.ReadUShort();
                Color = hasAlpha ? swf.ReadRGBA() : swf.ReadRGB();

                StartCapStyle = VGLineCap.Round;
                EndCapStyle = VGLineCap.Round;
                JoinStyle = VGLineJoin.Round;
                HasFill = false;
                NoHScale = false;
                NoVScale = false;
                PixelHinting = false;
                NoClose = false;
                MiterLimit = 1m;
                Fill = null;
            }

            if (Width < 1) Width = 1;
        }

        private VGLineCap ReadCap(SwfStream swf)
        {
            switch (swf.ReadBitUInt(2))
            {
                case 0: return VGLineCap.Round;
                case 1: return VGLineCap.Butt;
                case 2: return VGLineCap.Square;
            }
            throw new SwfCorruptedException("Inavlid line cap value!");
        }

        private VGLineJoin ReadJoin(SwfStream swf)
        {
            switch (swf.ReadBitUInt(2))
            {
                case 0: return VGLineJoin.Round;
                case 1: return VGLineJoin.Bevel;
                case 2: return VGLineJoin.Miter;
            }
            throw new SwfCorruptedException("Inavlid line join value!");
        }

        public enum FillStyleType
        {
            Solid = 0x00,
            Linear = 0x10,
            Radial = 0x12,
            Focal = 0x13,

            RepeatingBitmap = 0x40,
            ClippedBitmap = 0x41,
            RepeatingNonsmoothedBitmap = 0x42,
            ClippedNonsmoothedBitmap = 0x43
        }
    }
}
