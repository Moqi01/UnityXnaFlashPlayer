using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XnaFlash.Swf.Paths;
using XnaFlash.Swf.Structures;
using XnaFlash.Swf.Structures.Fonts;
using XnaVG;
using XnaVG.Loaders;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(48)]
    public class DefineFont2Tag : ISwfDefinitionTag
    {
        public Content.CharacterType Type { get { return Content.CharacterType.Font; } }
        public ushort CharacterID { get; protected set; }
        public FontFlags Flags { get; protected set; }
        public SwfLanguage Language { get; protected set; }
        public string Name { get; protected set; }
        public FontGlyph[] Glyphs { get; protected set; }
        public char[] Characters { get; protected set; }
        public short Ascent { get; private set; }
        public short Descent { get; private set; }
        public short Leading { get; private set; }
        public short[] Advances { get; private set; }
        public Rectangle[] Bounds { get; private set; }
        public VGKerningTable Kerning { get; private set; }
        public virtual int Scale { get { return 1; } }

        #region ISwfTag Members

        public virtual void Load(SwfStream stream, uint length, byte version)
        {
            CharacterID = stream.ReadUShort();
            Flags = (FontFlags)stream.ReadByte();
            Language = stream.ReadLanguage();
            Name = stream.ReadString(stream.ReadByte());
            ushort count = stream.ReadUShort();
            if(count>0)
               stream.Skip((count + 1) * (((Flags & FontFlags.WideOffsets) != 0) ? 4 : 2)); // Offsets

            Glyphs = new FontGlyph[count];
            for (int i = 0; i < count; i++)
            {
                var subShapes = new Shape(ShapeInfo.ReadShape(stream, false, false, false, false), true).SubShapes;
                if (subShapes == null || subShapes.Length < 1) continue;

                var shape = subShapes[0];
                if (shape != null && shape.Fills.Count > 0)
                {
                    Glyphs[i] = new FontGlyph
                    {
                        GlyphPath = shape.Fills.Values.First().GetPath(),
                        ReferencePoint = new Vector2(shape.Shape.ReferencePoint.X, shape.Shape.ReferencePoint.Y) * DefineFontTag.EMSquareInv
                    };
                    Glyphs[i].GlyphPath.Scale(DefineFontTag.EMSquareInv);
                }
                else
                    Glyphs[i] = new FontGlyph { GlyphPath = new VGPath(), ReferencePoint = Vector2.Zero, SubShape = shape };
            }


            if ((Flags & FontFlags.WideCodes) != 0)
                Characters = stream.ReadCharArray((int)count);
            else
            {
                Characters = new char[count];
                for (int i = 0; i < count; i++)
                    Characters[i] = (char)stream.ReadByte();
            }

            if ((Flags & FontFlags.HasLayout) != 0)
            {
                Ascent = stream.ReadShort();
                Descent = stream.ReadShort();
                Leading = stream.ReadShort();
                Advances = stream.ReadShortArray(count);
                Bounds = stream.ReadRectangleArray(count);

                char l, r;
                int adj;
                ushort kerns = stream.ReadUShort();
                Kerning = new VGKerningTable(kerns);
                if ((Flags & FontFlags.WideCodes) != 0)
                {
                    for (int i = 0; i < kerns; i++)
                    {
                        l = (char)stream.ReadUShort();
                        r = (char)stream.ReadUShort();
                        adj = stream.ReadShort();
                        Kerning.Add(l, r, adj);
                    }
                }
                else
                {
                    for (int i = 0; i < kerns; i++)
                    {
                        l = (char)stream.ReadByte();
                        r = (char)stream.ReadByte();
                        adj = stream.ReadShort();
                        Kerning.Add(l, r, adj);
                    }
                }
            }        
        }

        #endregion

        [Flags()]
        public enum FontFlags : byte
        {
            HasLayout = 0x80,
            ShiftJIS = 0x40,
            SmallText = 0x20,           
            ANSI = 0x10,
            WideOffsets = 0x08,
            WideCodes = 0x04,
            Italic = 0x02,
            Bold = 0x01            
        }
    }

    [SwfTag(75)]
    public class DefineFont3Tag : DefineFont2Tag
    {
        public override int Scale { get { return 20; } }
    }
}