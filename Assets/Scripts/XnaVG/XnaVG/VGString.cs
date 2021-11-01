using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XnaVG.Rendering;
using XnaVG.Rendering.Text;

namespace XnaVG
{
    public abstract class VGString : IDisposable
    {
        public string Text { get; private set; }
        public VGFont Font { get; private set; }
        public Vector4 Extents { get; protected set; }
        public object Tag { get; set; }
        
        internal VGString(VGFont font)
        {
            Font = font;
            Text = string.Empty;
        }

        public void SetText(string text)
        {
            var size = SetString(text);
            Vector4 extents = Font.MaxExtents;
            extents.Z += size.X;
            extents.Y -= size.Y;
            Extents = extents;
            Text = text;
        }

        internal static IEnumerable<T> ProcessString<T>(VGFont font, string text, GlyphPositionInfo position) where T : Glyph
        {
            var tabs = new List<float>(8);
            var kern = font.KerningTable ?? VGKerningTable.EmptyTable;
            int i, t = 0, len = text.Length;
            float xStart = position.Offset.X;
            T g;
            
            if (len <= 0) 
                yield break;

            len--;
            for (i = 0; i < len; i++)
            {
                // Control character
                if (text[i] < 0x20)
                {
                    if (text[i] == '\n')
                    {
                        position.Offset.X = xStart;
                        position.Offset.Y -= font.LeadingSize;
                        position.Size.Y += font.LeadingSize;
                        t = 0;
                    }
                    else if (text[i] == '\r')
                    {
                        position.Offset.X = xStart;
                        t = 0;
                    }
                    else if (text[i] == '\t')
                    {
                        if (tabs.Count <= t)
                        {
                            position.Offset.X += position.TabSize.X;
                            position.Size.X = Math.Max(position.Size.X, position.Offset.X);
                            tabs.Add(position.Offset.X);
                        }
                        else
                        {
                            position.Offset.X = Math.Max(tabs[t], position.Offset.X + position.TabSize.X);
                            position.Size.X = Math.Max(position.Size.X, position.Offset.X);
                            tabs[t] = position.Offset.X;
                        }

                        t++;
                    }
                    else if (text[i] == '\v')
                    {
                        position.Offset.Y -= position.TabSize.Y * font.LeadingSize;
                        position.Size.Y += position.TabSize.Y * font.LeadingSize;
                    }

                    continue;
                }

                g = font[text[i]] as T;
                if (g == null) 
                    continue;
                
                if (!g.IsEmpty)
                    yield return g;                

                position.Offset.X += g.Escape.X + kern[text[i], text[i + 1]];
                position.Size.X = Math.Max(position.Size.X, position.Offset.X);
            }


            // When last character is control character, just ignore it
            g = font[text[i]] as T;
            if (g != null && !g.IsEmpty)
            {
                yield return g;
                position.Offset.X += g.Escape.X;
                position.Size.X = Math.Max(position.Size.X, position.Offset.X);
            }
        }
        public static implicit operator string(VGString s)
        {
            return s.Text;
        }
        public override string ToString()
        {
            return Text;
        }

        internal abstract bool IsEmpty { get; }
        internal abstract void Prepare();
        internal abstract void Stencil(Pipeline pipeline);
        internal abstract void Cover(Pipeline pipeline);
        public abstract void Dispose();
        protected abstract Vector2 SetString(string text);

        internal class GlyphPositionInfo
        {
            public Vector2 TabSize;
            public Vector2 Offset;
            public Vector2 Size;
        }
    }
}
