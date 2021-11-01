using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace XnaVG.Rendering
{
    internal static class VectorMath
    {
        public static void GetTangentFrom(LinkedListNode<VGPath.Segment> seg, out Vector2 tangent)
        {
            tangent = seg.Next.Value.Target - seg.Value.Target;
            tangent.Normalize();
        }

        public static void GetTangentTo(LinkedListNode<VGPath.Segment> seg, out Vector2 tangent)
        {
            tangent = seg.Value.Target - seg.Previous.Value.Target;
            tangent.Normalize();
        }

        public static void GetLeftNormal(ref Vector2 tangent, out Vector2 normal)
        {
            normal = new Vector2(tangent.Y, -tangent.X);
            normal.Normalize();
        }

        public static void GetLeftNormal(Vector2 from, Vector2 to, out Vector2 normal)
        {
            normal = new Vector2(to.Y - from.Y, from.X - to.X);
            normal.Normalize();
        }

        public static float LinesIntersection(ref Vector2 a, ref Vector2 b, ref Vector2 aDirection, ref Vector2 bDirection, out Vector2 result)
        {
            float numer = (bDirection.X * (a.Y - b.Y) - bDirection.Y * (a.X - b.X));
            float denom = (bDirection.Y * aDirection.X) - (bDirection.X * aDirection.Y);
            if (Math.Abs(denom) <= Constants.Epsilon)
            {
                if (Math.Abs(numer) <= Constants.Epsilon)
                {
                    result = GetCenter(ref a, ref b);
                    return numer;
                }
                else
                {
                    result = new Vector2(float.NaN, float.NaN);
                    return float.NaN;
                }
            }
            else
            {
                numer /= denom;
                result = new Vector2(a.X + numer * aDirection.X, a.Y + numer * aDirection.Y);
                return numer;
            }
        }

        public static Vector2 GetCenter(ref Vector2 a, ref Vector2 b)
        {
            return (a + b) * 0.5f;
        }

        public static float SignedAngle(ref Vector2 a, ref Vector2 b)
        {
            return (float)(Math.Atan2(b.Y, b.X) - Math.Atan2(a.Y, a.X));
        }

        public static int CrossProduct(ref Vector2 a, ref Vector2 b)
        {
            return Math.Sign(a.X * b.Y - b.X * a.Y);
        }

        public static Vector2 GetReflection(ref Vector2 center, ref Vector2 reflect)
        {
            return center + center - reflect;                 
        }

        public static void ExpandExtents(ref Vector4 actual, ref Vector4 expansion)
        {
            actual.X = Math.Min(actual.X, expansion.X);
            actual.Y = Math.Min(actual.Y, expansion.Y);
            actual.Z = Math.Max(actual.Z, expansion.Z);
            actual.W = Math.Max(actual.W, expansion.W);
        }
    }
}