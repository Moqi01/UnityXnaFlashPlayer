using System;
using XnaVG;

namespace XnaFlash.Swf.Structures
{
    public class ButtonRecord
    {
        private byte mFlags;

        public ushort CharacterID { get; private set; }
        public ushort CharacterDepth { get; private set; }
        public VGMatrix CharacterMatrix { get; private set; }
        public VGCxForm CxForm { get; private set; }
        public Filter[] Filters { get; private set; }
        public BlendMode Blending { get; private set; }
        public bool HitTest { get { return (mFlags & 0x08) != 0; } }
        public bool Down { get { return (mFlags & 0x04) != 0; } }
        public bool Over { get { return (mFlags & 0x02) != 0; } }
        public bool Up { get { return (mFlags & 0x01) != 0; } }

        public ButtonRecord(SwfStream stream, Type defButtonType, out bool ok)
        {
            mFlags = stream.ReadByte();
            ok = (mFlags != 0);
            if (!ok)
                return;

            bool hasBlend = (mFlags & 0x20) != 0;
            bool hasFilters = (mFlags & 0x10) != 0;
            CharacterID = stream.ReadUShort();
            CharacterDepth = stream.ReadUShort();
            CharacterMatrix = stream.ReadMatrix();

            if (defButtonType == typeof(Tags.DefineButton2Tag))
            {
                CxForm = stream.ReadCxForm(true);
                Filters = hasFilters ? Filter.ReadFilterList(stream) : new Filter[0];
                Blending = hasBlend ? (BlendMode)stream.ReadByte() : BlendMode.Normal;
            }
            else
            {
                CxForm = VGCxForm.Identity;
                Filters = new Filter[0];
                Blending = BlendMode.Normal;
            }

        }
    }
}