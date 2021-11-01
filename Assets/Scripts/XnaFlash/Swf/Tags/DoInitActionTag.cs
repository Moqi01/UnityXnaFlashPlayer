using XnaFlash.Actions;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(59)]
    public class DoInitActionTag : ISwfControlTag
    {
        public ushort SpriteID { get; private set; }
        public ActionBlock Actions { get; private set; }

        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            SpriteID = stream.ReadUShort();
            Actions = ActionRecord.ReadActions(stream, null);
        }

        public static int count = 0;
        #endregion
    }
}
