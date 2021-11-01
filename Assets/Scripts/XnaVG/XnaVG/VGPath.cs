using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XnaVG.Rendering;
using XnaVG.Rendering.Tesselation;

namespace XnaVG
{
    public sealed class VGPath
    {
        private Vector4 _extents = new Vector4();
        private int[] _counts = new int[(int)SegmentType._Count];
        private LinkedList<Segment> _segments = new LinkedList<Segment>();
        private Vector2 _last = Vector2.Zero;
        private Vector2 _start = Vector2.Zero;
        private Vector2 _smooth = Vector2.Zero;        
        private Vector2? _startTarget = null;
        private bool _smoothIsQuadratic = false;
        private bool _flattened = true;
        
        public Vector2 Current 
        {
            get { return _last; }
        }
        public Vector2 Position 
        {
            get { return new Vector2(_extents.X, _extents.Y); } 
        }
        public Vector2 Size 
        {
            get { return new Vector2(_extents.Z - _extents.X, _extents.W - _extents.Y); }
        }
        
        internal LinkedListNode<Segment> FirstSegment 
        {
            get { return _segments.First; }
        }
        internal LinkedListNode<Segment> LastSegment
        {
            get { return _segments.Last; }
        }

        public bool IsEmpty
        {
            get { return _segments.First == _segments.Last; }
        }

        public int Count
        {
            get { return _segments.Count; }
        }

        public Vector4 Extents
        {
            get { return _extents; }
        }

        public object Tag { get; set; }

        public VGPath() 
        {
            MoveTo(Vector2.Zero);
        }

        private void UpdateExtents(float x, float y)
        {
            if (x < _extents.X) _extents.X = x;
            if (y < _extents.Y) _extents.Y = y;
            if (x > _extents.Z) _extents.Z = x;
            if (y > _extents.W) _extents.W = y;
        }
        private void SetExtents(float x, float y)
        {
            _extents.X = _extents.Z = x;
            _extents.Y = _extents.W = y;
        }

        public void Append(VGPath path) 
        {
            if (_extents == Vector4.Zero)
                _extents = path._extents;
            else
            {
                UpdateExtents(path._extents.X, path._extents.Y);
                UpdateExtents(path._extents.Z, path._extents.W);
            }

            foreach (var s in path._segments)
                _segments.AddLast(new Segment(s));

            _last = path._last;
            _flattened &= path._flattened;
            _smooth = path._smooth;
            _smoothIsQuadratic = path._smoothIsQuadratic;

            for (int i = 0; i < _counts.Length; i++)
                _counts[i] += path._counts[i];
        }
        public void ClosePath() 
        {
            if (_segments.Last != null && _segments.Last.Value.Type == SegmentType.Close)
                return;

            if (_last != _start)
                LineTo(_start);
                        
            AddSegment(new Segment(SegmentType.Close, _startTarget.HasValue ? _startTarget.Value : Vector2.Zero, false, null));
            _smooth = _last = _start;
        }

        public void MoveTo(Vector2 point) 
        {          
            _last = _start = point;
            _startTarget = null;
            var seg = new Segment(SegmentType.MoveTo, point, false, null);
            bool setExtents = false;

            if (!IsEmpty || _segments.First == null)
            {
                if (!IsEmpty && _segments.Last.Value.Type != SegmentType.Close)
                    _segments.Last.Value.MakeJoin = false;
                else if (IsEmpty)
                    setExtents = true;

                AddSegment(seg);
            }
            else
            {
                _segments.First.Value = seg;
                setExtents = true;
            }
        
            if (setExtents)
                SetExtents(point.X, point.Y);
            else
                UpdateExtents(point.X, point.Y);

            _smooth = point;
        }
        public void LineTo(Vector2 point) 
        {
            _last = point;
            var l = AddSegment(new Segment(SegmentType.LineTo, point, true, null));
            UpdateExtents(point.X, point.Y);

            if (!_startTarget.HasValue)
                _startTarget = point;

            _smooth = point;
        }
        public void QuadraticTo(Vector2 control, Vector2 end)
        {
            _flattened = false;
            _last = end;
            var q = AddSegment(new Segment(SegmentType.CurveTo, end, true, control));
            UpdateExtents(control.X, control.Y);
            UpdateExtents(end.X, end.Y);

            if (!_startTarget.HasValue)
                _startTarget = control;

            _smooth = Rendering.VectorMath.GetReflection(ref end, ref control);
            _smoothIsQuadratic = true;
        }
        public void QuadraticSmoothTo(Vector2 end)
        {
            QuadraticTo(_smoothIsQuadratic ? _smooth : _last, end);
        }
        public void CubicTo(Vector2 controlA, Vector2 controlB, Vector2 end)
        {
            // Taken from http://commaexcess.com/projects/8/algae
            Vector2 pa = Vector2.Lerp(_last, controlA, 0.75f);
            Vector2 pb = Vector2.Lerp(end, controlB, 0.75f);
            Vector2 dif = (end - _last) * (1.0f / 16.0f);

            // Calculate control points
            Vector2 c1 = Vector2.Lerp(_last, controlA, 0.375f);
            Vector2 c2 = Vector2.Lerp(pa, pb, 0.375f) - dif;
            Vector2 c3 = Vector2.Lerp(pb, pa, 0.375f) + dif;
            Vector2 c4 = Vector2.Lerp(end, controlB, 0.375f);

            // Anchor points
            Vector2 a1 = Vector2.Lerp(c1, c2, 0.5f);
            Vector2 a2 = Vector2.Lerp(pa, pb, 0.5f);
            Vector2 a3 = Vector2.Lerp(c3, c4, 0.5f);

            // Quadratic curves
            QuadraticTo(c1, a1);
            QuadraticTo(c2, a2);
            QuadraticTo(c3, a3);
            QuadraticTo(c4, end);

            _smooth = Rendering.VectorMath.GetReflection(ref end, ref controlB);
            _smoothIsQuadratic = false;
        }
        public void CubicSmoothTo(Vector2 controlB, Vector2 end)
        {
            CubicTo(_smoothIsQuadratic ? _last : _smooth, controlB, end);
        }
     
        public void MoveToRelative(Vector2 point)
        {
            MoveTo(point + _last);
        }
        public void LineToRelative(Vector2 point) 
        {
            LineTo(point + _last);
        }        
        public void QuadraticToRelative(Vector2 control, Vector2 end)
        {
            QuadraticTo(control + _last, end + _last);
        }
        public void QuadraticSmoothToRelative(Vector2 end)
        {
            QuadraticSmoothTo(end + _last);
        }
        public void CubicToRelative(Vector2 controlA, Vector2 controlB, Vector2 end)
        {
            CubicTo(controlA + _last, controlB + _last, end + _last);
        }
        public void CubicSmoothToRelative(Vector2 controlB, Vector2 end)
        {
            CubicSmoothTo(controlB + _last, end + _last);
        }
        
        #region Helpers

        public VGPath Clone()
        {
            return new VGPath
            {
                _extents = this._extents,
                _counts = this._counts.Clone() as int[],
                _last = this._last,
                _start = this._start,
                _segments = new LinkedList<Segment>(this._segments.Select(s => new Segment(s))),
                _flattened = _flattened,
                _startTarget = _startTarget
            };
        }  
        public void Offset(Vector2 offset)
        {
            for (var s = _segments.First; s != null; s = s.Next)
            {
                s.Value.Target += offset;
                if (s.Value.Controls != null)
                {
                    for (int i = 0; i < s.Value.Controls.Length; i++)
                        s.Value.Controls[i] += offset;
                }
            }

            _extents.X += offset.X;
            _extents.Y += offset.Y;
            _extents.Z += offset.X;
            _extents.W += offset.Y;

            _last += offset;
            _start += offset;
            _smooth += offset;
            if (_startTarget.HasValue) 
                _startTarget = _startTarget.Value + offset;
        }
        public void Scale(Vector2 factor)
        {
            for (var s = _segments.First; s != null; s = s.Next)
            {
                s.Value.Target *= factor;
                if (s.Value.Controls != null)
                {
                    for (int i = 0; i < s.Value.Controls.Length; i++)
                        s.Value.Controls[i] *= factor;
                }
            }

            _extents.X *= factor.X;
            _extents.Y *= factor.Y;
            _extents.Z *= factor.X;
            _extents.W *= factor.Y;

            _last *= factor;
            _start *= factor;
            _smooth *= factor;
            if (_startTarget.HasValue)
                _startTarget = _startTarget.Value * factor;
        }
        /// <summary>
        /// Align path to start of the first path
        /// </summary>
        public void Align()
        {
            var s = _segments.First;
            for (; s != null && (s.Value.Type == SegmentType.MoveTo || s.Value.Type == SegmentType.Close); s = s.Next) ;
            if (s == null) return;
            Offset(-s.Previous.Value.Target);
        }
        public void ExpandExtents(ref Vector4 extents)
        {
            VectorMath.ExpandExtents(ref extents, ref _extents);
        }
        public VGPath Flatten()
        {
            if (_flattened)
                return this;
            
            VGPath p = new VGPath();
            p._extents = _extents;
            p._last = _last;
            p._start = _start;
            p._startTarget = _startTarget;
            p._flattened = true;

            Action<Vector2, Vector2, bool> callback = (pt, n, join) =>
            {
                n.Normalize();
                p._segments.AddLast(new Segment(SegmentType._Tesselated, pt, join, n));
                p._counts[(int)SegmentType._Tesselated]++;
            };

            Vector2 last = new Vector2();
            for (var s = _segments.First; s != null; s = s.Next)
            {
                if (s.Value.Type == SegmentType.CurveTo)
                {
                    QuadraticFlattener.Flatten(
                        last.X, last.Y, s.Value.Target.X, s.Value.Target.Y,
                        s.Value.Controls[0].X, s.Value.Controls[0].Y,
                        s.Value.MakeJoin, callback);
                }
                else
                {
                    p._segments.AddLast(s.Value);
                    p._counts[(int)s.Value.Type]++;
                }

                last = s.Value.Target;
            }

            return p;
        }
        internal int GetCount(SegmentType type)
        {
            return _counts[(int)type];
        }
        private LinkedListNode<Segment> AddSegment(Segment segment)
        {                        
            _counts[(int)segment.Type]++;
            return _segments.AddLast(segment);            
        }

        #endregion

        #region Primitives

        public void Rectangle(float left, float top, float right, float bottom)
        {
            MoveTo(new Vector2(left, top));
            LineTo(new Vector2(right, top));
            LineTo(new Vector2(right, bottom));
            LineTo(new Vector2(left, bottom));
            ClosePath();
        }

        #endregion

        #region Types

        internal class Segment
        {
            public SegmentType Type;            
            public Vector2 Target;
            public Vector2[] Controls;
            public bool MakeJoin;

            internal Segment(SegmentType type, Vector2 target, bool makeJoin, params Vector2[] controls)
            {
                Type = type;
                Target = target;
                Controls = controls;
                MakeJoin = makeJoin;
            }

            internal Segment(Segment other)
            {
                Type = other.Type;
                Target = other.Target;                
                MakeJoin = other.MakeJoin;

                if (other.Controls != null)
                {
                    Controls = new Vector2[other.Controls.Length];
                    Array.Copy(other.Controls, Controls, Controls.Length);
                }
            }
        }
        internal enum SegmentType : byte
        {
            MoveTo = 0,
            LineTo = 1,
            CurveTo = 2,
            Close = 3,

            _Tesselated = 4,
            _Count = 5
        }

        #endregion
    }
}
