using XnaVG;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(4)]
    public class PlaceObjectTag : ISwfControlTag
    {
        public ushort CharacterID { get; private set; }
        public ushort Depth { get; private set; }
        public VGMatrix Matrix { get; private set; }
        public VGCxForm CxForm { get; private set; }

        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            CharacterID = stream.ReadUShort();
            Depth = stream.ReadUShort();
            Matrix = stream.ReadMatrix();
            CxForm = stream.TagPosition < length ? stream.ReadCxForm(false) : VGCxForm.Identity;
        }

        #endregion
    }
}
