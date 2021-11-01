using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaFlash.Actions;
using XnaFlash.Actions.Objects;
using XnaFlash.Content;
using XnaFlash.Swf.Structures;
using XnaFlash.Swf.Tags;
using XnaVG;

namespace XnaFlash.Movie
{
    public class MovieClip : StageObject
    {
        protected Sprite _sprite;
        protected ushort _frame = 0;
        protected string _dropTarget = null;

        public bool IsPlaying { get; protected set; } 
        
        internal MovieClip(RootMovieClip root, Sprite sprite, DisplayObject container)
            : base(root, container)
        {
            _sprite = sprite;
            IsPlaying = true;
            DontLoop = false;
        }

        public void NextFrame()
        {
            GoTo((ushort)(_frame + 1));
        }
        public void PrevFrame()
        {
            if (_frame > 0)
                GoTo((ushort)(_frame - 1));
        }
        public void Play()
        {
            IsPlaying = true;
        }
        public void Stop()
        {
            IsPlaying = false;
        }
        public void GoTo(ushort frame)
        {
            frame = Math.Max((ushort)1, Math.Min(frame, TotalFrames));
            if (frame >= _frame)
            {
                for (; _frame < frame; _frame++)
                    _displayList.ProcessSpriteFrame(_sprite.Frames[_frame], this);
            }
            else
            {
                _displayList.Clear();
                _frame = 0;
                GoTo(frame);
            }

            if (_sprite.Frames[_frame - 1].Actions != null)
            {
                foreach (var a in _sprite.Frames[_frame - 1].Actions)
                    a.RunSafe(Context.MakeLocalScope(4, 1));
            }
        }

        public override bool OnMouseMove()
        {
            if (!Visible) return false;

            Root.ButtonStack.PushCombineLeft(_container.Matrix);
            bool val = _displayList.OnMouseMove();
            Root.ButtonStack.Pop();
            return val;
        }
        public override void OnNextFrame()
        {
            if (IsPlaying)
            {
                if (_frame >= TotalFrames)
                {
                    if (DontLoop)
                        IsPlaying = false;
                    else
                        GoTo(1);
                }
                else
                    NextFrame();
            }

            base.OnNextFrame();
        }

        public override void Load()
        {            
            NextFrame();
            base.Load();
        }       
        public ushort GetFrameByLabel(string frameLabel)
        {
            return _sprite.GetFrameByLabel(frameLabel) ?? _frame;
        }
        public ActionBlock[] GetFrameAction(ushort frame)
        {
            if (frame >= _sprite.Frames.Length) return null;
            return _sprite.Frames[frame].Actions;
        }
        public void StartDrag(bool lockCenter, Rectangle? constraint)
        {
            Root.StartDrag(this, lockCenter, constraint);
        }        

        #region Native properties

        public bool DontLoop
        {
            get;
            set;
        }
        public virtual ushort CurrentFrame
        {
            get { return _frame; }
        }
        public virtual ushort TotalFrames
        {
            get { return _sprite.FrameCount; }
        }
        public virtual ushort FramesLoaded
        {
            get { return TotalFrames; }
        }
        public virtual string DropTarget
        {
            get { return _dropTarget; }
        }
        
        public override ActionVar this[Properties prop]
        {
            get
            {
                switch (prop)
                {
                    case Properties._currentframe: return CurrentFrame;
                    case Properties._totalframes: return TotalFrames;
                    case Properties._framesloaded: return FramesLoaded;
                    case Properties._droptarget: return DropTarget;
                    default:
                        return base[prop];
                }
            }
            set
            {
                base[prop] = value;
            }
        }

        #endregion
    }
}
