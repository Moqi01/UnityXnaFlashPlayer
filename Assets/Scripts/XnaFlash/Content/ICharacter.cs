using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaFlash.Movie;

namespace XnaFlash.Content
{
    public interface ICharacter : IDisposable
    {
        ushort ID { get; }
        CharacterType Type { get; }
        Rectangle? Bounds { get; }
        Movie.IDrawable MakeInstance(DisplayObject container, RootMovieClip root);
    }
}
