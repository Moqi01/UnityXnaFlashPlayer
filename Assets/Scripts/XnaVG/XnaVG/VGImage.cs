using Microsoft.Xna.Framework.Graphics;

namespace XnaVG
{
    public sealed class VGImage
    {
        public bool LinearColorspace { get; private set; }
        public bool Premultiplied { get; private set; }
        public VGDevice Device { get; private set; }
        public Texture2D Texture { get; private set; }
        public TextureFilter ImageFilter { get; set; }
        public TextureAddressMode AddressMode { get; set; }

        internal VGImage(VGDevice device, Texture2D texture, bool linearColorspace, bool premultiplied)
        {
            Device = device;
            Texture = texture;
            LinearColorspace = linearColorspace;
            ImageFilter = TextureFilter.Linear;
            Premultiplied = premultiplied;
        }

        internal SamplerState GetSamplerState()
        {
            return new SamplerState
            {
                AddressU = AddressMode,
                AddressV = AddressMode,
                AddressW = AddressMode,
                Filter = ImageFilter,
            };
        }
    }
}
