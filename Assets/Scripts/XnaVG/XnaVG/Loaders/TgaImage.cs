using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaVG.Loaders
{
    public static class TgaImage
    {
        public static bool ThrowOnLosslessOps = false;
        public const int MetadataSize = 18;

        public static Texture2D LoadTga(Stream stream, GraphicsDevice device)
        {
            var header = TgaHeader.FromStream(stream);
            stream.Seek(header.IDLength, SeekOrigin.Current);

            if (header.ColorMapType != 0 || header.CMapDepth != 0 || header.CMapLength != 0)
                throw new NotSupportedException("Color maps are not supported!");

            if ((header.ImageDescriptor & 0xF0) != 0 && ThrowOnLosslessOps)
                throw new NotSupportedException("Lossless operations are not supported!");

            header.ImageDescriptor &= 0x0F;
            bool Alpha8 = (header.ImageType == 3 && header.PixelDepth == 8 && header.ImageDescriptor == 0);
            bool ColorN = (header.ImageType == 2 && header.PixelDepth == 24 && header.ImageDescriptor == 0);
            bool ColorA = (header.ImageType == 2 && header.PixelDepth == 32 && header.ImageDescriptor == 8);

            if (Alpha8)
            {
                byte[] data = new byte[header.Width * header.Height];

                for (int y = header.Height - 1; y >= 0; y--)
                {
                    int end = (y + 1) * header.Width;
                    for (int i = end - header.Width; i < end; i++)
                        data[i] = (byte)stream.ReadByte();
                }

                var tex = new Texture2D(device, header.Width, header.Height, false, SurfaceFormat.Alpha8);
                tex.SetData(data);
                return tex;
            }
            else if (ColorN || ColorA)
            {
                Color[] data = new Color[header.Width * header.Height];
                Color c = Color.Black;

                for (int y = header.Height - 1; y >= 0; y--)
                {
                    int end = (y + 1) * header.Width;
                    for (int i = end - header.Width; i < end; i++)
                    {
                        c.B = (byte)stream.ReadByte();
                        c.G = (byte)stream.ReadByte();
                        c.R = (byte)stream.ReadByte();
                        if (ColorA) c.A = (byte)stream.ReadByte();
                        data[i] = c;
                    }
                }

                var tex = new Texture2D(device, header.Width, header.Height, false, SurfaceFormat.Color);
                tex.SetData(data);
                return tex;
            }
            else
                throw new NotSupportedException("Only uncompressed 8-bit grayscale, 24/32-bit RGB are supported!");
        }
        public static void SaveAsTga(Stream stream, Texture2D texture)
        {
            int w = texture.Width;
            int h = texture.Height;

            if (texture.Format == SurfaceFormat.Alpha8)
            {
                var data = new byte[w * h];
                texture.GetData(data);
                SaveAsTga(stream, data, w, h);
            }
            else if (texture.Format == SurfaceFormat.Color)
            {
                var data = new uint[w * h];
                texture.GetData(data);
                SaveAsTga(stream, data, w, h, true);
            }
            else
                throw new NotSupportedException("Only Alpha8 and Color surfaces can be saved as TGA!");
        }
        public static void SaveAsTga(Stream stream, byte[] data, int width, int height)
        {
            new TgaHeader
            {
                IDLength = 0,
                ColorMapType = 0,
                ImageType = 3,
                CMapStart = 0,
                CMapLength = 0,
                CMapDepth = 0,
                XOffset = 0,
                YOffset = 0,
                Width = (short)width,
                Height = (short)height,
                PixelDepth = 8,
                ImageDescriptor = 0
            }.ToStream(stream);

            for (int y = height - 1; y >= 0; y--)
            {
                int end = (y + 1) * width;
                for (int i = end - width; i < end; i++)
                    stream.WriteByte((byte)data[i]);
            }
        }
        public static void SaveAsTga(Stream stream, uint[] data, int width, int height, bool alpha)
        {
            new TgaHeader
            {
                IDLength = 0,
                ColorMapType = 0,
                ImageType = 2,
                CMapStart = 0,
                CMapLength = 0,
                CMapDepth = 0,
                XOffset = 0,
                YOffset = 0,
                Width = (short)width,
                Height = (short)height,
                PixelDepth = (byte)(alpha ? 32 : 24),
                ImageDescriptor = (byte)(alpha ? 8 : 0)
            }.ToStream(stream);

            Color c = new Color();
            if (alpha)
            {
                for (int y = height - 1; y >= 0; y--)
                {
                    int end = (y + 1) * width;
                    for (int i = end - width; i < end; i++)
                    {
                        c.PackedValue = data[i];
                        stream.WriteByte(c.B);
                        stream.WriteByte(c.G);
                        stream.WriteByte(c.R);
                        stream.WriteByte(c.A);
                    }
                }
            }
            else
            {
                for (int y = height - 1; y >= 0; y--)
                {
                    int end = (y + 1) * width;
                    for (int i = end - width; i < end; i++)
                    {
                        c.PackedValue = data[i];
                        stream.WriteByte(c.B);
                        stream.WriteByte(c.G);
                        stream.WriteByte(c.R);
                    }
                }
            }
        }
        
        private struct TgaHeader
        {
            public byte IDLength;
            public byte ColorMapType;
            public byte ImageType;
            public short CMapStart;
            public short CMapLength;
            public byte CMapDepth;
            public short XOffset;
            public short YOffset;
            public short Width;
            public short Height;
            public byte PixelDepth;
            public byte ImageDescriptor;

            public static TgaHeader FromStream(Stream stream)
            {
                var h = new TgaHeader();
                h.IDLength = (byte)stream.ReadByte();
                h.ColorMapType = (byte)stream.ReadByte();
                h.ImageType = (byte)stream.ReadByte();
                h.CMapStart = ReadShort(stream);
                h.CMapLength = ReadShort(stream);
                h.CMapDepth = (byte)stream.ReadByte();
                h.XOffset = ReadShort(stream);
                h.YOffset = ReadShort(stream);
                h.Width = ReadShort(stream);
                h.Height = ReadShort(stream);
                h.PixelDepth = (byte)stream.ReadByte();
                h.ImageDescriptor = (byte)stream.ReadByte();
                return h;
            }
            public void ToStream(Stream stream)
            {
                stream.WriteByte(IDLength);
                stream.WriteByte(ColorMapType);
                stream.WriteByte(ImageType);

                WriteShort(stream, CMapStart);
                WriteShort(stream, CMapLength);
                stream.WriteByte(CMapDepth);

                WriteShort(stream, XOffset);
                WriteShort(stream, YOffset);
                WriteShort(stream, Width);
                WriteShort(stream, Height);
                stream.WriteByte(PixelDepth);
                stream.WriteByte(ImageDescriptor);
            }
            private static void WriteShort(Stream stream, short number)
            {
                stream.WriteByte((byte)(number & 0xFF));
                stream.WriteByte((byte)((number >> 8) & 0xFF));
            }
            private static short ReadShort(Stream stream)
            {
                int s = stream.ReadByte() & 0xFF;
                s |= (stream.ReadByte() & 0xFF) << 8;
                return (short)s;
            }
        }
    }
}
