using System;
using System.Text;
using Microsoft.Xna.Framework;
using XnaFlash.Content;
using XnaFlash.Swf.Paths;
using XnaFlash.Swf.Structures;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(13)]
    public class DefineFontInfoTag : ISwfControlTag
    {
        public ushort FontID { get; protected set; }
        public string Name { get; protected set; }
        public FontFlags Flags { get; protected set; }
        public char[] Characters { get; protected set; }
        public SwfLanguage LanguageCode { get; protected set; }

        public virtual void ReadLangCode(SwfStream stream) 
        {
            LanguageCode = SwfLanguage.None;
        }

        #region ISwfTag Members

        public virtual void Load(SwfStream stream, uint length, byte version)
        {
            FontID = stream.ReadUShort();
            Name = stream.ReadString(stream.ReadByte());
            Flags = (FontFlags)stream.ReadByte();
            ReadLangCode(stream);

            uint count = (uint)(length - stream.TagPosition);
            if ((Flags & FontFlags.WideCodes) != 0)
            {
                count /= 2;
                Characters = stream.ReadCharArray((int)count);
            }
            else
            {
                Characters = new char[count];
                for (int i = 0; i < count; i++)
                    Characters[i] = (char)stream.ReadByte();
            }
        }

        #endregion

        [Flags()]
        public enum FontFlags : byte
        {
            SmallText = 0x20,
            ShiftJIS = 0x10,
            ANSI = 0x08,
            Italic = 0x04,
            Bold = 0x02,
            WideCodes = 0x01
        }
    }

    [SwfTag(62)]
    public class DefineFontInfo2Tag : DefineFontInfoTag
    {
        public override void ReadLangCode(SwfStream stream)
        {
            LanguageCode = stream.ReadLanguage();
        }
    }
}