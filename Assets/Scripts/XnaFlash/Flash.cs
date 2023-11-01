using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XnaFlash.Movie;
using XnaFlash.Swf;
using XnaVG;

namespace XnaFlash
{
    public class Flash : DrawableGameComponent
    {
        private bool _redraw = true, _forceRedraw = false;
        private double _time = 0;
        private VGSurface _surface;

        public event Func<Flash, Vector2> MouseCallback;
        public event Action<Flash, bool> CursorCallback;

        public Vector2 SizeInTwips { get; private set; }
        public Vector2 SurfaceSize { get; private set; }
        public RootMovieClip Root { get; private set; }
        public bool IsTransparent { get { return Root.Transparent; } set { Root.Transparent = value; } }
        public VGSurface Surface { get { return _surface; } }

        public Flash(Game game, FlashDocument document, int surfaceWidth, int surfaceHeight)
            : this(game, game.Services, document, surfaceWidth, surfaceHeight)
        { }

        public Flash(IServiceProvider services, FlashDocument document, int surfaceWidth, int surfaceHeight)
            : this(null, services, document, surfaceWidth, surfaceHeight)
        { }

        private Flash(Game game, IServiceProvider services, FlashDocument document, int surfaceWidth, int surfaceHeight)
            : base(null)
        {
            if (document == null) throw new ArgumentNullException("document");
            if (services == null) throw new ArgumentNullException("services");

            ISystemServices system = (ISystemServices)services.GetService(typeof(ISystemServices));
            if (system == null) throw new InvalidOperationException("Flash can be used only with ISystemServices registered!");

            _surface = system.VectorDevice.CreateSurface(surfaceWidth, surfaceHeight, SurfaceFormat.Color);
            Root = new RootMovieClip(document, system);
            SurfaceSize = new Vector2(surfaceWidth, surfaceHeight);
            SizeInTwips = new Vector2(document.Width, document.Height);
        }

        public override void Initialize()
        {            
            Root.Load();
            base.Initialize();
        }

        public void ForceRedraw()
        {
            _forceRedraw = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Visible || !Enabled) return;
            float t = UnityEngine.Time.deltaTime*400;
            //_time += gameTime.ElapsedGameTime.TotalMilliseconds;
            _time += t;
            if (_time > Root.Document.FrameDelay)
            {
                if (MouseCallback != null)
                {
                    var res = Root.SetMouse(MouseCallback(this) * SizeInTwips, Mouse.GetState().LeftButton == ButtonState.Pressed);
                    if (CursorCallback != null && res.HasValue)
                        CursorCallback(this, res.Value);
                }

                Root.OnNextFrame();
                _redraw = true;
            }
            while (_time > Root.Document.FrameDelay) _time -= Root.Document.FrameDelay; 
            
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                if (_forceRedraw || (Enabled && _redraw))
                {
                    Root.Draw(_surface);
                    _redraw = false;
                    _forceRedraw = false;
                }

                _surface.Device.TextureUtils.CopyToRenderTarget(null, _surface.Target, ColorWriteChannels.All, null, _surface.Device.GraphicsDevice.Viewport.Bounds, null);
            }

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _surface != null)
            {
                _surface.Dispose();
                _surface = null;
            }
            base.Dispose(disposing);
        }
    }
}
