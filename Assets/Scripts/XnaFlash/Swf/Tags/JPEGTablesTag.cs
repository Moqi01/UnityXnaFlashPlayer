
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
            if((int)length<=0)
            {
                JpegData = new byte[0];
                UnityEngine.Debug .LogWarning(string.Format("JPEGTablesTag JpegData Length:{0}", length));
            }
            else
               JpegData = BitmapUtils.RepairJpegMarkers(stream.ReadByteArray(length));
        }

        #endregion
    }
}