using System.Collections.Generic;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(39)]
    public class DefineSpriteTag : ISwfDefinitionTag
    {
        public Content.CharacterType Type { get { return Content.CharacterType.Sprite; } }
        public ushort CharacterID { get; private set; }
        public ushort FrameCount { get; private set; }
        public ISwfControlTag[] Tags { get; private set; }

        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            CharacterID = stream.ReadUShort();
            FrameCount = stream.ReadUShort();

            List<ISwfControlTag> tags = new List<ISwfControlTag>(256);
            ISwfTag tag;

            do
            {
                tag = stream.ReadTag();
                if (tag == null)
                    continue;

                CheckTagValidity(tag);

                if (tag is EndTag)
                    break;
                else
                    tags.Add((ISwfControlTag)tag);
            } while (true);

            Tags = tags.ToArray();
        }

        #endregion

        #region Tag Filter

        private static void CheckTagValidity(ISwfTag tag)
        {
            if (
                tag is ShowFrameTag ||
                tag is PlaceObjectTag ||
                tag is PlaceObject2Tag ||
                tag is RemoveObjectTag ||
                tag is RemoveObject2Tag ||
                tag is FrameLabelTag ||
                tag is DoActionTag ||
                //tag is StartSoundTag ||
                //tag is SoundStreamHeadTag ||
                //tag is SoundStreamHead2Tag ||
                //tag is SoundStreamBlockTag ||
                tag is EndTag)
                    return;
            throw new SwfCorruptedException("Invalid tag '" + tag.ToString() + "' found inside DefineSprite!");
        }

        #endregion
    }
}
