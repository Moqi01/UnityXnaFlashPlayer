using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;

#if WINDOWS

using Microsoft.Xna.Framework.Design;
namespace XnaVG
{
    [Serializable]
    [TypeConverter(typeof(ColorConverter))]
#else
namespace XnaVG
{
#endif
    public struct VGColor : IPackedVector<uint>, IPackedVector, IEquatable<VGColor>
    {
        private Vector4 _value;

        public byte A
        {
            get { return (byte)Math.Max(0, Math.Min(255, (int)(_value.W * 255f))); }
            set { _value.W = value / 255f; }
        }
        public byte R
        {
            get { return (byte)Math.Max(0, Math.Min(255, (int)(_value.X * 255f))); }
            set { _value.X = value / 255f; }
        }
        public byte G
        {
            get { return (byte)Math.Max(0, Math.Min(255, (int)(_value.Y * 255f))); }
            set { _value.Y = value / 255f; }
        }
        public byte B
        {
            get { return (byte)Math.Max(0, Math.Min(255, (int)(_value.Z * 255f))); }
            set { _value.Z = value / 255f; }
        }
        public uint PackedValue
        {
            get { return ((uint)A) << 24 | ((uint)B) << 16 | ((uint)G) << 8 | ((uint)R); }
            set { _value = new Vector4(value & 0xFF, (value >> 8) & 0xFF, (value >> 16) & 0xFF, (value >> 24) & 0xFF); }
        }        

        public VGColor(Vector3 vector)
        {
            _value = new Vector4(vector, 1f);
        }
        public VGColor(Vector4 vector)
        {
            _value = vector;
        }
        public VGColor(float r, float g, float b)
        {
            _value = new Vector4(r, g, b, 1f);
        }
        public VGColor(int r, int g, int b)
        {
            _value = new Vector4(r / 255f, g / 255f, b / 255f, 1f);
        }
        public VGColor(float r, float g, float b, float a)
        {
            _value = new Vector4(r, g, b, a);
        }
        public VGColor(int r, int g, int b, int a)
        {
            _value = new Vector4(r / 255f, g / 255f, b / 255f, a / 255f);
        }
        public VGColor(uint packedValue)
            : this()
        {
            PackedValue = packedValue;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(_value.X, _value.Y, _value.Z);
        }
        public Vector4 ToVector4()
        {
            return _value;
        }

        public static bool operator !=(VGColor a, VGColor b)
        {
            return !(a == b);
        }
        public static VGColor operator *(VGColor value, float scale)
        {
            return new VGColor(value._value * scale);
        }
        public static bool operator ==(VGColor a, VGColor b)
        {
            return a._value == b._value;
        }
        public static implicit operator Color(VGColor color)
        {
            return Color.FromNonPremultiplied(color._value);
        }
        public static implicit operator VGColor(Color color)
        {
            Vector4 c = color.ToVector4();
            if (c.W != 0f)
            {
                c.X /= c.W;
                c.Y /= c.W;
                c.Z /= c.W;
            }
            return new VGColor(c);
        }
        public static implicit operator Vector3(VGColor color)
        {
            return color.ToVector3();
        }
        public static implicit operator Vector4(VGColor color)
        {
            return color.ToVector4();
        }
        public static implicit operator VGColor(Vector3 color)
        {
            return new VGColor(color);
        }
        public static implicit operator VGColor(Vector4 color)
        {
            return new VGColor(color);
        }

        public static VGColor Lerp(VGColor value1, VGColor value2, float amount)
        {
            return new VGColor(Vector4.Lerp(value1._value, value2._value, amount));
        }
        public static VGColor Multiply(VGColor value, float scale)
        {
            return value * scale;
        }

        public static VGColor FromLinearRGB(Vector4 color)
        {
            return FromLinearRGB(color.X, color.Y, color.Z, color.W);
        }
        public static VGColor FromLinearRGB(float r, float g, float b, float a)
        {
            r = (r <= 0.00304) ? (r * 12.92f) : (float)(Math.Pow(r, 1.0 / 2.4) * 1.0556 - 0.0556);
            g = (g <= 0.00304) ? (g * 12.92f) : (float)(Math.Pow(g, 1.0 / 2.4) * 1.0556 - 0.0556);
            b = (b <= 0.00304) ? (b * 12.92f) : (float)(Math.Pow(b, 1.0 / 2.4) * 1.0556 - 0.0556);
            return new VGColor(r, g, b, a);
        }
        public static VGColor FromLinearLuminance(float luminance, float alpha)
        {
            luminance = (luminance <= 0.00304) ? (luminance * 12.92f) : (float)(Math.Pow(luminance, 1.0 / 2.4) * 1.0556 - 0.0556);
            return new VGColor(luminance, luminance, luminance, alpha);
        }
        public static VGColor FromLuminance(float luminance, float alpha)
        {
            return new VGColor(luminance, luminance, luminance, alpha);
        }

        public Vector4 ToLinearRGB()
        {
            float r = _value.X, g = _value.Y, b = _value.Z, a = _value.W;
            r = (r <= 0.03928) ? (r / 12.92f) : (float)Math.Pow((r + 0.0556) / 1.0556, 2.4f);
            g = (g <= 0.03928) ? (g / 12.92f) : (float)Math.Pow((g + 0.0556) / 1.0556, 2.4f);
            b = (b <= 0.03928) ? (b / 12.92f) : (float)Math.Pow((b + 0.0556) / 1.0556, 2.4f);
            return new Vector4(r, g, b, a);
        }
        public float ToLinearLuminance()
        {
            Vector4 lRGB = ToLinearRGB();
            return 0.2126f * lRGB.X + 0.7152f * lRGB.Y + 0.0722f * lRGB.Z;
        }
        public float ToLuminance()
        {
            float l = ToLinearLuminance();
            l = (l <= 0.00304) ? (l * 12.92f) : (float)(Math.Pow(l, 1.0 / 2.4) * 1.0556 - 0.0556);
            return l;
        }

        public bool Equals(VGColor other)
        {
            return this == other;
        }
        public override bool Equals(object obj)
        {
            return (obj is VGColor) && ((VGColor)obj) == this;
        }
        public override int GetHashCode()
        {
            unchecked { return (int)PackedValue; }
        }
        public override string ToString()
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", R, G, B, A);
        }
        
        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            _value = vector;
        }        
    }
}
