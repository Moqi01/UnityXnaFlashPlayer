using System.Collections.Generic;

namespace XnaVG.Paints
{
    public sealed class VGLinearPaint : VGGradientPaint
    {
        public override VGPaintType Type { get { return VGPaintType.LinearGradient; } }

        internal VGLinearPaint(VGDevice device, bool linear, IEnumerable<KeyValuePair<byte, VGColor>> stops)
            : base(device, linear, stops)
        { }
    }
}
