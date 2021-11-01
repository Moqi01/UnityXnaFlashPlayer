using System;
using Microsoft.Xna.Framework.Content;
using XnaFlash.Content;
using XnaFlash.Swf;
using XnaFlash;
using XnaVG;

namespace XnaFlash
{
    public class SwfImporter : ContentTypeReader<FlashDocument>
    {
        protected override FlashDocument Read(ContentReader input, FlashDocument existingInstance)
        {
            byte[] data = input.ReadBytes(input.ReadInt32());
            ISystemServices system = (ISystemServices)input.ContentManager.ServiceProvider.GetService(typeof(ISystemServices));
            if (system == null) throw new InvalidOperationException("Flash content can be loaded only with ISystemServices registered!");
            return new FlashDocument(input.AssetName, new SwfStream(data), system);
        }
    }
}
