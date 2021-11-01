using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaVG;

namespace XnaFlash.Movie
{
    public class DisplayState
    {
#region Clipping

        private VGStencilMasks _clippingMask = VGStencilMasks.None;
        private ClipLayer[] _clipLayers = new ClipLayer[7];
        public VGStencilMasks ClippingMask { get { return _clippingMask; } }
        public VGStencilMasks AddClipping(ushort clipDepth)
        {
            for (int i = 0, m = (int)VGStencilMasks.Mask1; i < _clipLayers.Length; i++, m <<= 1)
                if (_clipLayers[i] == null)
                {
                    _clipLayers[i] = new ClipLayer((VGStencilMasks)m, clipDepth);
                    _clippingMask |= (VGStencilMasks)m;
                    return _clipLayers[i].Layer;
                }

            throw new NotSupportedException("Too much nested clipping characters!");
        }
        public VGStencilMasks ReleaseClippings(ushort currentDepth)
        {
            VGStencilMasks mask = VGStencilMasks.None;
            if (_clippingMask == VGStencilMasks.None)
                return mask;
                        
            for (int i = 0; i < _clipLayers.Length; i++)
            {
                if (_clipLayers[i] == null)
                    continue;

                if (_clipLayers[i].ClipDepth == currentDepth)
                {
                    _clipLayers[i] = null;
                    continue;
                }

                mask |= _clipLayers[i].Layer;
            }

            _clippingMask = mask;
            return mask;
        }

        public class ClipLayer 
        {
            public readonly VGStencilMasks Layer;
            public readonly ushort ClipDepth;

            public ClipLayer(VGStencilMasks layer, ushort clipDepth)
            {
                Layer = layer;
                ClipDepth = clipDepth;
            }

        }

#endregion
    }
}
