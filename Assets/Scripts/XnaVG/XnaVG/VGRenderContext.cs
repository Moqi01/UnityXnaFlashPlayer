using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaVG.Rendering;
using XnaVG.Rendering.Text;

namespace XnaVG
{
    internal class VGRenderContext<T> : IVGRenderContext<T>
    {
        private Pipeline _pipeline;
        private DeviceState _states;
        private Action _finished;
        private bool _stateWasActive;        

        public IVGDevice Device { get { return _pipeline.Device; } }
        public VGSurface Surface { get { return _pipeline.Surface; } }
        public VGState State { get { return _pipeline.State; } }
        public T UserState { get; set; }
        
        internal VGRenderContext(VGDevice device, VGSurface surface, VGState state, T userState, bool restoreXNAStates, Action finished)
        {
            UserState = userState;
            _states = restoreXNAStates ? new DeviceState(device.GraphicsDevice) : null;
            _pipeline = new Pipeline(device, surface, state);
            _finished = finished;
            _stateWasActive = state.IsActive;
            state.IsActive = true;
        }

        public void ClearStencilMask(VGStencilMasks mask)
        {
            if (mask == VGStencilMasks.None)
                return;

            _pipeline.ClearStencilMasks(mask);
        }
        public void DrawPath(VGPath path, VGPaintMode mode)
        {
            using (var pp = Device.PreparePath(path, mode, State))
                DrawPath(pp, mode);
        }
        public void DrawPath(VGPreparedPath path, VGPaintMode mode)
        {
            if (!path.HasFill) mode = ((VGPaintMode)(mode & ~VGPaintMode.Fill));
            if (!path.HasStroke) mode = ((VGPaintMode)(mode & ~VGPaintMode.Stroke));
            if (mode == VGPaintMode.None) return;

            _pipeline.RenderPath(path, mode);
        }
        public void DrawImage(VGImage image, Rectangle? sourceRectangle)
        {
            if (image == null)
                return;

            sourceRectangle = sourceRectangle ?? image.Texture.Bounds;
            _pipeline.RenderImage(image, sourceRectangle.Value);
        }
        public void DrawCharacter(char character, Vector2 offset)
        {
            var font = State.Font;
            if (font == null) return;

            var glyph = font[character];
            if (glyph == null) return;

            font.SetBuffer();

            Vector4 extents = font.MaxExtents;
            _pipeline.BeginCPUTextStenciling(font.FillRule);
            _pipeline.StencilGlyph(glyph, ref offset);

            if (State.WriteStencilMask == VGStencilMasks.None)
            {
                _pipeline.BeginCover(true);
                _pipeline.CoverGlyphs(ref extents, ref offset);
            }
        }
        public void DrawText(string text)
        {
            var font = State.Font;
            if (font == null || string.IsNullOrEmpty(text)) return;
            if (font.Vertices != null)
                throw new InvalidOperationException("Rendering text with GPU instanced font can be only done by using VGString!");

            var gpi = new VGString.GlyphPositionInfo { Offset = Vector2.Zero, Size = Vector2.Zero, TabSize = State.TabSize * font.EmSquareSize };
            Vector4 extents = new Vector4();

            font.SetBuffer();

            _pipeline.BeginCPUTextStenciling(font.FillRule);
            foreach (var g in VGString.ProcessString<Glyph>(font, text, gpi))
                _pipeline.StencilGlyph(g, ref gpi.Offset);

            if (State.WriteStencilMask == VGStencilMasks.None)
            {
                extents.X = font.MaxExtents.X;
                extents.Y = font.MaxExtents.Y;
                extents.Z = font.MaxExtents.Z + gpi.Size.X;
                extents.W = font.MaxExtents.W + gpi.Size.Y;
                gpi.Offset = Vector2.Zero;

                _pipeline.BeginCover(true);
                _pipeline.CoverGlyphs(ref extents, ref gpi.Offset);
            }
        }
        public void DrawText(VGString text)
        {
            if (text.IsEmpty)
                return;

            text.Prepare();
            text.Stencil(_pipeline);

            if (State.WriteStencilMask == VGStencilMasks.None)
            {
                _pipeline.BeginCover(true);
                text.Cover(_pipeline);
            }
        }
        public void DrawRectangle(Vector4 extents)
        {
            _pipeline.RenderSolidRectangle(ref extents);
        }
        public void DrawRectangle(Rectangle rectangle)
        {
            DrawRectangle(new Vector4(rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom));
        }
        public void DrawSolid(Vector2[] vertices, PrimitiveType primitiveType, int startVertex, int primitiveCount)
        {
            if (vertices == null || vertices.Length == 0)
                return;

            using (var buff = new DynamicVertexBuffer(Device.GraphicsDevice, Pipeline._solidDecl, vertices.Length, BufferUsage.WriteOnly))
            {
                buff.SetData(vertices);
                DrawSolid(buff, primitiveType, startVertex, primitiveCount);
            }
        }
        public void DrawSolid(VertexBuffer vertices, PrimitiveType primitiveType, int startVertex, int primitiveCount)
        {
            if (vertices == null)
                return;

            _pipeline.RenderSolid(vertices, primitiveType, startVertex, primitiveCount);
        }
        public void DrawIndexedSolid(VertexBuffer vertices, IndexBuffer indices, PrimitiveType primitiveType, int startVertex, int primitiveCount)
        {
            if (vertices == null || indices == null)
                return;

            _pipeline.RenderIndexedSolid(vertices, indices, primitiveType, startVertex, primitiveCount);
        }

        public void Dispose()
        {
            if (_states != null)
                _states.Restore();

            State.IsActive = _stateWasActive;
            _finished();
        }
    }
}
