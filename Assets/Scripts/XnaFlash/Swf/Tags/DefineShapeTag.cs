using Microsoft.Xna.Framework;
using XnaFlash.Swf.Paths;
using XnaFlash.Swf.Structures;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(2)]
    public class DefineShapeTag : ISwfDefinitionTag
    {
        public Content.CharacterType Type { get { return Content.CharacterType.Shape; } }
        public ushort CharacterID { get; protected set; }
        public Rectangle ShapeBounds { get; protected set; }
        public Shape Shape { get; protected set; }

        #region ISwfTag Members

        public virtual void Load(SwfStream stream, uint length, byte version)
        {
            CharacterID = stream.ReadUShort();
            ShapeBounds = stream.ReadRectangle();
            Shape = new Shape(ShapeInfo.ReadShape(stream, false, false, true, false), false);
        }

        #endregion        
    }

    [SwfTag(22)]
    public class DefineShape2Tag : DefineShapeTag
    {
        #region ISwfTag Members

        public override void Load(SwfStream stream, uint length, byte version)
        {
            CharacterID = stream.ReadUShort();
            ShapeBounds = stream.ReadRectangle();
            Shape = new Shape(ShapeInfo.ReadShape(stream, false, false, true, true), false);
        }

        #endregion
    }

    [SwfTag(32)]
    public class DefineShape3Tag : DefineShapeTag
    {
        #region ISwfTag Members

        public override void Load(SwfStream stream, uint length, byte version)
        {
            CharacterID = stream.ReadUShort();
            ShapeBounds = stream.ReadRectangle();
            Shape = new Shape(ShapeInfo.ReadShape(stream, true, false, true, true), false);
        }

        #endregion
    }
}
