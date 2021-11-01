using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaVG.Rendering.Tesselation
{
    internal abstract class StrokeBuilder<T> where T : struct
    {
        protected List<T> _vertices;
        protected VertexDeclaration _decl;

        internal StrokeBuilder(int initialCapacity)
        {
            _vertices = new List<T>(initialCapacity);
        }

        internal VertexBuffer Build(GraphicsDevice device, out int tris)
        {
            tris = _vertices.Count / 3;
            if (tris == 0)
                return null;

            var count = tris * 3;
            var buff = new VertexBuffer(device, _decl, count, BufferUsage.WriteOnly);
            buff.SetData(_vertices.ToArray(), 0, count);            
            return buff;
        }
    }

    internal class SolidStrokeBuilder : StrokeBuilder<StencilVertex>
    {
        internal float MiterLimit { get; private set; }

        internal SolidStrokeBuilder(int initialCapacity, float miterLimit)
            : base(initialCapacity)
        {
            MiterLimit = miterLimit;
            _decl = StencilVertex.VertexDeclaration;
        }

        internal void AddVertex(float x, float y, float dx, float dy)
        {
            var v = new StencilVertex();
            v.Set(x, y, dx, dy);
            _vertices.Add(v);
        }

        internal void AddVertex(Vector2 pt, Vector2 d)
        {
            var v = new StencilVertex();
            v.Set(pt.X, pt.Y, d.X, d.Y);
            _vertices.Add(v);
        }
    }

    internal class RadialStrokeBuilder : StrokeBuilder<StencilRadialVertex>
    {
        internal RadialStrokeBuilder(int initialCapacity)
            : base(initialCapacity)
        {
            _decl = StencilRadialVertex.VertexDeclaration;
        }

        internal void AddVertex(Vector2 pt, Vector2 dir)
        {
            var v = new StencilRadialVertex();
            v.Set(pt.X, pt.Y, dir.X, dir.Y, dir);
            _vertices.Add(v);
        }

        internal void AddVertex(Vector2 pt, Vector2 dir, Vector2 tc)
        {
            var v = new StencilRadialVertex();
            v.Set(pt.X, pt.Y, dir.X, dir.Y, tc);
            _vertices.Add(v);
        }
    }
}
