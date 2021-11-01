using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaVG.Rendering.Tesselation
{
    internal struct StencilRadialVertex
    {
        private Vector4 position;
        private Vector2 texcoords;
        
        internal void Set(float x, float y, float dx, float dy, Vector2 tc)
        {
            this.position.X = x;
            this.position.Y = y;
            this.position.Z = dx;
            this.position.W = dy;
            this.texcoords = tc;
        }

        internal readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.Position, 0),
            new VertexElement(4 * sizeof(float), VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
        );        

    }
}
