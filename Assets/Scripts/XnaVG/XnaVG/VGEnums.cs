using System;

namespace XnaVG
{    
    public enum VGAntialiasing : byte
    {
        None = 0,   // 0x MSAA Fill
        Faster = 4, // 4x MSAA Fill
        Better = 8, // 8x MSAA Fill
        Best = 16   // 16x MSAA Fill
    }

    public enum VGFillRule : byte
    {
        EvenOdd,
        NonZero
    }

    public enum VGLineCap
    {
        Butt = 0,
        Triangle = 1,
        Square = 2,
        Round = 3
    }

    public enum VGLineJoin
    {
        None = 0,
        Bevel = 1,
        Miter = 2,
        Round = 3
    }

    public enum VGBlendMode : int
    {
        Src = 0,
        SrcOver = 1,        
        DstOver = 2,
        SrcIn = 3,
        DstIn = 4,
        SrcOut = 5,
        DstOut = 6,
        SrcAtop = 7,
        DstAtop = 8,
        Multiply = 9,
        Screen = 10,
        Darken = 11,
        Lighten = 12,
        Additive = 13,
        Xor = 14,
        LinearDodge = 15,
        LinearBurn = 16,
        Clear = 17
    }

    [Flags()]
    public enum VGPaintMode
    {
        None = 0,
        Fill = 1,
        Stroke = 2,
        Both = 3
    }

    public enum VGPaintType : byte
    {
        Color,
        LinearGradient,
        RadialGradient,
        Pattern,
        PatternPremultiplied
    }

    [Flags()]
    public enum VGStencilMasks : byte
    {
        None = 0,
        Mask1 = 0x02,
        Mask2 = 0x04,
        Mask3 = 0x08,
        Mask4 = 0x10,
        Mask5 = 0x20,
        Mask6 = 0x40,
        Mask7 = 0x80,
        All = 0xFE
    }

    public enum VGFontMode : byte
    {
        CPU,
        GPU_PreferRAM,
        GPU_PreferVRAM
    }
}
