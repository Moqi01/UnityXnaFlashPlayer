using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaFlash.Movie;
using XnaFlash.Swf;
using XnaFlash.Swf.Tags;
using XnaVG;

namespace XnaFlash.Content
{
    public class Bitmap : ICharacter, Movie.IDrawable
    {
        public ushort ID { get; private set; }
        public VGImage Image { get; private set; }
        public CharacterType Type { get { return CharacterType.Bitmap; } }
        public Rectangle? Bounds { get { return Image.Texture.Bounds; } }

        public Bitmap(ushort id, VGImage image)
        {
            ID = id;
            Image = image;
        }

        public Bitmap(ISwfDefinitionTag tag, ISystemServices services)
        {
            Texture2D texture;
            if (tag is DefineBitsLosslessTag)
            {
                var t = tag as DefineBitsLosslessTag;
                texture = new Texture2D(services.GraphicsDevice, t.Width, t.Height, false, SurfaceFormat.Color);
                if(t.Pixels!=null)
                    texture.SetData(t.Pixels);
                else 
                    texture.SetData(t.Dates,"",0,t.isFlip );
            }
            else if (tag is DefineBitsTag)
            {
                var t = tag as DefineBitsTag;
                var t3 = tag as DefineBitsJPEG3Tag;
                texture = Texture2D.FromStream(services.GraphicsDevice, new MemoryStream(t.ImageData));

                if (t3 != null && t3.HasAlpha)
                {
                    var alpha = Swf.BitmapUtils.DecompressAlphaValues(t3.CompressedAlpha, texture.Width, texture.Height);
                    
                    var data = new Color[texture.Width * texture.Height];
                    texture.GetData(data);
                    for (int i = data.Length - 1; i >= 0; data[i].A = alpha[i], i--) ;
                    texture.SetData(data);
                }
            }
            else
                throw new InvalidOperationException("Tag does not define bitmap!");

            ID = tag.CharacterID;
            Image = services.VectorDevice.CreateImage(texture, false, false);
        }

        public void Draw(IVGRenderContext<Movie.DisplayState> target)
        {
            target.DrawImage(Image, null);
        }  
        
        public void OnNextFrame() { }
        public void SetParent(StageObject parent) { }

        public Movie.IDrawable MakeInstance(Movie.DisplayObject container, RootMovieClip root) { return this; }

        public void Dispose()
        {
            if (Image != null)
            {
                Image.Texture.Dispose();
                Image = null;
            }
        }


    }
}