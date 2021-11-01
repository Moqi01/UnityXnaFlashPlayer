
namespace XnaVG.Paints
{
    public sealed class VGPatternPaint : VGPaint
    {
        public VGImage Pattern { get; private set; }
        
        public override VGPaintType Type { get { return Pattern.Premultiplied ? VGPaintType.PatternPremultiplied : VGPaintType.Pattern; } }

        internal VGPatternPaint(VGDevice device, VGImage image)
            : base(device)
        {
            Pattern = image;
        }

        public override void Dispose() { }
    }
}
