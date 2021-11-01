
namespace XnaFlash.Swf.Structures
{
    public class LineStyleArray
    {
        public LineStyle[] Styles { get; private set; }

        public LineStyleArray(SwfStream swf, bool hasAlpha, bool isExtended)
        {
            int count = swf.ReadByte();
            if (count == 0xFF) count = swf.ReadUShort();

            Styles = new LineStyle[count];
            for (int i = 0; i < count; i++)
                Styles[i] = new LineStyle(swf, hasAlpha, isExtended, i + 1);
        }

        public LineStyleArray()
        {
            Styles = new LineStyle[]
            {
                new LineStyle()
            };
        }
    }
}
