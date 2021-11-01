using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using XnaVG.Rendering.Tesselation;

namespace XnaVG.Loaders
{
    public sealed class VGFontData
    {
        internal StencilVertex[] Vertices { get; set; }
        internal GlyphInfo[] Glyphs { get; set; }

        public VGKerningTable Kerning { get; internal set; }
        public VGFillRule FillRule { get; internal set; }
        public float EmSquareSize { get; internal set; }
        public float LeadingSize { get; internal set; }
        public Vector4 Extents { get; internal set; }

        public IEnumerable<GlyphInfo> GetGlyphs()
        {
            return Glyphs;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)FillRule);
            writer.Write(EmSquareSize);
            writer.Write(LeadingSize);
            writer.Write(Extents.X);
            writer.Write(Extents.Y);
            writer.Write(Extents.Z);
            writer.Write(Extents.W);

            writer.Write(Vertices.Length);
            writer.Write(Glyphs.Length);

            foreach (var vert in Vertices)
                vert.Serialize(writer);

            foreach (var def in Glyphs)
            {
                writer.Write(def.Character);
                writer.Write(def.Offset);
                writer.Write(def.Triangles);
                writer.Write(def.Escape.X);
                writer.Write(def.Escape.Y);
            }

            (Kerning ?? VGKerningTable.EmptyTable).Serialize(writer);
        }
        public static VGFontData Deserialize(ContentReader reader)
        {
            var data = new VGFontData
            {
                FillRule = (VGFillRule)reader.ReadByte(),
                EmSquareSize = reader.ReadSingle(),
                LeadingSize = reader.ReadSingle(),
                Extents = reader.ReadVector4(),
                Vertices = new StencilVertex[reader.ReadInt32()],
                Glyphs = new VGFontData.GlyphInfo[reader.ReadInt32()]
            };

            for (int i = 0; i < data.Vertices.Length; i++)
                data.Vertices[i] = StencilVertex.Deserialize(reader);

            for (int i = 0; i < data.Glyphs.Length; i++)
                data.Glyphs[i] = new VGFontData.GlyphInfo
                {
                    Character = reader.ReadChar(),
                    Offset = reader.ReadInt32(),
                    Triangles = reader.ReadInt32(),
                    Escape = new Vector2
                    {
                        X = reader.ReadSingle(),
                        Y = reader.ReadSingle()
                    }
                };

            data.Kerning = VGKerningTable.Deserialize(reader);
            return data;
        }
        public static VGFontData FromPaths(VGFillRule fillRule, float emSquareSize, float leadingSize, VGKerningTable kerning, IDictionary<char, VGGlyphInfo> glyphs)
        {
            StencilVertex[] vertices;
            VGFont.CharBuffer[] defs;
            Vector4 extents;
            VGFont.CombineVertices(glyphs, out vertices, out defs, out extents);

            var data = new VGFontData
            {
                FillRule = fillRule,
                EmSquareSize = emSquareSize,
                Extents = extents,
                Vertices = vertices,
                Kerning = kerning,
                LeadingSize = leadingSize,
                Glyphs = new GlyphInfo[defs.Length]
            };

            for (int i = 0; i < defs.Length; i++)
                data.Glyphs[i] = new GlyphInfo
                {
                    Character = defs[i].Character,
                    Offset = defs[i].Offset,
                    Triangles = defs[i].Triangles,
                    Escape = glyphs[defs[i].Character].Advance
                };

            return data;
        }

        public struct GlyphInfo
        {
            public char Character { get; internal set; }
            public int Offset { get; internal set; }
            public int Triangles { get; internal set; }
            public Vector2 Escape { get; internal set; }
        }
    }
}
