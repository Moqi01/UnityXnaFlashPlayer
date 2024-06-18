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
    [SwfTag(10)]
    public class DefineFontTag : ISwfDefinitionTag
    {
        public static readonly Vector2 EMSquareInv = new Vector2(1f / 1024f, 1f / 1024f);

        public Content.CharacterType Type { get { return Content.CharacterType.Font; } }
        public ushort CharacterID { get; protected set; }        
        public FontGlyph[] Glyphs { get; protected set; }

        #region ISwfTag Members

        public virtual void Load(SwfStream stream, uint length, byte version)
        {
            CharacterID = stream.ReadUShort();

            int count = stream.ReadUShort() / 2;
            stream.Skip((count - 1) * 2);

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
                        ReferencePoint = new Vector2(shape.Shape.ReferencePoint.X, shape.Shape.ReferencePoint.Y) * EMSquareInv
                    };
                    Glyphs[i].GlyphPath.Scale(EMSquareInv);
                }
                else
                    Glyphs[i] = new FontGlyph { GlyphPath = new VGPath(), ReferencePoint = Vector2.Zero,SubShape=shape };                    
            }
        }

        #endregion
    }
}