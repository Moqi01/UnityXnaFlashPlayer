
namespace XnaFlash.Swf.Structures
{
    public enum ClipEventFlags : uint
    {
        None = 0,

        KeyUp = 0x80000000,
        KeyDown = 0x40000000,
        MouseUp = 0x20000000,
        MouseDown = 0x10000000,
        MouseMove = 0x8000000,
        Unload = 0x4000000,
        EnterFrame = 0x2000000,
        Load = 0x1000000,
        DragOver = 0x800000,
        RollOut = 0x400000,
        RollOver = 0x200000,
        ReleaseOutside = 0x100000,
        Release = 0x80000,
        Press = 0x40000,
        Initialize = 0x20000,
        Data = 0x10000,
        Construct = 0x400,
        KeyPress = 0x200,
        DragOut = 0x100,
    }
}
