using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace XnaVGPipeline.Font
{
    //[ContentImporter(".otf", ".ttf", DisplayName = "Font Importer - XnaVG", DefaultProcessor = "FontProcessor")]    
    public class FontImporter : ContentImporter<GlyphTypeface>
    {
        public override GlyphTypeface Import(string filename, ContentImporterContext context)
        {
            return new GlyphTypeface(new Uri(filename));
        }
    }
}
