using XnaFlash.Swf;

namespace XnaFlash.Actions.Records
{
    public class GoToLabelAction : ActionRecord
    {
        public string Label { get; private set; }

        protected override void Load(SwfStream stream, ushort length)
        {
            Label = stream.ReadString();
        }
    }
}
