using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaFlash.Movie;

namespace XnaFlash.Content
{
    public class BinaryData : ICharacter
    {
        public ushort ID { get; private set; }
        public CharacterType Type { get { return CharacterType.BinaryData; } }
        public Rectangle? Bounds { get { return null; } }
        public byte[] Data { get; private set; }

        public BinaryData(ushort id, byte[] data)
        {
            ID = id;
            Data = data;
        }

        public Movie.IDrawable MakeInstance(Movie.DisplayObject container, RootMovieClip root) { return null; }

        public void Dispose() { }        
    }
}
