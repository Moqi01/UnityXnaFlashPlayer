using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaVG
{
    public interface IVGRenderContext : IDisposable
    {
        IVGDevice Device { get; }
        VGSurface Surface { get; }
        VGState State { get; }

        void ClearStencilMask(VGStencilMasks mask);
        void DrawPath(VGPath path, VGPaintMode mode);
        void DrawPath(VGPreparedPath path, VGPaintMode mode);
        void DrawImage(VGImage image, Rectangle? sourceRectangle);
        void DrawCharacter(char character, Vector2 offset);
        void DrawText(string text);
        void DrawText(VGString text);
        void DrawRectangle(Vector4 extents);
        void DrawRectangle(Rectangle rectangle);
        void DrawSolid(Vector2[] vertices, PrimitiveType primitiveType, int startVertex, int primitiveCount);
        void DrawSolid(VertexBuffer vertices, PrimitiveType primitiveType, int startVertex, int primitiveCount);
        void DrawIndexedSolid(VertexBuffer vertices, IndexBuffer indices, PrimitiveType primitiveType, int startVertex, int primitiveCount);        
    }

    public interface IVGRenderContext<T> : IVGRenderContext
    {
        T UserState { get; }
    }

}
