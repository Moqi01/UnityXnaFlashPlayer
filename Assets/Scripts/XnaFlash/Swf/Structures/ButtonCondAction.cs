using XnaFlash.Actions;

namespace XnaFlash.Swf.Structures
{
    public class ButtonCondAction
    {
        private byte mFlags, mKey;

        public ActionBlock Actions { get; private set; }
        public KeyCode KeyPress { get { return new KeyCode((byte)(mKey >> 1)); } }
        public bool IdleToOverDown { get { return (mFlags & 0x80) != 0; } }
        public bool OutDownToIdle { get { return (mFlags & 0x40) != 0; } }
        public bool OutDownToOverDown { get { return (mFlags & 0x20) != 0; } }
        public bool OverDownToOutDown { get { return (mFlags & 0x10) != 0; } }
        public bool OverDownToOverUp { get { return (mFlags & 0x08) != 0; } }
        public bool OverUpToOverDown { get { return (mFlags & 0x04) != 0; } }
        public bool OverUpToIdle { get { return (mFlags & 0x02) != 0; } }
        public bool IdleToOverUp { get { return (mFlags & 0x01) != 0; } }
        public bool OverDownToIdle { get { return (mKey & 0x01) != 0; } }

        public ButtonCondAction(ActionBlock actions)
        {
            mFlags = 0x08; // Release
            Actions = actions;
        }

        public ButtonCondAction(SwfStream stream)
        {
            ushort size = stream.ReadUShort();
            mFlags = stream.ReadByte();
            mKey = stream.ReadByte();
            Actions = ActionRecord.ReadActions(stream, null);
        }
    }
}