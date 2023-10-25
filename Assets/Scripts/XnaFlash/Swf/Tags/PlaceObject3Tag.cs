using XnaFlash.Swf.Structures;
using XnaVG;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(70)]
    public class PlaceObject3Tag : ISwfControlTag
    {
        private ushort mFlags;

        public bool HasActions { get { return (mFlags & 0x0080) != 0; } }
        public bool HasClipDepth { get { return (mFlags & 0x0040) != 0; } }
        public bool HasName { get { return (mFlags & 0x0020) != 0; } }
        public bool HasRatio { get { return (mFlags & 0x0010) != 0; } }
        public bool HasCxForm { get { return (mFlags & 0x0008) != 0; } }
        public bool HasMatrix { get { return (mFlags & 0x0004) != 0; } }
        public bool HasCharacter { get { return (mFlags & 0x0002) != 0; } }
        public bool HasMove { get { return (mFlags & 0x0001) != 0; } }
        public bool HasImage { get { return (mFlags & 0x1000) != 0; } }
        public bool HasClassName { get { return (mFlags & 0x0800) != 0; } }
        public bool HasCacheAsBitmap { get { return (mFlags & 0x0400) != 0; } }
        public bool HasBlendMode { get { return (mFlags & 0x0200) != 0; } }
        public bool HasFilterList { get { return (mFlags & 0x0100) != 0; } }

        public byte BitmapCache { get; private set; }
        public ushort CharacterID { get; private set; }
        public ushort ClipDepth { get; private set; }
        public ushort Depth { get; private set; }
        public ushort Ratio { get; private set; }
        public string Name { get; private set; }
        public string ClassName { get; private set; }
        public VGMatrix Matrix { get; private set; }
        public VGCxForm CxForm { get; private set; }
        public ClipActions Actions { get; private set; }
        public BlendMode BlendMode { get; private set; }
        public Filter[] Filters { get; private set; }

        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            mFlags = stream.ReadUShort();
            Depth = stream.ReadUShort();
            if (HasClassName || (HasImage && HasCharacter&&version<7)) ClassName = stream.ReadString();
            if (HasCharacter) CharacterID = stream.ReadUShort();
            if (HasMatrix) Matrix = stream.ReadMatrix();
            if (HasCxForm) CxForm = stream.ReadCxForm(true);
            if (HasRatio) Ratio = stream.ReadUShort();
            if (HasName) Name = stream.ReadString();
            if (HasClipDepth) ClipDepth = stream.ReadUShort();
            Filters = HasFilterList ? Filter.ReadFilterList(stream) : new Filter[0];
            if (HasBlendMode) BlendMode = (BlendMode)stream.ReadByte();
            if (HasCacheAsBitmap) BitmapCache = stream.ReadByte();
            if (HasActions) Actions = new ClipActions(stream);
        }

        #endregion
    }
}
