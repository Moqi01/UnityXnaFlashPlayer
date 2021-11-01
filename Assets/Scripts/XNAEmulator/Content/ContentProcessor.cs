using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using UnityEngine;
using XnaVG.Loaders;

namespace Microsoft.Xna.Framework.Content.Pipeline.Processors
{
    public class ContentProcessor <T,V>
    {
        public virtual byte[] Process(byte[] input, ContentProcessorContext context)
        {
            return null;
        }

        public virtual VGFontData Process(GlyphTypeface input, ContentProcessorContext context)
        {
            return null;
        }

    }
}
