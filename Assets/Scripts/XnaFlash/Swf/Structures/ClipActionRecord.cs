using System.Collections.Generic;
using System.Linq;
using XnaFlash.Actions;

namespace XnaFlash.Swf.Structures
{
    public class ClipActionRecord
    {
        public ClipEventFlags EventFlags { get; private set; }
        public KeyCode KeyCode { get; private set; }
        public ActionBlock Actions { get; private set; }
        public IEnumerable<Event> Events
        {
            get
            {
                if ((EventFlags & ClipEventFlags.Data) != 0) yield return Event.onData;
                if ((EventFlags & ClipEventFlags.DragOut) != 0) yield return Event.onDragOut;
                if ((EventFlags & ClipEventFlags.DragOver) != 0) yield return Event.onDragOver;
                if ((EventFlags & ClipEventFlags.EnterFrame) != 0) yield return Event.onEnterFrame;
                if ((EventFlags & ClipEventFlags.KeyDown) != 0) yield return Event.onKeyDown;
                if ((EventFlags & ClipEventFlags.KeyUp) != 0) yield return Event.onKeyUp;
                if ((EventFlags & ClipEventFlags.Load) != 0) yield return Event.onLoad;
                if ((EventFlags & ClipEventFlags.MouseDown) != 0) yield return Event.onMouseDown;
                if ((EventFlags & ClipEventFlags.MouseMove) != 0) yield return Event.onMouseMove;
                if ((EventFlags & ClipEventFlags.MouseUp) != 0) yield return Event.onMouseUp;
                if ((EventFlags & ClipEventFlags.Press) != 0) yield return Event.onPress;
                if ((EventFlags & ClipEventFlags.Release) != 0) yield return Event.onRelease;
                if ((EventFlags & ClipEventFlags.ReleaseOutside) != 0) yield return Event.onReleaseOutside;
                if ((EventFlags & ClipEventFlags.RollOut) != 0) yield return Event.onRollOut;
                if ((EventFlags & ClipEventFlags.RollOver) != 0) yield return Event.onRollOver;
                if ((EventFlags & ClipEventFlags.Unload) != 0) yield return Event.onUnload;
                
                // TODO: This obscure actions
                // if ((EventFlags & ClipEventFlags.Construct) != 0) yield return Event.WUT???;
                // if ((EventFlags & ClipEventFlags.Initialize) != 0) yield return Event.WUT???;
            }
        }

        public ClipActionRecord(SwfStream stream)
        {
            EventFlags = stream.ReadClipEventFlags();
            if (EventFlags == ClipEventFlags.None) return;
            
            uint size = stream.ReadUInt();
            if ((EventFlags & ClipEventFlags.KeyPress) != 0) KeyCode = new KeyCode(stream.ReadByte());

            Actions = ActionRecord.ReadActions(stream, size);
        }
    }
}