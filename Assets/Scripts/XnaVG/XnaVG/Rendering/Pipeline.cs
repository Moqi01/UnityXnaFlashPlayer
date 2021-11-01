using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaVG.Paints;
using XnaVG.Rendering.States;
using XnaVG.Rendering.Text.Strings;

namespace XnaVG.Rendering
{
    internal sealed class Pipeline
    {
        public VGDevice Device { get; private set; }
        public VGSurface Surface { get; private set; }
        public VGState State { get; private set; }
        private GraphicsDevice _device;
        private Vector2[] _rectangle = new Vector2[4];             

        internal Pipeline(VGDevice device, VGSurface surface, VGState state)
        {
            Device = device;
            Surface = surface;
            State = state;

            _device = device.GraphicsDevice;
            _device.SetRenderTarget(surface.Target);
            _device.RasterizerState = State.RasterizerState;
        }               
        internal void BeginCover(bool glyph)
        {
            var cxForm = State.ColorTransformationEnabled ? State.ColorTransformation.CxForm : VGCxForm.Identity;
            var effect = Device.EffectManager.Cover;

            _device.DepthStencilState = State.Stencils.Cover;
            _device.BlendState = Device.BlendStates.GetBlendState(State.BlendMode, State.ColorChannels);
            effect.SetMask(State.MaskingEnabled ? State.Mask : null, State.MaskChannels);
            effect.SetParameters(State.Projection.Matrix, glyph ? State.GlyphToSurface.Matrix : State.PathToSurface.Matrix, cxForm);
        }

        #region Solid

        internal void RenderSolidRectangle(ref Vector4 extents)
        {
            // Setup stenciling
            _device.BlendState = Device.BlendStates.NoColor;
            _device.DepthStencilState = State.Stencils.Set;
            Device.EffectManager.StencilSolid.Apply(State.Projection.Matrix, State.PathToSurface.Matrix);

            // Render mesh
            _device.MultiSampleMask = -1;
            RenderRectangle(ref extents);

            // Do not cover when stenciling
            if (State.WriteStencilMask != VGStencilMasks.None)
                return;

            BeginCover(false);

            _device.DepthStencilState = State.Stencils.Cover;
            Device.EffectManager.Cover.SetPaint(State.StrokePaint, State.PathToStrokePaint.Matrix);
            Device.EffectManager.Cover.Apply();
            RenderRectangle(ref extents);
        }
        internal void RenderSolid(VertexBuffer vertices, PrimitiveType primitiveType, int startVertex, int primitiveCount)
        {            
            // Setup stenciling
            _device.BlendState = Device.BlendStates.NoColor;
            _device.DepthStencilState = State.Stencils.Set;
            Device.EffectManager.StencilSolid.Apply(State.Projection.Matrix, State.PathToSurface.Matrix);

            // Render mesh
            _device.MultiSampleMask = -1;
            _device.SetVertexBuffer(vertices);
            _device.DrawPrimitives(primitiveType, startVertex, primitiveCount);

            // Do not cover when stenciling
            if (State.WriteStencilMask != VGStencilMasks.None)
                return;

            BeginCover(false);

            _device.DepthStencilState = State.Stencils.Cover;
            Device.EffectManager.Cover.SetPaint(State.StrokePaint, State.PathToStrokePaint.Matrix);
            Device.EffectManager.Cover.Apply();
            _device.DrawPrimitives(primitiveType, startVertex, primitiveCount);
        }
        internal void RenderIndexedSolid(VertexBuffer vertices, IndexBuffer indices, PrimitiveType primitiveType, int startVertex, int primitiveCount)
        {
            // Setup stenciling
            _device.BlendState = Device.BlendStates.NoColor;
            _device.DepthStencilState = State.Stencils.Set;
            Device.EffectManager.StencilSolid.Apply(State.Projection.Matrix, State.PathToSurface.Matrix);

            // Render mesh
            _device.MultiSampleMask = -1;
            _device.Indices = indices;
            _device.SetVertexBuffer(vertices);
            _device.DrawIndexedPrimitives(primitiveType, 0, 0, vertices.VertexCount, startVertex, primitiveCount);

            // Do not cover when stenciling
            if (State.WriteStencilMask != VGStencilMasks.None)
                return;

            BeginCover(false);

            _device.DepthStencilState = State.Stencils.Cover;
            Device.EffectManager.Cover.SetPaint(State.StrokePaint, State.PathToStrokePaint.Matrix);
            Device.EffectManager.Cover.Apply();
            _device.DrawIndexedPrimitives(primitiveType, 0, 0, vertices.VertexCount, startVertex, primitiveCount);
        }

        #endregion

        #region Strokes

        internal void StencilStroke(IRenderable path)
        {
            var effect = Device.EffectManager.StencilStroke;

            // Setup stenciling
            _device.BlendState = Device.BlendStates.NoColor;
            _device.DepthStencilState = State.Stencils.Set;

            // Render solid mesh
            if (path.HasSolidStroke)
            {
                effect.SetSolid(State.NonScalingStroke, State.Projection.Matrix, State.PathToSurface.Matrix, State.StrokeExpand);
                effect.Apply();
                _device.MultiSampleMask = -1;
                path.BeforeRenderSolidStroke();                    
                path.RenderSolidStroke();
            }

            // Render radial mesh
            if (path.HasRadialStroke)
            {
                effect.SetRadial(State.NonScalingStroke, State.Projection.Matrix, State.PathToSurface.Matrix, State.StrokeExpand);
                path.BeforeRenderRadialStroke();
                if (State.Antialiasing == VGAntialiasing.None)
                {
                    effect.Apply();
                    path.RenderRadialStroke();
                }
                else
                    for (int samples = (int)State.Antialiasing - 1, MSAAMask = _msaaMasks[State.Antialiasing]; samples >= 0; samples--, MSAAMask <<= 1)
                    {
                        _device.MultiSampleMask = MSAAMask;
                        effect.SetOffset(ref Surface.MSAAPattern[samples]);
                        effect.Apply();
                        path.RenderRadialStroke();
                    }
            }
        }
        internal void CoverStroke(IRenderable path)
        {
            var effect = Device.EffectManager.Cover;
            var thickness = State.StrokeExpand;

            _device.DepthStencilState = State.Stencils.Cover;
            effect.SetPaint(State.StrokePaint, State.PathToStrokePaint.Matrix);
            effect.Apply();

            if (State.NonScalingStroke)
            {
                Vector2 newSize = ~State.PathToSurface.Matrix * new Vector2(thickness, thickness);
                thickness = Math.Max(Math.Abs(newSize.X), Math.Abs(newSize.Y));
            }
            
            Vector4 extents = path.Extents;
            extents.X -= thickness;
            extents.Y -= thickness;
            extents.Z += thickness;
            extents.W += thickness;
            RenderRectangle(ref extents);
        }

        #endregion

        #region Fills

        internal void StencilFill(IRenderable path)
        {
            var effect = Device.EffectManager.StencilFill;

            // Setup stenciling
            _device.BlendState = Device.BlendStates.NoColor;
            _device.DepthStencilState = State.FillRuleState;

            effect.SetParameters(State.Projection.Matrix, State.PathToSurface.Matrix);

            // Render mesh
            if (path.HasFill)
            {
                path.BeforeRenderFill();
                if (State.Antialiasing == VGAntialiasing.None)
                {
                    effect.Apply();
                    path.RenderFill();
                }
                else
                    for (int samples = (int)State.Antialiasing - 1, MSAAMask = _msaaMasks[State.Antialiasing]; samples >= 0; samples--, MSAAMask <<= 1)
                    {
                        _device.MultiSampleMask = MSAAMask;
                        effect.SetScreenOffset(ref Surface.MSAAPattern[samples]);
                        effect.Apply();
                        path.RenderFill();
                    }
            }
        }
        internal void CoverFill(IRenderable path)
        {
            var effect = Device.EffectManager.Cover;
                    
            effect.SetPaint(State.FillPaint, State.PathToFillPaint.Matrix);
            effect.Apply();

            Vector4 extents = path.Extents;
            RenderRectangle(ref extents);
        }

        #endregion

        #region Text

        internal void BeginCPUTextStenciling(VGFillRule rule)
        {
            // Setup stenciling
            _device.BlendState = Device.BlendStates.NoColor;
            _device.DepthStencilState = (rule == VGFillRule.NonZero) ? State.Stencils.NonZero : State.Stencils.EvenOdd;

            Device.EffectManager.StencilFill.SetParameters(State.Projection.Matrix, State.GlyphToSurface.Matrix);
        }
        internal void StencilGlyph(IRenderable path, ref Vector2 offset)
        {
            var effect = Device.EffectManager.StencilFill;
            effect.SetPathOffset(ref offset);

            // Render mesh
            if (path.HasFill)
            {
                path.BeforeRenderFill();
                if (State.Antialiasing == VGAntialiasing.None)
                {
                    effect.Apply();
                    path.RenderFill();
                }
                else
                    for (int samples = (int)State.Antialiasing - 1, MSAAMask = _msaaMasks[State.Antialiasing]; samples >= 0; samples--, MSAAMask <<= 1)
                    {
                        _device.MultiSampleMask = MSAAMask;
                        effect.SetScreenOffset(ref Surface.MSAAPattern[samples]);
                        effect.Apply();
                        path.RenderFill();
                    }
            }
        }
        internal void StencilInstancedGlyphs(VGFillRule rule, GPUBufferedString text)
        {
            var effect = Device.EffectManager.StencilText;

            // Setup stenciling
            _device.BlendState = Device.BlendStates.NoColor;
            _device.DepthStencilState = (rule == VGFillRule.NonZero) ? State.Stencils.NonZero : State.Stencils.EvenOdd;

            // Render mesh
            if (State.Antialiasing == VGAntialiasing.None)
            {
                effect.Apply(State.Projection.Matrix, State.GlyphToSurface.Matrix, Vector2.Zero, text.Font.Vertices, text.Glyphs);
                text.Render();
            }
            else
                for (int samples = (int)State.Antialiasing - 1, MSAAMask = _msaaMasks[State.Antialiasing]; samples >= 0; samples--, MSAAMask <<= 1)
                {
                    _device.MultiSampleMask = MSAAMask;
                    effect.Apply(State.Projection.Matrix, State.GlyphToSurface.Matrix, Surface.MSAAPattern[samples], text.Font.Vertices, text.Glyphs);
                    text.Render();
                }
        }
        internal void CoverGlyphs(ref Vector4 extents, ref Vector2 offset)
        {
            var effect = Device.EffectManager.Cover;
            
            effect.SetOffset(ref offset);
            effect.SetPaint(State.GlyphPaint, State.PathToTextPaint.Matrix);
            effect.Apply();

            RenderRectangle(ref extents);
        }

        #endregion
        
        #region Utils

        internal void RenderPath(IRenderable path, VGPaintMode mode)
        {
            if ((mode & VGPaintMode.Fill) != 0)
            {
                StencilFill(path);

                if (State.WriteStencilMask == VGStencilMasks.None)
                {
                    BeginCover(false);
                    CoverFill(path);
                }
            }

            if ((mode & VGPaintMode.Stroke) != 0)
            {
                StencilStroke(path);

                if (State.WriteStencilMask == VGStencilMasks.None)
                {
                    BeginCover(false);
                    CoverStroke(path);
                }
            }
        }
        internal void RenderImage(VGImage image, Rectangle source)
        {
            if (State.WriteStencilMask != VGStencilMasks.None)
            {
                Device.EffectManager.StencilSolid.Apply(State.Projection.Matrix, State.ImageToSurface.Matrix);

                _device.BlendState = Device.BlendStates.NoColor;
                _device.DepthStencilState = State.Stencils.Set;
                                                
                Vector4 extents = new Vector4(0, 0, source.Width, source.Height);
                RenderRectangle(ref extents);
            }
            else
            {
                var cxForm = State.ColorTransformationEnabled ? State.ColorTransformation.CxForm : VGCxForm.Identity;
                var effect = Device.EffectManager.Cover;

                _device.BlendState = Device.BlendStates.GetBlendState(State.BlendMode, State.ColorChannels);
                _device.DepthStencilState = DepthStencilState.None;

                effect.SetParameters(State.Projection.Matrix, State.ImageToSurface.Matrix, cxForm);
                effect.SetMask(State.MaskingEnabled ? State.Mask : null, State.MaskChannels);
                effect.SetImagePaint(image, VGMatrix.PaintToRectangle(-source.X, -source.Y, image.Texture.Width, image.Texture.Height));
                effect.Apply();

                Vector4 extents = new Vector4(0, 0, source.Width, source.Height);
                RenderRectangle(ref extents);
            }
        }
        internal void ClearStencilMasks(VGStencilMasks masks)
        {
            // Setup stenciling
            _device.BlendState = Device.BlendStates.NoColor;
            _device.DepthStencilState = StencilStates.ClearMasks(masks);
            Device.EffectManager.StencilSolid.Apply(VGMatrix.Identity, VGMatrix.Identity);

            // Render mesh
            _device.MultiSampleMask = -1;
            Vector4 extents = Constants.ScreenExtents * 1.1f;
            RenderRectangle(ref extents);
        }
        private void RenderRectangle(ref Vector4 extents)
        {
            _rectangle[0].X = extents.X; _rectangle[0].Y = extents.Y;
            _rectangle[1].X = extents.Z; _rectangle[1].Y = extents.Y;
            _rectangle[2].X = extents.X; _rectangle[2].Y = extents.W;
            _rectangle[3].X = extents.Z; _rectangle[3].Y = extents.W;
            _device.DrawUserPrimitives(PrimitiveType.TriangleStrip, _rectangle, 0, 2, _solidDecl);
        }

        #endregion

        #region Static Variables

        internal readonly static VertexDeclaration _solidDecl = new VertexDeclaration(new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0));
        private readonly static Dictionary<VGAntialiasing, int> _msaaMasks = new Dictionary<VGAntialiasing, int> 
        { 
            { VGAntialiasing.Best, 0x00010001 },
            { VGAntialiasing.Better, 0x01010101 },
            { VGAntialiasing.Faster, 0x11111111 } 
        };

        #endregion

    }
}
