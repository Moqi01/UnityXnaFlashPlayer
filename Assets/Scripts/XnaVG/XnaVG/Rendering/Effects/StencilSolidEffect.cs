using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaVG.Rendering.Effects
{
    internal sealed class StencilSolidEffect : IDisposable
    {
        private Effect _effect;
        private EffectPass _pass;
        private EffectParameter _projection;
        private EffectParameter _transformation;

        public StencilSolidEffect(Effect effect)
        {
            _effect = effect;
            _pass = _effect.CurrentTechnique.Passes[0];
            _projection = effect.Parameters["Projection"];
            _transformation = effect.Parameters["Transformation"];
        }

        public void Apply(VGMatrix projection, VGMatrix transformation)
        {
            _projection.SetValue(projection);
            _transformation.SetValue(transformation);
            _pass.Apply();
        }
        
        public void Dispose()
        {
            _effect.Dispose();
        }
    }
}
