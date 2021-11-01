using Microsoft.Xna.Framework;
using XnaFlash.Movie;
using XnaFlash.Swf;
using XnaFlash.Swf.Tags;
using XnaFlash.Swf.Structures.Fonts;
using XnaVG;

namespace XnaFlash.Content
{
    public class Font : ICharacter
    {
        public ushort ID { get; private set; }
        public CharacterType Type { get { return CharacterType.Font; } }
        public VGFont DeviceFont { get; private set; }
        public FontGlyph[] GlyphFont { get; private set; }
        public char[] GlyphChars { get; private set; }
        public Rectangle? Bounds { get { return null; } }

        internal Font(ushort id)
        {
            DeviceFont = null;
            GlyphFont = null;
            ID = id;
        }

        internal void AddInfo(ISwfTag tag, ISystemServices services)
        {
            if (tag is DefineFontInfoTag)
            {
                var i = (tag as DefineFontInfoTag);
                SetDeviceFont(i.Name, i.Characters, services);
            }
            else if (tag is DefineFontTag)
                GlyphFont = (tag as DefineFontTag).Glyphs;                
            else if (tag is DefineFont2Tag)
            {
                var font = tag as DefineFont2Tag;

                if (font.Glyphs != null && font.Glyphs.Length > 0)
                    GlyphFont = font.Glyphs;

                if (!string.IsNullOrEmpty(font.Name))
                    SetDeviceFont(font.Name, font.Characters, services);
            }
        }

        private void SetDeviceFont(string name, char[] table, ISystemServices services)
        {
            if (DeviceFont != null)
                return;

            GlyphChars = table;
            DeviceFont = services.LoadFont(name);
            if (DeviceFont == null)
                services.Log("Font '{0}' does not exist!", name);
        }

        public Movie.IDrawable MakeInstance(Movie.DisplayObject container, RootMovieClip root) { return null; }

        public void Dispose() { }
    }
}
