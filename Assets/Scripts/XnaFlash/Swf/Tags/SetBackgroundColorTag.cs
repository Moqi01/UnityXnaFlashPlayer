using XnaVG;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(9)]
    public class SetBackgroundColorTag : ISwfControlTag
    {
        public VGColor Color { get; private set; }

        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            Color = stream.ReadRGB();
        }

        #endregion
    }
}
