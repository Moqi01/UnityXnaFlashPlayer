using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using XnaVG;

namespace XnaVGPipeline.Path
{
    internal class PathBuilder : IDisposable
    {
        private BinaryWriter _writer;
        
        public PathBuilder(BinaryWriter writer)
        {
            _writer = writer;
        }

        private void Write(PathOperation op, bool relative, params Vector2[] points)
        {
            byte o = (byte)op;
            o &= 0x7f;
            if (relative) o |= 0x80;
           
            _writer.Write(o);
            foreach (var point in points)
            {
                _writer.Write(point.X);
                _writer.Write(point.Y);
            }
        }

        public void ClosePath() 
        {
            Write(PathOperation.Close, false);
        }
        public void MoveTo(Vector2 point) 
        {
            Write(PathOperation.MoveTo, false, point);
        }
        public void LineTo(Vector2 point)
        {
            Write(PathOperation.LineTo, false, point);
        }
        public void QuadraticTo(Vector2 control, Vector2 end)
        {
            Write(PathOperation.QuadraticTo, false, control, end);
        }
        public void QuadraticSmoothTo(Vector2 end)
        {
            Write(PathOperation.QuadraticSmoothTo, false, end);
        }
        public void CubicTo(Vector2 controlA, Vector2 controlB, Vector2 end)
        {
            Write(PathOperation.CubicTo, false, controlA, controlB, end);
        }
        public void CubicSmoothTo(Vector2 controlB, Vector2 end)
        {
            Write(PathOperation.CubicSmoothTo, false, controlB, end);
        }
        
        public void MoveToRelative(Vector2 point)
        {
            Write(PathOperation.MoveTo, true, point);
        }
        public void LineToRelative(Vector2 point)
        {
            Write(PathOperation.LineTo, true, point);
        }
        public void QuadraticToRelative(Vector2 control, Vector2 end)
        {
            Write(PathOperation.QuadraticTo, true, control, end);
        }
        public void QuadraticSmoothToRelative(Vector2 end)
        {
            Write(PathOperation.QuadraticSmoothTo, true, end);
        }
        public void CubicToRelative(Vector2 controlA, Vector2 controlB, Vector2 end)
        {
            Write(PathOperation.CubicTo, true, controlA, controlB, end);
        }
        public void CubicSmoothToRelative(Vector2 controlB, Vector2 end)
        {
            Write(PathOperation.CubicSmoothTo, true, controlB, end);
        }
        public void Dispose()
        {
            Write(PathOperation.End, false);
            _writer = null;
        }
    }
}
