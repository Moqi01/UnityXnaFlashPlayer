using System;
using ICSharpCode.SharpZipLib.Zip.Compression;
using XnaFlash.Swf.Structures;
using XnaVG;

namespace XnaFlash.Swf
{
    public static class BitmapUtils
    {
        private static readonly byte[] JpegPrefix = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0, 1, 1, 1, 0, 0x48, 0, 0x48, 0, 0 };
		private static readonly byte[] JpegSuffix = new byte[] { 0xFF, 0xD9 };

        public static byte[] RepairJpegMarkers(byte[] data)
        {
            if (data[0] == 0xFF && data[1] == 0xD9 && data[2] == 0xFF && data[3] == 0xD8)
            {
                byte[] copy = new byte[data.Length - 4];
                Array.Copy(data, 4, copy, 0, data.Length - 4);
                data = copy;
            }

            int index = 2;
            int len = data.Length;
            while (index + 3 < len)
            {
                byte b0 = data[index];
                if (b0 != 0xFF)
                      break;

                if (data[index + 1] == 0xD9 && data[index + 2] == 0xFF && data[index + 3] == 0xD8)
                {
                    byte[] copy = new byte[data.Length - 4];
                    Array.Copy(data, 0, copy, 0, index);
                    Array.Copy(data, index + 4, copy, index, data.Length - index - 4);
                    data = copy;
                    break;
                }
                else
                {
                    int tagLen = (data[index + 2] << 8) + data[index + 3] + 2;
                    index += tagLen;
                }
            }

            return data;
        }

        public static byte[] ComposeJpeg(byte[] tables, byte[] image)
        {
            byte[] data = new byte[tables.Length + image.Length + JpegPrefix.Length + JpegSuffix.Length];

            Array.Copy(JpegPrefix, 0, data, 0, JpegPrefix.Length);
            Array.Copy(tables, 0, data, JpegPrefix.Length, tables.Length);
            Array.Copy(image, 0, data, JpegPrefix.Length + tables.Length, image.Length);
            Array.Copy(JpegSuffix, 0, data, JpegPrefix.Length + tables.Length + image.Length, JpegSuffix.Length);

            return data;
        }

        public static BitmapFormat DetectFormat(byte[] data)
        {
            if (data.Length > 8 && BitConverter.ToUInt64(data, 0) == 0x0A1A0A0D474E5089u)
                return BitmapFormat.Png;
            if (data.Length > 6 && BitConverter.ToUInt32(data, 0) == 0x38464947u && BitConverter.ToUInt16(data, 4) == 0x6139u)
                return BitmapFormat.Gif89a;
            return BitmapFormat.Jpeg;
        }

        public static byte[] DecompressAlphaValues(byte[] alphaValues, int width, int height)
        {
            var data = new byte[width * height];
            var inflater = new Inflater();
            inflater.SetInput(alphaValues);
            if (inflater.Inflate(data) != data.Length)
                throw new ArgumentException("Alpha values are not in valid compressed format!");

            return data;
        }

        public static uint[] UnpackPIX24(byte[] data, int width, int height, bool hasAlpha)
        {
            VGColor c = new VGColor();
            c.A = 255;

            uint[] res = new uint[width * height];
            for (int i = 0, j = 0; i < data.Length; i += 4, j++)
            {
                if (hasAlpha) c.A = data[i];
                c.R = data[i + 1];
                c.G = data[i + 2];
                c.B = data[i + 3];
                res[res.Length-j-1] = c.PackedValue;
            }
            return res;
        }

        public static uint[] UnpackPIX15(byte[] data, int width, int height)
        {
            VGColor c = new VGColor();
            c.A = 255;

            uint[] res = new uint[width * height];
            int lineSize = data.Length / height;
            int pix = 0, b = 0;
            for (int offset = 0; height > 0; height--, offset += lineSize)
            {
                b = offset;
                for (int x = 0; x < width; x++, pix++, b+=2)
                {
                    c.R = (byte)(data[b] >> 2);
                    c.G = (byte)(((data[b] & 0x03) << 3) | (data[b + 1] >> 5));
                    c.B = (byte)(data[b + 1] & 0x1F);
                    res[pix] = c.PackedValue;
                }
            }
            return res;
        }

        public static uint[] UnpackIndexed(byte[] data, int width, int height, int table, bool hasAlpha)
        {
            uint[] colors = new uint[table + 1];
            int j = 0;
            if (hasAlpha)
            {
                for (int i = 0; i <= table; i++, j += 4)
                    colors[i] = new VGColor(data[j], data[j + 1], data[j + 2], data[j + 3]).PackedValue;
            }
            else
            {
                for (int i = 0; i <= table; i++, j += 3)
                    colors[i] = new VGColor(data[j], data[j + 1], data[j + 2]).PackedValue;
            }
            
            uint[] res = new uint[width * height];
            int lineSize = (data.Length - j) / height;
            int pix = 0, pos = 0;
            for (; height > 0; height--, j += lineSize)
            {
                pos = j;
                for (int x = 0; x < width; x++, pix++, pos++)
                {
                    int index = data[pos];
                    res[pix] = colors[index];
                }
            }
            return res;
        }

        public static byte[] UnpackIndexedToByte(byte[] data, int width, int height, int table, bool hasAlpha, byte[] res)
        {
            VGColor[] colors = new VGColor[table + 1];
            int j = 0;
            if (hasAlpha)
            {
                for (int i = 0; i <= table; i++, j += 4)
                    colors[i] = new VGColor(data[j], data[j + 1], data[j + 2], data[j + 3]);
            }
            else
            {
                for (int i = 0; i <= table; i++, j += 3)
                    colors[i] = new VGColor(data[j], data[j + 1], data[j + 2]);
            }


            int lineSize = (data.Length - j) / height;
            int pix = 0, pos = 0;
            for (; height > 0; height--, j += lineSize)
            {
                pos = j;
                for (int x = 0; x < width; x++, pos++)
                {
                    int index = data[pos];
                    res[pix] = colors[index].R;
                    res[pix+1] = colors[index].G;
                    res[pix+2] = colors[index].B;
                    res[pix+3] = colors[index].A;
                    pix += 4;
                }
            }
            return res;
        }

        //public static byte[] UnpackIndexedToByte(byte[] data, int width, int height, int table, bool hasAlpha, byte[] result)
        //{
        //    VGColor[] colors = new VGColor[table + 1];
        //    int z = 0;
        //    if (hasAlpha)
        //    {
        //        for (int i = 0; i <= table; i++, z += 4)
        //            colors[i] = new VGColor(data[z], data[z + 1], data[z + 2], data[z + 3]);
        //    }
        //    else
        //    {
        //        for (int i = 0; i <= table; i++, z += 3)
        //            colors[i] = new VGColor(data[z], data[z + 1], data[z + 2]);
        //    }

        //    var palette_pitch = width % 4 == 0
        //            ? width
        //            : width + (4 - width % 4);
        //    for (var i = 0; i < height; ++i)
        //    {
        //        for (var j = 0; j < width; ++j)
        //        {
        //            var result_index = j + i * width;
        //            var palette_index = data[j + i * palette_pitch+z];
        //            var palette_color = colors[palette_index];
        //            result[result_index * 4 + 0] = palette_color.A;
        //            result[result_index * 4 + 1] = palette_color.R;
        //            result[result_index * 4 + 2] = palette_color.G;
        //            result[result_index * 4 + 3] = palette_color.B;
        //        }
        //    }
        //    return result;
        //}

    }
}
