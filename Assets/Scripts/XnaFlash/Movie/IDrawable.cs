using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaFlash.Movie;
using XnaFlash.Swf.Tags;
using XnaVG;

namespace XnaFlash.Movie
{
    public interface IDrawable
    {
        void SetParent(StageObject parent);
        void Draw(IVGRenderContext<DisplayState> target);
    }
}
