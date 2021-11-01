using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    public class SpriteFont : IDisposable
    {
        #region Fields
        private string fontName;
        private float size;
		private float spacing;
        private bool useKerning;
        private string style;
        private readonly Texture2D _texture;
        #endregion

        #region Properties
        public Texture2D Texture
        {
            get
            {
                return this._texture;
            }
        }

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
		
        public float Spacing
        {
            get
            {
                return this.spacing;
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
            this._texture = texture;
            this.fontName = fontName;
            this.size = size;
			this.spacing = spacing;
            this.useKerning = useKerning;
            this.style = style;
        }
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
