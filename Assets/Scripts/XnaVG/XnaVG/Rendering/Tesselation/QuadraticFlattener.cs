using System;
using Microsoft.Xna.Framework;

namespace XnaVG.Rendering.Tesselation
{
    internal static class QuadraticFlattener
    {      
        private const double AngleLimit = 0.01;
        private const double DistanceLimit = 0.01;
        private const double CollinearityLimit = 0.01;
        private const int DepthLimit = 16;

        public static void Flatten(double sx, double sy, double ex, double ey, double cx, double cy, bool join, Action<Vector2, Vector2, bool> emit)
        {
            _Flatten(sx, sy, ex, ey, cx, cy, emit, 0);
            emit(new Vector2((float)ex, (float)ey), new Vector2((float)(ey - cy), (float)(cx - ex)), join);
        }

        private static void _Flatten(double sx, double sy, double ex, double ey, double cx, double cy, Action<Vector2, Vector2, bool> emit, int depth)
        {
            if (depth > DepthLimit)
                return;

            double ltX = (sx + cx) / 2;
            double ltY = (sy + cy) / 2;
            double rtX = (cx + ex) / 2;
            double rtY = (cy + ey) / 2;
            double midX = (ltX + rtX) / 2;
            double midY = (ltY + rtY) / 2;

            double dx = ex - sx;
            double dy = ey - sy;
            double d = Math.Abs((cx - ex) * dy - (cy - ey) * dx);

            double nx = ey - sy;
            double ny = sx - ex;

            if (d > CollinearityLimit)
            {
                if (d * d <= DistanceLimit * (dx * dx + dy * dy))
                {
                    double da = Math.Abs(Math.Atan2(ey - cy, ex - cx) - Math.Atan2(cy - sy, cx - sx));
                    if (da >= Math.PI) da = 2 * Math.PI - da;

                    if (da < AngleLimit)
                    {
                        emit(new Vector2((float)midX, (float)midY), new Vector2((float)nx, (float)ny), false);
                        return;
                    }
                }
            }
            else
            {
                dx = midX - (sx + ex) / 2;
                dy = midY - (sy + ey) / 2;
                if (dx * dx + dy * dy <= DistanceLimit)
                {
                    emit(new Vector2((float)midX, (float)midY), new Vector2((float)nx, (float)ny), false);
                    return;
                }
            }

            depth++;
            _Flatten(sx, sy, midX, midY, ltX, ltY, emit, depth);
            _Flatten(midX, midY, ex, ey, rtX, rtY, emit, depth); 
        }             
    }
}
