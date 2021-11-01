
namespace XnaFlash.Swf.Tags
{
    [SwfTag(87)]
    public class DefineBinaryDataTag : ISwfDefinitionTag
    {
        public Content.CharacterType Type { get { return Content.CharacterType.BinaryData; } }
        public ushort CharacterID { get; private set; }
        public byte[] Data { get; private set; }

        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            CharacterID = stream.ReadUShort();
            stream.ReadUInt();
            Data = stream.ReadByteArray(length - 6);
        }

        #endregion
    }
}