using Microsoft.Xna.Framework;

namespace XnaVG.Rendering.Text
{
    internal sealed class PathGlyph : Glyph
    {
        public VGPreparedPath Path { get; private set; }
        public override Vector4 Extents { get { return Path.Extents; } }
        public override bool IsEmpty { get { return Path != null && !Path.HasFill; } }

        public PathGlyph(VGPreparedPath path, Vector2 escape)
            : base(escape)
        {
            Path = path;
        }

        public override void BeforeRender()
        {
            Path.Fill.Activate();
        }

        public override void Render()
        {
            Path.Fill.Render();
        }

        public override void Dispose()
        {
            if (Path != null)
            {
                Path.Dispose();
                Path = null;
            }
        }        
    }
}
