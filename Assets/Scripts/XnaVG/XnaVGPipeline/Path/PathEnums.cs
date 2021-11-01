using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaVGPipeline.Path
{
    internal enum PathOperation : byte
    {
        End = 0,
        Close,
        MoveTo,
        LineTo,
        QuadraticTo,
        QuadraticSmoothTo,
        CubicTo,
        CubicSmoothTo,    
    }
}
