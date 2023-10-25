using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaVG.Paints;

namespace XnaVG.Rendering.Effects
{
    internal sealed class CoverEffect : IDisposable
    {
        private int _passIndex;
        private Effect _effect;
        private EffectTechnique[] _techniques;

        private EffectParameter _projection;
        private EffectParameter _transformation;
        private EffectParameter _paintTransformation;
        private EffectParameter _focalPoint;
        private EffectParameter _offset;
        private EffectParameter _color;
        private EffectParameter _addTerm;
        private EffectParameter _mulTerm;
        private EffectParameter _maskChannels;

        public CoverEffect(Effect effect)
        {
            _effect = effect;

            _projection = effect.Parameters["Projection"];
            _transformation = effect.Parameters["Transformation"];
            _paintTransformation = effect.Parameters["PaintTransformation"];
            _focalPoint = effect.Parameters["FocalPoint"];
            _offset = effect.Parameters["Offset"];
            _color = effect.Parameters["Color"];
            _addTerm = effect.Parameters["AddTerm"];
            _mulTerm = effect.Parameters["MulTerm"];
            _maskChannels = effect.Parameters["MaskChannels"];

            //_techniques = new EffectTechnique[_effect.Techniques.Count];
            _techniques = new EffectTechnique[7];
            _techniques[(byte)VGPaintType.Color] = effect.Techniques["Fill_" + VGPaintType.Color.ToString()];
            _techniques[(byte)VGPaintType.RadialGradient] = effect.Techniques["Fill_" + VGPaintType.RadialGradient.ToString()];
            _techniques[(byte)VGPaintType.LinearGradient] = effect.Techniques["Fill_" + VGPaintType.LinearGradient.ToString()];
          
            _techniques[(byte)VGPaintType.Pattern] = effect.Techniques["Fill_" + VGPaintType.Pattern.ToString()];
            _techniques[(byte)VGPaintType.PatternPremultiplied] = effect.Techniques["Fill_" + VGPaintType.PatternPremultiplied.ToString()];
        }

        public void SetParameters(VGMatrix projection, VGMatrix transformation, VGCxForm cxForm)
        {
            _projection.SetValue(projection);
            _transformation.SetValue(transformation);            
            _addTerm.SetValue(cxForm.AddTerm);
            _mulTerm.SetValue(cxForm.MulTerm);
            _offset.SetValue(Vector2.Zero);
            DrawGL.ins.SetMatrices(transformation,projection);
        }

        public void SetMask(VGImage mask, Vector4 channels)
        {
            if (mask == null)
            {
                _passIndex &= ~2;
                _effect.GraphicsDevice.Textures[0] = null;
            }
            else
            {
                _passIndex |= 2;
                _effect.GraphicsDevice.Textures[0] = mask.Texture;
                _effect.GraphicsDevice.SamplerStates[0] = mask.GetSamplerState();
                _maskChannels.SetValue(channels);
            }
        }

        public void SetImagePaint(VGImage image, VGMatrix paintTransformation)
        {            
            if (image.LinearColorspace)
                _passIndex |= 1;
            else
                _passIndex &= ~1;

            _effect.CurrentTechnique = _techniques[(int)(image.Premultiplied ? VGPaintType.PatternPremultiplied : VGPaintType.Pattern)];
            _paintTransformation.SetValue(paintTransformation);
            _focalPoint.SetValue(Vector2.Zero);
            _effect.GraphicsDevice.Textures[1] = image.Texture;
            _effect.GraphicsDevice.SamplerStates[1] = image.GetSamplerState();            
        }
        public void SetPaint(VGPaint paint, VGMatrix paintTransformation)
        {
            _passIndex &= ~1;
            _paintTransformation.SetValue(paintTransformation);
            _focalPoint.SetValue(new Vector2(paint.Type == VGPaintType.RadialGradient ? (paint as VGRadialPaint).FocalPoint : 0, 0));
            _effect.CurrentTechnique = _techniques[(int)paint.Type];

            switch (paint.Type)
            {
                case VGPaintType.Color:
                    Vector4 color = (paint as VGColorPaint).Color.ToVector4();
                    _color.SetValue(color);
                    DrawGL.ins.SetColor ( new UnityEngine. Color(color.X,color.Y,color.Z,color.W));
                    break;
                case VGPaintType.Pattern:
                case VGPaintType.PatternPremultiplied:
                    var pattern = (paint as VGPatternPaint).Pattern;
                    _effect.GraphicsDevice.Textures[1] = pattern.Texture;
                    _effect.GraphicsDevice.SamplerStates[1] = pattern.GetSamplerState();
                    DrawGL.ins.SetColor(UnityEngine.Color.white);
                    if (pattern.LinearColorspace) _passIndex |= 1;
                    break;
                case VGPaintType.LinearGradient:
                case VGPaintType.RadialGradient:
                    var gradient = paint as VGGradientPaint;
                    _effect.GraphicsDevice.Textures[1] = gradient.Gradient;
                    _effect.GraphicsDevice.SamplerStates[1] = gradient.GetSamplerState();
                    DrawGL.ins.SetColor( UnityEngine.Color.white);
                    if (gradient.LinearColorspace) _passIndex |= 1;
                    break;
                default:
                    throw new ArgumentException("Invalid paint type!", "paint");
            }                        
        }

        public void SetOffset(ref Vector2 offset)
        {
            _offset?.SetValue(offset);
        }

        public void Apply()
        {
            _effect?.CurrentTechnique?.Passes[_passIndex].Apply();
        }

        public void Dispose()
        {
            _effect?.Dispose();
        }
    }
}
