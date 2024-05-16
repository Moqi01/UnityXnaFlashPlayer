using System;
using XnaFlash.Swf.Structures;

using System.IO;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(6)]
    public class DefineBitsTag : ISwfDefinitionTag
    {
        public Content.CharacterType Type { get { return Content.CharacterType.Bitmap; } }
        public ushort CharacterID { get; protected set; }
        public byte[] ImageData { get; protected set; }

        #region ISwfTag Members

        public virtual void Load(SwfStream stream, uint length, byte version)
        {
            CharacterID = stream.ReadUShort();
            //ImageData = BitmapUtils.ComposeJpeg(stream.JpegTables.JpegData, BitmapUtils.RepairJpegMarkers(stream.ReadByteArray(length - 2)));
            //ImageData = stream.ReadByteArray(length - 2);
            ImageData = BitmapUtils.RepairJpegMarkers(stream.ReadByteArray(length - 2));
        }

        #endregion
    }

    [SwfTag(21)]
    public class DefineBitsJPEG2Tag : DefineBitsTag
    {
        public BitmapFormat Format { get; protected set; }

        #region ISwfTag Members

        public override void Load(SwfStream stream, uint length, byte version)
        {
            CharacterID = stream.ReadUShort();
            ImageData = BitmapUtils.RepairJpegMarkers(stream.ReadByteArray(length - 2));
            Format = BitmapUtils.DetectFormat(ImageData);
        }

        #endregion
    }

    [SwfTag(35)]
    public class DefineBitsJPEG3Tag : DefineBitsJPEG2Tag
    {
        public byte[] CompressedAlpha { get; protected set; }
        public bool HasAlpha { get { return CompressedAlpha != null && CompressedAlpha.Length > 0; } }
        public ushort? Deblock { get; protected set; }

        protected void Load(SwfStream stream, uint length, bool hasDeblock)
        {
            CharacterID = stream.ReadUShort();
            uint alpha = stream.ReadUInt();
            Deblock = hasDeblock ? (ushort?)stream.ReadUShort() : null;
            byte[] data = stream.ReadByteArray(length - 6);

            ImageData = new byte[alpha];
            Array.Copy(data, 0, ImageData, 0, (int)alpha);
            Format = BitmapUtils.DetectFormat(ImageData);
            if (Format == BitmapFormat.Jpeg)
                ImageData = BitmapUtils.RepairJpegMarkers(ImageData);

            CompressedAlpha = new byte[data.Length - alpha];
            if (CompressedAlpha.Length > 0)
                Array.Copy(data, (int)alpha, CompressedAlpha, 0, CompressedAlpha.Length);            
        }

        #region ISwfTag Members

        public override void Load(SwfStream stream, uint length, byte version)
        {
            Load(stream, length, false);
        }

        #endregion
    }

    [SwfTag(90)]
    public class DefineBitsJPEG4Tag : DefineBitsJPEG3Tag
    {
        #region ISwfTag Members

        public override void Load(SwfStream stream, uint length, byte version)
        {
            Load(stream, length, true);
        }

        #endregion
    }
}