using XnaFlash.Swf;

namespace XnaFlash.Actions.Records
{
    public class StoreRegisterAction : ActionRecord
    {
        public byte Register { get; private set; }

        protected override void Load(SwfStream stream, ushort length)
        {
            Register = stream.ReadByte();
        }
    }
}
