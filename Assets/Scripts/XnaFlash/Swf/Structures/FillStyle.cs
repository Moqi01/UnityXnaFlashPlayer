using XnaFlash.Swf.Structures.Gradients;
using XnaVG;

namespace XnaFlash.Swf.Structures
{
    public class FillStyle
    {
        private readonly static VGMatrix GradientSquare = VGMatrix.Scale(1f / 16384f, 1f / 16384f);

        public int Index { get; private set; }
        public FillStyleType FillType { get; private set; }
        public VGColor Color { get; private set; }
        public VGMatrix Matrix { get; private set; }
        public GradientInfo Gradient { get; private set; }
        public ushort BitmapID { get; private set; }

        public bool HasAlpha;
        public FillStyle()
        {
            Index = 1;
            FillType = FillStyleType.Solid;
            Color = Microsoft.Xna.Framework.Color.Red;
            Matrix = VGMatrix.Identity;
        }

        public FillStyle(SwfStream swf, bool hasAlpha, int index)
        {
            Index = index;
            FillType = (FillStyleType)swf.ReadByte();
            HasAlpha = hasAlpha;
            switch (FillType)
            {
                case FillStyleType.Solid:
                    Matrix = VGMatrix.Identity;
                    Color = hasAlpha ? swf.ReadRGBA() : swf.ReadRGB();
                    break;
                case FillStyleType.Linear:
                case FillStyleType.Radial:
                case FillStyleType.Focal:
                    SetFillMatrix(swf.ReadMatrix());
                    Gradient = FillType == FillStyleType.Focal ? new FocalGradientInfo(swf, hasAlpha) : new GradientInfo(swf, hasAlpha);
                    break;

                case FillStyleType.RepeatingBitmap:
                case FillStyleType.RepeatingNonsmoothedBitmap:
                case FillStyleType.ClippedBitmap:
                case FillStyleType.ClippedNonsmoothedBitmap:
                    BitmapID = swf.ReadUShort();
                    SetFillMatrix(swf.ReadMatrix());
                    break;
            }
        }

        private void SetFillMatrix(VGMatrix matrix)
        {
            Matrix = ~matrix * GradientSquare;
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
