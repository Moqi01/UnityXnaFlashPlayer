using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaFlash.Movie;
using XnaFlash.Swf;
using XnaFlash.Swf.Structures;
using XnaFlash.Swf.Tags;
using XnaVG;

namespace XnaFlash.Content
{
    public class ButtonInfo : ICharacter
    {
        private const int HitTestSize = 512;
        private byte[,] _hitTestBitmap = new byte[HitTestSize / 8, HitTestSize];
        private Rectangle _hitBounds = Rectangle.Empty;

        public ushort ID { get; private set; }
        public CharacterType Type { get { return CharacterType.Button; } }
        public VGCxForm CxForm { get; private set; }
        public Rectangle? Bounds { get; private set; }
        public bool TrackAsMenu { get; private set; }
        public ButtonCondAction[] Actions { get; private set; }
        public ButtonPart[] Parts { get; private set; }

        public ButtonInfo(ISwfDefinitionTag tag, ISystemServices services, FlashDocument document)
        {            
            if (tag is DefineButtonTag)
                tag = new DefineButton2Tag(tag as DefineButtonTag);

            var b = tag as DefineButton2Tag;
            CxForm = null;
            TrackAsMenu = b.TrackAsMenu;
            Actions = b.Actions;
            ID = tag.CharacterID;

            Parts = b.Parts
                .Where(r => r.Up || r.Down || r.Over)
                .OrderBy(r => r.CharacterDepth)
                .Select(r => new ButtonPart(r, document))
                .Where(p => p.Character != null)
                .ToArray();

            GenerateHits(b, services, document);           
        }

        public bool CheckHit(Vector2 mousePos, VGMatrix matrix)
        {
            if (_hitBounds.IsEmpty) return false;

            //mousePos = ~matrix * mousePos;
            mousePos.X = (mousePos.X - _hitBounds.Left) / _hitBounds.Width;
            mousePos.Y = (mousePos.Y - _hitBounds.Top) / _hitBounds.Height;
            
            if (mousePos.X < 0f || mousePos.X > 1f) return false;
            if (mousePos.Y < 0f || mousePos.Y > 1f) return false;

            int xPx = (int)(mousePos.X * (HitTestSize - 1));
            int yPx = (int)(mousePos.Y * (HitTestSize - 1));

            return (_hitTestBitmap[xPx >> 3, yPx] & (0x80 >> (xPx & 0x07))) != 0;
            //return true;
        }
        public void SetCxForm(VGCxForm cxform)
        {
            CxForm = cxform;
        }
        public Movie.IDrawable MakeInstance(Movie.DisplayObject container, RootMovieClip root)
        {
            return new Button(root, this, container);
        }
        public void Dispose() { }

        private Rectangle TransformBounds(Rectangle r, VGMatrix m)
        {
            var b = m.TransformExtents(new Vector4(r.Left, r.Top, r.Right, r.Bottom));
            return new Rectangle((int)b.X, (int)b.Y, (int)(b.Z - b.X + 1), (int)(b.W - b.Y + 1));
        }
        private void GenerateHits(DefineButton2Tag tag, ISystemServices services, FlashDocument document)
        {
            var hit = tag.Parts.Where(r => r.HitTest).Select(r => new ButtonPart(r, document));
            foreach (var h in hit)
            {
                if (h.Character == null) continue;
                if (!h.Character.Bounds.HasValue) continue;
                _hitBounds = Rectangle.Union(_hitBounds, TransformBounds(h.Character.Bounds.Value, h.Matrix));
            }
            if (_hitBounds == Rectangle.Empty)
                return;

            // Make Hit (using Color surface since Alpha8 is not supported everywhere)
            using (var surface = services.VectorDevice.CreateSurface(HitTestSize, HitTestSize, SurfaceFormat.Color))
            {
                var state = services.VectorDevice.CreateState();
                state.SetAntialiasing(VGAntialiasing.None);
                state.NonScalingStroke = true;
                state.FillRule = VGFillRule.EvenOdd;
                state.ColorTransformationEnabled = true;
                state.SetProjection(_hitBounds.Width, _hitBounds.Height);
                state.PathToSurface.Push(VGMatrix.Translate(-_hitBounds.Left, -_hitBounds.Top));

                using (var context = services.VectorDevice.BeginRendering(surface, state, new DisplayState(), true))
                {
                    foreach (var h in hit.OrderBy(c => c.Depth))
                    {
                        if (!(h.Character is Movie.IDrawable)) continue;
                        if (!h.Character.Bounds.HasValue) continue;

                        state.PathToSurface.PushCombineLeft(h.Matrix);
                        (h.Character as Movie.IDrawable).Draw(context);
                        state.PathToSurface.Pop();
                    }
                }
                return;
                Func<Color, byte> isCovered = (c) => (byte)((c.A != 0) ? 1 : 0);
                Color[] data = new Color[surface.Width * surface.Height];
                surface.Target.GetData(data);
                for (int y = 0; y < surface.Height; y++)
                {
                    for (int x = 0; x < surface.Width; )
                    {
                        var b = isCovered(data[x++]);
                        b <<= 1;
                        b |= isCovered(data[x++]);
                        b <<= 1;
                        b |= isCovered(data[x++]);
                        b <<= 1;
                        b |= isCovered(data[x++]);
                        b <<= 1;
                        b |= isCovered(data[x++]);
                        b <<= 1;
                        b |= isCovered(data[x++]);
                        b <<= 1;
                        b |= isCovered(data[x++]);
                        b <<= 1;
                        b |= isCovered(data[x++]);
                        _hitTestBitmap[(x / 8) - 1, y] = b;
                    }
                }
            }
        }   
    }
}
