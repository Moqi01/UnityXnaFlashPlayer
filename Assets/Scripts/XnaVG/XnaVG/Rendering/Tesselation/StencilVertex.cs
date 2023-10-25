using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaVG.Rendering.Tesselation
{
    public struct StencilVertex
    {
        private Vector4 position;

        public Vector2 Position { get { return new Vector2(position.X, position.Y); } }
        public Vector2 Control { get { return new Vector2(position.Z, position.W); } }

        internal void Set(float x, float y, float[] coef)
        {
            this.position.X = x;
            this.position.Y = y;
            this.position.Z = coef[0];
            this.position.W = coef[1];
        }

        internal void Set(float x, float y, float dx, float dy)
        {
            this.position.X = x;
            this.position.Y = y;
            this.position.Z = dx;
            this.position.W = dy;
        }

        internal void Serialize(BinaryWriter writer)
        {
            writer.Write(position.X);
            writer.Write(position.Y);
            writer.Write(position.Z);
            writer.Write(position.W);
        }

        internal static StencilVertex Deserialize(BinaryReader reader)
        {
            return new StencilVertex
            {
                position = new Vector4
                {
                    X = reader.ReadSingle(),
                    Y = reader.ReadSingle(),
                    Z = reader.ReadSingle(),
                    W = reader.ReadSingle()
                }
            };
        }

        internal readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.Position, 0)
        );               
    }
}
