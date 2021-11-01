using Microsoft.Xna.Framework;

namespace XnaVG
{
    public interface IRenderable
    {
        Vector4 Extents { get; }
        bool HasFill { get; }
        bool HasSolidStroke { get; }
        bool HasRadialStroke { get; }

        void Render();
        void BeforeRenderFill();
        void BeforeRenderSolidStroke();
        void BeforeRenderRadialStroke();
        void RenderFill();
        void RenderSolidStroke();
        void RenderRadialStroke();
    }
}
