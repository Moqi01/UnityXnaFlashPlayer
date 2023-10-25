using System.Collections.Generic;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(86)]
    public class DefineScenesAndFrameLabelsTag : ISwfControlTag
    {
        public IDictionary<string, ushort> Scenes { get; private set; }
        public IDictionary<string, ushort> FrameLabels { get; private set; }

        #region ISwfTag Members

        public void Load(SwfStream stream, uint length, byte version)
        {
            uint numScenes = stream.ReadEncodedUInt();
            var scenes = new Dictionary<string, ushort>((int)numScenes);
            for (uint i = 0; i < numScenes; i++)
            {
                ushort frame = (ushort)stream.ReadEncodedUInt();
                scenes.Add(stream.ReadString(), frame);
            }

            uint numLabels = stream.ReadEncodedUInt();
            var labels = new Dictionary<string, ushort>((int)numLabels);
            for (uint i = 0; i < numLabels; i++)
            {
                ushort label = (ushort)stream.ReadEncodedUInt();
                labels.Add(stream.ReadString(), label);
            }

            Scenes = scenes;
            FrameLabels = labels;
        }

        #endregion
    }
}
