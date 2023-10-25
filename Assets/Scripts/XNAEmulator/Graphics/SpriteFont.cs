using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    public class SpriteFont : IDisposable
    {
        #region Fields
        private string fontName;
        private float size;
		
        private bool useKerning;
        private string style;
       
        #endregion

        #region Properties
        public float Spacing { get; }
        public Texture2D Texture { get; }

        public string FontName
        {
            get
            {
                return this.fontName;
            }
        }

        public float Size
        {
            get
            {
                return this.size;
            }
        }
		
        

        public string Style
        {
            get
            {
                return this.style;
            }
        }

        public bool UseKerning
        {
            get
            {
                return this.useKerning;
            }
        } 
        #endregion

        public SpriteFont(string fontName, float size, float spacing, bool useKerning, string style,Texture2D texture)
        {
            this.Texture = texture;
            this.fontName = fontName;
            this.size = size;
			this.Spacing = spacing;
            this.useKerning = useKerning;
            this.style = style;
        }
        private SpriteFont(Texture2D texture, List<Rectangle> glyph, List<Rectangle> cropping, List<char> characters, int lineSpacing, float spacing, List<Vector3> kerning, char? defaultCharacter)
        {
            Characters = new ReadOnlyCollection<char>(characters.ToArray());
            DefaultCharacter = defaultCharacter;
            LineSpacing = lineSpacing;
            Spacing = spacing;

            Texture = texture;
            GlyphData = glyph;
            CroppingData = cropping;
            Kerning = kerning;
            CharacterMap = characters;
        }
        public ReadOnlyCollection<char> Characters { get; }
        public char? DefaultCharacter { get; }
      
       
        public List<Rectangle> GlyphData { get; }
        public List<Rectangle> CroppingData { get; }
        public List<Vector3> Kerning { get; }
        public List<char> CharacterMap { get; }
        public Vector2 MeasureString(string text)
        {			
			
			UnityEngine.GUISkin skin = UnityEngine.GUISkin.CreateInstance<UnityEngine.GUISkin>();
			
			UnityEngine.Vector2 size = skin.label.CalcSize(new UnityEngine.GUIContent(text));
			return new Vector2(size.x, size.y);
        }

        public Vector2 MeasureString(StringBuilder text)
        {

            UnityEngine.GUISkin skin = UnityEngine.GUISkin.CreateInstance<UnityEngine.GUISkin>();

            UnityEngine.Vector2 size = skin.label.CalcSize(new UnityEngine.GUIContent(text.ToString()));
            return new Vector2(size.x, size.y);
        }
        public void Dispose()
        { }

        private int lineSpacing;
        public int LineSpacing
        {
            get
            {
                return this.lineSpacing;
            }
            set
            {
                this.lineSpacing = value;
            }
        }
    }
}
