using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaFlash.Actions;
using XnaFlash.Swf.Tags;

namespace XnaFlash.Content
{
    public class SpriteFrame
    {
        public ActionBlock[] Actions { get; private set; }
        public ushort[] RemovedObjects { get; private set; }
        public PlaceObject2Tag[] ModifiedObjects { get; private set; }

        internal SpriteFrame(ActionBlock[] actions, List<ushort> removed, List<PlaceObject2Tag> modified)
        {
            removed.Sort();
            modified.Sort((a, b) => a.Depth.CompareTo(b.Depth));

            Actions = actions;
            RemovedObjects = removed.ToArray();
            ModifiedObjects = modified.ToArray();

            removed.Clear();
            modified.Clear();
        }
    }
}
