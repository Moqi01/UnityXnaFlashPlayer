using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using XnaVG;
using XnaVG.Loaders;

namespace XnaVGPipeline.Font
{
    //[ContentProcessor(DisplayName = "Font - XnaVG")]
    public class FontProcessor : ContentProcessor<GlyphTypeface, VGFontData>
    {
        [DisplayName("Embedded Characters")]
        public string Filter { get; set; }

        /*[DisplayName("Exclude Kerning Table")]
        public bool ExcludeKerning { get; set; }*/

        public override VGFontData Process(GlyphTypeface input, ContentProcessorContext context)
        {
            var glyphs = new Dictionary<char, VGGlyphInfo>(input.CharacterToGlyphMap.Count);
            foreach (var charToGlyph in FilterChars(input))
                if (charToGlyph.Key < char.MaxValue)
                    glyphs.Add((char)charToGlyph.Key, LoadGlyph(charToGlyph.Value, input));

            if (!glyphs.ContainsKey(VGFont.MissingGlyphCode))
                glyphs.Add(VGFont.MissingGlyphCode, LoadGlyph(0, input));

            return VGFontData.FromPaths(VGFillRule.EvenOdd, 1f, (float)input.Height, null, glyphs);
        }

        private VGGlyphInfo LoadGlyph(ushort code, GlyphTypeface face)
        {
            var geometry = face.GetGlyphOutline(code, 1, 1);            
            var path = PathGeometry.CreateFromGeometry(geometry);

            var draw = new VGPath();
            foreach (var fig in path.Figures)
            {
                if (!fig.IsFilled)
                    continue;

                draw.MoveTo(fig.StartPoint.ToVec2());

                foreach (var seg in fig.Segments)
                {
                    if (seg is BezierSegment)
                    {
                        var s = seg as BezierSegment;
                        draw.CubicTo(s.Point1.ToVec2(), s.Point2.ToVec2(), s.Point3.ToVec2());
                    }
                    else if (seg is QuadraticBezierSegment)
                    {
                        var s = seg as QuadraticBezierSegment;
                        draw.QuadraticTo(s.Point1.ToVec2(), s.Point2.ToVec2());
                    }
                    else if (seg is LineSegment)
                    {
                        var s = seg as LineSegment;
                        draw.LineTo(s.Point.ToVec2());
                    }
                    else if (seg is PolyBezierSegment)
                    {
                        var s = seg as PolyBezierSegment;
                        for (int i = 0; i < s.Points.Count; i += 3)
                            draw.CubicTo(s.Points[i].ToVec2(), s.Points[i + 1].ToVec2(), s.Points[i + 2].ToVec2());
                    }
                    else if (seg is PolyQuadraticBezierSegment)
                    {
                        var s = seg as PolyQuadraticBezierSegment;
                        for (int i = 0; i < s.Points.Count; i += 2)
                            draw.QuadraticTo(s.Points[i].ToVec2(), s.Points[i + 1].ToVec2());
                    }
                    else if (seg is PolyLineSegment)
                    {
                        var s = seg as PolyLineSegment;
                        foreach (var pt in s.Points)
                            draw.LineTo(pt.ToVec2());
                    }
                    else
                        throw new NotSupportedException(seg.GetType().Name + " is not supported!");
                }

                if (fig.IsClosed)
                    draw.ClosePath();
            }

            return new VGGlyphInfo
            {
                Path = draw,
                Advance = new Vector2((float)face.AdvanceWidths[code], (float)face.AdvanceHeights[code])
            };
        }

        private IEnumerable<KeyValuePair<int, ushort>> FilterChars(GlyphTypeface input)
        {
            if (string.IsNullOrEmpty(Filter))
                return input.CharacterToGlyphMap.AsEnumerable();

            return Filter
                .Distinct()
                .Where(c => input.CharacterToGlyphMap.ContainsKey(c))
                .Select(c => new KeyValuePair<int, ushort>(c, input.CharacterToGlyphMap[c]));
        }
    }
}