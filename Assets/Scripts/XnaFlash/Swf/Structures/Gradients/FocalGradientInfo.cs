
namespace XnaFlash.Swf.Structures.Gradients
{
    public class FocalGradientInfo : GradientInfo
    {
        public decimal FocalPoint { get; private set; }

        public FocalGradientInfo(SwfStream swf, bool hasAlpha)
            : base(swf, hasAlpha)
        {            
            FocalPoint = swf.ReadFixedHalf();
        }
    }
}
