using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaFlash.Actions;
using XnaFlash.Actions.Objects;
using XnaFlash.Content;
using XnaFlash.Movie;
using XnaFlash.Swf.Tags;
using XnaVG;
using XnaVG.Utils;

namespace XnaFlash.Movie
{
    public class RootMovieClip : MovieClip
    {
        private Random _random = new Random((int)DateTime.Now.Ticks);

        internal VGMatrixStack ButtonStack { get; private set; }
        public VGImage IdleCursor { get; set; }
        public VGImage HandCursor { get; set; }
        public GlobalScope GlobalScope { get; private set; }
        public bool MouseDown { get; private set; }
        public Button ActiveButton { get; set; }
        public bool Transparent { get; set; }
        public Vector2 MousePosition { get; private set; }
        public FlashDocument Document { get; private set; }
        public DateTime StartTime { get; private set; }
        public VGAntialiasing Antialiasing { get; private set; }
        public ISystemServices Services { get; private set; }
        public long Runtime { get { return (long)(DateTime.Now - StartTime).TotalMilliseconds; } }

        internal RootMovieClip(FlashDocument document, ISystemServices services)
            : base(null, document, null)
        {
            ButtonStack = new VGMatrixStack(32);
            Services = services;
            Transparent = true;
            Root = this;
            StartTime = DateTime.Now;
            Document = document;
            Antialiasing = VGAntialiasing.Faster;
            GlobalScope = new GlobalScope(this);
            Context.RootClip = this;
            Context.Scope.AddFirst(GlobalScope);
        }

        public bool? SetMouse(Vector2 mouse, bool down)
        {
            bool? res = null;
            if (MousePosition != mouse)
            {
                ButtonStack.Clear();
                MousePosition = mouse;
                res = OnMouseMove();                
            }
            if (MouseDown != down)
            {
                MouseDown = down;
                if (ActiveButton != null)
                {
                    if (down) ActiveButton.OnMouseDown();
                    else ActiveButton.OnMouseUp();
                }
            }
            return res;
        }
        public void Draw(VGSurface surface)
        {
            using (var draw = Services.VectorDevice.BeginRendering(surface, new DisplayState(), true))
            {
                draw.State.ResetDefaultValues();
                draw.State.SetAntialiasing(Antialiasing);
                draw.State.SetProjection(Width, Height);
                draw.State.NonScalingStroke = true;
                draw.State.MaskingEnabled = false;
                draw.State.FillRule = VGFillRule.EvenOdd;
                draw.State.ColorTransformationEnabled = true;
                draw.Device.GraphicsDevice.Clear(
                    ClearOptions.Stencil | ClearOptions.Target | ClearOptions.DepthBuffer,
                    Transparent ? Color.Transparent.ToVector4() : Document.BackgroundColor.ToVector4(),
                    0f, 0);
                (this as IDrawable).Draw(draw);
            }
        }
        public int GetRandom(int max)
        {
            return _random.Next(max);
        }
        public void ToggleQuality()
        {
            switch (Antialiasing)
            {
                case VGAntialiasing.None: Antialiasing = VGAntialiasing.Faster; break;
                case VGAntialiasing.Faster: Antialiasing = VGAntialiasing.Better; break;
                case VGAntialiasing.Better: Antialiasing = VGAntialiasing.Best; break;
                default:
                    Antialiasing = VGAntialiasing.None;
                    break;
            }
        }
        public void StopSounds() { }
        public void Trace(string message, params object[] args)
        {
            Services.Log(message, args);
        }
        public void StartDrag(MovieClip clip, bool lockCenter, Rectangle? constraint) { }        
        public void EndDrag() { }

        public override bool OnMouseMove()
        {
            return _displayList.OnMouseMove();
        }
        public override void SetName(string name) { }
        public override string GetRootedPath(bool slashes)
        {
            return slashes ? "" : "_root";
        }

        #region Native Properties

        public override float X
        {
            get { return 0f; }
            set {  }
        }
        public override float Y
        {
            get { return 0f; }
            set {  }
        }
        public override float XScale
        {
            get { return 100f; }
            set {  }
        }
        public override float YScale
        {
            get { return 100f; }
            set {  }
        }
        public override ushort TotalFrames
        {
            get { return Document.FrameCount; }
        }
        public override float Alpha
        {
            get { return 1f; }
            set {  }
        }
        public override float Width
        {
            get { return Document.Width; }
            set { }
        }
        public override float Height
        {
            get { return Document.Height; }
            set { }
        }
        public override float Rotation
        {
            get { return 0f; }
            set {  }
        }
        public override string Name
        {
            get { return "_root"; }
        }
        public override string DropTarget
        {
            get { return null; }
        }
        public override bool FocusRect
        {
            get { return false; }
            set { }
        }
        public override float SoundBufTime
        {
            get { return 0f; }
            set { }
        }
        public override int HighQuality
        {
            get
            {
                switch (Antialiasing)
                {
                    case VGAntialiasing.None: return 0;
                    case VGAntialiasing.Faster: return 1;
                    case VGAntialiasing.Best: return 3;
                    default:
                        return 2;
                }
            }
            set
            {
                switch (value)
                {
                    case 0: Antialiasing = VGAntialiasing.None; break;
                    case 1: Antialiasing = VGAntialiasing.Faster; break;
                    case 3: Antialiasing = VGAntialiasing.Best; break;
                    default:
                        Antialiasing = VGAntialiasing.Better;
                        break;
                }
            }
        }
        public override string Quality
        {
            get 
            {
                switch (Antialiasing)
                {
                    case VGAntialiasing.None: return "LOW";
                    case VGAntialiasing.Faster: return "MEDIUM";
                    case VGAntialiasing.Best: return "BEST";
                    default:
                        return "HIGH";
                }
            }
            set
            {
                switch (value)
                {
                    case "LOW": Antialiasing = VGAntialiasing.None; break;
                    case "MEDIUM": Antialiasing = VGAntialiasing.Faster; break;
                    case "BEST": Antialiasing = VGAntialiasing.Best; break;
                    default:
                        Antialiasing = VGAntialiasing.Better;
                        break;
                }
            }
        }
        public override string Url
        {
            get { return Document.Name; }
        }
        public override float MouseX
        { 
            get { return MousePosition.X; }            
        }
        public override float MouseY
        {
            get { return MousePosition.Y; }
        }

        #endregion
    }
}
