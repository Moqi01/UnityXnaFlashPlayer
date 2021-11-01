using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaFlash.Movie;
using XnaFlash.Swf;
using XnaFlash.Swf.Tags;
using XnaVG;
using XnaVG.Loaders;

namespace XnaFlash.Content
{
    public class Text : ICharacter, Movie.IDrawable
    {
        public ushort ID { get; private set; }
        public VGMatrix Matrix { get; private set; }
        public VGPreparedPath[] TextParts { get; private set; }
        public Rectangle? Bounds { get; private set; }
        public CharacterType Type { get { return CharacterType.Text; } }

        public Text(DefineTextTag tag, ISystemServices services, FlashDocument document)
        {
            ID = tag.CharacterID;
            Matrix = tag.Matrix;
            Bounds = tag.Bounds;

            var path = new VGPath();
            var parts = new List<VGPreparedPath>();
            var scale = Vector2.One;
            var leftTop = new Vector2(tag.Bounds.Left, tag.Bounds.Top);

            Font font = null;
            ushort? lastFont = null;
            VGColor? lastColor = null;

            foreach (var rec in tag.TextRecords)
            {
                if ((rec.HasFont && lastFont != rec.FontId) || (rec.HasColor && lastColor != rec.Color))
                {
                    if (!path.IsEmpty && lastFont.HasValue && lastColor.HasValue)
                    {
                        var pp = services.VectorDevice.PreparePath(path, VGPaintMode.Fill);
                        pp.Tag = services.VectorDevice.CreateColorPaint(lastColor.Value);
                        parts.Add(pp);
                    }

                    path = new VGPath();
                }

                if (rec.HasColor) lastColor = rec.Color;
                if (rec.HasFont)
                {
                    font = document[rec.FontId] as Font;
                    scale = scale = new Vector2(rec.FontSize, rec.FontSize);
                    if (font != null) lastFont = rec.FontId;
                }

                if (font == null || !lastColor.HasValue || rec.Glyphs.Length == 0)
                    continue;

                var offset = new Vector2(rec.HasXOffset ? rec.XOffset : 0, rec.HasYOffset ? rec.YOffset : 0);
                var refPt = Vector2.Zero;
                if (rec.Glyphs[0].GlyphIndex < font.GlyphFont.Length)
                    refPt = font.GlyphFont[rec.Glyphs[0].GlyphIndex].ReferencePoint * scale;
                var xoff = Vector2.Zero;

                foreach (var g in rec.Glyphs)
                {
                    if (g.GlyphIndex >= font.GlyphFont.Length) continue;

                    var fg = font.GlyphFont[g.GlyphIndex];
                    if (fg.GlyphPath == null) continue;

                    var rpt = fg.ReferencePoint.X * scale.X;

                    var p = fg.GlyphPath.Clone();
                    p.Scale(scale);
                    p.Offset(offset + xoff);
                    path.Append(p);

                    xoff.X += g.GlyphAdvance;
                }
            }

            if (!path.IsEmpty && lastFont.HasValue && lastColor.HasValue)
            {
                var pp = services.VectorDevice.PreparePath(path, VGPaintMode.Fill);
                pp.Tag = services.VectorDevice.CreateColorPaint(lastColor.Value);
                parts.Add(pp);
            }

            TextParts = parts.ToArray();
        }

        public Movie.IDrawable MakeInstance(Movie.DisplayObject container, RootMovieClip root) { return this; }
        public void Draw(IVGRenderContext<Movie.DisplayState> target) 
        {
            target.State.PathToSurface.PushCombineLeft(Matrix);
            foreach (var part in TextParts)
            {
                target.State.SetFillPaint(part.Tag as VGPaint);
                target.DrawPath(part, VGPaintMode.Fill);
            }
            target.State.PathToSurface.Pop();
        }        
        public void SetParent(StageObject parent) { }
        public void OnNextFrame() { }

        public void Dispose()
        {
            if (TextParts != null)
            {
                foreach (var part in TextParts)
                {
                    (part.Tag as IDisposable).Dispose();
                    part.Dispose();
                }
                TextParts = null;
            }
        }
    }
}
