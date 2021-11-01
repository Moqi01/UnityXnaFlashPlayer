using XnaFlash.Actions;


namespace XnaFlash.Swf.Tags
{
    [SwfTag(12)]
    public class DoActionTag : ISwfControlTag
    {
        public ActionBlock Actions { get; private set; }

        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            Actions = ActionRecord.ReadActions(stream, null);
        }

        #endregion
    }
}
