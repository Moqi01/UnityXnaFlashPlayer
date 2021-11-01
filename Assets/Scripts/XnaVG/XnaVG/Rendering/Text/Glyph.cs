using System;
using Microsoft.Xna.Framework;

namespace XnaVG.Rendering.Text
{
    internal abstract class Glyph : IDisposable, IRenderable
    {
        public Vector2 Escape { get; private set; }
        public abstract bool IsEmpty { get; }
        bool IRenderable.HasFill { get { return !IsEmpty; } }
        bool IRenderable.HasSolidStroke { get { return false; } }
        bool IRenderable.HasRadialStroke { get { return false; } }
        virtual public Vector4 Extents { get { throw new InvalidOperationException("Glyph does not have any extents! Use font maximum extents instead."); } }

        internal Glyph(Vector2 escape)
        {
            Escape = escape;
        }

        public abstract void BeforeRender();
        public abstract void Render();
        public abstract void Dispose();

        void IRenderable.BeforeRenderFill()
        {
            BeforeRender();
        }
        void IRenderable.RenderFill()
        {
            Render();
        }
        void IRenderable.RenderSolidStroke() { }
        void IRenderable.RenderRadialStroke() { }
        void IRenderable.BeforeRenderSolidStroke() { }
        void IRenderable.BeforeRenderRadialStroke() { }
    }
}
