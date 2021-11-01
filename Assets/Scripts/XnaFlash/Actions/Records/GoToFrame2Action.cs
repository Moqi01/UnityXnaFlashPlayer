using XnaFlash.Swf;

namespace XnaFlash.Actions.Records
{
    public class GoToFrame2Action : ActionRecord
    {
        public ushort SceneBias { get; private set; }
        public bool Play { get; private set; }

        protected override void Load(SwfStream stream, ushort length)
        {
            byte flags = stream.ReadByte();
            SceneBias = ((flags & 0x02) != 0) ? stream.ReadUShort() : (ushort)0;
            Play = (flags & 0x01) != 0;
        }
    }
}
