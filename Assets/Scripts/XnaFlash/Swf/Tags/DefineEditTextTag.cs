using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XnaFlash.Swf.Paths;
using XnaFlash.Swf.Structures;
using XnaVG;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(37)]
    public class DefineEditTextTag : ISwfDefinitionTag
    {
        public virtual bool HasAlpha { get { return false; } }
        public Content.CharacterType Type { get { return Content.CharacterType.EditText; } }
        public ushort CharacterID { get; protected set; }        
        public Rectangle Bounds { get; protected set; }
        public EditTextFlags Flags { get; protected set; }
        public ushort FontID { get; protected set; }
        public string FontClass { get; protected set; }
        public ushort FontHeight { get; protected set; }
        public VGColor TextColor { get; protected set; }
        public ushort MaxLength { get; protected set; }
        public Align TextAlign { get; protected set; }
        public ushort LeftMargin { get; protected set; }
        public ushort RightMargin { get; protected set; }
        public ushort Indent { get; protected set; }
        public short Leading { get; protected set; }
        public string Variable { get; protected set; }
        public string InitialText { get; protected set; }

        #region ISwfTag Members

        public virtual void Load(SwfStream stream, uint length, byte version)
        {
            CharacterID = stream.ReadUShort();
            Bounds = stream.ReadRectangle();
            Flags = (EditTextFlags)stream.ReadUShort();
            if ((Flags & EditTextFlags.HasFont) != 0) FontID = stream.ReadUShort();
            if ((Flags & EditTextFlags.HasFontClass) != 0) FontClass = stream.ReadString();
            if ((Flags & EditTextFlags.HasFont) != 0) FontHeight = stream.ReadUShort();
            TextColor = ((Flags & EditTextFlags.HasTextColor) != 0) ? stream.ReadRGBA() : (VGColor)Color.Black;
            MaxLength = ((Flags & EditTextFlags.HasMaxLength) != 0) ? stream.ReadUShort() : ushort.MaxValue;
            if ((Flags & EditTextFlags.HasLayout) != 0)
            {
                TextAlign = (Align)stream.ReadByte();
                LeftMargin = stream.ReadUShort();
                RightMargin = stream.ReadUShort();
                Indent = stream.ReadUShort();
                Leading = stream.ReadShort();
            }
            Variable = stream.ReadString();
            InitialText = ((Flags & EditTextFlags.HasText) != 0) ? stream.ReadString() : string.Empty;
        }

        #endregion

        public enum Align : byte
        {
            Left = 0,
            Right = 1,
            Center = 2,
            Justify = 3
        }

        [Flags()]
        public enum EditTextFlags : ushort
        {
            HasText = 0x0080,
            WordWrap = 0x0040,
            Multiline = 0x0020,
            Password = 0x0010,
            ReadOnly = 0x0008,
            HasTextColor = 0x0004,
            HasMaxLength = 0x0002,
            HasFont = 0x0001,
            HasFontClass = 0x8000,
            AutoSize = 0x4000,
            HasLayout = 0x2000,
            NoSelect = 0x1000,
            Border = 0x0800,
            WasStatic = 0x0400,
            HTML = 0x0200,
            UseOutlines = 0x0100
        }
    }
}
