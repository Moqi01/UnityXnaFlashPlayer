using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaVG.Rendering.Effects
{
    internal sealed class StencilFillEffect : IDisposable
    {
        private Vector4 _offsetValue;

        private Effect _effect;
        private EffectPass _pass;
        private EffectParameter _projection;
        private EffectParameter _transformation;
        private EffectParameter _offset;        

        public StencilFillEffect(Effect effect)
        {
            _effect = effect;
            _pass = _effect.CurrentTechnique.Passes[0];
            _projection = effect.Parameters["Projection"];
            _transformation = effect.Parameters["Transformation"];
            _offset = effect.Parameters["Offset"];
            _offsetValue = Vector4.Zero;
        }

        public void Apply()
        {
            _pass.Apply();
        }

        public void SetParameters(VGMatrix projection, VGMatrix transformation)
        {
            _projection.SetValue(projection);
            _transformation.SetValue(transformation);
            _offset.SetValue(_offsetValue = Vector4.Zero);            
        }

        public void SetScreenOffset(ref Vector2 vector)
        {
            _offsetValue.Z = vector.X;
            _offsetValue.W = vector.Y;
            _offset.SetValue(_offsetValue);
        }
        
        public void SetPathOffset(ref Vector2 vector)
        {
            _offsetValue.X = vector.X;
            _offsetValue.Y = vector.Y;
            _offset.SetValue(_offsetValue);
        }

        public void ClearOffsets()
        {
            _offsetValue = Vector4.Zero;
            _offset.SetValue(_offsetValue);
        }

        public void Dispose()
        {
            _effect.Dispose();
        }
    }
}
