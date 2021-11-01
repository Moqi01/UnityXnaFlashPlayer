using System;
using Microsoft.Xna.Framework.Graphics;
using XnaVG;

namespace XnaFlash
{
    public interface ISystemServices
    {
        IVGDevice VectorDevice { get; }
        GraphicsDevice GraphicsDevice { get; }

        void Log(string message, params object[] objects);
        VGFont LoadFont(string name);
    }
}
