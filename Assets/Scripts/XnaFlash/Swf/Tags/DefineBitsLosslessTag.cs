using ICSharpCode.SharpZipLib.Zip.Compression;

namespace XnaFlash.Swf.Tags
{
    [SwfTag(20)]
    public class DefineBitsLosslessTag : ISwfDefinitionTag
    {
        public Content.CharacterType Type { get { return Content.CharacterType.Bitmap; } }
        public ushort CharacterID { get; protected set; }
        public ushort Width { get; protected set; }
        public ushort Height { get; protected set; }
        public uint[] Pixels { get; protected set; }
        public bool HasAlpha { get; protected set; }
        public byte[] Dates { get; protected set; }
        public bool isFlip;

        public DefineBitsLosslessTag()
        {
            HasAlpha = false;
        }

        protected bool Load(SwfStream stream, uint length, bool hasAlpha)
        {
            CharacterID = stream.ReadUShort();

            byte format = stream.ReadByte();
            Width = stream.ReadUShort();
            Height = stream.ReadUShort();
            byte table = (format == 3) ? stream.ReadByte() : (byte)0;
            byte[] compressed = stream.ReadByteArray(length - stream.TagPosition);
            byte[] data;
            var inflater = new Inflater();
            inflater.SetInput(compressed);
           // byte[] _uncompressedData = Flazzy.Compression.ZLIB.Decompress(compressed);
            if (format == 3)
            {
                int rem = Width % 4;
               
                //data = new byte[((rem == 0) ? Width : (Width + 4 - rem)) * Height * 4];
                data = new byte[((table+1)* (hasAlpha?4:3)) + Height*Width];
                int len = inflater.Inflate(data,0,data .Length);
                if (len != data.Length)
                    throw new SwfCorruptedException("Bitmap data are not valid ZLIB stream!");
                Pixels = BitmapUtils.UnpackIndexed(data, Width, Height, table, hasAlpha);
            }
            else if (format == 4 && !hasAlpha)
            {
                data = new byte[(Width + Width & 0x01) * Height * 2];
                if (inflater.Inflate(data) != data.Length)
                    throw new SwfCorruptedException("Bitmap data are not valid ZLIB stream!");
                //Pixels = BitmapUtils.UnpackPIX15(data, Width, Height);
            }
            else if (format == 5)
            {
                data = new byte[Width * Height * 4];
                if (inflater.Inflate(data) != data.Length)
                    return true;
                //System.Array.Reverse(data, 0, data.Length);
                isFlip = true;
                
               // Pixels = BitmapUtils.UnpackPIX24(data, Width, Height, hasAlpha);

            }
            else
                throw new SwfCorruptedException("Invalid lossless bitmap format found!");
            Dates = data;

            return true;
        }

        #region ISwfTag Members

        public virtual void Load(SwfStream stream, uint length, byte version)
        {
            if (!Load(stream, length, HasAlpha))
                throw new SwfCorruptedException("Bitmap data are not valid ZLIB stream!");
        }

        #endregion
    }

    [SwfTag(36)]
    public class DefineBitsLossless2Tag : DefineBitsLosslessTag
    {
        public DefineBitsLossless2Tag()
            : base()
        {
            HasAlpha = true;
        }        
    }
}