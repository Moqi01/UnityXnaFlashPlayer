
namespace XnaFlash.Swf.Tags
{
    [SwfTag(66)]
    public class SetTabIndexTag : ISwfControlTag
    {
        public ushort TabIndex { get; private set; }
        public ushort Depth { get; private set; }
   
        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            Depth = stream.ReadUShort();
            TabIndex = stream.ReadUShort();            
        }

        #endregion
    }
}
