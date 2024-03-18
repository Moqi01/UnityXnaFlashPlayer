
namespace XnaFlash.Swf.Tags
{
    [SwfTag(8)]
    public class JPEGTablesTag : ISwfControlTag
    {
        public byte[] JpegData { get; private set; }

        public JPEGTablesTag()
        {
            JpegData = new byte[0];
        }
        public void CloneTo(JPEGTablesTag other)
        {
            other.JpegData = JpegData;
        }

        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            if(length==0)
                JpegData = new byte[0];
            else
               JpegData = BitmapUtils.RepairJpegMarkers(stream.ReadByteArray(length));
        }

        #endregion
    }
}