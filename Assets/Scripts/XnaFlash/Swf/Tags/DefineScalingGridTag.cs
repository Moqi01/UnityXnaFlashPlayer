using Microsoft.Xna.Framework;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(78)]
    public class DefineScalingGridTag : ISwfControlTag
    {
        public ushort CharacterID { get; private set; }
        public Rectangle Splitter { get; private set; }

        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            CharacterID = stream.ReadUShort();
            Splitter = stream.ReadRectangle();            
        }

        #endregion
    }
}
