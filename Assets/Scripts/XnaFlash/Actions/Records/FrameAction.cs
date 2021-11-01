using XnaFlash.Swf;

namespace XnaFlash.Actions.Records
{
    public class FrameAction : ActionRecord
    {
        public ushort Frame { get; private set; }

        protected override void Load(SwfStream stream, ushort length)
        {
            Frame = stream.ReadUShort();
        }
    }
}
