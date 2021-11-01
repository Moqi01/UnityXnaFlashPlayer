using XnaFlash.Swf.Structures;
using XnaVG;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(26)]
    public class PlaceObject2Tag : ISwfControlTag
    {
        private byte mFlags;

        public bool HasActions { get { return (mFlags & 0x80) != 0; } }
        public bool HasClipDepth { get { return (mFlags & 0x40) != 0; } }
        public bool HasName { get { return (mFlags & 0x20) != 0; } }
        public bool HasRatio { get { return (mFlags & 0x10) != 0; } }
        public bool HasCxForm { get { return (mFlags & 0x08) != 0; } }
        public bool HasMatrix { get { return (mFlags & 0x04) != 0; } }
        public bool HasCharacter { get { return (mFlags & 0x02) != 0; } }
        public bool HasMove { get { return (mFlags & 0x01) != 0; } }

        public ushort CharacterID { get; private set; }
        public ushort ClipDepth { get; private set; }
        public ushort Depth { get; private set; }
        public ushort Ratio { get; private set; }
        public string Name { get; private set; }
        public VGMatrix Matrix { get; private set; }
        public VGCxForm CxForm { get; private set; }
        public ClipActions Actions { get; private set; }

        public PlaceObject2Tag() { }
        public PlaceObject2Tag(PlaceObjectTag tag)
        {
            mFlags = 0x0E; // Matrix | Character | CxForm
            Depth = tag.Depth;

            CharacterID = tag.CharacterID;
            Matrix = tag.Matrix;
            CxForm = tag.CxForm;
        }

        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            mFlags = stream.ReadByte();
            Depth = stream.ReadUShort();
            if (HasCharacter) CharacterID = stream.ReadUShort();
            if (HasMatrix) Matrix = stream.ReadMatrix();
            if (HasCxForm) CxForm = stream.ReadCxForm(true);
            if (HasRatio) Ratio = stream.ReadUShort();
            if (HasName) Name = stream.ReadString();
            if (HasClipDepth) ClipDepth = stream.ReadUShort();
            if (HasActions) Actions = new ClipActions(stream);
        }

        #endregion
    }
}
