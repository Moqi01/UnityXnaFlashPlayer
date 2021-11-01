using XnaVG;
using XnaFlash.Swf.Structures;

namespace XnaFlash.Content
{
    public class ButtonPart
    {
        public ICharacter Character { get; private set; }
        public ushort Depth { get; private set; }
        public VGMatrix Matrix { get; private set; }
        public VGCxForm CxForm { get; private set; }
        public bool Up { get; private set; }
        public bool Over { get; private set; }
        public bool Down { get; private set; }

        public ButtonPart(ButtonRecord rec, FlashDocument doc)
        {
            Character = doc[rec.CharacterID];
            Depth = rec.CharacterDepth;
            Matrix = rec.CharacterMatrix;
            CxForm = rec.CxForm;
            Up = rec.Up;
            Over = rec.Over;
            Down = rec.Down;
        }
    }
}
