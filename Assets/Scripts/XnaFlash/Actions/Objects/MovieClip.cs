using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaFlash.Actions.Objects.Old
{
    public abstract class MovieClip : ActionObject
    {
        public abstract IRootMovieClip RootClip { get; }
        public abstract MovieClip ParentClip { get;  }
        public abstract string Path { get; }        

        public abstract void NextFrame();
        public abstract void PrevFrame();
        public abstract void Play();
        public abstract void Stop();
        public abstract void GoTo(ushort frame);

        public abstract MovieClip GetInstanceByPath(string path);
        public abstract ushort GetFrameByLabel(string frame);
        public abstract ActionBlock[] GetFrameAction(ushort frame);

        public abstract void CloneTo(ushort depth, string name);
        public abstract void Remove();
        public abstract void StartDrag(bool lockCenter, Rectangle? constraint);

        protected override string AsString() 
        {
            return "[" + Name + "]";
        }

        #region Native properties

        public abstract float X { get; set; }
        public abstract float Y { get; set; }
        public abstract float XScale { get; set; }
        public abstract float YScale { get; set; }
        public abstract ushort CurrentFrame { get; }
        public abstract ushort TotalFrames { get; }
        public abstract float Alpha { get; set; }
        public abstract bool Visible { get; set; }
        public abstract float Width { get; set; }
        public abstract float Height { get; set; }
        public abstract float Rotation { get; set; }
        public abstract string Target { get; }
        public abstract ushort FramesLoaded { get; }
        public abstract string Name { get; }
        public abstract string DropTarget { get; }
        public abstract string Url { get; }
        public abstract int HighQuality { get; set; }
        public abstract bool FocusRect { get; set; }
        public abstract float SoundBufTime { get; set; }
        public abstract string Quality { get; set; }
        public abstract float MouseX { get; }
        public abstract float MouseY { get; }

        public abstract void SetName(string name);

        public ActionVar this[Properties prop] 
        {
            get 
            {
                switch (prop)
                {
                    case Properties._x: return X;
                    case Properties._y: return Y;
                    case Properties._xscale: return XScale;
                    case Properties._yscale: return YScale;
                    case Properties._currentframe: return CurrentFrame;
                    case Properties._totalframes: return TotalFrames;
                    case Properties._alpha: return Alpha;
                    case Properties._visible: return Visible;
                    case Properties._width: return Width;
                    case Properties._height: return Height;
                    case Properties._rotation: return Rotation;
                    case Properties._target: return Target;
                    case Properties._framesloaded: return FramesLoaded;
                    case Properties._name: return Name;
                    case Properties._droptarget: return DropTarget;
                    case Properties._url: return Url;
                    case Properties._highquality: return HighQuality;
                    case Properties._focusrect: return FocusRect;
                    case Properties._soundbuftime: return SoundBufTime;
                    case Properties._quality: return Quality;
                    case Properties._mousex: return MouseX;
                    case Properties._mousey: return MouseY;
                    default:
                        return new ActionVar();
                }
            } 
            set 
            {
                switch (prop)
                {
                    case Properties._x: X = (float)value; break;
                    case Properties._y: Y = (float)value; break;
                    case Properties._xscale: XScale = (float)value; break;
                    case Properties._yscale: YScale = (float)value; break;
                    case Properties._alpha: Alpha = (float)value; break;
                    case Properties._visible: Visible = (bool)value; break;
                    case Properties._width: Width = (float)value; break;
                    case Properties._height: Height = (float)value; break;
                    case Properties._rotation: Rotation = (float)value; break;
                    case Properties._name: SetName(value.String); break;
                    case Properties._highquality: HighQuality = (int)value; break;
                    case Properties._focusrect: FocusRect = (bool)value; break;
                    case Properties._soundbuftime: SoundBufTime = (float)value; break;
                    case Properties._quality: Quality = value.String; break;
                    default:
                        RootClip.Trace("Unhandled property {0}!", prop.ToString());
                        break;
                }
            } 
        }
        public override ActionVar this[string name]
        {
            get
            {
                try
                {
                    ActionVar v = null;
                    if (!string.IsNullOrEmpty(name) && name[0] == '_')
                        v = this[(Properties)Enum.Parse(typeof(Properties), name, true)];
                    if (v != null && v.IsValid)
                        return v;
                }
                catch (Exception)
                { }

                return base[name];
            }
            set
            {
                try
                {
                    if (!string.IsNullOrEmpty(name) && name[0] == '_')
                        this[(Properties)Enum.Parse(typeof(Properties), name, true)] = value;
                }
                catch (Exception)
                {
                    base[name] = value;
                }                
            }
        }

        #endregion
    }

    public interface IRootMovieClip
    {
        long Runtime { get; }
        MovieClip AsClip { get; }

        int GetRandom(int max);
        void ToggleQuality();
        void StopSounds();
        void Trace(string message, params object[] args);
        void EndDrag();
    }
}
