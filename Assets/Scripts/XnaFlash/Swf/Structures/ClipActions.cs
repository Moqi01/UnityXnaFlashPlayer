using System.Collections.Generic;


namespace XnaFlash.Swf.Structures
{
    public class ClipActions
    {
        public ClipEventFlags AllEvents { get; private set; }
        public ClipActionRecord[] Records { get; private set; }

        public ClipActions(SwfStream stream)
        {
            stream.ReadUShort();
            AllEvents = stream.ReadClipEventFlags();

            var records = new List<ClipActionRecord>();
            ClipActionRecord current;

            while(true)
            {
                current = new ClipActionRecord(stream);
                if (current.EventFlags == ClipEventFlags.None)
                    break;
                records.Add(current);
            }

            Records = records.ToArray();
        }
    }
}
