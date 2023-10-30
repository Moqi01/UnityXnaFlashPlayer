using System;
using System.Collections.Generic;
//using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UnityEngine;
using XnaVG.Paints;
using PrimitiveType = Microsoft.Xna.Framework.Graphics.PrimitiveType;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace XnaVG.Rendering.Tesselation
{
    internal sealed class FillMesh : IDisposable
    {
        private VertexBuffer _vertices = null;
        private int _tris;
        public Mesh mesh;
        public List<Vector3> vertices = new List<Vector3>();
        public List<Color> colors = new List<Color>();
        public List<int> triangles = new List<int>();
        public List<UnityEngine.Vector2> uvs = new List<UnityEngine.Vector2>();
        Color Color;
        public int TriangleCount { get { return _tris; } }
        public bool HasVertices { get { return _vertices != null; } }

        internal FillMesh(GraphicsDevice device, VGPath path)
        {
            StencilVertex[] vertices;
            if (!Make(path, out vertices, out _tris))
            {
                return;
            }

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

        public void CreaceMesh(VGPaint paint,VGMatrix value, VGMatrix projection, VGMatrix paintTransformation)
        {

            if (paint is VGColorPaint)
            {
                VGColorPaint colorPaint = paint as VGColorPaint;
                VGColor color = colorPaint.Color;
                Color = new Color(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
            }
            Transformation = value;
            Projection = projection;
            PaintTransformation = paintTransformation;
            //SetMatrices(value, projection);
            //if (DrawGL.ins.ContainsMesh(mesh))
            //{
            //    if (meshs.ContainsKey(Matrix))
            //    {
            //        DrawGL.ins.SetMesh(meshs[Matrix]);
            //    }
            //    else
            //    {
            //        SetMesh();
            //        meshs.Add(Matrix, mesh);
            //        DrawGL.ins.SetMesh(mesh);
            //    }
            //}
            //else
            //{
            //    if (mesh == null)
            //    {
            //        SetMesh();
            //        meshs.Add(Matrix, mesh);

            //    }
            //    DrawGL.ins.SetMesh(mesh);
            //}

            if (meshs.ContainsKey(Transformation))
            {
                DrawGL.ins.SetMesh(meshs[Transformation]);
            }
            else
            {
                SetMesh();
                meshs.Add(Transformation, mesh);
                DrawGL.ins.SetMesh(mesh);
            }

           

        }

        void SetMesh()
        {
            mesh = new Mesh();
            vertices.Clear();
            colors.Clear();
            triangles.Clear();
            uvs.Clear();
            var t = 0f;
            float tStep = 1f / _vertices.vertices.Length;
            for (int i = 0; i < _vertices.vertices.Length; i++)
            {

                Vector2 Position = _vertices.vertices[i].Position;
                Vector2 uv = new Vector2();
                Microsoft.Xna.Framework.Vector3 p = new Microsoft.Xna.Framework.Vector3(Position.X, Position.Y, 1);
                Vector3 p0 = new Vector3(p.X, p.Y, 1);
                Vector3 pc = new Vector3(_vertices.vertices[i].Control.X, _vertices.vertices[i].Control.Y, 1);
                Vector3 NextP = Vector3.zero;
                Vector3 NextC = Vector3.zero;
                Vector3 NextB = Vector3.zero;
                Vector3 rus = new Vector3(p.X, p.Y);

                if (i + 1 < _vertices.vertices.Length)
                {
                    NextC = new Vector3(_vertices.vertices[i + 1].Control.X, _vertices.vertices[i + 1].Control.Y, 1);
                    if (pc.x == 0.5f)
                    {
                        NextP = new Vector3(_vertices.vertices[i + 1].Position.X, _vertices.vertices[i + 1].Position.Y, 1);
                        NextB = new Vector3(_vertices.vertices[i - 1].Position.X, _vertices.vertices[i - 1].Position.Y, 1);
                        //rus = CalculateLineBezierPoint(1, NextB, p0);
                        rus = CalculateCubicBezierPoint(t, NextB, p0, NextP);
                        //rus = CalculateThreePowerBezierPoint(1, p0,pc, NextC, NextP);
                        t += tStep;
                        p = new Microsoft.Xna.Framework.Vector3(rus.x, rus.y, rus.z);
                    }

                }

                // rus = CalculateLineBezierPoint(0.9f, p0, pc);
                //p = new Microsoft.Xna.Framework.Vector3(rus.x, rus.y, rus.z);
                //uv = PaintTransformation * Position;

                Microsoft.Xna.Framework.Vector3 p2 = Transformation * p;
                Microsoft.Xna.Framework.Vector3 p3 = Projection * p2;
                Vector3 v = new Vector3(p3.X, p3.Y, p3.Z);

                vertices.Add(v);
                colors.Add(Color);
                triangles.Add(i);
                //uvs.Add(new UnityEngine.Vector2((uv.X), (uv.Y)));
            }
            mesh.SetVertices ( vertices);
            mesh.SetColors (colors);
            mesh.SetTriangles( triangles,0);
            //mesh.uv = uvs.ToArray();
        }

        public VGMatrix Transformation;
        public VGMatrix Projection;
        public VGMatrix PaintTransformation;

        private static Vector3 CalculateLineBezierPoint(float t, Vector3 p0, Vector3 p1)
        {
            float u = 1 - t;

            Vector3 p = u * p0;
            p += t * p1;


            return p;
        }

        private static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 p = uu * p0;
            p += 2 * u * t * p1;
            p += tt * p2;

            return p;
        }

        private static Vector3 CalculateThreePowerBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float ttt = tt * t;
            float uuu = uu * u;

            Vector3 p = uuu * p0;
            p += 3 * t * uu * p1;
            p += 3 * tt * u * p2;
            p += ttt * p3;

            return p;
        }

        public Dictionary<VGMatrix, Mesh> meshs = new Dictionary<VGMatrix, Mesh>();
        public Matrix4x4 Matrix;
        internal Matrix4x4 SetMatrices(VGMatrix value, VGMatrix rot)
        {
            UnityEngine.Matrix4x4 v = UnityEngine.Matrix4x4.identity;
            v.m00 = value.M11;
            v.m10 = value.M12;
            //v.m13 = value.M13;

            v.m01 = value.M21;
            v.m11 = value.M22;
            //v.m23 = value.M23;

            v.m03 = value.M31;
            v.m13 = value.M32;
            //v.m33 = value.M33;

            Matrix = v;
            Scale = rot.ScaleY * 2000;
            return v;
        }
        public float Scale;
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
