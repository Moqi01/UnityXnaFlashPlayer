using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaVG.Paints
{
    public abstract class VGGradientPaint : VGPaint
    {
        internal Texture2D Gradient { get; private set; }
        public bool LinearColorspace { get; private set; }
        public TextureFilter GradientFilter { get; set; }
        public TextureAddressMode AddressMode { get; set; }

        protected VGGradientPaint(VGDevice device, bool linear, IEnumerable<KeyValuePair<byte, VGColor>> stops)
            : base(device)
        {
            LinearColorspace = linear;
            GradientFilter = TextureFilter.Linear;
            AddressMode = TextureAddressMode.Clamp;
            
            var colors = GetColors(stops);
            Gradient = new Texture2D(device.GraphicsDevice, colors.Length, 1, false, SurfaceFormat.Color);
            Gradient.SetData(colors);
        }

        internal SamplerState GetSamplerState()
        {
            return new SamplerState
            {
                AddressU = AddressMode,
                AddressV = AddressMode,
                AddressW = AddressMode,
                Filter = GradientFilter
            };
        }

        public override void Dispose()
        {
            if (Gradient == null)
                return;

            Gradient.Dispose();
            Gradient = null;
        }

        private static Color[] GetColors(IEnumerable<KeyValuePair<byte, VGColor>> stops)
        {
            VGColor?[] colors = new VGColor?[256];
            foreach (var s in stops)
                colors[s.Key] = s.Value;
            
            int i, j, k;
            
            // Pad first color
            for (i = 0; i < colors.Length && !colors[i].HasValue; i++) ;
            for (j = i; j > 0; j--) colors[j - 1] = colors[j];

            while (true)
            {
                // Find next stop
                for (j = i + 1; j < colors.Length && !colors[j].HasValue; j++) ;
                if (j >= colors.Length) break;

                // Interpolate color
                for (k = i + 1; k < j; k++)
                    colors[k] = VGColor.Lerp(colors[i].Value, colors[j].Value, (k - i) / ((float)(j - i)));

                i = j;
            }

            // Pad last color
            for (j = i + 1; j < colors.Length; j++) colors[j] = colors[i];

            // Make non-nullable
            Color[] result = new Color[colors.Length];
            for (i = 0; i < colors.Length; i++)
                result[i] = colors[i].Value;

            return result;
        }
    }
}
