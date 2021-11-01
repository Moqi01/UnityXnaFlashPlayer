using System;

namespace XnaFlash.Swf
{
    public class SwfCorruptedException : Exception
    {
        public SwfCorruptedException(string message)
            : base(message)
        { }

        public SwfCorruptedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
