using System.Collections.Generic;
using XnaFlash.Actions;
using XnaFlash.Swf.Structures;
using XnaVG;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(7)]
    public class DefineButtonTag : ISwfDefinitionTag
    {
        public Content.CharacterType Type { get { return Content.CharacterType.Button; } }
        public ushort CharacterID { get; private set; }
        public ButtonRecord[] Parts { get; private set; }
        public ActionBlock Actions { get; private set; }

        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            CharacterID = stream.ReadUShort();

            var type = GetType();
            var parts = new List<ButtonRecord>();
            bool ok;
            while (true)
            {
                var part = new ButtonRecord(stream, type, out ok);
                if (!ok) break;
                parts.Add(part);
            }
            Parts = parts.ToArray();
            Actions = ActionRecord.ReadActions(stream, null);
        }

        #endregion
    }

    [SwfTag(34)]
    public class DefineButton2Tag : ISwfDefinitionTag
    {
        public Content.CharacterType Type { get { return Content.CharacterType.Button; } }
        public bool TrackAsMenu { get; private set; }
        public ushort CharacterID { get; private set; }        
        public ButtonRecord[] Parts { get; private set; }
        public ButtonCondAction[] Actions { get; private set; }

        public DefineButton2Tag() { }
        public DefineButton2Tag(DefineButtonTag tag)
        {
            TrackAsMenu = false;
            CharacterID = tag.CharacterID;
            Parts = tag.Parts;
            Actions = new ButtonCondAction[]
            {
                new ButtonCondAction(tag.Actions)
            };
        }

        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            long end = length + stream.TagPosition;

            CharacterID = stream.ReadUShort();
            TrackAsMenu = (stream.ReadByte() & 0x01) != 0;
            ushort actionOffset = stream.ReadUShort();

            var type = GetType();
            var parts = new List<ButtonRecord>();
            bool ok;
            while (true)
            {
                var part = new ButtonRecord(stream, type, out ok);
                if (!ok) break;
                parts.Add(part);
            }
            Parts = parts.ToArray();

            var actions = new List<ButtonCondAction>();
            while (stream.TagPosition < end)
                actions.Add(new ButtonCondAction(stream));                
            Actions = actions.ToArray();
        }

        #endregion
    }

    [SwfTag(23)]
    public class DefineButtonCxFormTag : ISwfControlTag
    {
        public ushort CharacterID { get; private set; }
        public VGCxForm CxForm { get; private set; }

        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            CharacterID = stream.ReadUShort();
            CxForm = stream.ReadCxForm(false);
        }

        #endregion
    }
}
