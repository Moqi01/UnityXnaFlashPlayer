using System.Collections.Generic;

namespace XnaVG.Paints
{
    public sealed class VGRadialPaint : VGGradientPaint
    {
        public override VGPaintType Type { get { return VGPaintType.RadialGradient; } }
        public float FocalPoint { get; set; }

        internal VGRadialPaint(VGDevice device, bool linear, IEnumerable<KeyValuePair<byte, VGColor>> stops)
            : base(device, linear, stops)
        { }
    }
}
