using Microsoft.Xna.Framework;
using XnaFlash.Swf.Paths;
using XnaFlash.Swf.Structures;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(83)]
    public class DefineShape4Tag : DefineShapeTag
    {
        public Rectangle EdgeBounds { get; protected set; }
        public bool UsesFillWindingRule { get; protected set; }
        public bool UsesNonScalingStrokes { get; protected set; }
        public bool UsesScalingStrokes { get; protected set; }

        #region ISwfTag Members

        public override void Load(SwfStream stream, uint length, byte version)
        {           
            CharacterID = stream.ReadUShort();
            ShapeBounds = stream.ReadRectangle();
            EdgeBounds = stream.ReadRectangle();

            byte res = stream.ReadByte();
            UsesFillWindingRule = version >= 10 && (res & 4) != 0;
            UsesNonScalingStrokes = (res & 2) != 0;
            UsesScalingStrokes = (res & 1) != 0;

            Shape = new Shape(ShapeInfo.ReadShape(stream, true, true, true, true), false);
        }

        #endregion
    }
}
