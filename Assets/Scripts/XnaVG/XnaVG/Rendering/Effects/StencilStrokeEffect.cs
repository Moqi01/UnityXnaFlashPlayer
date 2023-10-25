using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaVG.Rendering.Effects
{
    internal sealed class StencilStrokeEffect : IDisposable
    {
        private Effect _effect;
        private EffectPass _passSolid;
        private EffectPass _passRadial;
        private EffectPass _passSolidNS;
        private EffectPass _passRadialNS;
        private EffectPass _currentPass;
        private EffectParameter _projection;
        private EffectParameter _transformation;
        private EffectParameter _offset;
        private EffectParameter _thickness;
        private EffectTechnique _scalingTechnique;
        private EffectTechnique _nonscalingTechnique;

        public StencilStrokeEffect(Effect effect)
        {
            _effect = effect;
            _scalingTechnique = effect.Techniques["StencilStroke_Scaling"];
            _nonscalingTechnique = effect.Techniques["StencilStroke_Nonscaling"];
            //if(_scalingTechnique)
            _passSolid = _scalingTechnique?.Passes["Solid"];
            _passRadial = _scalingTechnique?.Passes["Radial"];
            _passSolidNS = _nonscalingTechnique?.Passes["Solid"];
            _passRadialNS = _nonscalingTechnique?.Passes["Radial"];
            _projection = effect.Parameters["Projection"];
            _transformation = effect.Parameters["Transformation"];
            _offset = effect.Parameters["Offset"];            
            _thickness = effect.Parameters["Thickness"];  
        }

        public void SetSolid(bool isNonscaling, VGMatrix projection, VGMatrix transformation, float thickness)
        {
            _projection.SetValue(projection);
            _transformation.SetValue(transformation);
            _thickness.SetValue(thickness);
            _offset.SetValue(Vector2.Zero);
            //DrawGL.ins.SetMatrices(transformation, projection);

            if (isNonscaling)
            {
                _effect.CurrentTechnique = _nonscalingTechnique;
                _currentPass = _passSolidNS;
            }
            else
            {
                _effect.CurrentTechnique = _scalingTechnique;
                _currentPass = _passSolid;
            }            
        }

        public void SetRadial(bool isNonscaling, VGMatrix projection, VGMatrix transformation, float thickness)
        {
            _projection.SetValue(projection);
            _transformation.SetValue(transformation);
            _thickness.SetValue(thickness);
            _offset.SetValue(Vector2.Zero);
            //DrawGL.ins.SetMatrices(transformation, projection);

            if (isNonscaling)
            {
                _effect.CurrentTechnique = _nonscalingTechnique;
                _currentPass = _passRadialNS;
            }
            else
            {
                _effect.CurrentTechnique = _scalingTechnique;
                _currentPass = _passRadial;
            }   
        }

        public void SetOffset(ref Vector2 vector)
        {
            _offset?.SetValue(vector);
        }

        public void Apply()
        {
            _currentPass?.Apply();
        }

        public void Dispose()
        {
            _effect?.Dispose();
        }
    }
}
