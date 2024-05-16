using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaFlash;
using XnaVG;

namespace XnaFlashPlayer
{
    using XNAColor = Microsoft.Xna.Framework.Color;
    using GDIColor = System.Drawing.Color;

    public class FlashPlayerControl : Control, ISystemServices
    {
        private Dictionary<Type, object> services = new Dictionary<Type, object>();
        private GraphicsDeviceService graphicsDeviceService;
        private GameServiceContainer gameServiceContainer = new GameServiceContainer();
        private FlashDocument document = null;
        private Flash instance = null;
        private DateTime lastDraw, startTime;
        private int quality;
        private bool paused;
        private bool looping;

        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDeviceService.GraphicsDevice; }
        }

        public bool Open(string file)
        {
#if !UNITY_EDITOR
            try
#endif
            {
                using (var fs = new FileStream(file, FileMode.Open))
                    document = new FlashDocument("document", new XnaFlash.Swf.SwfStream(fs), this);
                instance = new Flash(gameServiceContainer, document, Math.Min(GraphicsDevice.Adapter.CurrentDisplayMode.Width, VectorDevice.MaxTextureSize), Math.Min(GraphicsDevice.Adapter.CurrentDisplayMode.Height, VectorDevice.MaxTextureSize));
                instance.Visible = true;
                instance.IsTransparent = false;
                instance.Root.HighQuality = quality;
                instance.Root.DontLoop = !looping;
                instance.Enabled = !paused;                              
                instance.Root.NextFrame();
                instance.MouseCallback += MouseCallbackEvent;
                lastDraw = startTime = DateTime.Now;
                return true;
            }
#if !UNITY_EDITOR

            catch (Exception e)
            {
                MessageBox.Show("Error!\n\n" + e.Message);
                return false;
            }
#endif

        }
        public void Close()
        {
            try
            {
                instance.Dispose();
                document.Dispose();
            }
            catch (Exception)
            { }
            finally
            {
                document = null;
                instance = null;
            }
        }
        public void Snapshot(string file)
        {
            if (instance == null)
                return;

            try
            {
                using (var fs = new FileStream(file, FileMode.Create))
                    instance.Surface.Target.SaveAsJpeg(fs, instance.Surface.Width, instance.Surface.Height);
            }
            catch (Exception e)
            {
                MessageBox.Show("Snímek se nepodařilo uložit!\n\n" + e.Message);
            }
        }
        public void SetQuality(int qualityLevel)
        {
            quality = qualityLevel;
            if (instance != null)
                instance.Root.HighQuality = qualityLevel;
        }

        public Vector2 MouseCallbackEvent(Flash flash)
        {
            Vector2 pos =new Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y);
            //UnityEngine. Debug.Log(pos);
            return pos;
        }

        public void Play()
        {
            Rewind();
            if (instance == null)
                return;
            instance.Root.Play();
        }

        public void Pause(bool paused)
        {
            if (instance == null)
                return;

            instance.Enabled = !paused;
            this.paused = paused;
        }
        public void Rewind()
        {
            if (instance == null)
                return;

            instance.Root.GoTo(1);
            instance.Enabled = !paused;
        }
        public void SetLooping(bool looping)
        {
            this.looping = looping;
            if (instance != null)
                instance.Root.DontLoop = !looping;
        }
        public void NextFrame()
        {
            if (instance == null)
                return;

            instance.Root.OnNextFrame();
            instance.ForceRedraw();
        }
        public void PrevFrame()
        {
            if (instance == null)
                return;

            instance.Root.PrevFrame();
            instance.ForceRedraw();
        }
        public void ExportAnimation(int skip, int count, bool transparent, string targetDirectory)
        {
            var flash = new Flash(gameServiceContainer, document, document.Width / 20, document.Height / 20);
            flash.Visible = true;
            flash.IsTransparent = transparent;
            flash.Root.HighQuality = quality;
            flash.Root.DontLoop = !looping;
            flash.Root.NextFrame();

            for (int i = 0; i < skip; i++)
                flash.Root.OnNextFrame();

            for (int i = 1; i <= count; i++)
            {
                flash.ForceRedraw();
                flash.Draw(new GameTime());

                var file = Path.Combine(targetDirectory, "frame" + i.ToString("D6") + ".png");
                using (var fs = new FileStream(file, FileMode.Create))
                    flash.Surface.Target.SaveAsPng(fs, flash.Surface.Width, flash.Surface.Height);

                flash.Root.OnNextFrame();
            }
        }
        protected override void OnCreateControl()
        {
            if (!DesignMode)
            {
                graphicsDeviceService = GraphicsDeviceService.AddRef(Handle, ClientSize.Width, ClientSize.Height);
                gameServiceContainer.AddService(typeof(IGraphicsDeviceService), graphicsDeviceService);
                gameServiceContainer.AddService(typeof(ISystemServices), this);
                Initialize();
            }

            base.OnCreateControl();
        }
        protected override void Dispose(bool disposing)
        {
            if (graphicsDeviceService != null)
            {
                gameServiceContainer.RemoveService(typeof(IGraphicsDeviceService));
                graphicsDeviceService.Release(disposing);
                graphicsDeviceService = null;
            }

            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            string beginDrawError = BeginDraw();

            if (string.IsNullOrEmpty(beginDrawError))
            {
                Draw();
                EndDraw();
            }
            else
            {
                //PaintUsingSystemDrawing(e.Graphics, beginDrawError);
            }
        }
        private string BeginDraw()
        {
            if (graphicsDeviceService == null)
                return Text + "\n\n" + GetType();

            string deviceResetError = HandleDeviceReset();
            if (!string.IsNullOrEmpty(deviceResetError))
                return deviceResetError;

            Viewport viewport = new Viewport();
            viewport.X = 0;
            viewport.Y = 0;
            viewport.Width = ClientSize.Width;
            viewport.Height = ClientSize.Height;
            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;
            GraphicsDevice.Viewport = viewport;

            return null;
        }

        internal void ExportAnimation(int skipFrames, int exportFrames, bool transparent, object selectedPath)
        {
            throw new NotImplementedException();
        }

        private void EndDraw()
        {
            try
            {
                var sourceRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
                GraphicsDevice.Present(sourceRectangle, null, this.Handle);
            }
            catch
            { }
        }
        private string HandleDeviceReset()
        {
            bool deviceNeedsReset = false;

            switch (GraphicsDevice.GraphicsDeviceStatus)
            {
                case GraphicsDeviceStatus.Lost:
                    return "Grafické zažízení ztraceno";

                case GraphicsDeviceStatus.NotReset:
                    deviceNeedsReset = true;
                    break;

                default:
                    PresentationParameters pp = GraphicsDevice.PresentationParameters;
                    deviceNeedsReset = (ClientSize.Width > pp.BackBufferWidth) ||
                                       (ClientSize.Height > pp.BackBufferHeight);
                    break;
            }

            if (deviceNeedsReset)
            {
                try
                {
                    graphicsDeviceService.ResetDevice(ClientSize.Width, ClientSize.Height, null);
                }
                catch (Exception e)
                {
                    return "Nelze restarovat grafické zařízení\n\n" + e;
                }
            }
            return null;
        }
        //protected virtual void PaintUsingSystemDrawing(Graphics graphics, string text)
        //{
        //    graphics.Clear(GDIColor.CornflowerBlue);

        //    using (Brush brush = new SolidBrush(GDIColor.Black))
        //    {
        //        using (StringFormat format = new StringFormat())
        //        {
        //            format.Alignment = StringAlignment.Center;
        //            format.LineAlignment = StringAlignment.Center;

        //            graphics.DrawString(text, Font, brush, ClientRectangle, format);
        //        }
        //    }
        //}
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected void Initialize()
        {
            VectorDevice = VGDevice.Initialize(GraphicsDevice, gameServiceContainer);
            Application.Idle += delegate { Invalidate(); };
        }

        public GameTime UpDateTime;

        protected void Draw()
        {
            if (instance != null)
            {
                instance.Update(new GameTime(DateTime.Now - startTime, DateTime.Now - lastDraw));
                instance.Draw(new GameTime(DateTime.Now - startTime, DateTime.Now - lastDraw));
                lastDraw = DateTime.Now;
            }
            else
                GraphicsDevice.Clear(XNAColor.CornflowerBlue);
        }

#region ISystemServices

        public IVGDevice VectorDevice
        {
            get;
            set;
        }
        public void Log(string message, params object[] objects) {
            UnityEngine.Debug.LogWarning(string .Format ( message,objects));
        }
        public VGFont LoadFont(string name) { return null; }

#endregion
    }
}
