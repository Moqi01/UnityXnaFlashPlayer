using System.Collections.Generic;
using XnaFlash.Swf;

namespace XnaFlash.Actions.Records
{
    public class PushAction : ActionRecord
    {
        public ActionVar[] Values { get; private set; }

        protected override void Load(SwfStream stream, ushort length)
        {
            List<ActionVar> values = new List<ActionVar>();
            long end = length + stream.TagPosition;

            while (stream.TagPosition < end)
            {
                ActionVar value;
                switch (stream.ReadByte())
                {
                    case 0: value = stream.ReadString(); break;
                    case 1: value = stream.ReadSingle(); break;
                    case 2: value = new ActionVar((string)null); break;
                    case 3: value = new ActionVar(); break;
                    case 4: value = stream.ReadByte(); break;
                    case 5: value = stream.ReadByte() != 0; break;
                    case 6: value = stream.ReadDouble(); break;
                    case 7: value = stream.ReadUInt(); break;
                    case 8: value = new ActionVar.IndexActionVar(stream.ReadByte()); break;
                    case 9: value = new ActionVar.IndexActionVar(stream.ReadUShort()); break;
                    default:
                        throw new SwfCorruptedException("Invalid push action value type!");
                }
                values.Add(value);
            }

            Values = values.ToArray();
        }
    }
}