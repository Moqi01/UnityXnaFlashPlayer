using Microsoft.Xna.Framework.Graphics;

namespace XnaVG.Rendering.States
{
    using BS = BlendState;

    internal sealed class BlendStates
    {
        private delegate void CreateSeparate(VGBlendMode mode, BlendFunction aFunc, Blend aSrc, Blend aDst, BlendFunction cFunc, Blend cSrc, Blend cDst);
        private delegate void Create(VGBlendMode mode, BlendFunction func, Blend src, Blend dst);

        internal readonly BS NoColor = new BS
        {
            ColorWriteChannels = ColorWriteChannels.None
        };

        private BS[] _blendStates;
        public static BlendStates BlendStatesIns;

        internal BlendStates()
        {
            CreateSeparate createSeparate =
                (mode, aFunc, aSrc, aDst, cFunc, cSrc, cDst) => _blendStates[(int)mode] = new BS
                {
                    AlphaBlendFunction = aFunc,
                    AlphaDestinationBlend = aDst,
                    AlphaSourceBlend = aSrc,
                    ColorBlendFunction = cFunc,
                    ColorDestinationBlend = cDst,
                    ColorSourceBlend = cSrc,
                    ColorWriteChannels = ColorWriteChannels.All,
                    MultiSampleMask = -1
                };

            Create create =
                (mode, func, src, dst) => _blendStates[(int)mode] = new BS
                {
                    AlphaBlendFunction = func,
                    AlphaDestinationBlend = dst,
                    AlphaSourceBlend = src,
                    ColorBlendFunction = func,
                    ColorDestinationBlend = dst,
                    ColorSourceBlend = src,
                    ColorWriteChannels = ColorWriteChannels.All,
                    MultiSampleMask = -1
                };

            _blendStates = new BS[(int)VGBlendMode.Clear + 1];
            create(VGBlendMode.Src, BlendFunction.Add, Blend.One, Blend.Zero);
            create(VGBlendMode.SrcOver, BlendFunction.Add, Blend.One, Blend.InverseSourceAlpha);
            create(VGBlendMode.SrcIn, BlendFunction.Add, Blend.DestinationAlpha, Blend.One);
            create(VGBlendMode.SrcOut, BlendFunction.Add, Blend.InverseDestinationAlpha, Blend.Zero);
            create(VGBlendMode.SrcAtop, BlendFunction.Add, Blend.DestinationAlpha, Blend.InverseSourceAlpha);
            create(VGBlendMode.DstOver, BlendFunction.Add, Blend.InverseDestinationAlpha, Blend.One);
            create(VGBlendMode.DstIn, BlendFunction.Add, Blend.Zero, Blend.SourceAlpha);
            create(VGBlendMode.DstOut, BlendFunction.Add, Blend.Zero, Blend.InverseSourceAlpha);
            create(VGBlendMode.DstAtop, BlendFunction.Add, Blend.InverseDestinationAlpha, Blend.SourceAlpha);
            create(VGBlendMode.Xor, BlendFunction.Add, Blend.InverseDestinationAlpha, Blend.InverseSourceAlpha);
            create(VGBlendMode.Additive, BlendFunction.Add, Blend.One, Blend.One);
            create(VGBlendMode.Clear, BlendFunction.Add, Blend.Zero, Blend.Zero);

            createSeparate(VGBlendMode.Multiply, BlendFunction.Add, Blend.SourceAlpha, Blend.InverseSourceAlpha, BlendFunction.Add, Blend.DestinationColor, Blend.InverseSourceAlpha);
            createSeparate(VGBlendMode.Screen, BlendFunction.Add, Blend.SourceAlpha, Blend.InverseSourceAlpha, BlendFunction.Add, Blend.InverseDestinationColor, Blend.InverseSourceAlpha);
            createSeparate(VGBlendMode.Darken, BlendFunction.Add, Blend.SourceAlpha, Blend.InverseSourceAlpha, BlendFunction.Min, Blend.One, Blend.One);
            createSeparate(VGBlendMode.Lighten, BlendFunction.Add, Blend.SourceAlpha, Blend.InverseSourceAlpha, BlendFunction.Max, Blend.One, Blend.One);
            createSeparate(VGBlendMode.LinearDodge, BlendFunction.Add, Blend.SourceAlpha, Blend.InverseSourceAlpha, BlendFunction.Add, Blend.One, Blend.InverseSourceAlpha);
            createSeparate(VGBlendMode.LinearBurn, BlendFunction.Add, Blend.SourceAlpha, Blend.InverseSourceAlpha, BlendFunction.ReverseSubtract, Blend.One, Blend.InverseSourceAlpha);
            BlendStatesIns = this;
        }

        internal BS GetBlendState(VGBlendMode blendMode, ColorWriteChannels channels)
        {
            if (channels == ColorWriteChannels.All)
                return _blendStates[(int)blendMode];

            var o = _blendStates[(int)blendMode];
            return new BS()
            {
                AlphaBlendFunction = o.AlphaBlendFunction,
                AlphaDestinationBlend = o.AlphaDestinationBlend,
                AlphaSourceBlend = o.AlphaSourceBlend,
                ColorBlendFunction = o.ColorBlendFunction,
                ColorDestinationBlend = o.ColorDestinationBlend,
                ColorSourceBlend = o.ColorSourceBlend,
                ColorWriteChannels = channels,
                MultiSampleMask = o.MultiSampleMask
            };
        }
    }
}
