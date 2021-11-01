using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaVGPipeline
{
    public static class Utils
    {
        //public static Vector2 ToVec2(this System.Windows.Point point)
        //{
        //    return new Vector2((float)point.X, (float)-point.Y);
        //}

        public static Vector2 ToVec2(this Point point)
        {
            return new Vector2((float)point.X, (float)-point.Y);
        }
    }
}
