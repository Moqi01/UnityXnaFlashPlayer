using XnaFlash.Swf;

namespace XnaFlash.Actions.Records
{
    public class TryAction : ActionRecord
    {
        public ActionBlock Try { get; private set; }
        public ActionBlock Catch { get; private set; }
        public ActionBlock Finally { get; private set; }
        public string CatchVariable { get; private set; }
        public byte? CatchRegister { get; private set; }

        protected override void Load(SwfStream stream, ushort length)
        {
            byte flags = stream.ReadByte();
            ushort trySize = stream.ReadUShort();
            ushort catchSize = stream.ReadUShort();
            ushort finallySize = stream.ReadUShort();

            if ((flags & 0x04) != 0)
            {
                CatchRegister = stream.ReadByte();
                CatchVariable = null;
            }
            else
            {
                CatchRegister = null;
                CatchVariable = stream.ReadString();
            }

            Try = ActionRecord.ReadActions(stream, trySize);

            if (catchSize > 0 && (flags & 0x01) != 0)
                Catch = ActionRecord.ReadActions(stream, trySize);

            if (finallySize > 0 && (flags & 0x02) != 0)
                Finally = ActionRecord.ReadActions(stream, finallySize);
        }
    }
}
