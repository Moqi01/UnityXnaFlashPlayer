using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaFlash.Swf.Structures;

namespace XnaFlash.Movie
{
    public interface IInstanceable
    {
        bool OnMouseMove();
        void OnNextFrame();
        void SetName(string name);
        void SetClipActions(ClipActions actions);        
        void Load();
        void Unload();

        string GetRootedPath(bool slashes);
    }
}
