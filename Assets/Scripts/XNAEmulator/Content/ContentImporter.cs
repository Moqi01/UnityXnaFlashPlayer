using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using UnityEngine;
namespace Microsoft.Xna.Framework.Content.Pipeline
{
    public class ContentImporter<T>
    {
        public virtual GlyphTypeface Import(string filename, ContentImporterContext context)
        {
            return null;
        }
    }
}
