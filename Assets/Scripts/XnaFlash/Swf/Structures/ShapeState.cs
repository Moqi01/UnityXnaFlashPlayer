
namespace XnaFlash.Swf.Structures
{
    public class ShapeState
    {
        public FillStyleArray FillStyles { private get; set; }
        public LineStyleArray LineStyles { private get; set; }
        public int FillBits { get; set; }
        public int LineBits { get; set; }

        public FillStyle GetFill(int index)
        {
            if (index == 0) return null;
            return FillStyles.Styles[index - 1];
        }

        public LineStyle GetLine(int index)
        {
            if (index == 0) return null;
            return LineStyles.Styles[index - 1];
        }

        public FillStyle[] GetFillStyles()
        {
            return FillStyles.Styles;
        }

        public LineStyle[] GetLineStyles()
        {
            return LineStyles.Styles;
        }
    }
}
