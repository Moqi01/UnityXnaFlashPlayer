using XnaFlash.Swf;

namespace XnaFlash.Actions.Records
{
    public class WithAction : ActionRecord
    {
        public ActionBlock Actions { get; private set; }

        protected override void Load(SwfStream stream, ushort length)
        {
            Actions = ActionRecord.ReadActions(stream, stream.ReadUShort());
        }
    }
}
