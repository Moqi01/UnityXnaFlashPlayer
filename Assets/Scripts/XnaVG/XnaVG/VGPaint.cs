using System;

namespace XnaVG
{
    public abstract class VGPaint : IDisposable
    {
        public VGDevice Device { get; private set; }
        public object Tag { get; set; }
        public abstract VGPaintType Type { get; }

        protected VGPaint(VGDevice device)
        {
            Device = device;
        }

        public abstract void Dispose();
    }
}
