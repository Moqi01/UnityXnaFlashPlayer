
namespace XnaVG.Paints
{
    public sealed class VGColorPaint : VGPaint
    {
        public override VGPaintType Type { get { return VGPaintType.Color; } }
        public VGColor Color { get; private set; }

        internal VGColorPaint(VGDevice device, VGColor color)
            : base(device)
        {
            Color = color;            
        }

        public override void Dispose()
        { }
    }
}
