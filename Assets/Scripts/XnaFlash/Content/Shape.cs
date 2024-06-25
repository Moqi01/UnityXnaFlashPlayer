using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Unity.VectorGraphics;
using XnaFlash.Movie;
using XnaFlash.Swf;
using XnaFlash.Swf.Structures;
using XnaFlash.Swf.Structures.Gradients;
using XnaFlash.Swf.Tags;
using XnaVG;
using XnaVG.Paints;
using static XnaFlash.Swf.Structures.FillStyle;

namespace XnaFlash.Content
{
    public class Shape : ICharacter, Movie.IDrawable
    {
        private SubShape[] _subShapes;

        public ushort ID { get; private set; }
        public CharacterType Type { get { return CharacterType.Shape; } }
        public Rectangle? Bounds { get; private set; }

        public Shape(ISwfDefinitionTag tag, ISystemServices services, FlashDocument document)
        {
            var state = services.VectorDevice.State;
            var t = tag as DefineShapeTag;
            var shapes = t.Shape.SubShapes;

            ID = tag.CharacterID;
            Bounds = t.ShapeBounds;
            _subShapes = new SubShape[shapes.Length];
            for (int s = 0; s < _subShapes.Length; s++)
            {
                var subShape = _subShapes[s] = new SubShape();
                var shape = shapes[s];
                subShape.shapeParser = shape.shapeParser;

                subShape._fills = new SubShapeFill[shape.Fills.Count];
                subShape._strokes = new SubShapeStroke[shape.Lines.Count];                

                var paints = new List<VGPaint>(shape.Fills.Count + shape.Lines.Count);
                int i = 0;
                VGPaint paint;

                if (DrawGL.ins.isNewDraw)
                {

                    for ( ; i < shape.shapeParser.FillStyles.Count; i++)
                    {
                        //if(shape.shapeParser.FillStyles[i].FillType!= FillStyleType.Solid)
                               shape.shapeParser.Paint.Add( MakeFill(shape.shapeParser.FillStyles[i], services.VectorDevice, document));
                    }
                }
                i = 0;
                foreach (var f in shape.Fills)
                {
                    paints.Add(paint = MakeFill(f.Key, services.VectorDevice, document));
                    subShape._fills[i++] = new SubShapeFill
                    {
                        Paint = paint,
                        FillMatrix = f.Key.Matrix,
                        Path = services.VectorDevice.PreparePath(f.Value.GetPath(), VGPaintMode.Fill)
                    };
                }

                i = 0;
                foreach (var l in shape.Lines)
                {
                    paint = l.Key.HasFill ? MakeFill(l.Key.Fill, services.VectorDevice, document) : services.VectorDevice.CreateColorPaint(l.Key.Color);
                    paints.Add(paint);

                    state.StrokeStartCap = l.Key.StartCapStyle;
                    state.StrokeEndCap = l.Key.EndCapStyle;
                    state.StrokeJoin = l.Key.JoinStyle;
                    state.StrokeMiterLimit = (float)l.Key.MiterLimit;
                    subShape._strokes[i++] = new SubShapeStroke
                    {
                        Paint = paint,
                        Path = services.VectorDevice.PreparePath(l.Value.GetPath(), VGPaintMode.Stroke),
                        Thickness = l.Key.Width,
                        FillMatrix = l.Key.HasFill ? l.Key.Fill.Matrix : VGMatrix.Identity,
                        ScaleX = !l.Key.NoHScale,
                        ScaleY = !l.Key.NoVScale
                    };
                }

                subShape._paints = paints.ToArray();
            }
        }

        public void Draw(IVGRenderContext<Movie.DisplayState> target)
        {
            int i, e;
            float factor;
            var state = target.State;
            var matrix = state.PathToSurface.Matrix;
            var scaleX = matrix.ScaleX;
            var scaleY = matrix.ScaleY;
            var scaleAvg = (scaleX + scaleY) / 2f;
            
            foreach (var shape in _subShapes)
            {
                if (DrawGL.ins.isNewDraw)
                {
                    Texture2D texture = null;
                    bool isSolidFill=false;
                    //DrawGL.ins.SetMatrices(state.PathToSurface.Matrix, state.Projection.Matrix, state.PathToFillPaint.Matrix);
                    //for (int index = 0; index < shape.shapeParser.FillStyles.Count; index++)
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
                                if (paint is VGPatternPaint)
                                {
                                    texture = (paint as VGPatternPaint).Pattern.Texture;
                                }
                            }
                            else if (shape.shapeParser.shapes[index].Fill is GradientFill)
                            {
                                //VGPaint paint = shape.shapeParser.Paint[index];
                                if (paint is VGGradientPaint)
                                {
                                    texture = (paint as VGGradientPaint).Gradient;
                                }
                            }
                            else if (shape.shapeParser.shapes[index].Fill is PatternFill)
                            {
                                //VGPaint paint = shape.shapeParser.Paint[index];
                                if (paint is VGPatternPaint)
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
                    /* if (shape.shapeParser.shapes.Count > 0)
                     {
                         if (shape.shapeParser.shapes[0].Fill is TextureFill)
                         {
                             //TextureFill Fill = shape.shapeParser.shapes[0].Fill as TextureFill;
                             if (shape.shapeParser.Paint != null)
                             {

                                 var pattern = (shape.shapeParser.Paint as VGPatternPaint);
                                 if (pattern != null)
                                 {
                                     texture = pattern.Pattern.Texture;
                                 }
                                 else
                                 {
                                     VGRadialPaint RadialPaint = (shape.shapeParser.Paint as VGRadialPaint);
                                     if (RadialPaint != null)
                                     {
                                         texture = RadialPaint.Gradient;

                                     }
                                     else
                                     {
                                         VGLinearPaint LinearPaint = (shape.shapeParser.Paint as VGLinearPaint);
                                         if (LinearPaint != null)
                                         {
                                             texture = LinearPaint.Gradient;

                                         }
                                         else
                                         {
                                             //VGPaint Paint = (shape.shapeParser.Paint as VGPaint);
                                             //if (Paint != null)
                                             //{
                                             //    texture = Paint.Gradient;

                                             //}
                                             //else
                                             //{

                                             //}
                                         }
                                     }
                                 }


                                 DrawGL.ins.SetTextures(texture);
                             }

                         }
                         else if (shape.shapeParser.shapes[0].Fill is GradientFill)
                         {
                             //GradientFill gradientFill = shape.shapeParser.shapes[0].Fill as GradientFill;
                             if (shape.shapeParser.Paint != null)
                             {
                                 texture = (shape.shapeParser.Paint as VGGradientPaint).Gradient;
                                 //_effect.GraphicsDevice.Textures[1] = pattern.Texture;
                                 //_effect.GraphicsDevice.SamplerStates[1] = pattern.GetSamplerState();
                                 //DrawGL.ins.SetColor(UnityEngine.Color.white);
                                 //DrawGL.ins.SetTextures(tex);
                             }

                         }
                         else if (shape.shapeParser.shapes[0].Fill is PatternFill)
                         {
                             //PatternFill Fill = shape.shapeParser.shapes[0].Fill as PatternFill;
                             if (shape.shapeParser.Paint != null)
                             {

                                 var pattern = (shape.shapeParser.Paint as VGPatternPaint);
                                 if (pattern != null)
                                 {
                                     texture = pattern.Pattern.Texture;
                                 }
                                 else
                                 {
                                     VGRadialPaint RadialPaint = (shape.shapeParser.Paint as VGRadialPaint);
                                     if (RadialPaint != null)
                                     {
                                         texture = RadialPaint.Gradient;

                                     }
                                     else
                                     {
                                         VGLinearPaint LinearPaint = (shape.shapeParser.Paint as VGLinearPaint);
                                         if (LinearPaint != null)
                                         {
                                             texture = LinearPaint.Gradient;

                                         }
                                         else
                                         {
                                             //VGPaint Paint = (shape.shapeParser.Paint as VGPaint);
                                             //if (Paint != null)
                                             //{
                                             //    texture = Paint.Gradient;

                                             //}
                                             //else
                                             //{

                                             //}
                                         }
                                     }
                                 }


                                 DrawGL.ins.SetTextures(texture);
                             }
                         }
                     }*/
                    //else if (shape._strokes.Length > 0)
                    //    target.DrawPath(shape._strokes[0].Path, VGPaintMode.Stroke);
                    //if (isSolidFill)
                    //{
                    //    if (shape.shapeParser.oneMesh == null)
                    //    {
                    //        shape.shapeParser.oneMesh = DrawGL.ins.SetMesh(shape.shapeParser.shapes, texture);
                    //    }
                    //    DrawGL.ins.SetDrawShape(shape.shapeParser.oneMesh, state.PathToSurface.Matrix, state.Projection.Matrix, texture);
                    //}
                }
                else
                {
                    for (i = 0, e = shape._fills.Length; i < e; i++)
                    {
                        state.PathToFillPaint.Push(shape._fills[i].FillMatrix);
                        state.SetFillPaint(shape._fills[i].Paint);
                        target.DrawPath(shape._fills[i].Path, VGPaintMode.Fill);
                        state.PathToFillPaint.Pop();
                        VGPaint paint = shape._fills[i].Paint;
                        shape._fills[i].Path.Fill.CreaceMesh(paint, matrix, state.Projection.Matrix, state.PathToFillPaint.Matrix);
                    }

                    for (i = 0, e = shape._strokes.Length; i < e; i++)
                    {
                        if (shape._strokes[i].ScaleX && shape._strokes[i].ScaleY)
                            factor = scaleAvg;
                        else if (shape._strokes[i].ScaleX)
                            factor = scaleX;
                        else if (shape._strokes[i].ScaleY)
                            factor = scaleY;
                        else
                            factor = 1f;

                        state.NonScalingStroke = true;
                        state.PathToStrokePaint.Push(shape._strokes[i].FillMatrix);
                        state.SetStrokePaint(shape._strokes[i].Paint);
                        state.StrokeThickness = Math.Max(2f, shape._strokes[i].Thickness * factor);
                        target.DrawPath(shape._strokes[i].Path, VGPaintMode.Stroke);
                        state.PathToStrokePaint.Pop();
                    }
                }
                  
            }
        }
        public void OnNextFrame() { }
        public void SetParent(StageObject parent) { }
        public Movie.IDrawable MakeInstance(Movie.DisplayObject container, RootMovieClip root) { return this; }

        public void Dispose()
        {
            if (_subShapes != null)
            {
                foreach (var shape in _subShapes)
                    foreach (var paint in shape._paints)
                        paint.Dispose();
                _subShapes = null;
            }
        }

        private VGPaint MakeFill(FillStyle f, IVGDevice device, FlashDocument document)
        {
            VGPaint paint = null;

            switch (f.FillType)
            {
                case FillStyle.FillStyleType.Solid:
                    paint = device.CreateColorPaint(f.Color);
                    break;
                case FillStyle.FillStyleType.Linear:
                    paint = device.CreateLinearPaint(f.Gradient.AsEnumerable(), f.Gradient.InterpolationMode == GradientInfo.Interpolation.Linear);
                    break;
                case FillStyle.FillStyleType.Radial:
                case FillStyle.FillStyleType.Focal:
                    paint = device.CreateRadialPaint(f.Gradient.AsEnumerable(), f.Gradient.InterpolationMode == GradientInfo.Interpolation.Linear);
                    break;
                case FillStyle.FillStyleType.RepeatingBitmap:
                case FillStyle.FillStyleType.RepeatingNonsmoothedBitmap:
                case FillStyle.FillStyleType.ClippedBitmap:
                case FillStyle.FillStyleType.ClippedNonsmoothedBitmap:
                    paint = device.CreatePatternPaint((document[f.BitmapID] as Bitmap).Image);
                    break;
            }

            if (paint is VGColorPaint)
                return paint;

            if (paint is VGGradientPaint)
            {
                if (f.Gradient is FocalGradientInfo)
                    (paint as VGRadialPaint).FocalPoint = (float)(f.Gradient as FocalGradientInfo).FocalPoint;

                var p = paint as VGGradientPaint;
                p.GradientFilter = TextureFilter.Linear;
                switch (f.Gradient.PadMode)
                {
                    case GradientInfo.Padding.Pad:
                        p.AddressMode = TextureAddressMode.Clamp;
                        break;
                    case GradientInfo.Padding.Reflect:
                        p.AddressMode = TextureAddressMode.Mirror;
                        break;
                    case GradientInfo.Padding.Repeat:
                        p.AddressMode = TextureAddressMode.Wrap;
                        break;
                }
            }

            if (paint is VGPatternPaint)
            {
                var i = (paint as VGPatternPaint).Pattern;
                switch (f.FillType)
                {
                    case FillStyle.FillStyleType.RepeatingBitmap:
                        i.ImageFilter = TextureFilter.Linear;
                        i.AddressMode = TextureAddressMode.Wrap;
                        break;
                    case FillStyle.FillStyleType.RepeatingNonsmoothedBitmap:
                        i.ImageFilter = TextureFilter.Point;
                        i.AddressMode = TextureAddressMode.Wrap;
                        break;
                    case FillStyle.FillStyleType.ClippedBitmap:
                        i.ImageFilter = TextureFilter.Linear;
                        i.AddressMode = TextureAddressMode.Clamp;
                        break;
                    case FillStyle.FillStyleType.ClippedNonsmoothedBitmap:
                        i.ImageFilter = TextureFilter.Point;
                        i.AddressMode = TextureAddressMode.Clamp;
                        break;
                }
            }

            return paint;
        }

        private class SubShape
        {
            public VGPaint[] _paints;
            public SubShapeFill[] _fills;
            public SubShapeStroke[] _strokes;
            public Unity.Flash.ShapeParser shapeParser;

        }

        private struct SubShapeFill
        {
            public VGPreparedPath Path;
            public VGPaint Paint;
            public VGMatrix FillMatrix;
        }
        private struct SubShapeStroke
        {
            public VGPreparedPath Path;
            public VGPaint Paint;
            public VGMatrix FillMatrix;
            public float Thickness;
            public bool ScaleX;
            public bool ScaleY;
        }
    }
}
