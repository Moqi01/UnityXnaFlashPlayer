using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaVG.Rendering.Tesselation
{
    using _Cap = Action<LinkedListNode<VGPath.Segment>, bool>;
    using _Join = Action<LinkedListNode<VGPath.Segment>>;

    internal sealed class StrokeMesh : IDisposable
    {
        //private const int _preallocatedFontVertexCount = 1

        // Solid and radial parts
        private VertexBuffer[] _vertices = new VertexBuffer[2];
        private int[] _tris = new int[2];
        private RadialStrokeBuilder _radial;
        private SolidStrokeBuilder _solid;

        public bool HasSolid { get { return _vertices[0] != null; } }
        public bool HasRadial { get { return _vertices[1] != null; } }
        public int SolidTriangleCount { get { return _tris[0]; } }
        public int RadialTriangleCount { get { return _tris[1]; } }

        internal StrokeMesh(GraphicsDevice device, VGPath path, VGLineCap startCap, VGLineCap endCap, VGLineJoin join, float miterLimit)
        {
            int solidCount, radialCount, i, j, moves;

            // No triangles at all, don't bother...
            if (path.IsEmpty)
                return;

            // Approximate triangle counts
            moves = path.GetCount(VGPath.SegmentType.MoveTo);
            GetCapTris(startCap, out i, out j);
            solidCount = moves * i;
            radialCount = moves * j;

            GetCapTris(endCap, out i, out j);
            solidCount += moves * i;
            radialCount += moves * j;

            GetJoinTris(join, out i, out j);
            solidCount += path.Count * i;
            radialCount += path.Count * j;

            solidCount += (path.GetCount(VGPath.SegmentType.LineTo) + path.GetCount(VGPath.SegmentType._Tesselated)) * 2;
            
            // Initialize
            _solid = new SolidStrokeBuilder(solidCount, miterLimit);
            _radial = new RadialStrokeBuilder(radialCount);

            var startCapFunc = GetCapFunction(startCap);
            var endCapFunc = GetCapFunction(endCap);
            var joinFunc = GetJoinFunction(join);            

            // Tesselate
            VGPath.Segment last = null;
            Vector2 lastNormal = Vector2.Zero;
            var lastStart = path.FirstSegment;

            for (var s = lastStart; s != null; s = s.Next)
            {
                switch (s.Value.Type)
                {
                    case VGPath.SegmentType.LineTo:
                        {
                            Vector2 n, mn;
                            VectorMath.GetLeftNormal(last.Target, s.Value.Target, out n);
                            mn = -n;

                            _solid.AddVertex(last.Target, mn);
                            _solid.AddVertex(last.Target, n);
                            _solid.AddVertex(s.Value.Target, n);
                            _solid.AddVertex(last.Target, mn);
                            _solid.AddVertex(s.Value.Target, n);
                            _solid.AddVertex(s.Value.Target, mn);
                        }
                        break;
                    case VGPath.SegmentType._Tesselated:
                        {
                            if (last.Type != VGPath.SegmentType._Tesselated)
                                VectorMath.GetLeftNormal(last.Target, s.Value.Target, out lastNormal);                           

                            Vector2 lastRtNormal = -lastNormal;
                            _solid.AddVertex(last.Target, lastRtNormal);
                            _solid.AddVertex(last.Target, lastNormal);
                            _solid.AddVertex(s.Value.Target, s.Value.Controls[0]);
                            _solid.AddVertex(last.Target, lastRtNormal);
                            _solid.AddVertex(s.Value.Target, s.Value.Controls[0]);
                            _solid.AddVertex(s.Value.Target, -s.Value.Controls[0]);

                            lastNormal = s.Value.Controls[0];
                        }
                        break;

                    case VGPath.SegmentType.MoveTo:
                        {
                            if (s.Previous != null && s.Previous.Value.Type != VGPath.SegmentType.Close && s.Previous.Value.Type != VGPath.SegmentType.MoveTo)
                            {
                                startCapFunc(lastStart, true);
                                endCapFunc(s.Previous, false);
                            }

                            lastStart = s;
                        }
                        break;
                    case VGPath.SegmentType.Close:
                        break;
                    default:
                        throw new InvalidOperationException("Trying to stroke non-flattened path!");
                }


                if (s.Value.MakeJoin && s.Next != null)
                    joinFunc(s);

                last = s.Value;
            }

            // Do the caps
            if (path.LastSegment.Value.Type != VGPath.SegmentType.Close && path.LastSegment.Value.Type != VGPath.SegmentType.MoveTo)
            {
                startCapFunc(lastStart, true);
                endCapFunc(path.LastSegment, false);
            }

            // Create buffer
            _vertices[0] = _solid.Build(device, out _tris[0]);
            _vertices[1] = _radial.Build(device, out _tris[1]);

            // Free builders
            _radial = null;
            _solid = null;
        }

        internal void ActivateSolid()
        {
            if (_vertices[0] != null)
                _vertices[0].GraphicsDevice.SetVertexBuffer(_vertices[0]);
        }

        internal void ActivateRadial()
        {
            if (_vertices[1] != null)
                _vertices[1].GraphicsDevice.SetVertexBuffer(_vertices[1]);
        }

        internal void RenderSolid()
        {
            if (_vertices[0] != null)
                _vertices[0].GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _tris[0]);
        }

        internal void RenderRadial()
        {
            if (_vertices[1] != null)
                _vertices[1].GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _tris[1]);
        }

        public void Dispose()
        {
            if (_vertices[0] != null)
            {
                _vertices[0].Dispose();
                _vertices[0] = null;
            }

            if (_vertices[1] != null)
            {
                _vertices[1].Dispose();
                _vertices[1] = null;
            }
        }

        #region Caps and Joins

        #region Round Caps and Joins

        private void MakeRoundCap(LinkedListNode<VGPath.Segment> segment, bool start)
        {
            Vector2 left, right;
            Vector2 dir;
            Vector2 pt;

            pt = segment.Value.Target;
            if (start)
            {
                VectorMath.GetTangentFrom(segment, out dir);
                dir = -dir;
            }
            else
                VectorMath.GetTangentTo(segment, out dir);

            VectorMath.GetLeftNormal(ref dir, out left);
            right = -left;

            _radial.AddVertex(pt, left, Constants.LeftMiddle);
            _radial.AddVertex(pt, right + dir, Constants.RightTop);
            _radial.AddVertex(pt, left + dir, Constants.LeftTop);

            _radial.AddVertex(pt, left, Constants.LeftMiddle);
            _radial.AddVertex(pt, right, Constants.RightMiddle);
            _radial.AddVertex(pt, right + dir, Constants.RightTop);
        }
        private void MakeRoundJoin(LinkedListNode<VGPath.Segment> node)
        {
            var pt = node.Value.Target;
            _radial.AddVertex(pt, Constants.LeftTop);
            _radial.AddVertex(pt, Constants.RightTop);
            _radial.AddVertex(pt, Constants.LeftBottom);
            _radial.AddVertex(pt, Constants.LeftBottom);
            _radial.AddVertex(pt, Constants.RightTop);
            _radial.AddVertex(pt, Constants.RightBottom);
        }

        #endregion

        #region Other Caps

        private void MakeTriangleCap(LinkedListNode<VGPath.Segment> segment, bool start)
        {
            Vector2 left, right;
            Vector2 tip;
            Vector2 pt;

            pt = segment.Value.Target;
            if (start)
            {
                VectorMath.GetTangentFrom(segment, out tip);
                tip = -tip;                
            }
            else
                VectorMath.GetTangentTo(segment, out tip);
           
            VectorMath.GetLeftNormal(ref tip, out left);
            right = -left;

            _solid.AddVertex(pt, left);
            _solid.AddVertex(pt, right);
            _solid.AddVertex(pt, tip);
        }
        private void MakeSquareCap(LinkedListNode<VGPath.Segment> segment, bool start)
        {
            Vector2 left, right;
            Vector2 dir, rd, ld;
            Vector2 pt;

            pt = segment.Value.Target;
            if (start)
            {
                VectorMath.GetTangentFrom(segment, out dir);
                dir = -dir;
            }
            else
                VectorMath.GetTangentTo(segment, out dir);

            VectorMath.GetLeftNormal(ref dir, out left);
            right = -left;

            rd = right + dir;
            ld = left + dir;            

            _solid.AddVertex(pt, left);
            _solid.AddVertex(pt, rd);
            _solid.AddVertex(pt, ld);

            _solid.AddVertex(pt, left);
            _solid.AddVertex(pt, right);
            _solid.AddVertex(pt, rd);          
        }

        #endregion

        #region Other Joins

        private void MakeBevelJoin(LinkedListNode<VGPath.Segment> segment)
        {
            Vector2 tangentA, tangentB, ptA, ptB, normalA, normalB;
            
            VectorMath.GetTangentTo(segment, out tangentA);
            VectorMath.GetTangentFrom(segment, out tangentB);

            ptA = segment.Previous.Value.Target;
            ptB = segment.Value.Target;
            
            VectorMath.GetLeftNormal(ref tangentA, out normalA);
            VectorMath.GetLeftNormal(ref tangentB, out normalB);

            _solid.AddVertex(ptB, Vector2.Zero);
            _solid.AddVertex(ptB, normalA);
            _solid.AddVertex(ptB, normalB);
        }
        private void MakeMiterJoin(LinkedListNode<VGPath.Segment> segment)
        {
            Vector2 tangentA, tangentB, ptA, ptB, normalA, normalB, intersection;

            VectorMath.GetTangentTo(segment, out tangentA);
            VectorMath.GetTangentFrom(segment, out tangentB);

            ptA = ptB = segment.Value.Target;

            VectorMath.GetLeftNormal(ref tangentA, out normalA);
            VectorMath.GetLeftNormal(ref tangentB, out normalB);

            float angle = VectorMath.SignedAngle(ref tangentA, ref tangentB);
            float length = (float)(1.0 / Math.Cos(angle * 0.5));

            if (Math.Abs(length) > _solid.MiterLimit)
            {
                MakeBevelJoin(segment);
                return;
            }

            int side = VectorMath.CrossProduct(ref tangentA, ref tangentB);
            if (side == 0)
                return;

            if (side < 0)
            {
                normalA = -normalA;
                normalB = -normalB;
            }

            ptA += normalA;
            ptB += normalB;

            if (float.IsNaN(VectorMath.LinesIntersection(ref ptA, ref ptB, ref tangentA, ref tangentB, out intersection)))
                intersection = ptA;

            ptA = segment.Value.Target;
            intersection -= ptA;

            _solid.AddVertex(ptA, Vector2.Zero);
            _solid.AddVertex(ptA, intersection);
            _solid.AddVertex(ptA, normalA);

            _solid.AddVertex(ptA, Vector2.Zero);
            _solid.AddVertex(ptA, normalB);
            _solid.AddVertex(ptA, intersection);
        }

        #endregion

        #region Helpers

        private _Cap GetCapFunction(VGLineCap cap)
        {
            switch (cap)
            {
                case VGLineCap.Butt: return (a, b) => { };
                case VGLineCap.Round: return MakeRoundCap;
                case VGLineCap.Triangle: return MakeTriangleCap;
                case VGLineCap.Square: return MakeSquareCap;
                default:
                    throw new NotImplementedException("This cap style (" + cap + ") is not implemented!");
            }
        }
        private _Join GetJoinFunction(VGLineJoin join)
        {
            switch (join)
            {
                case VGLineJoin.None: return (a) => { };
                case VGLineJoin.Round: return MakeRoundJoin;
                case VGLineJoin.Bevel: return MakeBevelJoin;
                case VGLineJoin.Miter: return MakeMiterJoin;
                default:
                    throw new NotImplementedException("This join style (" + join + ") is not implemented!");
            }
        }
        private void GetCapTris(VGLineCap cap, out int solidTris, out int roundTris)
        {
            solidTris = roundTris = 0;
            switch (cap)
            {
                case VGLineCap.Butt:                   
                    return;
                case VGLineCap.Round:
                    roundTris = 2;
                    return;
                case VGLineCap.Triangle:
                    solidTris = 1;
                    break;
                case VGLineCap.Square:
                    solidTris = 2;
                    break;
                default:
                    throw new NotImplementedException("This cap style (" + cap + ") is not implemented!");
            }
        }
        private void GetJoinTris(VGLineJoin join, out int solidTris, out int roundTris)
        {
            solidTris = roundTris = 0;

            switch (join)
            {
                case VGLineJoin.None: 
                    return;
                case VGLineJoin.Round:
                    roundTris = 2;
                    return;
                case VGLineJoin.Bevel:
                    solidTris = 1;
                    return;
                case VGLineJoin.Miter:
                    solidTris = 2;
                    return;
                default:
                    throw new NotImplementedException("This join style (" + join + ") is not implemented!");
            }
        }

        #endregion

        #endregion
    }
}
