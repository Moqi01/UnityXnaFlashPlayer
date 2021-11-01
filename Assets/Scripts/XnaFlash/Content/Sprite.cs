using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaFlash.Actions;
using XnaFlash.Movie;
using XnaFlash.Swf;
using XnaFlash.Swf.Tags;

namespace XnaFlash.Content
{
    public class Sprite : ICharacter
    {
        protected Dictionary<string, ushort> _frameLabels = new Dictionary<string, ushort>();

        public ushort ID { get; private set; }
        internal SpriteFrame[] Frames { get; private set; }
        public CharacterType Type { get { return CharacterType.Sprite; } }
        public virtual Rectangle? Bounds { get { return null; } }
        public ushort FrameCount { get { return (ushort)Frames.Length; } }
        public ActionBlock InitAction { get; set; }
        
        internal Sprite(IEnumerable<ISwfTag> tags, ushort id, ushort frames, ISystemServices services)
        {
            ushort frame = 0;         
            var removed = new List<ushort>();
            var modified = new List<PlaceObject2Tag>();
            var actions = new List<ActionBlock>();

            ID = id;
            Frames = new SpriteFrame[frames];
            foreach (var tag in tags)
            {
                if (tag is ShowFrameTag)
                {
                    Frames[frame++] = new SpriteFrame(actions.ToArray(), removed, modified);
                    actions.Clear();
                }
                else if (tag is FrameLabelTag)
                    _frameLabels.Add((tag as FrameLabelTag).Label, frame);
                else if (tag is PlaceObjectTag)
                    modified.Add(new PlaceObject2Tag(tag as PlaceObjectTag));
                else if (tag is PlaceObject2Tag)
                    modified.Add(tag as PlaceObject2Tag);
                else if (tag is RemoveObjectTag)
                    removed.Add((tag as RemoveObjectTag).Depth);
                else if (tag is RemoveObject2Tag)
                    removed.Add((tag as RemoveObject2Tag).Depth);
                else if (tag is DoActionTag)
                    actions.Add((tag as DoActionTag).Actions);
                else
                    // TODO: Implement remaining valid tags
                    // DoABC, StartSound, SoundStreamHead, SoundStreamHead2, SoundStreamBlock, PlaceObject3  
                    UnhandledTag(tag, services);              
            }
        }

        public ushort? GetFrameByLabel(string label)
        {
            ushort frame;
            if (_frameLabels.TryGetValue(label, out frame))
                return frame;
            if (ushort.TryParse(label, out frame))
                return frame;
            return null;
        }

        protected virtual void UnhandledTag(ISwfTag tag, ISystemServices services) 
        {
            services.Log("Unhandled tag '{0}' inside {1}!", tag.ToString(), GetType().Name);
        }

        Movie.IDrawable ICharacter.MakeInstance(DisplayObject container, RootMovieClip root)
        {
            return new Movie.MovieClip(root, this, container);
        }

        public virtual void Dispose() { }
    }
}
