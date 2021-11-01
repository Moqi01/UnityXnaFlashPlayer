using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XnaFlash;
using XnaVG.Loaders;

namespace Microsoft.Xna.Framework.Content
{
    public class ContentTypeReader<T>
    {
        protected virtual VGFontData Read(ContentReader input, VGFontData existingInstance)
        {
            return null;
        }

        protected virtual FlashDocument Read(ContentReader input, FlashDocument existingInstance)
        {
            return null;
        }
    }
}
