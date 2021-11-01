using System;
using System.IO;

namespace XnaFlash.Swf
{
    public class BitStream : IDisposable
    {
        private Stream mStream;
        private byte mCurrentByte;
        private byte mBytePosition;

        internal Stream BaseStream { get { return mStream; } }
        public uint Position { get { return (uint)mStream.Position; } }

        public BitStream(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            mStream = stream;
            Align();
        }
        
        public byte ReadByte()
        {
            int value = mStream.ReadByte();
            Align();

            if (value < 0)
                throw new EndOfStreamException();

            return (byte)value;
        }

        public byte[] ReadBytes(int bytes)
        {
            byte[] data = new byte[bytes];
            int read;

            Align();

            while (bytes > 0)
            {
                read = mStream.Read(data, data.Length - bytes, bytes);
                if (read <= 0) throw new EndOfStreamException();
                bytes -= read;
            }

            
            return data;
        }

        public bool ReadBit()
        {
            if (mBytePosition == 0 && !LoadByte())
                throw new EndOfStreamException();

            bool value = (mCurrentByte & mBytePosition) != 0;
            mBytePosition >>= 1;
            return value;
        }

        public int ReadBits(int bits)
        {
            int result = 0;
            bool? bit;

            while (bits > 0)
            {
                bit = ReadBit();
                if (!bit.HasValue)
                    throw new EndOfStreamException();

                result <<= 1;
                result |= bit.Value ? 1 : 0;
                bits--;
            }

            return result;
        }

        public int ReadInteger(int bits)
        {
            int expand = (-1) << bits;
            int value = ReadBits(bits);
            if ((value >> (bits - 1)) != 0)
                return value | expand;
            return value;
        }

        public void Skip(long bytes)
        {
            Align();
            mStream.Seek(bytes, SeekOrigin.Current);
        }

        public void Align()
        {
            mCurrentByte = mBytePosition = 0;
        }

        #region Private Methods

        private bool LoadByte()
        {
            int value = mStream.ReadByte();
            if (value < 0) return false;

            mCurrentByte = (byte)value;
            mBytePosition = 0x80;
            return true;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (mStream != null)
            {
                mStream.Dispose();
                mStream = null;
            }
        }

        #endregion
    }
}
