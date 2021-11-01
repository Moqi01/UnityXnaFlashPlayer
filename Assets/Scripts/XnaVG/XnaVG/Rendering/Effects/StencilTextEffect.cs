using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaVG.Rendering.Effects
{
    internal sealed class StencilTextEffect : IDisposable
    {
        private Effect _effect;
        private EffectPass _pass;
        private EffectParameter _projection;
        private EffectParameter _transformation;
        private EffectParameter _offset;
        private EffectParameter _offsets;
        private EffectParameter _vertices;        
        private EffectParameter _numVertices;        
        private EffectParameter _numOffsets;

        public StencilTextEffect(Effect effect)
        {
            _effect = effect;
            _pass = _effect.CurrentTechnique.Passes[0];
            _projection = effect.Parameters["Projection"];
            _transformation = effect.Parameters["Transformation"];
            _offset = effect.Parameters["Offset"];
            _offsets = effect.Parameters["GlyphOffsets"];
            _vertices = effect.Parameters["FontVertices"];
            _numVertices = effect.Parameters["VertexCount"];
            _numOffsets = effect.Parameters["GlyphCount"];
        }

        public void Apply(VGMatrix projection, VGMatrix transformation, Vector2 offset, Texture2D vertices, Texture2D offsets)
        {
            _projection.SetValue(projection);
            _transformation.SetValue(transformation);
            _offset.SetValue(offset);
            _offsets.SetValue(offsets);
            _vertices.SetValue(vertices);
            _numOffsets.SetValue(new Vector2(offsets.Width, offsets.Height));
            _numVertices.SetValue(new Vector2(vertices.Width, vertices.Height));
            _pass.Apply();
        }

        public void Dispose()
        {
            _effect.Dispose();
        }
    }
}
