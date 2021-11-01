
namespace XnaFlash.Swf.Tags
{
    [SwfTag(43)]
    public class FrameLabelTag : ISwfControlTag
    {
        public string Label { get; private set; }
        public bool IsAnchor { get; private set; }

        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            Label = stream.ReadString();
            IsAnchor = (stream.TagPosition < length) && (stream.ReadByte() != 0);                
        }

        #endregion
    }
}
