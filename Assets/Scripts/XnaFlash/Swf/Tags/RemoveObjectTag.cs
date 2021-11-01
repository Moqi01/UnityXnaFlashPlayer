
namespace XnaFlash.Swf.Tags
{
    [SwfTag(5)]
    public class RemoveObjectTag : ISwfControlTag
    {
        public ushort CharacterID { get; private set; }
        public ushort Depth { get; private set; }

        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            CharacterID = stream.ReadUShort();
            Depth = stream.ReadUShort();
        }

        #endregion
    }

    [SwfTag(28)]
    public class RemoveObject2Tag : ISwfControlTag
    {
        public ushort Depth { get; private set; }

        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            Depth = stream.ReadUShort();
        }

        #endregion
    }
}
