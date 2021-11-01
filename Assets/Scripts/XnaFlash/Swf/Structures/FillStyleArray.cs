
namespace XnaFlash.Swf.Structures
{
    public class FillStyleArray
    {
        public FillStyle[] Styles { get; private set; }

        public FillStyleArray(SwfStream swf, bool hasAlpha)
        {
            int count = swf.ReadByte();
            if (count == 0xFF) count = swf.ReadUShort();

            Styles = new FillStyle[count];
            for (int i = 0; i < count; i++)
                Styles[i] = new FillStyle(swf, hasAlpha, i + 1);
        }

        public FillStyleArray()
        {
            Styles = new FillStyle[] {
                new FillStyle()
            };
        }
    }
}
