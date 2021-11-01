using XnaFlash.Actions;
using XnaFlash.Swf;

namespace XnaFlash.Actions.Records
{
    public class GetURLAction : ActionRecord
    {
        public string Url { get; private set; }
        public string Target { get; private set; }

        protected override void Load(SwfStream stream, ushort length)
        {
            Url = stream.ReadString();
            Target = stream.ReadString();
        }
    }

    public class GetURL2Action : ActionRecord
    {
        public GetURLFlags Flags { get; private set; }

        protected override void Load(SwfStream stream, ushort length)
        {
            Flags = (GetURLFlags)stream.ReadByte();
        }
    }
}
