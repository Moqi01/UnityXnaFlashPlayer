using Microsoft.Xna.Framework;

namespace XnaVG.Rendering
{
    internal static class Constants
    {
        // Does not equal Single.Epsilon
        internal const float Epsilon = 1.175494351e-37f;
        internal const float DefaultDPI = 96f;
        internal const float PixelsPerPoint = DefaultDPI / 72f;

        internal const int MSAA_HalfMask = 0x01010101;
        internal const int MSAA_FullMask = 0x11111111;

        internal const int VerticesPerRoundCapJoint = 4;
        internal const int IndicesPerRoundCapJoint = 6;

        internal static readonly float[] Coef_BezierStart = { 0, 0 };
        internal static readonly float[] Coef_BezierControl = { 0.5f, 0.5f };
        internal static readonly float[] Coef_BezierEnd = { 1, 0 };
        internal static readonly float[] Coef_Solid = { 0, 0 };

        internal static readonly float RoundDistanceRatio = Vector2.One.Length();
        internal static readonly Vector2 LeftTop = -Vector2.One;
        internal static readonly Vector2 LeftMiddle = -Vector2.UnitX;
        internal static readonly Vector2 LeftBottom = new Vector2(-1, 1);
        internal static readonly Vector2 MiddleTop = -Vector2.UnitY;
        internal static readonly Vector2 MiddleMiddle = Vector2.Zero;
        internal static readonly Vector2 MiddleBottom = Vector2.UnitY;
        internal static readonly Vector2 RightTop = new Vector2(1, -1);
        internal static readonly Vector2 RightMiddle = Vector2.UnitX;
        internal static readonly Vector2 RightBottom = Vector2.One;
        internal static readonly Vector2 QuadSize = 2f * Vector2.One;
        internal static readonly Vector4 ScreenExtents = new Vector4(-1f, -1f, 1f, 1f);

        internal static readonly Vector2[] MSAAPattern = new Vector2[] {
            // Faster
            new Vector2(-0.122f, -0.377f),
            new Vector2(-0.377f,  0.122f),
            new Vector2( 0.122f,  0.377f),
            new Vector2( 0.377f, -0.122f),
            
            // Better
            new Vector2(-0.444f, -0.066f),
            new Vector2(-0.072f,  0.194f),
            new Vector2( 0.305f,  0.066f),
            new Vector2( 0.072f, -0.194f),

            // Best
            new Vector2(-0.188f, -0.316f),
            new Vector2(-0.316f,  0.316f),
            new Vector2( 0.188f,  0.433f),
            new Vector2( 0.433f, -0.433f),
            new Vector2( 0.444f,  0.066f),
            new Vector2(-0.305f, -0.066f),
            new Vector2( 0.377f,  0.122f),
            new Vector2( 0.188f,  0.316f),
        };
    }
}
