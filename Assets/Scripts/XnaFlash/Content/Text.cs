using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Unity.VectorGraphics;
using XnaFlash.Movie;
using XnaFlash.Swf;
using XnaFlash.Swf.Tags;
using XnaVG;
using XnaVG.Loaders;
using XnaVG.Paints;
using static XnaFlash.Swf.Paths.Shape;

namespace XnaFlash.Content
{
    public class Text : ICharacter, Movie.IDrawable
    {
        public ushort ID { get; private set; }
        public VGMatrix Matrix { get; private set; }
        public VGPreparedPath[] TextParts { get; private set; }
        public Rectangle? Bounds { get; private set; }
        public CharacterType Type { get { return CharacterType.Text; } }
        private List<SubShape> _subShapes = new List<SubShape>();

        public Text(DefineTextTag tag, ISystemServices services, FlashDocument document)
        {
            ID = tag.CharacterID;
            Matrix = tag.Matrix;
            Bounds = tag.Bounds;

            var path = new VGPath();
            var parts = new List<VGPreparedPath>();
            var scale = Vector2.One;
            var scaleP = Vector2.One;
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
                    scale = new Vector2(rec.FontSize, rec.FontSize);
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

                    if (fg.SubShape!=null)
                    {
                        _subShapes.Add(fg.SubShape);
                        for (int i = 0; i < fg.SubShape.shapeParser.shapes.Count; i++)
                        {
                            Vector2 NewOffset = offset + xoff;
                            //Matrix2D matrix2D = fg.SubShape.shapeParser.shapes[i].FillTransform;
                            //matrix2D.m00 *= scale.X;
                            //matrix2D.m11 *= scale.Y;
                            //matrix2D.m02 += offsets.X;
                            //matrix2D.m12 += offsets.Y;
                            //fg.SubShape.shapeParser.shapes[i].FillTransform= matrix2D;
                            for (int j = 0; j < fg.SubShape.shapeParser.shapes[i].Contours.Length; j++)
                            {
                                for (int s = 0; s < fg.SubShape.shapeParser.shapes[i].Contours[j].Segments.Length; s++)
                                {
                                    fg.SubShape.shapeParser.shapes[i].Contours[j].Segments[s].P0.x+= NewOffset.X;
                                    fg.SubShape.shapeParser.shapes[i].Contours[j].Segments[s].P0.y+= NewOffset.Y;
                                    fg.SubShape.shapeParser.shapes[i].Contours[j].Segments[s].P1.x += NewOffset.X;
                                    fg.SubShape.shapeParser.shapes[i].Contours[j].Segments[s].P1.y += NewOffset.Y;
                                    fg.SubShape.shapeParser.shapes[i].Contours[j].Segments[s].P2.x += NewOffset.X;
                                    fg.SubShape.shapeParser.shapes[i].Contours[j].Segments[s].P2.y += NewOffset.Y;
                                }
                            }
                        }
                    }
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
            var state = target.State;
            foreach (var shape in _subShapes)
            {
                if (DrawGL.ins.isNewMeshMake)
                {
                    Texture2D texture = null;
                    bool isSolidFill = false;
                    //DrawGL.ins.SetMatrices(state.PathToSurface.Matrix, state.Projection.Matrix, state.PathToFillPaint.Matrix);
                    for (int index = 0; index < shape.shapeParser.shapes.Count; index++)
                    {
                        if (shape.shapeParser.shapes[index].Fill is SolidFill)
                        {
                            isSolidFill = true;
                            //break;
                        }
                        else
                        {
                            VGPaint paint = shape.shapeParser.Paint[index];

                            if (shape.shapeParser.shapes[index].Fill is TextureFill)
                            {
                                //if (paint is VGPatternPaint)
                                {
                                    texture = (paint as VGPatternPaint).Pattern.Texture;
                                }
                            }
                            else if (shape.shapeParser.shapes[index].Fill is GradientFill)
                            {
                                //VGPaint paint = shape.shapeParser.Paint[index];
                                //if (paint is VGGradientPaint)
                                {
                                    texture = (paint as VGGradientPaint).Gradient;
                                }
                            }
                            else if (shape.shapeParser.shapes[index].Fill is PatternFill)
                            {
                                //VGPaint paint = shape.shapeParser.Paint[index];
                                //if (paint is VGPatternPaint)
                                {
                                    texture = (paint as VGPatternPaint).Pattern.Texture;
                                }
                            }

                        }
                        if (shape.shapeParser.mesh.Count <= index)
                        {
                            shape.shapeParser.mesh.Add(DrawGL.ins.SetMesh(shape.shapeParser.shapes[index], texture));
                        }
                        var cxForm = state.ColorTransformationEnabled ? state.ColorTransformation.CxForm : VGCxForm.Identity;
                        DrawGL.ins.SetDrawShape(shape.shapeParser.mesh[index], state.PathToSurface.Matrix, state.Projection.Matrix, texture, cxForm);
                    }
                }
                else
                {
                   
                }

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
