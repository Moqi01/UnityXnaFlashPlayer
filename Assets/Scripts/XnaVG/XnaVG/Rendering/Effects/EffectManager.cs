using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XnaVG.Rendering.Effects
{
    internal sealed class EffectManager : IDisposable
    {
        private ContentManager _content;

        internal CoverEffect Cover { get; private set; }        
        internal StencilFillEffect StencilFill { get; private set; }
        internal StencilTextEffect StencilText { get; private set; }
        internal StencilSolidEffect StencilSolid { get; private set; }
        internal StencilStrokeEffect StencilStroke { get; private set; }

        internal EffectManager(IServiceProvider provider)
        {
            _content = new ContentManager(provider, "XnaVG");
            Cover = new CoverEffect(_content.Load<Effect>("Cover"));
            StencilFill = new StencilFillEffect(_content.Load<Effect>("Stencil_Fill"));
            StencilText = new StencilTextEffect(_content.Load<Effect>("Stencil_Text"));
            StencilSolid = new StencilSolidEffect(_content.Load<Effect>("Stencil_Solid"));
            StencilStroke = new StencilStrokeEffect(_content.Load<Effect>("Stencil_Stroke"));
        }

        public void Dispose()
        {
            if (_content != null)
            {
                Cover.Dispose();
                StencilFill.Dispose();
                StencilText.Dispose();
                StencilSolid.Dispose();
                StencilStroke.Dispose();

                Cover = null;
                StencilFill = null;
                StencilText = null;
                StencilSolid = null;
                StencilStroke = null;

                _content.Dispose();
                _content = null;
            }
        }
    }
}
