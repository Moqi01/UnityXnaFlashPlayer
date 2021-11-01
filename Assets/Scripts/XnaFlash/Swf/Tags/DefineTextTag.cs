using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XnaFlash.Swf.Paths;
using XnaFlash.Swf.Structures;
using XnaFlash.Swf.Structures.Fonts;
using XnaVG;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(11)]
    public class DefineTextTag : ISwfDefinitionTag
    {
        public virtual bool HasAlpha { get { return false; } }
        public Content.CharacterType Type { get { return Content.CharacterType.Text; } }
        public ushort CharacterID { get; protected set; }        
        public Rectangle Bounds { get; protected set; }
        public VGMatrix Matrix { get; protected set; }
        public TextRecord[] TextRecords { get; protected set; }

        #region ISwfTag Members

        public virtual void Load(SwfStream stream, uint length, byte version)
        {
            CharacterID = stream.ReadUShort();
            Bounds = stream.ReadRectangle();
            Matrix = stream.ReadMatrix();

            if (CharacterID == 347)
            { }

            byte gBits = stream.ReadByte();
            byte aBits = stream.ReadByte();

            List<TextRecord> recs = new List<TextRecord>();
            while (true)
            {
                var rec = new TextRecord(stream, HasAlpha, gBits, aBits);
                if (rec.EndRecord) break;
                recs.Add(rec);
            }
            TextRecords = recs.ToArray();
        }

        #endregion
    }

    [SwfTag(33)]
    public class DefineText2Tag : DefineTextTag
    {
        public override bool HasAlpha { get { return true; } }
    }
}