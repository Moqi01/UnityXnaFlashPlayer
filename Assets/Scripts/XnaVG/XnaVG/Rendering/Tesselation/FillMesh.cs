using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaVG.Rendering.Tesselation
{
    internal sealed class FillMesh : IDisposable
    {
        private VertexBuffer _vertices = null;
        private int _tris;

        public int TriangleCount { get { return _tris; } }
        public bool HasVertices { get { return _vertices != null; } }

        internal FillMesh(GraphicsDevice device, VGPath path)
        {
            StencilVertex[] vertices;
            if (!Make(path, out vertices, out _tris))
                return;

            _vertices = new VertexBuffer(device, StencilVertex.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            _vertices.SetData(vertices);
        }
        internal void Activate()
        {
            if (_vertices == null)
                return;

            _vertices.GraphicsDevice.SetVertexBuffer(_vertices);
        }
        internal void Render()
        {
            if (_vertices == null)
                return;

            _vertices.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _tris);
        }
        public void Dispose()
        {
            if (_vertices == null)
                return;

            _vertices.Dispose();
            _vertices = null;
        }

        #region Tesselation

        internal static bool Make(VGPath path, out StencilVertex[] vertices, out int tris)
        {
            vertices = null;
            // Count triangles
            tris =
                path.GetCount(VGPath.SegmentType.CurveTo) * 2 +
                path.GetCount(VGPath.SegmentType.LineTo) +
                path.GetCount(VGPath.SegmentType._Tesselated);

            if (tris == 0)
                return false;

            // Tesselate
            vertices = new StencilVertex[tris * 3];
            StencilVertex start = new StencilVertex();
            Vector2 last = new Vector2();
            int index = 0;

            start.Set(0, 0, Constants.Coef_Solid);
            for (var s = path.FirstSegment; s != null; s = s.Next)
            {
                switch (s.Value.Type)
                {
                    case VGPath.SegmentType.CurveTo:
                        {
                            vertices[index++].Set(last.X, last.Y, Constants.Coef_Solid);
                            vertices[index++].Set(s.Value.Target.X, s.Value.Target.Y, Constants.Coef_Solid);
                            vertices[index++] = start;
                            
                            vertices[index++].Set(last.X, last.Y, Constants.Coef_BezierStart);
                            vertices[index++].Set(s.Value.Controls[0].X, s.Value.Controls[0].Y, Constants.Coef_BezierControl);
                            vertices[index++].Set(s.Value.Target.X, s.Value.Target.Y, Constants.Coef_BezierEnd);
                        }
                        break;
                    case VGPath.SegmentType.LineTo:
                    case VGPath.SegmentType._Tesselated:
                        {
                            vertices[index++].Set(last.X, last.Y, Constants.Coef_Solid);
                            vertices[index++].Set(s.Value.Target.X, s.Value.Target.Y, Constants.Coef_Solid);
                            vertices[index++] = start;
                        }
                        break;
                    case VGPath.SegmentType.MoveTo:
                        {
                            start.Set(s.Value.Target.X, s.Value.Target.Y, Constants.Coef_Solid);
                        }
                        break;
                    default:
                        continue;
                }

                last = s.Value.Target;
            }

            return true;
        }

        #endregion
    }
}
