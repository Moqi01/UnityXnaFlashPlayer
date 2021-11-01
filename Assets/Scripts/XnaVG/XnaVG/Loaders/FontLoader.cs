using Microsoft.Xna.Framework.Content;

namespace XnaVG.Loaders
{
    /// <summary>
    /// Třída umožňující načtení fontu prostřednictvím content pipeline
    /// </summary>
    internal sealed class FontLoader : ContentTypeReader<VGFontData>
    {
        protected override VGFontData Read(ContentReader input, VGFontData existingInstance)
        {
            return VGFontData.Deserialize(input);
        }
    }
}
