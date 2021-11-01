
namespace XnaFlash.Swf.Structures.Filters
{
    public class ColorMatrixFilter : Filter
    {
        public override Filter.ID FilterID { get { return ID.ColorMatrix; } }
        public float[] Matrix { get; private set; }

        public ColorMatrixFilter(SwfStream stream)
        {
            Matrix = stream.ReadSingleArray(20);
        }
    }
}
