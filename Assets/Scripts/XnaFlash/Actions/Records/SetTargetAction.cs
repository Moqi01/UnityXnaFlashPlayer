using XnaFlash.Swf;

namespace XnaFlash.Actions.Records
{
    public class SetTargetAction : ActionRecord
    {
        public string Target { get; private set; }

        protected override void Load(SwfStream stream, ushort length)
        {
            Target = stream.ReadString();
        }
    }
}
