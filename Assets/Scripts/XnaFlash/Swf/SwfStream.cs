using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Microsoft.Xna.Framework;
using XnaFlash.Swf.Structures;
using XnaFlash.Swf.Tags;
using XnaVG;

namespace XnaFlash.Swf
{
    /// <summary>
    /// Parses SWF file
    /// </summary>
    public class SwfStream : IDisposable
    {
        public static int SupportedVersion { get { return 7; } }

        private List<ushort> mUnknownTags = new List<ushort>(16);
        private BitStream mBitStream;
        private uint mTagStart;

        public JPEGTablesTag JpegTables { get; private set; }
        public bool Compressed { get; private set; }
        public byte Version { get; private set; }
        public uint Length { get; private set; }
        public Rectangle Rectangle { get; private set; }
        public decimal FrameRate { get; private set; }
        public ushort FrameCount { get; private set; }
        internal int TagPosition { get { return (int)(mBitStream.Position - mTagStart); } }

        public SwfStream(Stream stream)
        {
            JpegTables = new JPEGTablesTag();
            mBitStream = new BitStream(stream);
            ReadHeader();
        }
        public SwfStream(byte[] data)
            : this(new MemoryStream(data))
        { }

        public IEnumerable<ISwfTag> ProcessFile()
        {
            ISwfTag tag;

            while (true)
            {
                tag = ReadTag();

                if (tag == null)
                    continue;

                if (tag is JPEGTablesTag)
                    (tag as JPEGTablesTag).CloneTo(JpegTables);

                if (tag is EndTag)
                    break;

                yield return tag;
            }
        }
        public IEnumerable<string> GetSkippedTags()
        {
            return mUnknownTags.Select(t => SwfTagAttribute.GetName(t));
        }
        public void Skip(long bytes)
        {
            mBitStream.Skip(bytes);
        }

        #region Integer Types

        public byte ReadByte()
        {
            return mBitStream.ReadByte();
        }

        public sbyte ReadSByte()
        {
            return (sbyte)mBitStream.ReadByte();
        }

        public ushort ReadUShort()
        {
            byte[] bytes = mBitStream.ReadBytes(2);
            return (ushort)(bytes[0] | (bytes[1] << 8));
        }

        public short ReadShort()
        {
            byte[] bytes = mBitStream.ReadBytes(2);
            return (short)(bytes[0] | (bytes[1] << 8));
        }

        public uint ReadUInt()
        {
            byte[] bytes = mBitStream.ReadBytes(4);
            return (uint)(bytes[0] | (bytes[1] << 8) | (bytes[2] << 16) | (bytes[3] << 24));
        }

        public int ReadInt()
        {
            byte[] bytes = mBitStream.ReadBytes(4);
            return (bytes[0] | (bytes[1] << 8) | (bytes[2] << 16) | (bytes[3] << 24));
        }

        public ulong ReadULong()
        {
            ulong lower = (ulong)ReadUInt();
            ulong upper = (ulong)ReadUInt();
            upper <<= 32;
            return lower | upper;
        }

        public long ReadLong()
        {
            long lower = (long)ReadInt();
            long upper = (long)ReadInt();
            upper <<= 32;
            return lower | upper;
        }

        #endregion

        #region Integer Arrays

        public byte[] ReadByteArray(long count)
        {
            byte[] data = new byte[count];
            while (count > 0)
                data[data.Length - count--] = ReadByte();
            return data;
        }

        public sbyte[] ReadSByteArray(long count)
        {
            sbyte[] data = new sbyte[count];
            while (count > 0)
                data[data.Length - count--] = ReadSByte();
            return data;
        }

        public ushort[] ReadUShortArray(long count)
        {
            ushort[] data = new ushort[count];
            while (count > 0)
                data[data.Length - count--] = ReadUShort();
            return data;
        }

        public short[] ReadShortArray(long count)
        {
            short[] data = new short[count];
            while (count > 0)
                data[data.Length - count--] = ReadShort();
            return data;
        }

        public uint[] ReadUIntArray(long count)
        {
            uint[] data = new uint[count];
            while (count > 0)
                data[data.Length - count--] = ReadUInt();
            return data;
        }

        public int[] ReadIntArray(long count)
        {
            int[] data = new int[count];
            while (count > 0)
                data[data.Length - count--] = ReadInt();
            return data;
        }

        public ulong[] ReadULongArray(long count)
        {
            ulong[] data = new ulong[count];
            while (count > 0)
                data[data.Length - count--] = ReadULong();
            return data;
        }

        public long[] ReadLongArray(long count)
        {
            long[] data = new long[count];
            while (count > 0)
                data[data.Length - count--] = ReadLong();
            return data;
        }

        #endregion

        #region Fixed Types

        public decimal ReadFixed()
        {
            return ((decimal)ReadInt()) / 65536m;
        }

        public decimal ReadFixedHalf()
        {
            return ((decimal)ReadShort()) / 256m;
        }

        #endregion

        #region Floating Types
        // FIXME: Are aligned or not?!

        public float ReadHalf()
        {
            int sign = mBitStream.ReadBit() ? -1 : 1;
            int exp = mBitStream.ReadInteger(5);
            return (float)(sign * mBitStream.ReadBits(10) * Math.Pow(2.0, exp));
        }

        public float ReadSingle()
        {
            int sign = mBitStream.ReadBit() ? -1 : 1;
            int exp = mBitStream.ReadInteger(8);
            return (float)(sign * mBitStream.ReadBits(23) * Math.Pow(2.0, exp));
        }

        public double ReadDouble()
        {
            int sign = mBitStream.ReadBit() ? -1 : 1;
            int exp = mBitStream.ReadInteger(11);
            return sign * mBitStream.ReadBits(52) * Math.Pow(2.0, exp);
        }

        public float ReadColorByte()
        {
            return ReadByte() / 255f;
        }

        #endregion

        #region Encoded Integers

        public uint ReadEncodedUInt()
        {
            int value = ReadByte();
            if ((value & 0x80) == 0)
                return (uint)value;

            value = (value & 0x7F) | (ReadByte() << 7);
            if ((value & 0x4000) == 0)
                return (uint)value;

            value = (value & 0x3FFF) | (ReadByte() << 14);
            if ((value & 0x200000) == 0)
                return (uint)value;

            value = (value & 0x1FFFFF) | (ReadByte() << 21);
            if ((value & 0x10000000) == 0)
                return (uint)value;

            value = (value & 0x0FFFFFFF) | (ReadByte() << 28);
            return (uint)value;
        }

        #endregion

        #region Bitfields

        public bool ReadBit()
        {
            return mBitStream.ReadBit();
        }

        public int ReadBitInt(int bits)
        {
            return mBitStream.ReadInteger(bits);
        }

        public uint ReadBitUInt(int bits)
        {
            return (uint)mBitStream.ReadBits(bits);
        }

        public decimal ReadBitFixed(int bits)
        {
            return mBitStream.ReadInteger(bits) / 65536m;
        }

        #endregion

        #region Basic Structures

        public Rectangle ReadRectangle()
        {
            mBitStream.Align();

            int bits = (int)ReadBitUInt(5);
            int xmin = ReadBitInt(bits);
            int xmax = ReadBitInt(bits);
            int ymin = ReadBitInt(bits);
            int ymax = ReadBitInt(bits);

            return new Rectangle(xmin, ymin, xmax - xmin, ymax - ymin);
        }
        
        public string ReadString()
        {
            List<byte> bytes = new List<byte>(256);
            byte b;

            while ((b = ReadByte()) != 0) 
                bytes.Add(b);

            if (Version >= 6)
                return Encoding.UTF8.GetString(bytes.ToArray(), 0, bytes.Count);

            var sb = new StringBuilder(bytes.Count);
            foreach (var c in bytes) sb.Append((char)c);
            return sb.ToString();
        }
        public string ReadString(int count)
        {
            byte[] bytes = new byte[count];

            for (int i = 0; i < count; i++)
                bytes[i] = ReadByte();

            if (Version >= 6)
                return Encoding.UTF8.GetString(bytes, 0, bytes.Length).TrimEnd('\0');

            var sb = new StringBuilder(bytes.Length);
            foreach (var c in bytes) sb.Append((char)c);
            return sb.ToString().TrimEnd('\0');
        }

        public SwfLanguage ReadLanguage()
        {
            return (SwfLanguage)ReadByte();
        }

        public ClipEventFlags ReadClipEventFlags()
        {
            var data = ReadByteArray(Version >= 6 ? 4 : 2);
            if (data.Length == 2) Array.Resize(ref data, 4);
            return (ClipEventFlags)((data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3]);
        }

        public VGColor ReadRGB()
        {
            float r = ReadColorByte();
            float g = ReadColorByte();
            float b = ReadColorByte();

            return new VGColor(r, g, b);
        }

        public VGColor ReadRGBA()
        {
            float r = ReadColorByte();
            float g = ReadColorByte();
            float b = ReadColorByte();
            float a = ReadColorByte();

            return new VGColor(r, g, b, a);
        }

        public VGColor ReadARGB()
        {
            float a = ReadColorByte();
            float r = ReadColorByte();
            float g = ReadColorByte();
            float b = ReadColorByte();            

            return new VGColor(r, g, b, a);
        }

        public VGMatrix ReadMatrix()
        {
            int scaleBits, rotateBits, translateBits;
            decimal scaleX = 1m, scaleY = 1m, rotateA = 0, rotateB = 0;
            int translateX, translateY;
            
            mBitStream.Align();
            if (mBitStream.ReadBit())
            {
                scaleBits = (int)ReadBitUInt(5);
                scaleX = ReadBitFixed(scaleBits);
                scaleY = ReadBitFixed(scaleBits);            
            }

            if (mBitStream.ReadBit())
            {
                rotateBits = (int)ReadBitUInt(5);
                rotateA = ReadBitFixed(rotateBits);
                rotateB = ReadBitFixed(rotateBits);
            }

            translateBits = (int)ReadBitUInt(5);
            translateX = ReadBitInt(translateBits);
            translateY = ReadBitInt(translateBits);

            return new VGMatrix((float)scaleX, (float)rotateA, (float)rotateB, (float)scaleY, translateX, translateY);
        }

        public VGCxForm ReadCxForm(bool hasAlpha)
        {
            int bits;
            float[] values = new float[8] { 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f };
            bool hasMul, hasAdd;

            mBitStream.Align();
            hasAdd = mBitStream.ReadBit();
            hasMul = mBitStream.ReadBit();
            bits = mBitStream.ReadBits(4);

            if (hasMul)
            {
                values[0] = ReadBitInt(bits) / 256.0f;
                values[1] = ReadBitInt(bits) / 256.0f;
                values[2] = ReadBitInt(bits) / 256.0f;
                
                if (hasAlpha)
                    values[3] = ReadBitInt(bits) / 256.0f;
            }

            if (hasAdd)
            {
                values[4] = ReadBitInt(bits) / 255.0f;
                values[5] = ReadBitInt(bits) / 255.0f;
                values[6] = ReadBitInt(bits) / 255.0f;

                if (hasAlpha)
                    values[7] = ReadBitInt(bits) / 255.0f;
            }

            return new VGCxForm(values[4], values[5], values[6], values[7], values[0], values[1], values[2], values[3]);
        }

        public void Align()
        {
            mBitStream.Align();
        }

        #endregion

        #region Other Arrays

        public float[] ReadSingleArray(int count)
        {
            float[] data = new float[count];
            while (count > 0)
                data[data.Length - count--] = ReadSingle();
            return data;
        }

        public Rectangle[] ReadRectangleArray(int count)
        {
            Rectangle[] data = new Rectangle[count];
            while (count > 0)
                data[data.Length - count--] = ReadRectangle();
            return data;
        }

        public VGColor[] ReadRGBAArray(int count)
        {
            VGColor[] data = new VGColor[count];
            while (count > 0)
                data[data.Length - count--] = ReadRGBA();
            return data;
        }

        public char[] ReadCharArray(int count)
        {
            char[] data = new char[count];
            while (count > 0)
                data[data.Length - count--] = (char)ReadUShort();
            return data;
        }

        #endregion

        #region SWF Structures

        private void ReadHeader()
        {
            char[] codeChars = mBitStream.ReadBytes(3).Select(b => (char)b).ToArray();
            string code = new string(codeChars);
            if (code != "FWS" && code != "CWS") throw new SwfCorruptedException("Invalid format header found!");

            Version = mBitStream.ReadByte();
            //if (Version > SupportedVersion) throw new SwfCorruptedException("Only SWF up to version " + SupportedVersion + " are supported!");
            UnityEngine.Debug.Log("Swf Version: " +Version);

            Length = ReadUInt();
            Compressed = code == "CWS";
            if (Compressed) Decompress();

            Rectangle = ReadRectangle();
            FrameRate = ReadFixedHalf();
            FrameCount = ReadUShort();
        }
        private void Decompress()
        {
            byte[] data = new byte[Length - 8];
            var iis = new InflaterInputStream(mBitStream.BaseStream);
            int i = 0, j;
            
            do
            {
                j = iis.Read(data, i, data.Length - i);
                if (j <= 0) throw new EndOfStreamException();
                i += j;
            }
            while(i < data.Length);
            mBitStream = new BitStream(new MemoryStream(data));
        }
        static int num;

        List<int> TagSkipId = new List<int>();
        internal ISwfTag ReadTag()
        {
            num++;
            ushort v  = ReadUShort();
            //UnityEngine.Debug.Log("id:" + v + "Num: " + num);
            ushort id = v;
            v >>= 6;

            bool _isLongTag = false;
           
            int length = id & 0x3F;


            if (length > 0x3E)
            {
                length = ReadInt();
                _isLongTag = (length <= 0x3E);
            }

            mTagStart = mBitStream.Position;
            id >>= 6;
           

            string name = string.Empty;
            var tag = SwfTagFactory.LoadTag(this, id, (uint)length, Version, ref name);
            
            //UnityEngine.Debug.Log(num++);
            if (tag == null)
            {
                BitStreamSkip(length,id,tag);
                if (!mUnknownTags.Contains(id))
                    mUnknownTags.Add(id);
            }else
            {
                //if (tag is DefineShapeTag || tag is DefineShape2Tag || tag is DefineShape3Tag || tag is DefineShape4Tag)
                if (!(tag is DefineSpriteTag ))
                {
                    long end = mTagStart + length;
                    if (end > mBitStream.Position)
                    {
                        BitStreamSkip(end - mBitStream.Position,id,tag);
                    }
                    else if (end < mBitStream.Position)
                    {
                        throw new Exception("Error " + tag.ToString()+ " Offset:" +  (mBitStream.Position-end));
                    }
                }
                
            }
            
            return tag;
        }

        void BitStreamSkip(long lengh,int id,ISwfTag tag)
        {
            mBitStream.Skip(lengh);
            if (!TagSkipId.Contains(id))
            {
                UnityEngine.Debug.LogWarning(string.Format("Tag: {0} Id:{1} SkipLengh: {2}",tag==null?"Null": tag.ToString(), id, lengh));
                TagSkipId.Add(id);
            }
        }
        #endregion

        public void Dispose()
        {
            if (mBitStream != null)
            {
                mBitStream.Dispose();
                mBitStream = null;
            }
        }
    }
}
