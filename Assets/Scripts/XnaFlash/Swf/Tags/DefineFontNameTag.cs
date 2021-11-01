using System;
using System.Text;
using Microsoft.Xna.Framework;
using XnaFlash.Content;
using XnaFlash.Swf.Paths;
using XnaFlash.Swf.Structures;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(88)]
    public class DefineFontNameTag : ISwfControlTag
    {
        public ushort FontID { get; protected set; }
        public string Name { get; protected set; }
        public string Copyright { get; protected set; }

        #region ISwfTag Members

        public virtual void Load(SwfStream stream, uint length, byte version)
        {
            FontID = stream.ReadUShort();
            Name = stream.ReadString();
            Copyright = stream.ReadString();
        }

        #endregion
    }
}