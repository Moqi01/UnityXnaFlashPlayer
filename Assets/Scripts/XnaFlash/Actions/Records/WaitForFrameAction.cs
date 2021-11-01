using XnaFlash.Swf;

namespace XnaFlash.Actions.Records
{
    public class WaitForFrameAction : ActionRecord
    {
        public ushort Frame { get; private set; }
        public byte SkipCount { get; private set; }

        protected override void Load(SwfStream stream, ushort length)
        {
            Frame = stream.ReadUShort();
            SkipCount = stream.ReadByte();
        }
    }

    public class WaitForFrame2Action : ActionRecord
    {
        public byte SkipCount { get; private set; }

        protected override void Load(SwfStream stream, ushort length)
        {
            SkipCount = stream.ReadByte();
        }
    }
}
