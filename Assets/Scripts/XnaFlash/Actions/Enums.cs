using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaFlash.Actions
{
    [Flags()]
    public enum GetURLFlags : byte
    {
        None = 0,

        MethodPost = 0x80,
        MethodGet = 0x40,

        LoadTarget = 0x02,
        LoadVariables = 0x01
    }

    [Flags()]
    public enum FuncFlags : ushort
    {
        None = 0,
        PreloadParent = 0x0080,
        PreloadRoot = 0x0040,
        SupressSuper = 0x0020,
        PreloadSuper = 0x0010,
        SupressArguments = 0x0008,
        PreloadArguments = 0x0004,
        SupressThis = 0x0002,
        PreloadThis = 0x0001,
        PreloadGlobal = 0x0100
    }

    public enum Properties : byte
    {
        _x = 0,
        _y = 1,
        _xscale = 2,
        _yscale = 3,
        _currentframe = 4,
        _totalframes = 5,
        _alpha = 6,
        _visible = 7,
        _width = 8,
        _height = 9,
        _rotation = 10,
        _target = 11,
        _framesloaded = 12,
        _name = 13,
        _droptarget = 14,
        _url = 15,
        _highquality = 16,
        _focusrect = 17,
        _soundbuftime = 18,
        _quality = 19,
        _mousex = 20,
        _mousey = 21,
    }
    
    public enum Event
    {
        onData = 0,
        onDragOut = 1,
        onDragOver = 2,
        onEnterFrame = 3,
        onKeyDown = 4,
        onKeyUp = 5,
        onKillFocus = 6,
        onLoad = 7,
        onMouseDown = 8,
        onMouseMove = 9,
        onMouseUp = 10,
        onPress = 11,
        onRelease = 12,
        onReleaseOutside = 13,
        onRollOut = 14,
        onRollOver = 15,
        onSetFocus = 16,
        onUnload = 17,
    }
}
