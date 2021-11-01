using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XnaFlash.Swf.Structures;

namespace XnaFlash.Swf.Paths
{
    using FillStyles = Dictionary<FillStyle, PathBuilder>;
    using LineStyles = Dictionary<LineStyle, PathBuilder>;

    public class Shape
    {
        public Point ReferencePoint { get; private set; }
        public SubShape[] SubShapes { get; private set; }
        public int TotalFillStyles { get; private set; }
        public int TotalLineStyles { get; private set; }

        public Shape(IEnumerable<ShapeRecord> records, bool reverse)
        {
            LoadEdges(records, reverse);
        }
        private void LoadEdges(IEnumerable<ShapeRecord> records, bool rtReverse)
        {
            bool ltReverse = !rtReverse;
            int x = 0, y = 0, tx, ty, cx, cy;
            Point? refPt = null;
            PathBuilder ltFill = null, rtFill = null;
            PathBuilder stroke = null;
            List<SubShape> subShapes = new List<SubShape>();
            SubShape subShape = new SubShape(this);

            foreach (var r in records)
            {
                switch (r.Type)
                {
                    case ShapeRecord.ShapeRecordType.StraightEdge:
                        tx = x + r.DrawDeltaX;
                        ty = y + r.DrawDeltaY;

                        if (!refPt.HasValue) refPt = new Point(x, y);
                        if (ltFill != null) ltFill.Line(x, y, tx, ty, ltReverse);
                        if (rtFill != null) rtFill.Line(x, y, tx, ty, rtReverse);
                        if (stroke != null) stroke.Line(x, y, tx, ty, rtReverse);

                        x = tx;
                        y = ty;
                        break;
                    case ShapeRecord.ShapeRecordType.CurvedEdge:
                        cx = x + r.DrawControlX;
                        cy = y + r.DrawControlY;
                        tx = cx + r.DrawDeltaX;
                        ty = cy + r.DrawDeltaY;

                        if (!refPt.HasValue) refPt = new Point(x, y);
                        if (ltFill != null) ltFill.Curve(x, y, tx, ty, cx, cy, ltReverse);
                        if (rtFill != null) rtFill.Curve(x, y, tx, ty, cx, cy, rtReverse);
                        if (stroke != null) stroke.Curve(x, y, tx, ty, cx, cy, rtReverse);

                        x = tx;
                        y = ty;
                        break;
                    case ShapeRecord.ShapeRecordType.StyleChange:
                        if (r.NewMoveTo)
                        {
                            x = r.MoveDeltaX;
                            y = r.MoveDeltaY;
                        }
                        if (r.NewFillStyle0)
                            ltFill = GetByFillStyle(r.FillStyle0, subShape);
                        if (r.NewFillStyle1)
                            rtFill = GetByFillStyle(r.FillStyle1, subShape);
                        if (r.NewLineStyle)
                            stroke = GetByLineStyle(r.LineStyle, subShape);
                        if (r.NewStyles)
                        {
                            foreach (var s in subShape.Fills.Values) s.Flush();
                            foreach (var s in subShape.Lines.Values) s.Flush();
                            subShapes.Add(subShape);
                            subShape = new SubShape(this);
                        }
                        break;
                }
            }

            foreach (var s in subShape.Fills.Values) s.Flush();
            foreach (var s in subShape.Lines.Values) s.Flush();
            subShapes.Add(subShape);

            ReferencePoint = refPt ?? new Point(0, 0);
            SubShapes = subShapes.ToArray();
        }
        private PathBuilder GetByFillStyle(FillStyle style, SubShape subShape)
        {
            if (style == null) return null;
            if (subShape.Fills.ContainsKey(style)) return subShape.Fills[style];
            var b = new PathBuilder(true, false);
            b.Tag = style;
            subShape.Fills.Add(style, b);
            return b;
        }
        private PathBuilder GetByLineStyle(LineStyle style, SubShape subShape)
        {
            if (style == null) return null;
            if (subShape.Lines.ContainsKey(style)) return subShape.Lines[style];
            var b = new PathBuilder(false, style.NoClose);
            b.Tag = style;
            subShape.Lines.Add(style, b);
            return b;
        }

        public class SubShape
        {
            public Shape Shape { get; protected set; }
            public FillStyles Fills { get; private set; }
            public LineStyles Lines { get; private set; }

            internal SubShape(Shape shape)
            {
                Shape = shape;
                Fills = new FillStyles();
                Lines = new LineStyles();
            }
        }
    }
}