using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaVG.Rendering.Text
{
    internal sealed class BufferGlyph : Glyph
    {
        public VertexBuffer Buffer { get; private set; }
        public int VertexOffset { get; private set; }
        public int TriangleCount { get; private set; }
        public override bool IsEmpty { get { return TriangleCount == 0; } }

        public BufferGlyph(VertexBuffer buffer, int offset, int triangles, Vector2 escape)
            : base(escape)
        {
            Buffer = buffer;
            VertexOffset = offset;
            TriangleCount = triangles;
        }

        public override void Render()
        {
            Buffer.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, VertexOffset, TriangleCount);
        }

        public override void BeforeRender() { }

        public override void Dispose()
        { }
    }
}
