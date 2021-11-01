using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XnaVG;
using XnaVG.Rendering;

namespace XnaFlash.Swf.Paths
{
    public class PathBuilder
    {
        private VGPath _path = new VGPath();
        private List<Edge> _edges = new List<Edge>(256);
        private bool _fill, _close;

        public int StyleIndex { get; set; }
        public object Tag { get; set; }
        public bool IsEmpty { get { return _path.IsEmpty; } }

        public PathBuilder(bool isFill, bool noCloseStrokes)
        {
            _fill = isFill;
            _close = !noCloseStrokes;
        }

        public void Line(int fromX, int fromY, int toX, int toY, bool reverse)
        {
            if (fromX == toX && fromY == toY) return;
            AddEdge(new Edge { From = new Point(fromX, fromY), To = new Point(toX, toY), Curve = false }, reverse);
        }

        public void Curve(int fromX, int fromY, int toX, int toY, int ctlX, int ctlY, bool reverse)
        {
            if (fromX == toX && fromY == toY) return;
            if ((fromX == ctlX && fromY == ctlY) || (toX == ctlX && toY == ctlY))
                AddEdge(new Edge { From = new Point(fromX, fromY), To = new Point(toX, toY), Curve = false }, reverse);
            else
                AddEdge(new Edge { From = new Point(fromX, fromY), To = new Point(toX, toY), Curve = true, Ctl = new Point(ctlX, ctlY) }, reverse);
        }

        public VGPath GetPath()
        {
            return _path;
        }

        private void AddEdge(Edge edge, bool reverse)
        {
            if (reverse) edge.Reverse();
            _edges.Add(edge);
        }

        private void AppendEdgeToPath(Edge edge)
        {
            if (edge.Curve)
                _path.QuadraticTo(new Vector2(edge.Ctl.X, edge.Ctl.Y), new Vector2(edge.To.X, edge.To.Y));
            else
                _path.LineTo(new Vector2(edge.To.X, edge.To.Y));
        }        
        private IEnumerable<Edge> PrepareEdgesStroke()
        {
            yield return _edges[0];

            var start = _edges[0].From;
            var end = _edges[0].To;
            _edges.RemoveAt(0);

            bool match = true;
            while (match && _edges.Count > 0)
            {
                match = false;
                for (int i = 0; i < _edges.Count; i++)
                {
                    if (!_edges[i].GoesFrom(ref end))
                        continue;

                    var edge = _edges[i];
                    _edges.RemoveAt(i);

                    yield return edge;
                    end = edge.To;

                    match = !(end == start);
                    break;
                }
                if (!match && _edges.Count > 0)
                {
                    start = _edges[0].From;
                    end = _edges[0].To;
                    yield return _edges[0];
                    _edges.RemoveAt(0);
                    match = true;
                }
            }
        }
        private IEnumerable<Edge> PrepareEdgesFill()
        {
            yield return _edges[0];

            var start = _edges[0].From;
            var end = _edges[0].To;
            _edges.RemoveAt(0);

            bool match = true;
            while (match && _edges.Count > 0)
            {
                match = false;
                for (int i = 0; i < _edges.Count; i++)
                {
                    if (!_edges[i].GoesFrom(ref end))
                        continue;

                    var edge = _edges[i];
                    _edges.RemoveAt(i);

                    yield return edge;
                    end = edge.To;

                    match = true;
                    if (end == start)
                    {
                        if (_edges.Count > 0)
                        {
                            start = _edges[0].From;
                            end = _edges[0].To;
                            yield return _edges[0];
                            _edges.RemoveAt(0);
                        }
                        else
                            match = false;
                    }
                    break;
                }                
            }
        }
        public void Flush()
        {
            if (_edges.Count < 1)
                return;

            _edges.Sort();

            var start = _edges[0];
            var end = _edges[0];
            foreach(var edge in (_fill ? PrepareEdgesFill() : PrepareEdgesStroke()))
            {
                if (_close && end.EndsIn(start))
                    _path.ClosePath();

                if (!end.EndsIn(edge))
                {
                    if (_fill)
                        _path.ClosePath();
                                        
                    _path.MoveTo(new Vector2(edge.From.X, edge.From.Y));
                }
                AppendEdgeToPath(edge);
                end = edge;
            }

            if (_fill)
                _path.ClosePath();
        }

        #region OLD
        /* private void AddToPath(Edge edge)
        {
            if (edge.Curve)
                _path.QuadraticTo(new Vector2(edge.CtlX, edge.CtlY), new Vector2(edge.ToX, edge.ToY));
            else
                _path.LineTo(new Vector2(edge.ToX, edge.ToY));
        }

        public void Flush()
        {
            if (_edges.Count < 1)
                return;
                        
            _edges.Sort();

            int i;
            Edge start = _edges[0], end = _edges[0];
            _edges.RemoveAt(0);
            
            _path.MoveTo(new Vector2(start.FromX, start.FromY));
            AddToPath(start);

            bool match = true;
            while (_edges.Count > 0 && match)
            {
                match = false;
                for (i = 0; i < _edges.Count; i++)
                {
                    var edge = _edges[i];
                    if (!end.ToEnd(edge))
                        continue;

                    _edges.RemoveAt(i);
                    AddToPath(edge);
                    end = edge;
                    match = true;

                    if (!end.EndsIn(start))
                        break;

                    if (_edges.Count < 1)
                        break;

                    start = end = _edges[0];
                    if (_close) _path.ClosePath();
                    _path.MoveTo(new Vector2(start.FromX, start.FromY));
                    AddToPath(start);
                    _edges.RemoveAt(0);

                    break;
                }
            }

            if (_close) _path.ClosePath();
            _edges.Clear();
        }*/
        #endregion

        private class Edge : IComparable<Edge>
        {
            public Point From, To, Ctl;
            public bool Curve;
            
            public bool EndsIn(Edge e)
            {
                return To.X == e.From.X && To.Y == e.From.Y;
            }

            public bool GoesFrom(ref Point end)
            {
                if (From == end)
                    return true;

                if (To == end)             
                {
                    Reverse();
                    return true;
                }

                return false;
            }
            public void Reverse()
            {
                var f = From;
                From = To;
                To = f;
            }
            public int CompareTo(Edge other)
            {
                if (From.X != other.From.X) return From.X.CompareTo(other.From.X);
                if (From.Y != other.From.Y) return From.Y.CompareTo(other.From.Y);
                if (To.X != other.To.X) return To.X.CompareTo(other.To.X);
                if (To.Y != other.To.Y) return To.Y.CompareTo(other.To.Y);
                return 0;
            }
        }
    }
}