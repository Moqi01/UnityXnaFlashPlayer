using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaVG;
using XnaFlash.Actions;
using XnaFlash.Content;
using XnaFlash.Swf;
using XnaFlash.Swf.Tags;

namespace XnaFlash
{
    public class FlashDocument : Sprite
    {
        private Dictionary<ushort, ICharacter> _characters = new Dictionary<ushort, ICharacter>();
        private Dictionary<string, ushort> _scenes = new Dictionary<string, ushort>();
        private List<ushort> _initOrder = new List<ushort>();

        public static Vector2 WH;
        public VGColor BackgroundColor { get; private set; }
        public string Name { get; private set; }
        public byte Version { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public double FrameDelay { get; private set; }
        public override Rectangle? Bounds { get { return new Rectangle(0, 0, Width, Height); } }
        internal ICharacter this[ushort id]
        {
            get
            {
                ICharacter value;
                if (!_characters.TryGetValue(id, out value))
                    return null;
                return value;
            }
        }

        public FlashDocument(string name, SwfStream stream, ISystemServices services)
            : base(stream.ProcessFile(), 0, stream.FrameCount, services)
        {
            Name = name;
            Version = stream.Version;
            Width = stream.Rectangle.Width;
            Height = stream.Rectangle.Height;
            WH = new Vector2(Width, Height);
            FrameDelay = 1000.0 / (double)stream.FrameRate;
        }

        protected override void UnhandledTag(ISwfTag tag, ISystemServices services)
        {
            if (tag is ISwfDefinitionTag)
            {
                var defTag = tag as ISwfDefinitionTag;

                ICharacter character = null;
                switch (defTag.Type)
                {
                    case CharacterType.BinaryData: character = new BinaryData(defTag.CharacterID, (tag as DefineBinaryDataTag).Data); break;
                    case CharacterType.Bitmap: character = new Bitmap(defTag, services); break;
                    case CharacterType.Button: character = new ButtonInfo(defTag, services, this); break;
                    case CharacterType.Shape: character = new Shape(defTag, services, this); break;
                    case CharacterType.Text: character = new Text(tag as DefineTextTag, services, this); break;
                    case CharacterType.Sprite:
                        {
                            var sprite = tag as DefineSpriteTag;
                            character = new Sprite(sprite.Tags, defTag.CharacterID, sprite.FrameCount, services);
                        }
                        break;
                    case CharacterType.Font:
                        {
                            var font = new Font(defTag.CharacterID);
                            font.AddInfo(tag, services);
                            character = font;
                        }
                        break;
                }

                if (character != null)
                    _characters.Add(defTag.CharacterID, character);
            }
            else if (tag is SetBackgroundColorTag)
            {
                BackgroundColor = (tag as SetBackgroundColorTag).Color;
            }
            else if (tag is DefineScenesAndFrameLabelsTag)
            {
                var def = tag as DefineScenesAndFrameLabelsTag;
                _frameLabels = new Dictionary<string, ushort>(def.FrameLabels);
                _scenes = new Dictionary<string, ushort>(def.Scenes);
            }
            else if (tag is DefineFontInfoTag)
            {
                var fi = tag as DefineFontInfoTag;
                (_characters[fi.FontID] as Font).AddInfo(tag, services);
            }
            else if (tag is DoInitActionTag)
            {
                var init = tag as DoInitActionTag;
                (_characters[init.SpriteID] as Sprite).InitAction = init.Actions;
                _initOrder.Add(init.SpriteID);
            }
            else if (tag is DefineButtonCxFormTag)
            {
                var cxf = tag as DefineButtonCxFormTag;
                (_characters[cxf.CharacterID] as ButtonInfo).SetCxForm(cxf.CxForm);
            }

            #region Not implemented tags

            /*            
            else if (tag is DefineEditTextTag) { }       
             */


            // else if (tag is SetTabIndexTag) { }
            // else if (tag is DefineScalingGridTag) { }
            else
                base.UnhandledTag(tag, services);

            #endregion

            #region Unsupported tags
            //TODO: Tags not yet implemented or unsupported
            //DefineFontNameTag
            //{14, "DefineSound"},
            //{17, "DefineButtonSound"}, 
            //{24, "Protect"},  
            //{46, "DefineMorphShape"},
            //{56, "ExportAssets"},
            //{57, "ImportAssets"},
            //{58, "EnableDebugger"},    
            //{60, "DefineVideoStream"},
            //{61, "VideoFrame"},
            //{64, "EnableDebugger2"},
            //{65, "ScriptLimits"},
            //{69, "FileAttributes"},
            //{71, "ImportAssets2"},
            //{73, "DefineFontAlignZones"},
            //{74, "CSMTextSettings"},
            //{76, "SymbolClass"},
            //{77, "Metadata"},
            //{84, "DefineMorphShape2"},
            //{89, "StartSound2"},
            //{91, "DefineFont4"}
            #endregion
        }

        public override void Dispose()
        {
            foreach (var c in _characters.Values)
                c.Dispose();
            _characters.Clear();

            base.Dispose();
        }

        public void DumpCharacters(ISystemServices services, bool shapes, bool texts)
        {
            var dev = services.VectorDevice;
            int i = 0;
            foreach (var ch in _characters.Where(c => c.Value != null))
            {
                if (!ch.Value.Bounds.HasValue) continue;

                dev.State.ResetDefaultValues();
                dev.State.SetAntialiasing(VGAntialiasing.Better);
                dev.State.NonScalingStroke = true;
                dev.State.MaskingEnabled = false;
                dev.State.FillRule = VGFillRule.EvenOdd;
                dev.State.ColorTransformationEnabled = true;
                dev.State.SetProjection(ch.Value.Bounds.Value.Width, ch.Value.Bounds.Value.Height);
                dev.State.PathToSurface.Push(VGMatrix.Translate(-ch.Value.Bounds.Value.Left, -ch.Value.Bounds.Value.Top));

                using (var surface = dev.CreateSurface(ch.Value.Bounds.Value.Width / 10, ch.Value.Bounds.Value.Height / 10, Microsoft.Xna.Framework.Graphics.SurfaceFormat.Color))
                {
                    switch (ch.Value.Type)
                    {
                        case CharacterType.Shape:
                            if (shapes)
                            {
                                var shape = ch.Value as Shape;
                                using (var context = dev.BeginRendering(surface, new Movie.DisplayState(), true))
                                    shape.Draw(context);

                                using (var fs = new System.IO.FileStream("Shape_" + ch.Key + ".tga", System.IO.FileMode.Create))
                                    XnaVG.Loaders.TgaImage.SaveAsTga(fs, surface.Target);
                            }
                            break;
                        case CharacterType.Text:
                            if (texts)
                            {
                                var text = ch.Value as Text;
                                using (var context = dev.BeginRendering(surface, new Movie.DisplayState(), true))
                                    text.Draw(context);

                                using (var fs = new System.IO.FileStream("Text_" + ch.Key + ".tga", System.IO.FileMode.Create))
                                    XnaVG.Loaders.TgaImage.SaveAsTga(fs, surface.Target);
                            }
                            break;
                    }
                }

                i++;
                Console.WriteLine(i + " / " + _characters.Count);
            }
        }
    }
}
